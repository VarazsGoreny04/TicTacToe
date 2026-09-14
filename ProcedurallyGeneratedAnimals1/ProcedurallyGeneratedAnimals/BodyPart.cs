namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes a body part of a creature.
/// </summary>
public abstract class BodyPart
{
	protected Segment segment;
	protected Render render;
	protected Color color;

	public Segment Segment { get => segment; set => segment = value; }
	public Render Render => render;
	public Color Color => color;

	/// <summary>
	/// Creates a BodyPart object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="color">Color of the bodyPart.</param>
	public BodyPart(Segment segment, Render render, Color color)
	{
		this.segment = segment;
		this.render = render;
		this.color = color;
	}

	/// <summary>
	/// Draws this body part instance.
	/// </summary>
	public abstract void Draw();
}

/// <summary>
/// Describes one pair of eyes of a creature.
/// </summary>
public class Eye : BodyPart
{
	protected double radianToFront;
	protected int distanceToOrigin;
	protected int radius;

	public double RadianToFront => radianToFront;
	public int DistanceToOrigin => distanceToOrigin;
	public int Radius => radius;

	/// <summary>
	/// Creates an Eye object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="angleToFront">The angle of the eye from the front vector of the segment.</param>
	/// <param name="distanceToOrigin">The distance of the eye from the center of the segment.</param>
	/// <param name="radius">Radius of the eye.</param>
	/// <param name="color">Color of the eye.</param>
	public Eye(Segment segment, Render render, double angleToFront, int distanceToOrigin, int radius, Color color) : base(segment, render, color)
	{
		radianToFront = double.DegreesToRadians(angleToFront);
		this.distanceToOrigin = distanceToOrigin;
		this.radius = radius;
	}

	/// <summary>
	/// Draws this eye instance.
	/// </summary>
	public override void Draw()
	{
		Point frontScaled = Point.Scale(Segment.GetFrontVector(segment), distanceToOrigin);

		Point eyePoint = Point.Add(segment.Origin, Point.RotateRadian(frontScaled, radianToFront));
		Animal.OnDrawEllipse(new Point(radius, radius), [new Translate(eyePoint)], color);

		Point eyePointMirrored = Point.Add(segment.Origin, Point.RotateRadian(frontScaled, -radianToFront));
		Animal.OnDrawEllipse(new Point(radius, radius), [new Translate(eyePointMirrored)], color);
	}
}

/// <summary>
/// Describes a pair of fins of a creature.
/// </summary>
public class SideFin : BodyPart
{
	protected int length;
	protected int width;
	protected double angle;

	public int Length => length;
	public int Width => width;
	public double Angle => angle;

	public SideFin(Segment segment, Render render, int length, int width, double angle, Color color) : base(segment, render, color)
	{
		this.length = length;
		this.width = width;
		this.angle = angle;
	}

	/// <summary>
	/// Draws an ellipse.
	/// </summary>
	/// <param name="position">The position of the ellipse.</param>
	/// <param name="angle">The angle of the ellipse.</param>
	/// <param name="width">The width of the ellipse.</param>
	/// <param name="length">The height of the ellipse.</param>
	public static void DrawEllipseByOrientation(Point position, int width, int length, double angle, Color color)
	{
		Animal.OnDrawEllipse(new Point(width, length / 2), [new Rotate(angle), new Translate(position)], color);
	}

	/// <summary>
	/// Draws this fin instance.
	/// </summary>
	public override void Draw()
	{
		Point front = Segment.GetFrontVector(segment);
		double frontAngle = Point.AngleOfVector(Point.NormalRight(front));

		Point originOne = Point.Add(segment.Origin, Point.NormalLeft(front));
		DrawEllipseByOrientation(originOne, width, length, frontAngle - angle, color);

		Point originTwo = Point.Add(segment.Origin, Point.NormalRight(front));
		DrawEllipseByOrientation(originTwo, width, length, frontAngle + angle, color);
	}
}

/// <summary>
/// Describes the back fin of a creature.
/// </summary>
public class BackFin : BodyPart
{
	protected int lengthInSegments;

	public int LengthInSegments => lengthInSegments;

	/// <summary>
	/// Creates a BackFin object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="lengthInSegments">The number of segments the fin will go through.</param>
	/// <param name="color">Color of the body part.</param>
	public BackFin(Segment segment, Render render, int lengthInSegments, Color color) : base(segment, render, color)
	{
		if (lengthInSegments < 2)
			throw new ArgumentException("A backfin must have a length of 2 or more!", nameof(lengthInSegments));

		this.lengthInSegments = lengthInSegments;
	}

	/// <summary>
	/// Calculates the outline points of the fin.
	/// </summary>
	/// <param name="fin">The fin to calculate with.</param>
	/// <returns>The calculated points.</returns>
	public static Point[] GetPoints(BackFin fin)
	{
		List<Point> points = [];

		int counter = 0;
		foreach (Segment nextSegment in fin.segment)
		{
			if (counter > fin.lengthInSegments)
				break;

			points.Add(nextSegment.Origin);
			++counter;
		}

		double angle = Point.SinOfPoints(points[^3], points[^2], points[^1]);

		for (int index = points.Count - 1; index > 0; --index)
		{
			Point topPoint = Point.NormalRight(Point.Subtract(points[index - 1], points[index]));
			points.Add(Point.Add(points[index], Point.Multiply(topPoint, angle)));
		}

		return [.. points];
	}

	/// <summary>
	/// Draws this fin instance.
	/// </summary>
	public override void Draw() => Animal.OnDrawBezierLine(GetPoints(this), [], color);
}

/// <summary>
/// Describes the tail fin of a creature.
/// </summary>
public class TailFin : BodyPart
{
	protected Segment headJoint;

	public Segment HeadJoint => headJoint;

	/// <summary>
	/// Creates a TailFin object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="distances">The distances between the segments of the fin.</param>
	/// <param name="color">Color of the body part.</param>
	public TailFin(Segment segment, Render render, int[] distances, Color color) : base(segment, render, color)
	{
		if (distances.Length < 2)
			throw new ArgumentException("A tailfin must have at least 2 distance descriptors!", nameof(distances));

		List<SegmentDescriptor> descriptors = [new SegmentDescriptor(1, 1)];
		foreach (int distance in distances)
			descriptors.Add(new SegmentDescriptor(distance, 1));

		headJoint = Segment.CreateAndLink(segment.Origin, [.. descriptors]);
	}

	/// <summary>
	/// Calculates the outline points of the fin.
	/// <param name="fin">The fin to calculate with.</param>
	/// <returns>The calculated points.</returns>
	public static Point[] GetPoints(TailFin fin)
	{
		List<Point> points = [];

		foreach (Segment nextSegment in fin.headJoint)
			points.Add(nextSegment.Origin);

		double angle = Point.SinOfPoints(points[^3], points[^2], points[^1]);
		double magicMultiplier = (13 * angle) / (points.Count - 1);

		for (int index = points.Count - 1; index > 0; --index)
		{
			Point topPoint = Point.NormalRight(Point.Subtract(points[index - 1], points[index]));
			points.Add(Point.Add(points[index], Point.Multiply(topPoint, index * magicMultiplier)));
		}

		return [.. points];
	}

	/// <summary>
	/// Draws this fin instance.
	/// </summary>
	public override void Draw()
	{
		headJoint.Origin = segment.Origin;
		Segment.PullNext(headJoint);

		Animal.OnDrawBezierLine(GetPoints(this), [], color);
	}
}

/// <summary>
/// Describes a pair of antennas of a creature.
/// </summary>
public class Antenna : BodyPart
{
	protected Point[] points;
	protected Point[] pointsMirrored;
	protected double angle;

	public Point[] Points => points;
	public Point[] PointsMirrored => pointsMirrored;
	public double Angle => angle;

	/// <summary>
	/// Creates a TailFin object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="descriptors">The descriptors of the segments of the antenna.</param>
	/// <param name="angle">The angle of the antennas.</param>
	/// <param name="color">Color of the body part.</param>
	public Antenna(Segment segment, Render render, SegmentDescriptor[] descriptors, double angle, Color color) : base(segment, render, color)
	{
		points = Segment.GetPoints(Segment.CreateAndLink(new Point(0, 0), descriptors));

		if (Math.Abs(this.angle) < 1)
			pointsMirrored = [];
		else
		{
			pointsMirrored = new Point[points.Length];

			for (int i = points.Length - 1; i >= 0; --i)
				pointsMirrored[i] = (new Point(points[i].X, -points[i].Y));
		}

		this.angle = angle;
	}

	/// <summary>
	/// Draws a loop.
	/// </summary>
	/// <param name="position">The position of the loop.</param>
	/// <param name="angle">The angle of the loop.</param>
	/// <param name="points">The points of the loop.</param>
	public static void DrawLoopByOrientation(Point position, Point[] points, double angle, Color color)
	{
		Animal.OnDrawBezierLine(points, [new Rotate(angle), new Translate(position)], color);
	}

	/// <summary>
	/// Draws this antenna instance.
	/// </summary>
	public override void Draw()
	{
		double bodyAngle = Point.AngleOfVector(Point.Reverse(Segment.GetFrontVector(segment)));

		DrawLoopByOrientation(segment.Origin, points, bodyAngle + angle, color);
		DrawLoopByOrientation(segment.Origin, pointsMirrored, bodyAngle - angle, color);
	}
}

/// <summary>
/// Describes one leg of a creature.
/// </summary>
public class OneLeg
{
	protected Segment headSegment;
	protected Segment tailSegment;
	protected Point standsOn;
	protected double range;

	public Segment HeadSegment => headSegment;
	public Segment TailSegment => tailSegment;
	public Point StandsOn { get => standsOn; set => standsOn = value; }
	public double Range => range;

	/// <summary>
	/// Creates a OneLeg object.
	/// </summary>
	/// <param name="origin">The origin of the parent segment.</param>
	/// <param name="descriptors">The descriptors of the segments of the leg.</param>
	public OneLeg(Point origin, SegmentDescriptor[] descriptors)
	{
		if (descriptors.Length < 2)
			throw new ArgumentException("A leg must have at least 2 segment descriptors!", nameof(descriptors));

		headSegment = Segment.CreateAndLink(origin, descriptors);

		Segment tail = headSegment;
		foreach (Segment nextSegment in headSegment)
			tail = nextSegment;

		tailSegment = tail;
		standsOn = tailSegment.Origin;

		range = Point.Distance(tailSegment.Origin, headSegment.Origin);
	}

	/// <summary>
	/// Gets a new step location for the given leg.
	/// </summary>
	/// <param name="leg">The leg to get a new target for.</param>
	/// <param name="frontVector">The normalized front vector of the parent segment.</param>
	/// <param name="normalVector">The normalized normal vector of the parent segment pointing towards the legs direction.</param>
	/// <param name="stepStyler">The direction vector to calculate the location of the next step.</param>
	/// <returns>The new location to step to.</returns>
	public static Point GetNewTarget(OneLeg leg, Point frontVector, Point normalVector, Point stepStyler)
	{
		Point toSide = Point.Multiply(normalVector, stepStyler.X);
		Point toFront = Point.Multiply(frontVector, stepStyler.Y);
		Point direction = Point.Add(toSide, toFront);

		return Point.Add(leg.headSegment.Origin, Point.Magnitude(direction) > leg.range ? Point.Scale(direction, leg.range) : direction);
	}

	/// <summary>
	/// Performs two way inverse kinematics on the given leg.
	/// </summary>
	/// <param name="leg">The given leg.</param>
	public static void TwoWayKinematics(OneLeg leg)
	{
		Point joinPoint = leg.headSegment.Origin;

		leg.tailSegment.Origin = leg.standsOn;
		Segment.PullPrev(leg.tailSegment);

		leg.headSegment.Origin = joinPoint;
		Segment.PullNext(leg.headSegment);
	}

	/// <summary>
	/// Mirrors the position of the points of the leg around a certain point.
	/// </summary>
	/// <param name="origin">The point to mirror around.</param>
	/// <param name="leg">The given leg.</param>
	public static void Break(Point origin, OneLeg leg)
	{
		foreach (Segment segment in leg.headSegment)
			segment.Origin = Point.Subtract(Point.Multiply(origin, 2), segment.Origin);
	}

	/// <summary>
	/// Draws a OneLeg instance.
	/// </summary>
	/// <param name="leg">The leg to draw.</param>
	/// <param name="color">The color of the leg.</param>
	public static void Draw(OneLeg leg, Color color)
	{
		TwoWayKinematics(leg);

		double distanceFromTarget = Point.Magnitude(Point.Subtract(leg.standsOn, leg.tailSegment.Origin));

		if (distanceFromTarget > leg.tailSegment.DistanceFromPrev)
		{
			Break(leg.headSegment.Origin, leg);

			for (int i = 0; i < 5; ++i)
				TwoWayKinematics(leg);
		}

		foreach (Segment segment in leg.headSegment)
			Segment.DrawBodyParts(segment, Render.Bottom);

		Animal.OnDrawBezierLine(Segment.GetPoints(leg.headSegment), [], color);

		foreach (Segment segment in leg.headSegment)
			Segment.DrawBodyParts(segment, Render.Top);
	}
}

/// <summary>
/// Describes one pair of legs of a creature.
/// </summary>
public class Leg : BodyPart
{
	protected OneLeg left;
	protected OneLeg right;
	protected Point stepTo;

	/// <summary>
	/// Creates a Leg object.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="render">Where to render.</param>
	/// <param name="descriptors">The descriptors of the segments of one leg.</param>
	/// <param name="stepTo">Point to step on.</param>
	/// <param name="color">The color of the legs.</param>
	public Leg(Segment segment, Render render, LegSegmentDescriptor[] descriptors, Point stepTo, Color color) : base(segment, render, color)
	{
		left = new OneLeg(segment.Origin, descriptors);

		List<SegmentDescriptor> mirroredDescriptors = [];
		foreach (LegSegmentDescriptor descriptor in descriptors)
			mirroredDescriptors.Add(LegSegmentDescriptor.Mirror(descriptor));
		right = new OneLeg(segment.Origin, [.. mirroredDescriptors]);

		this.stepTo = stepTo;
	}

	/// <summary>
	/// Draws one leg.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <param name="frontVector">The normalized front vector of the parent segment.</param>
	/// <param name="normalVector">The normalized normal vector of the parent segment pointing towards the legs direction.</param>
	/// <param name="leg">The leg to draw.</param>
	/// <param name="color">The color of the leg.</param>
	/// <param name="stepTo">Point to step on.</param>
	public static void DrawOne(Segment segment, Point frontVector, Point normalVector, OneLeg leg, Color color, Point stepTo)
	{
		leg.HeadSegment.Origin = Point.Add(segment.Origin, Point.Scale(normalVector, leg.HeadSegment.DistanceFromPrev));

		double distanceFromTarget = Point.Distance(leg.StandsOn, leg.HeadSegment.Origin);
		double bodyLegAngle = Math.Abs(Point.AngleOfVectors(frontVector, Point.Subtract(leg.HeadSegment.Origin, leg.HeadSegment.NextSegment.Origin)));

		if (distanceFromTarget > leg.Range || bodyLegAngle < 30)
			leg.StandsOn = OneLeg.GetNewTarget(leg, frontVector, normalVector, stepTo);

		OneLeg.Draw(leg, color);
	}

	/// <summary>
	/// Draws this leg instance.
	/// </summary>
	public override void Draw()
	{
		Point normalizedFrontVector = Point.Normalize(Segment.GetFrontVector(segment));

		DrawOne(segment, normalizedFrontVector, Point.NormalRight(normalizedFrontVector), left, color, stepTo);
		DrawOne(segment, normalizedFrontVector, Point.NormalLeft(normalizedFrontVector), right, color, stepTo);
	}
}