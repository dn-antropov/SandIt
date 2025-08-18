/*
	Based on https://github.com/kiwijuice56/sand-slide
*/

#pragma once

#include <unordered_set>
#include <godot_cpp/classes/ref.hpp>
#include <godot_cpp/variant/vector2i.hpp>
#include "./particles/particle.h"

#include "./thirdparty/cpp-marching-squares/MarchingSquares.h"

using namespace godot;
using namespace MarchingSquares;

class GranularSimulation : public RefCounted {
	GDCLASS(GranularSimulation, RefCounted)


private:
	std::vector<Particle*> particles;
	std::unordered_set<int> idPool;
	PackedByteArray render_data;
	TypedArray<PackedVector2Array> outlines;
	TypedArray<PackedVector2Array> simplified_outlines;

public:
	unsigned int g_seed = 12345;

	int width = 128;
    int height = 128;

protected:
	static void _bind_methods();

public:
	GranularSimulation();
	~GranularSimulation();

	void initialize_grid(std::vector<Particle*> *ps);
	void step(int iterations);

	void create_particle(int row, int col, int typeID);
	void destroy_particle(int row, int col);
	void fill_id_pool(std::unordered_set<int> *idPool);
	void assing_id(Particle* particle);
	void remove_id(Particle* particle);
	void swap(int rowA, int colA, int rowB, int colB);
	bool is_in_bounds(int row, int col);
	bool is_swappable(int rowA, int colA, int rowB, int colB);

	float randf();

	void pack_outlines(std::vector<MarchingSquares::Result> results);
	void simplify_outlines();

	Vector2i get_dimensions();
	TypedArray<PackedVector2Array> get_outlines();
	TypedArray<PackedVector2Array> get_simplified_outlines();
	PackedByteArray get_render_data();
};