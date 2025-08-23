#pragma once

class GranularSimulation;

class Packet {
public:
    int id = -1;
    int type = -1;
    double density = 0;

private:
    bool isUpdated = false;
    bool isMoving = false;

public:
    GranularSimulation *sim;
    virtual void update(GranularSimulation *sim, int row, int col) = 0;

    double get_density() {
        return density;
    };

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