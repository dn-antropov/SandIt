using Godot;
using System;

public partial class RenderSim : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	private bool idsPrinted = false;
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
		if (!idsPrinted)
		{
			for (int i = 0; i < width * height; i++)
			{
				int idx = i * 3;
				int g = data[idx + 1];
				int b = data[idx + 2];
				int id = g + (b << 8);
				// GD.Print($"Pixel {i}: id={id}");
				if (id < 0 || id >= width * height)
				{
					GD.Print($"Pixel {i}: illegal id={id}");
				}
			}
			idsPrinted = true;
		}

		var material = (ShaderMaterial)Material;
		var texture = ImageTexture.CreateFromImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		material.SetShaderParameter("in_texture", texture);
	}
}
