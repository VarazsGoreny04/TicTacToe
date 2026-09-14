namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes a color value.
/// </summary>
public class Color
{
	private int r;
	private int g;
	private int b;
	private int a;

	public int R { get => r; set => r = value; }
	public int G { get => g; set => g = value; }
	public int B { get => b; set => b = value; }
	public int A { get => a; set => a = value; }

	/// <summary>Creates a <see cref="Color"/> object.</summary>
	/// <param name="r">The amount of red from 0 to 255.</param>
	/// <param name="g">The amount of green from 0 to 255.</param>
	/// <param name="b">The amount of blue from 0 to 255.</param>
	/// <param name="a">The alpha value from 0 to 255.</param>
	public Color(int r, int g, int b, int a = 255)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}
}