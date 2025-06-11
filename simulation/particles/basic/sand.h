#pragma once

#include "../particle.h"

class Sand: public Particle {

private:
    uint32_t color = 0xf1c232;

public:
    void update(GranularSimulation *sim, int row, int col) override {
        // color = 0xf1c232;
        bool can_move_left = sim->is_swappable(row, col, row + 1, col - 1);
        bool can_move_down = sim->is_swappable(row, col, row + 1, col);
        bool can_move_right = sim->is_swappable(row, col, row + 1, col + 1);

        if (can_move_down) {
            sim->swap(row, col, row + 1, col);
        } else if (can_move_left && can_move_right) {
            sim->swap(row, col, row + 1, col + (sim->randf() < 0.5 ? 1 : -1));
        } else if (can_move_left) {
            sim->swap(row, col, row + 1, col - 1);
        } else if (can_move_right) {
            sim->swap(row, col, row + 1, col + 1);
        } else {
            // used for debugging
            // color = 0xF44336;
        }
    }

    double get_density() override {
        return 2.0;
    }

    uint32_t get_color() override {
        return color;
    }

};