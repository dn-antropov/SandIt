#include "granular_simulation.h"
#include <godot_cpp/variant/utility_functions.hpp>
#include <vector>

#include "particles/basic/nothing.h"
#include "particles/basic/sand.h"

#include "./thirdparty/douglas-peucker/polygon-simplify.h"

using namespace godot;

GranularSimulation::GranularSimulation() {
    initialize_grid(&particles);
    fill_id_pool(&idPool);
}

GranularSimulation::~GranularSimulation() {}

void GranularSimulation::initialize_grid(std::vector<Particle*> *ps) {
    ps->resize(width*height);
    for (int i = 0; i < width*height; i++) {
        ps->at(i) = new Nothing();
    }

    render_data = PackedByteArray();
    render_data.resize(width * height * 3);
}

void GranularSimulation::fill_id_pool(std::unordered_set<int> *idPool) {
    idPool->clear();
    for (int i = 0; i <= width * height; i++) {
        idPool->insert(i);
    }
}

void GranularSimulation::step(int iterations) {
    for (int i = 0; i < iterations; i++) {
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                particles[row * width + col]->set_is_updated(false);
                particles[row * width + col]->set_is_moving(false);
            }
        }
        for (int row = height-1; row >=0; row--) {
            // fix left to right bias
            bool left_to_right = (randf() < 0.5f);
            if (left_to_right) {
                for (int col = 0; col < width; col++) {
                    if (!particles[row * width + col]->get_is_updated())
                        particles[row * width + col]->update(this, row, col);
                }
            }
            else {
                for (int col = width - 1; col >= 0; col--) {
                    if (!particles[row * width + col]->get_is_updated())
                        particles[row * width + col]->update(this, row, col);
                }
            }

        }
    }

    // disable outlines detection, as for now i don't think i need them
    // std::vector<MarchingSquares::Result> rs = MarchingSquares::FindPerimeters(width, height, 16, data);
    // pack_outlines(rs);
    // simplify_outlines();
}

void GranularSimulation::create_particle(int row, int col, int typeID) {
    if (!is_in_bounds(row, col))
        return;

    delete particles[row * width + col];
    particles[row * width + col] = new Sand();
    assing_id(particles[row * width + col]);
}

void GranularSimulation::destroy_particle(int row, int col) {
    if (!is_in_bounds(row, col))
        return;

    if (particles[row * width + col]->id > -1) {
        remove_id(particles[row * width + col]);
        delete particles[row * width + col];
        particles[row * width + col] = new Nothing();
    }
}

void GranularSimulation::assing_id(Particle* particle) {
    particle->id = *idPool.begin();
    idPool.erase(particle->id);
}

void GranularSimulation::remove_id(Particle* particle) {
    idPool.insert(particle->id);
    particle->id = -1;
}

void GranularSimulation::swap(int rowA, int colA, int rowB, int colB) {
    if (!is_in_bounds(rowA, colA) || !is_in_bounds(rowB, colB))
        return;

    Particle *tempP = particles[rowA * width + colA];
    particles[rowA * width + colA] = particles[rowB * width + colB];
    particles[rowB * width + colB] = tempP;
    tempP = NULL;

    particles[rowA * width + colA]->set_is_updated(true);
    particles[rowB * width + colB]->set_is_updated(true);

    particles[rowA * width + colA]->set_is_moving(true);
    particles[rowB * width + colB]->set_is_moving(true);

}

bool GranularSimulation::is_in_bounds(int row, int col) {
    return row >= 0 && col >= 0 && row < height && col < width;
}

bool GranularSimulation::is_swappable(int rowA, int colA, int rowB, int colB) {
    if (!is_in_bounds(rowA, colA) || !is_in_bounds(rowB, colB))
        return false;

    if (particles[rowA * width + colA]->get_density() <= particles[rowB * width + colB]->get_density())
        return false;

    return true;
}

inline float GranularSimulation::randf() {
    g_seed = (214013 * g_seed + 2531011);
    return ((g_seed>>16) & 0x7FFF) / (double) 0x7FFF;
}

Vector2i GranularSimulation::get_dimensions() {
    Vector2i dimensions = Vector2i(width, height);
    return dimensions;
}

PackedByteArray GranularSimulation::get_render_data() {
    for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
            int idx = (x * width + y) * 3;
            int id = particles[x * width + y]->id;

            render_data.set(idx, particles[x * width + y]->type);
            // convert id to b and g channels
            render_data.set(idx + 1, id / width);
            render_data.set(idx + 2, id / height);

            // // use color data
            // uint32_t col = particles[x * width + y]->get_color();
            // // convert hex to rgb

            // render_data.set(idx, (col & 0xFF0000) >> 16);
            // render_data.set(idx + 1, (col & 0x00FF00) >> 8);
            // render_data.set(idx + 2, col & 0x0000FF);
        }
    }
    return render_data;
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
    // UtilityFunctions::print(outlines.size());
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
    ClassDB::bind_method(D_METHOD("draw_particle"), &GranularSimulation::create_particle);
    ClassDB::bind_method(D_METHOD("get_dimensions"), &GranularSimulation::get_dimensions);
    ClassDB::bind_method(D_METHOD("get_render_data"), &GranularSimulation::get_render_data);
    ClassDB::bind_method(D_METHOD("get_outlines"), &GranularSimulation::get_outlines);
    ClassDB::bind_method(D_METHOD("get_simplified_outlines"), &GranularSimulation::get_simplified_outlines);
    ClassDB::bind_method(D_METHOD("is_in_bounds"), &GranularSimulation::is_in_bounds);
}
