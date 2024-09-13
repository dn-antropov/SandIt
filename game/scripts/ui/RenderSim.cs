using Godot;
using System;

public partial class RenderSim : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Texture = ImageTexture.CreateFromImage(Image.CreateEmpty(256,256,false,Image.Format.Rgb8));
	}

	public override void _Process(double delta){
		Repaint();
	}
	public void Repaint() {
		var width = Common.main.GetDimensions().X;
		var height = Common.main.GetDimensions().Y;
		byte[] data = Common.main.GetRenderData();

		//testing 
		// byte[] data = new byte[width * height * 3];
		// for (int y = 0; y < height; y++) {
		// 	for (int x = 0; x < width; x++) {
		// 		//uint32_t col = particles[x * width + y]->get_color();
		// 		UInt32 col = 0x0000FF;
				
		// 		int idx = (y * width + x) * 3;
		// 		//convert hex to rgb
		// 		data[idx] = Convert.ToByte((col & 0xFF0000) >> 16);
		// 		data[idx + 1] = Convert.ToByte((col & 0x00FF00) >> 8);
		// 		data[idx + 2] =  Convert.ToByte(col & 0x0000FF);
		// 	}
   		// }

		Texture = ImageTexture.CreateFromImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
	}
}
