namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes a form of transformation.
/// </summary>
public abstract class Transform { }

/// <summary>
/// Describes a rotation transformation.
/// </summary>
public sealed class Rotate : Transform
{
	private readonly double angle;

	public double Angle => angle;

	/// <summary>
	/// Creates a <see cref="Translate"/> object.
	/// </summary>
	/// <param name="angle">The angle of the rotation.</param>
	public Rotate(double angle) => this.angle = angle;
}

/// <summary>
/// Describes a scaling transformation.
/// </summary>
public sealed class Scale : Transform
{
	private readonly Point value;

	public Point Value => value;

	/// <summary>
	/// Creates a <see cref="Translate"/> object.
	/// </summary>
	/// <param name="value">The value of the scaling.</param>
	public Scale(Point value) => this.value = value;
}

/// <summary>
/// Describes a translation transformation.
/// </summary>
public sealed class Translate : Transform
{
	private readonly Point value;

	public Point Value => value;

	/// <summary>
	/// Creates a <see cref="Translate"/> object.
	/// </summary>
	/// <param name="value">The value of the transformation.</param>
	public Translate(Point value) => this.value = value;
}