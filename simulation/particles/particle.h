//Basic particle class every element inherits from

#pragma once

class GranularSimulation;

#include "../granular_simulation.h"

class Particle {

private:
    bool isUpdated = false;

public:
    GranularSimulation *sim;
    virtual void update(GranularSimulation *sim, int row, int col) = 0;

    virtual double get_density() = 0;

    virtual uint32_t get_color() = 0;

    bool get_is_updated() {
        return isUpdated;
    };

    void set_is_updated(bool newIsUpdated) {
        isUpdated = newIsUpdated;
    };
};