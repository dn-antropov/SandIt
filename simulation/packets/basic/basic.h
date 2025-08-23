#pragma once

#include "../../granular_simulation.h"
#include "../packet.h"
#include "../packet_types.h"

class Basic: public Packet {

public:
    Basic() {
        type = PacketType::EBasic;
        density = 1;
    }
    void update(GranularSimulation *sim, int row, int col) override {
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
        }
    }
};