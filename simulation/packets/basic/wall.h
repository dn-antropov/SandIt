#pragma once

#include "../packet.h"
#include "../packet_types.h"

class Wall: public Packet {

public:
    Wall() {
        type = PacketType::EWall;
        density = 999;
    }

    void update(GranularSimulation *sim, int row, int col) override {}
};