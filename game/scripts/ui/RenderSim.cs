using Godot;
using System;

public partial class RenderSim : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Common.main.UpdateSim += Repaint;
	}

	public override void _Process(double delta)
	{
		// Repaint();
	}
	public void Repaint()
	{
		var width = Common.main.GetDimensions().X;
		var height = Common.main.GetDimensions().Y;
		byte[] data = Common.main.GetRenderData();

		var material = (ShaderMaterial)Material;
		var texture = ImageTexture.CreateFromImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		material.SetShaderParameter("in_texture", texture);
	}
}
