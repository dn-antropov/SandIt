using Godot;
using System;

public partial class RenderSim : TextureRect
{

	[Export] public PackedScene spriteScene;

	private Node2D spriteContainer;
	private Node2D[] sprites;
	// Called when the node enters the scene tree for the first time.
	private bool idsPrinted = false;
	private bool spritesInitialized = false;
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

		if (spriteContainer == null)
		{
			spriteContainer = new Node2D();
			spriteContainer.Name = "SpriteContainer";
			AddChild(spriteContainer);
		}
	}

	public override void _Process(double delta)
	{
		if (width == -1 || height == -1)
		{
			width = Common.main.GetDimensions().X;
			height = Common.main.GetDimensions().Y;
		}
		CreateSprites();

		stepTime += (float)delta;
		alpha = stepTime / (float)Common.main.timestep;
		alpha = Math.Clamp(alpha, 0.0F, 1.0F);

		UpdateSprites(alpha, (int)Common.pixelScale);
	}
	public void RepaintTextures()
	{

		if (width == -1 || height == -1)
		{
			width = Common.main.GetDimensions().X;
			height = Common.main.GetDimensions().Y;
		}

		byte[] data = Common.main.GetRenderData();
		texture.SetImage(Image.CreateFromData(width, height, false, Image.Format.Rgb8, data));
		material.SetShaderParameter("in_texture", texture);
		stepTime = 0;
	}

	private void CreateSprites()
	{
		if (spritesInitialized || width <= 0 || height <= 0)
			return;

		int totalSprites = width * height;
		sprites = new Node2D[totalSprites];
		for (int i = 0; i < totalSprites; i++)
		{
			Node2D newSprite = spriteScene.Instantiate<Node2D>();
			spriteContainer.AddChild(newSprite);

			newSprite.Position = new Vector2(i % width * Common.pixelScale, i / height * Common.pixelScale);
			// Initially hide all sprites
			newSprite.Visible = false;
			sprites[i] = newSprite;

		}

		spritesInitialized = true;
	}

	private void UpdateSprites(float _alpha, int _scale)
	{
		Vector4[] packets = Common.main.GetInterpolatedRenderData(_alpha, _scale);
		foreach (Vector4 packet in packets)
		{
			if (packet.Y <= 0)
			{
				sprites[(int)packet.X].Visible = false;
			}
			else
			{
				sprites[(int)packet.X].Visible = true;
				Packet p = sprites[(int)packet.X] as Packet;
				p.SetType((int)packet.Y);
				sprites[(int)packet.X].Position = new Vector2(packet.Z, packet.W);
			}

		}
	}
}
