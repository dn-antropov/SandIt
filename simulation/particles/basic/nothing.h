#pragma once

#include "../particle.h"

class Nothing: public Particle {
    void update(GranularSimulation *sim, int row, int col) override {}

    double get_density() override {
        return 0.0;
    }

    uint32_t get_color() override {
        return 0x181818;
    }
};