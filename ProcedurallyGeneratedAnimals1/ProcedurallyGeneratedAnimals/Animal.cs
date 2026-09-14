namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes an animal.
/// </summary>
public class Animal
{
	protected Segment headSegment;
	protected Color bodyColor;
	protected int speed;

	public Segment HeadSegment => headSegment;
	public Color BodyColor => bodyColor;
	public int Speed => speed;

	public static event EventHandler<EllipseEventArgs>? DrawEllipse;
	public static event EventHandler<BezierLineEventArgs>? DrawBezierLine;

	/// <summary>
	/// Creates an Animal object.
	/// </summary>
	/// <param name="headPosition">The head starting position.</param>
	/// <param name="descriptors">The descriptors of the body of the animal.</param>
	/// <param name="bodyColor">The color of the animals body.</param>
	/// <param name="speed">The speed of the animal.</param>
	public Animal(Point headPosition, SegmentDescriptor[] descriptors, Color bodyColor, int speed)
	{
		if (descriptors.Length < 2)
			throw new ArgumentException("An animal must have at least 2 segments!", nameof(descriptors));

		headSegment = Segment.CreateAndLink(headPosition, descriptors);

		this.bodyColor = bodyColor;
		this.speed = speed;
	}

	internal static void OnDrawEllipse(Point dimensions, Transform[] transforms)
	{
		DrawEllipse?.Invoke(null, new EllipseEventArgs(dimensions, transforms));
	}

	internal static void OnDrawEllipse(Point dimensions, Transform[] transforms, Color color)
	{
		DrawEllipse?.Invoke(null, new EllipseEventArgs(dimensions, transforms, color));
	}

	internal static void OnDrawBezierLine(Point[] points, Transform[] transforms)
	{
		DrawBezierLine?.Invoke(null, new BezierLineEventArgs(points, transforms));
	}

	internal static void OnDrawBezierLine(Point[] points, Transform[] transforms, Color color)
	{
		DrawBezierLine?.Invoke(null, new BezierLineEventArgs(points, transforms, color));
	}

	/// <summary>
	/// Draws a line on the spine of the animal.
	/// </summary>
	/// <param name="animal">The animal.</param>
	public static void DrawSpine(Animal animal)
	{
		List<Point> points = [];
		foreach (Segment segment in animal.headSegment)
			points.Add(segment.Origin);

		OnDrawBezierLine([.. points], []);
	}

	/// <summary>
	/// Draws a circle to every segment of the body.
	/// </summary>
	/// <param name="animal">The animal.</param>
	public static void DrawCircles(Animal animal)
	{
		foreach (Segment segment in animal.headSegment)
			OnDrawEllipse(new Point(segment.SkinRadius, segment.SkinRadius), [new Translate(segment.Origin)]);
	}

	/// <summary>
	/// Draws the outline of the animal.
	/// </summary>
	/// <param name="animal">The animal.</param>
	public static void DrawOutline(Animal animal) => OnDrawBezierLine(Segment.GetPoints(animal.headSegment), [], animal.bodyColor);

	/// <summary>
	/// Draws this animal instance.
	/// </summary>
	public void Draw()
	{
		foreach (Segment segment in headSegment)
			Segment.DrawBodyParts(segment, Render.Bottom);

		DrawOutline(this);
		// DrawCircles(this);
		// DrawSpine(this);

		foreach (Segment segment in headSegment)
			Segment.DrawBodyParts(segment, Render.Top);
	}

	/// <summary>
	/// Moves this animal instance to the given direction.
	/// </summary>
	/// <param name="destination">The given direction.</param>
	public void Step(Point destination)
	{
		Point vectorToDestination = Point.Subtract(destination, headSegment.Origin);

		if (Point.Magnitude(vectorToDestination) < speed)
			return;

		Point direction = Point.Scale(vectorToDestination, speed);

		Point restrictedDirection = headSegment.NextSegment is not null ? Segment.RestrictAngleOfRotation(headSegment, headSegment.NextSegment, direction) : direction;

		headSegment.Origin = Point.Add(headSegment.Origin, restrictedDirection);
		Segment.PullNext(headSegment);
	}
}