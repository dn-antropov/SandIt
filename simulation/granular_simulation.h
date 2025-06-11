/*
	Based on https://github.com/kiwijuice56/sand-slide
*/

#pragma once

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
	PackedByteArray render_data;
	// PackedVector2Array outline;
	// PackedVector2Array simplified_outline;
	TypedArray<PackedVector2Array> outlines;
	TypedArray<PackedVector2Array> simplified_outlines;

public:
	unsigned int g_seed = 12345;

	int width = 256;
    int height = 256;

protected:
	static void _bind_methods();

public:
	GranularSimulation();
	~GranularSimulation();
//System lifecycle
	void initialize_grid(std::vector<Particle*> *ps);
	void step(int iterations);

//Particle lifecycle
	void draw_particle(int row, int col, int typeID);
	void swap(int rowA, int colA, int rowB, int colB);
	bool is_in_bounds(int row, int col);
	bool is_swappable(int rowA, int colA, int rowB, int colB);

	float randf();

//Oulines
	void pack_outlines(std::vector<MarchingSquares::Result> results);
	void simplify_outlines();

//API
	Vector2i get_dimensions();
	TypedArray<PackedVector2Array> get_outlines();
	TypedArray<PackedVector2Array> get_simplified_outlines();
	PackedByteArray get_render_data();
};