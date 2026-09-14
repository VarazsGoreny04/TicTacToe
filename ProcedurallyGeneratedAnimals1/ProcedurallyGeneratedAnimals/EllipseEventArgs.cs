namespace ProcedurallyGeneratedAnimals;

public class EllipseEventArgs
{
	protected Point dimensions;
	protected Transform[] transforms;
	protected Color color;

	public Point Dimensions => dimensions;
	public Transform[] Transforms => transforms;
	public Color Color => color;

	public EllipseEventArgs(Point dimensions, Transform[] transforms, Color color)
	{
		this.dimensions = dimensions;
		this.transforms = transforms;
		this.color = color;
	}

	public EllipseEventArgs(Point dimensions, Transform[] transforms) : this(dimensions, transforms, new Color(0, 0, 0, 0)) { }
}