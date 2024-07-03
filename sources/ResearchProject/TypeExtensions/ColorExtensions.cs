using Godot;
using System;

public static class ColorExtensions
{
	public static Color GenerateRandomColor(bool randomizeAlpha = false)
	{
		float red = (float)Random.Shared.NextDouble(),
			  green = (float)Random.Shared.NextDouble(),
			  blue = (float)Random.Shared.NextDouble(),
			  alpha = randomizeAlpha ? (float)Random.Shared.NextDouble() : 1;

		var color = new Color(red, green, blue, alpha);

		return color;
	}
}