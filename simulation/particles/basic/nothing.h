#pragma once

#include "../particle.h"

class Nothing: public Particle {

public:
    Nothing() {
        type = 0;
    }

    void update(GranularSimulation *sim, int row, int col) override {}

    double get_density() override {
        return 0.0;
    }

    uint32_t get_color() override {
        return 0x000000;
    }
};