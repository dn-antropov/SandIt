//Basic particle class every element inherits from

#pragma once

class GranularSimulation;

#include "../granular_simulation.h"

class Particle {
public:
    int id = -1;
    int type = -1;

private:
    bool isUpdated = false;
    bool isMoving = false;

public:
    GranularSimulation *sim;
    virtual void update(GranularSimulation *sim, int row, int col) = 0;

    virtual double get_density() = 0;

    virtual uint32_t get_color() = 0;

    bool get_is_updated() {
        return isUpdated;
    };

    bool get_is_moving() {
        return isMoving;
    }

    void set_is_moving(bool newIsMoving) {
        isMoving = newIsMoving;
    }

    void set_is_updated(bool newIsUpdated) {
        isUpdated = newIsUpdated;
    };
};