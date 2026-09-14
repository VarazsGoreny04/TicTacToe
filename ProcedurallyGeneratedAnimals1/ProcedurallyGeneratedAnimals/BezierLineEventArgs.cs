namespace ProcedurallyGeneratedAnimals;

public class BezierLineEventArgs
{
	protected Point[] points;
	protected Transform[] transforms;
	protected Color color;

	public Point[] Points => points;
	public Transform[] Transforms => transforms;
	public Color Color => color;

	public BezierLineEventArgs(Point[] points, Transform[] transforms, Color color)
	{
		this.points = points;
		this.transforms = transforms;
		this.color = color;
	}

	public BezierLineEventArgs(Point[] points, Transform[] transforms) : this(points, transforms, new Color(0, 0, 0, 0)) { }
}