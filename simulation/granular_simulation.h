#pragma once

#include <godot_cpp/classes/ref.hpp>
#include <godot_cpp/variant/vector2i.hpp>
#include "./particles/particle.h"

using namespace godot;

class GranularSimulation : public RefCounted {
	GDCLASS(GranularSimulation, RefCounted)


private:
	std::vector<Particle*> particles;
	PackedByteArray render_data;
	PackedVector2Array outline;

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

//API
	Vector2i get_dimensions();
	PackedVector2Array get_outline();
	PackedByteArray get_render_data();
};