/*
	Based on https://github.com/kiwijuice56/sand-slide
*/

#pragma once

#include <unordered_set>
#include <godot_cpp/classes/ref.hpp>
#include <godot_cpp/variant/vector2i.hpp>

#include "./packets/packet.h"

#include "./thirdparty/cpp-marching-squares/MarchingSquares.h"

using namespace godot;
using namespace MarchingSquares;

class GranularSimulation : public RefCounted {
	GDCLASS(GranularSimulation, RefCounted)


private:
	std::vector<Packet*> packets;
	std::unordered_set<int> idPool;
	PackedByteArray render_data;
	PackedByteArray position_data;
	TypedArray<PackedVector2Array> outlines;
	TypedArray<PackedVector2Array> simplified_outlines;

public:
	unsigned int g_seed = 12345;

	int width = 64;
    int height = 64;

protected:
	static void _bind_methods();

public:
	GranularSimulation();
	~GranularSimulation();

	void initialize_grid(std::vector<Packet*> *ps);
	void step(int iterations);

	void create_packet(int row, int col, int type);
	int destroy_packet(int row, int col);
	void fill_id_pool(std::unordered_set<int> *idPool);
	void assign_id(Packet* packet);
	void remove_id(Packet* packets);
	void swap(int rowA, int colA, int rowB, int colB);
	bool is_in_bounds(int row, int col);
	bool is_swappable(int rowA, int colA, int rowB, int colB);
	void process_block(int start_row, int start_col, int block_size);

	float randf();

	void pack_outlines(std::vector<MarchingSquares::Result> results);
	void simplify_outlines();

	Vector2i get_dimensions();
	TypedArray<PackedVector2Array> get_outlines();
	TypedArray<PackedVector2Array> get_simplified_outlines();
	PackedByteArray get_render_data();
	PackedByteArray get_interpolated_render_data(float alpha, int render_scale);
};