#pragma once

#include "../packet.h"
#include "../packet_types.h"

class Nothing: public Packet {

public:
    Nothing() {
        type = PacketType::ENothing;
        density = 0;
    }

    void update(GranularSimulation *sim, int row, int col) override {}
};