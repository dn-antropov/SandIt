#include "granular_simulation.h"
#include <godot_cpp/variant/utility_functions.hpp>
#include <vector>

#include "./packets/packet_types.h"
#include "./packets/basic/nothing.h"
#include "./packets/basic/wall.h"
#include "./packets/basic/basic.h"
#include "./packets/basic/spam.h"

#include "./thirdparty/douglas-peucker/polygon-simplify.h"

using namespace godot;

GranularSimulation::GranularSimulation() {
    fill_id_pool(&idPool);
    initialize_grid(&packets);
}

GranularSimulation::~GranularSimulation() {}

void GranularSimulation::initialize_grid(std::vector<Packet*> *ps) {
    ps->resize(width*height);
    for (int i = 0; i < width*height; i++) {
        ps->at(i) = new Nothing();
        assign_id(ps->at(i));
        ps->at(i)->prevPosition = i;
    }

    render_data = PackedByteArray();
    position_data = PackedByteArray();
    render_data.resize(width * height * 3);
    position_data.resize(width * height * 3);
}

void GranularSimulation::fill_id_pool(std::unordered_set<int> *idPool) {
    idPool->clear();
    for (int i = 0; i < width * height; i++) {
        idPool->insert(i);
    }
}

void GranularSimulation::step(int iterations) {
    for (int i = 0; i < iterations; i++) {
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                packets[row * width + col]->prevPosition = row * width + col;
                packets[row * width + col]->set_is_updated(false);
                packets[row * width + col]->set_is_moving(false);
            }
        }
        int block_size = 2;
        for (int block_row = height - block_size; block_row >= 0; block_row -= block_size) {
            bool left_to_right = (block_row % 2 == 0);
            if (left_to_right) {
                for (int block_col = 0; block_col < width; block_col += block_size) {
                    process_block(block_row, block_col, block_size);
                }
            } else {
                int start_col = ((width - 1) / block_size) * block_size;
                for (int block_col = start_col; block_col >= 0; block_col -= block_size) {
                    process_block(block_row, block_col, block_size);
                }
            }
        }
    }

    // disable outlines detection, as for now i don't think i need them
    // std::vector<MarchingSquares::Result> rs = MarchingSquares::FindPerimeters(width, height, 16, data);
    // pack_outlines(rs);
    // simplify_outlines();
}

void GranularSimulation::process_block(int start_row, int start_col, int block_size) {
    std::vector<std::pair<int, int>> positions;

    for (int r = start_row; r < start_row + block_size; r++) {
        for (int c = start_col; c < start_col + block_size; c++) {
            positions.push_back({r, c});
        }
    }

    // Shuffle positions within the block
    for (int i = positions.size() - 1; i > 0; i--) {
        int j = (int)(randf() * (i + 1));
        std::swap(positions[i], positions[j]);
    }

    // Update particles in random order within the block
    for (auto pos : positions) {
        int row = pos.first;
        int col = pos.second;
        int idx = row * width + col;
        if (!packets[idx]->get_is_updated()) {
            packets[idx]->update(this, row, col);
        }
    }
}

void GranularSimulation::create_packet(int row, int col, int type) {
    if (!is_in_bounds(row, col))
        return;

    if (packets[row * width + col]->type == type)
        return;

    remove_id(packets[row * width + col]);
    delete packets[row * width + col];

    PacketType packetType = static_cast<PacketType>(type);
    Packet* packet;
    switch(packetType) {
        case ENothing:
            packet = new Nothing();
            break;
        case EWall:
            packet = new Wall();
            break;
        case EBasic:
            packet = new Basic();
            break;
        case ESpam:
            packet = new Spam();
            break;
        default:
            packet = new Nothing();
            break;
    }

    assign_id(packet);
    packets[row * width + col] = packet;
    packets[row * width + col]->prevPosition = row * width + col;
}

int GranularSimulation::destroy_packet(int row, int col) {
    if (!is_in_bounds(row, col))
        return -1;
    int type = packets[row * width + col]->type;

    if (type == PacketType::ENothing)
        return PacketType::ENothing;

    remove_id(packets[row * width + col]);
    delete packets[row * width + col];

    packets[row * width + col] = new Nothing();
    assign_id(packets[row * width + col]);
    return type;
}

void GranularSimulation::assign_id(Packet* packet) {
    packet->id = *idPool.begin();
    idPool.erase(packet->id);
}

void GranularSimulation::remove_id(Packet* packet) {
    idPool.insert(packet->id);
}

void GranularSimulation::swap(int rowA, int colA, int rowB, int colB) {
    if (!is_in_bounds(rowA, colA) || !is_in_bounds(rowB, colB))
        return;

    Packet *tempP = packets[rowA * width + colA];

    packets[rowA * width + colA] = packets[rowB * width + colB];
    packets[rowB * width + colB] = tempP;
    tempP = NULL;

    packets[rowA * width + colA]->set_is_updated(true);
    packets[rowB * width + colB]->set_is_updated(true);

    packets[rowA * width + colA]->set_is_moving(true);
    packets[rowB * width + colB]->set_is_moving(true);
}

bool GranularSimulation::is_in_bounds(int row, int col) {
    return row >= 0 && col >= 0 && row < height && col < width;
}

bool GranularSimulation::is_swappable(int rowA, int colA, int rowB, int colB) {
    if (!is_in_bounds(rowA, colA) || !is_in_bounds(rowB, colB))
        return false;

    if (packets[rowA * width + colA]->get_density() <= packets[rowB * width + colB]->get_density())
        return false;

    return true;
}

inline float GranularSimulation::randf() {
    g_seed = (214013 * g_seed + 2531011);
    return ((g_seed>>16) & 0x7FFF) / (double) (0x7FFF + 1);
}

Vector2i GranularSimulation::get_dimensions() {
    Vector2i dimensions = Vector2i(width, height);
    return dimensions;
}

PackedByteArray GranularSimulation::get_render_data() {
    for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
            int idx = (y * width + x) * 3;
            int id = packets[y * width + x]->id;

            render_data.set(idx, packets[y * width + x]->type);
            // convert id to b and g channels by storing lower and upper part
            render_data.set(idx + 1, id & 0xFF);
            render_data.set(idx + 2, (id >> 8) & 0xFF);
        }
    }
    return render_data;
}

PackedByteArray GranularSimulation::get_interpolated_render_data(float alpha, int render_scale) {
    int render_width = width * render_scale;
    int render_height = height * render_scale;

    PackedByteArray interpolated_data;
    interpolated_data.resize(render_width * render_height * 3);

    // Clear the render data
    for (int i = 0; i < render_width * render_height * 3; i++) {
        interpolated_data.set(i, 0);
    }

    // For each particle, calculate interpolated position and render it
    for (int row = 0; row < height; row++) {
        for (int col = 0; col < width; col++) {
            Packet* packet = packets[row * width + col];

            if (packet->type == PacketType::ENothing)
                continue;

            // Get previous position
            int prev_linear = packet->prevPosition;
            int prev_row = prev_linear / width;
            int prev_col = prev_linear % width;

            // Current position
            int curr_row = row;
            int curr_col = col;

            // Interpolate in simulation space
            float interp_row = prev_row + alpha * (curr_row - prev_row);
            float interp_col = prev_col + alpha * (curr_col - prev_col);

            // Convert to render space
            float render_x = (interp_col + 0.5f) * render_scale;
            float render_y = (interp_row + 0.5f) * render_scale;

            // Draw particle (simple approach - just center pixel)
            int pixel_x = (int)round(render_x);
            int pixel_y = (int)round(render_y);

            if (pixel_x >= 0 && pixel_x < render_width && pixel_y >= 0 && pixel_y < render_height) {
                int idx = (pixel_y * render_width + pixel_x) * 3;
                interpolated_data.set(idx, packet->type);
                interpolated_data.set(idx + 1, packet->id & 0xFF);
                interpolated_data.set(idx + 2, (packet->id >> 8) & 0xFF);
            }
        }
    }

    return interpolated_data;
}

TypedArray<PackedVector2Array> GranularSimulation::get_outlines() {
    return outlines;
}

TypedArray<PackedVector2Array> GranularSimulation::get_simplified_outlines() {
    return simplified_outlines;
}

void GranularSimulation::pack_outlines(std::vector<MarchingSquares::Result> results) {
    outlines.clear();
    for (MarchingSquares::Result result: results) {
        int prevX = result.initialY;
        int prevY = result.initialX;

        PackedVector2Array outlineToAdd;
        outlineToAdd.resize(size(result.directions));
        for (int dI = 0; dI < size(result.directions); dI++) {
            prevX = prevX - result.directions[dI].y;
            prevY = prevY + result.directions[dI].x;

            outlineToAdd.set(dI, Vector2(prevX, prevY));
        }
        outlines.append(outlineToAdd);
    }
}

void GranularSimulation::simplify_outlines() {
    simplified_outlines.clear();
    for (int i = outlines.size() - 1; i >= 0; i--) {
        PackedVector2Array simplifiedOutlineToAdd = DouglasPeucker::simplify(outlines[i], 1);
        simplified_outlines.append(simplifiedOutlineToAdd);
    }
}

void GranularSimulation::_bind_methods() {
    ClassDB::bind_method(D_METHOD("step"), &GranularSimulation::step);
    ClassDB::bind_method(D_METHOD("create_particle"), &GranularSimulation::create_packet);
    ClassDB::bind_method(D_METHOD("destroy_particle"), &GranularSimulation::destroy_packet);
    ClassDB::bind_method(D_METHOD("get_dimensions"), &GranularSimulation::get_dimensions);
    ClassDB::bind_method(D_METHOD("get_render_data"), &GranularSimulation::get_render_data);
    ClassDB::bind_method(D_METHOD("get_interpolated_render_data", "alpha", "render_scale"), &GranularSimulation::get_interpolated_render_data);
    ClassDB::bind_method(D_METHOD("get_outlines"), &GranularSimulation::get_outlines);
    ClassDB::bind_method(D_METHOD("get_simplified_outlines"), &GranularSimulation::get_simplified_outlines);
    ClassDB::bind_method(D_METHOD("is_in_bounds"), &GranularSimulation::is_in_bounds);
}
