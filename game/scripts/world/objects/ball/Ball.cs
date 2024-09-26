using Godot;
using System;


public partial class Ball : RigidBody2D
{
    double  elapsedTime = 0;
    Vector2 initialPosition;

    public override void _Ready()
    {
        base._Ready();
        initialPosition = Position;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        elapsedTime += delta;
    }
    public override void _Draw()
    {
        Color white = Colors.White;
        DrawCircle(new Vector2(0f, 0f), 16f, white);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (elapsedTime > 5) {
            elapsedTime = 0;
            state.Transform = new Transform2D(0,initialPosition);
            state.LinearVelocity = Vector2.Zero;
            state.LinearVelocity = Vector2.Zero;
        }
    }
}
