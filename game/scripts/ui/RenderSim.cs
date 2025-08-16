using Godot;
using System;

public partial class RenderSim : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Texture = ImageTexture.CreateFromImage(Image.CreateEmpty(256,256,false,Image.Format.Rgb8));
	}

	public override void _Process(double delta){
		Repaint();
	}
	public void Repaint()
	{
		var width = Common.main.GetDimensions().X;
		var height = Common.main.GetDimensions().Y;
		byte[] data = Common.main.GetRenderData();

		//Texture = ImageTexture.CreateFromImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		var material = (ShaderMaterial)Material;
		var texture = ImageTexture.CreateFromImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		material.SetShaderParameter("in_texture", texture);
	}
}
