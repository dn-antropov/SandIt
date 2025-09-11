using Godot;
using System;

public partial class RenderSim : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	private bool idsPrinted = false;

	int width = -1;
	int height = -1;

	float stepTime = 0;
	float alpha = 0;
	ShaderMaterial material;

	ImageTexture texture;
	ImageTexture prevPositionsTexture;

	public override void _Ready()
	{
		Common.main.UpdateSim += RepaintTextures;
		material = (ShaderMaterial)Material;

		texture = new ImageTexture();
		// prevPositionsTexture = new ImageTexture();
	}

	public override void _Process(double delta)
	{
		if (width == -1 || height == -1)
		{
			width = Common.main.GetDimensions().X;
			height = Common.main.GetDimensions().Y;
		}
		stepTime += (float)delta;
		alpha = stepTime / (float)Common.main.timestep;
		alpha = Math.Clamp(alpha, 0.0F, 1.0F);

		// if (Time.GetUnixTimeFromSystem() % 2 < 0.1) // Print every ~2 seconds
		// {
		// 	GD.Print($"Alpha: {alpha:F3}, StepTime: {stepTime:F3}, Timestep: {Common.main.timestep:F3}");
		// }
	    // Generate interpolated texture every frame
		byte[] interpolatedData = Common.main.GetInterpolatedRenderData(alpha, (int)Common.pixelScale);

		int renderWidth = width * (int)Common.pixelScale;
		int renderHeight = height * (int)Common.pixelScale;
		int expectedSize = renderWidth * renderHeight * 3;

		if (interpolatedData.Length != expectedSize)
		{
			GD.PrintErr($"Data size mismatch: got {interpolatedData.Length}, expected {expectedSize}");
			return;
		}

		texture.SetImage(Image.CreateFromData(renderWidth, renderHeight, false, Image.Format.Rgb8, interpolatedData));

		material.SetShaderParameter("in_texture", texture);
	}
	public void RepaintTextures()
	{


		// byte[] data = Common.main.GetRenderData();
		// byte[] positions = Common.main.GetInterpolatedRenderData(0, (int)Common.pixelScale);

		// texture.SetImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		// prevPositionsTexture.SetImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, positions));
		// material.SetShaderParameter("in_texture", texture);
		stepTime = 0;
	}
}
