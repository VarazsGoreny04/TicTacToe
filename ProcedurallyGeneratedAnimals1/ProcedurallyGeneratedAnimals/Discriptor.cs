namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes an animal.
/// </summary>
public class AnimalDescriptor
{
	protected SegmentDescriptor[] segmentDescriptors;
	protected Color color;
	protected int speed;

	public SegmentDescriptor[] SegmentDescriptors => segmentDescriptors;
	public Color Color => color;
	public int Speed => speed;

	/// <summary>
	/// Creates an AnimalDescriptor object.
	/// </summary>
	/// <param name="segmentDescriptors">The segments of the animal.</param>
	/// <param name="color">The color of the body.</param>
	/// <param name="speed">The speed of the animal.</param>
	public AnimalDescriptor(SegmentDescriptor[] segmentDescriptors, Color color, int speed)
	{
		this.segmentDescriptors = segmentDescriptors;
		this.color = color;
		this.speed = speed;
	}

	/// <summary>
	/// Creates an <see cref="Animal"/> object by this descriptor.
	/// </summary>
	/// <returns>The <see cref="Animal"/> object.</returns>
	public Animal Create(Point headPosition) => new(headPosition, segmentDescriptors, color, speed);
}

/// <summary>
/// Describes a segment.
/// </summary>
public class SegmentDescriptor
{
	protected int segmentDistance;
	protected int skinRadius;
	protected BodyPartDescriptor[] bodyPartDescriptors;

	public int SegmentDistance => segmentDistance;
	public int SkinRadius => skinRadius;
	public BodyPartDescriptor[] BodyPartDescriptors => bodyPartDescriptors;

	/// <summary>
	/// Creates a SegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="bodyPartDescriptors">The bodyParts of the segment.</param>
	public SegmentDescriptor(int segmentDistance, int skinRadius, BodyPartDescriptor[] bodyPartDescriptors)
	{
		this.segmentDistance = segmentDistance;
		this.skinRadius = Math.Abs(skinRadius);
		this.bodyPartDescriptors = bodyPartDescriptors;
	}

	/// <summary>
	/// Creates a SegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	public SegmentDescriptor(int segmentDistance, int skinRadius) : this(segmentDistance, skinRadius, []) { }

	/// <summary>
	/// Creates a Segment object by this descriptor.
	/// </summary>
	/// <param name="prevOrigin">The origin of the previous segment.</param>
	/// <returns>The Segment object.</returns>
	public virtual Segment Create(Point prevOrigin)
	{
		Segment segment = new(
			new Point(prevOrigin.X - segmentDistance, prevOrigin.Y),
			Math.Abs(segmentDistance),
			skinRadius,
			new BodyPart[bodyPartDescriptors.Length]
		);

		for (int i = bodyPartDescriptors.Length - 1; i >= 0; --i)
			segment.BodyParts[i] = bodyPartDescriptors[i].Create(segment);

		return segment;
	}
}

/// <summary>
/// Describes a segment with turning angles.
/// </summary>
public class AngledSegmentDescriptor : SegmentDescriptor
{
	protected double minAngle;
	protected double maxAngle;

	public double MinAngle => minAngle;
	public double MaxAngle => maxAngle;

	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="minAngle">The minimum angle of the joint.</param>
	/// <param name="maxAngle">The maximum angle of the joint.</param>
	/// <param name="bodyPartDescriptors">The bodyParts of the segment.</param>
	public AngledSegmentDescriptor(int segmentDistance, int skinRadius, double minAngle, double maxAngle, BodyPartDescriptor[] bodyPartDescriptors)
		: base(segmentDistance, skinRadius, bodyPartDescriptors)
	{
		if (minAngle > maxAngle)
			throw new ArgumentException($"The maxAngle ({maxAngle}) should be bigger or equal than the minAngle ({minAngle})!");

		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
	}

	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="angle">The minimum and maximum angle of the joint.</param>
	/// <param name="bodyPartDescriptors">The bodyParts of the segment.</param>
	public AngledSegmentDescriptor(int segmentDistance, int skinRadius, double angle, BodyPartDescriptor[] bodyPartDescriptors)
		: this(segmentDistance, skinRadius, -angle, angle, bodyPartDescriptors) { }

	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="minAngle">The minimum angle of the joint.</param>
	/// <param name="maxAngle">The maximum angle of the joint.</param>
	public AngledSegmentDescriptor(int segmentDistance, int skinRadius, double minAngle, double maxAngle)
		: this(segmentDistance, skinRadius, minAngle, maxAngle, []) { }

	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="angle">The minimum and maximum angle of the joint.</param>
	public AngledSegmentDescriptor(int segmentDistance, int skinRadius, double angle)
		: this(segmentDistance, skinRadius, -angle, angle, []) { }

	/// <summary>
	/// Creates a Segment object by this descriptor.
	/// </summary>
	/// <param name="prevOrigin">The origin of the previous segment.</param>
	/// <returns>The Segment object.</returns>
	public override Segment Create(Point prevOrigin)
	{
		Segment segment = new(
			new Point(prevOrigin.X, prevOrigin.Y - segmentDistance),
			Math.Abs(segmentDistance),
			skinRadius,
			new BodyPart[bodyPartDescriptors.Length],
			maxAngle,
			minAngle
		);

		for (int i = bodyPartDescriptors.Length - 1; i >= 0; --i)
			segment.BodyParts[i] = bodyPartDescriptors[i].Create(segment);

		return segment;
	}
}

/// <summary>
/// Describes a body part.
/// </summary>
public abstract class BodyPartDescriptor
{
	protected Render render;
	protected Color color;

	public Render Render => render;
	public Color Color => color;

	/// <summary>
	/// Creates a BodyPartDescriptor object.
	/// </summary>
	/// <param name="render">Where to render.</param>
	/// <param name="color">The color of the bodyPart.</param>
	public BodyPartDescriptor(Render render, Color color)
	{
		this.render = render;
		this.color = color;
	}

	/// <summary>
	/// Creates a BodyPart object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The BodyPart object.</returns>
	public abstract BodyPart Create(Segment segment);
}

/// <summary>
/// Describes an eye.
/// </summary>
public class EyeDescriptor : BodyPartDescriptor
{
	protected double degreeToFront;
	protected int distanceToOrigin;
	protected int radius;

	protected double DegreeToFront => degreeToFront;
	protected int DistanceToOrigin => distanceToOrigin;
	protected int Radius => radius;

	/// <summary>
	/// Creates an EyeDescriptor object.
	/// </summary>
	/// <param name="angleToFront">The angle to push the eye from the center of the segment.</param>
	/// <param name="distanceToOrigin">The distance to push the eye from the center of the segment.</param>
	/// <param name="radius">The radius of the eye.</param>
	/// <param name="color">The color of the bodyPart.</param>
	/// <param name="render">Where to render.</param>
	public EyeDescriptor(double angleToFront, int distanceToOrigin, int radius, Color color, Render render = Render.Top) : base(render, color)
	{
		degreeToFront = Math.Abs(angleToFront);
		this.distanceToOrigin = Math.Abs(distanceToOrigin);
		this.radius = Math.Abs(radius);
	}

	/// <summary>
	/// Creates an Eye object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The Eye object.</returns>
	public override BodyPart Create(Segment segment) => new Eye(segment, render, degreeToFront, distanceToOrigin, radius, color);
}

/// <summary>
/// Describes a fin.
/// </summary>
public class SideFinDescriptor : BodyPartDescriptor
{
	protected int length;
	protected int width;
	protected double angle;

	protected int Length => length;
	protected int Width => width;
	protected double Angle => angle;

	/// <summary>
	/// Creates a SideFinDescriptor object.
	/// </summary>
	/// <param name="length">The length of the fin.</param>
	/// <param name="width">The width of the fin.</param>
	/// <param name="angle">The angle of the fin.</param>
	/// <param name="color">The color of the fin.</param>
	/// <param name="render">Where to render.</param>
	public SideFinDescriptor(int length, int width, double angle, Color color, Render render = Render.Bottom) : base(render, color)
	{
		this.length = Math.Abs(length);
		this.width = Math.Abs(width);
		this.angle = angle;
	}

	/// <summary>
	/// Creates a SideFin object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The SideFin object.</returns>
	public override BodyPart Create(Segment segment) => new SideFin(segment, render, length, width, angle, color);
}

/// <summary>
/// Describes a fin.
/// </summary>
public class BackFinDescriptor : BodyPartDescriptor
{
	protected int lengthInSegments;

	public int LengthInSegments => lengthInSegments;

	/// <summary>
	/// Creates a BackFinDescriptor object.
	/// <param name="lengthInSegments">The number of segments the fin will go through.</param>
	/// <param name="color">The color of the fin.</param>
	/// <param name="render">Where to render.</param>
	public BackFinDescriptor(int lengthInSegments, Color color, Render render = Render.Top) : base(render, color) => this.lengthInSegments = lengthInSegments;

	/// <summary>
	/// Creates a BackFin object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The BackFin object.</returns>
	public override BodyPart Create(Segment segment) => new BackFin(segment, render, lengthInSegments, color);
}

/// <summary>
/// Describes a fin.
/// </summary>
public class TailFinDescriptor : BodyPartDescriptor
{
	protected int[] segmentDistances;

	public int[] SegmentDistances => segmentDistances;

	/// <summary>
	/// Creates a TailFinDescriptor object.
	/// </summary>
	/// <param name="segmentDistances">The distances of the segments of the fin.</param>
	/// <param name="color">The color of the fin.</param>
	/// <param name="render">Where to render.</param>
	public TailFinDescriptor(int[] segmentDistances, Color color, Render render = Render.Bottom) : base(render, color) => this.segmentDistances = segmentDistances;

	/// <summary>
	/// Creates a TailFin object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The TailFin object.</returns>
	public override BodyPart Create(Segment segment) => new TailFin(segment, render, segmentDistances, color);
}

/// <summary>
/// Describes an antenna.
/// </summary>
public class AntennaDescriptor : BodyPartDescriptor
{
	protected AntennaSegmentDescriptor[] segmentDescriptors;
	protected double angle;

	public AntennaSegmentDescriptor[] SegmentDescriptors => segmentDescriptors;
	public double Angle => angle;

	/// <summary>
	/// Creates an AntennaDescriptor object.
	/// </summary>
	/// <param name="antennaSegmentDescriptors">The segments of the antenna.</param>
	/// <param name="angle">The angle to push the eye from the center of the segment.</param>
	/// <param name="color">The color of the antenna.</param>
	/// <param name="render">Where to render.</param>
	public AntennaDescriptor(AntennaSegmentDescriptor[] antennaSegmentDescriptors, double angle, Color color, Render render = Render.Top) : base(render, color)
	{
		segmentDescriptors = antennaSegmentDescriptors;
		this.angle = angle;
	}

	/// <summary>
	/// Creates an Antenna object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The Antenna object.</returns>
	public override BodyPart Create(Segment segment) => new Antenna(segment, render, segmentDescriptors, angle, color);
}

/// <summary>
/// Describes a segment of an antenna.
/// </summary>
public class AntennaSegmentDescriptor : SegmentDescriptor
{
	protected double angle;

	public double Angle => angle;

	/// <summary>
	/// Creates an AntennaSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="angle">The angle of the segment from the previous one.</param>
	public AntennaSegmentDescriptor(int segmentDistance, int skinRadius, double angle) : base(segmentDistance, skinRadius) => this.angle = angle;

	/// <summary>
	/// Creates a Segment object by this descriptor.
	/// </summary>
	/// <param name="prevOrigin">The origin of the previous segment.</param>
	/// <returns>The Segment object.</returns>
	public override Segment Create(Point prevOrigin)
	{
		return new Segment(
			Point.RotateDegree(new Point(prevOrigin.X - segmentDistance, prevOrigin.Y), angle),
			segmentDistance,
			skinRadius,
			[]
		);
	}
}

/// <summary>
/// Describes a leg.
/// </summary>
public class LegDescriptor : BodyPartDescriptor
{
	protected LegSegmentDescriptor[] segmentDescriptors;
	protected Point stepTo;

	public LegSegmentDescriptor[] SegmentDescriptors => segmentDescriptors;
	public Point StepTo => stepTo;

	/// <summary>
	/// Creates a LegDescriptor object.
	/// </summary>
	/// <param name="legSegmentDescriptors">The segments of the leg.</param>
	/// <param name="stepTo">The position to step to.</param>
	/// <param name="color">The color of the leg.</param>
	/// <param name="render">Where to render.</param>
	public LegDescriptor(LegSegmentDescriptor[] legSegmentDescriptors, Point stepTo, Color color, Render render = Render.Bottom) : base(render, color)
	{
		segmentDescriptors = legSegmentDescriptors;
		this.stepTo = stepTo;
	}

	/// <summary>
	/// Creates a Leg object by this descriptor.
	/// </summary>
	/// <param name="segment">The parent segment.</param>
	/// <returns>The Leg object.</returns>
	public override BodyPart Create(Segment segment) => new Leg(segment, render, segmentDescriptors, stepTo, color);
}

/// <summary>
/// Describes a segment of a leg.
/// </summary>
public class LegSegmentDescriptor : AngledSegmentDescriptor
{
	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="minAngle">The minimum angle of the joint.</param>
	/// <param name="maxAngle">The maximum angle of the joint.</param>
	/// <param name="bodyPartDescriptors">The bodyParts of the segment.</param>
	public LegSegmentDescriptor(int segmentDistance, int skinRadius, double minAngle, double maxAngle, BodyPartDescriptor[] bodyPartDescriptors)
		: base(segmentDistance, skinRadius, minAngle, maxAngle, bodyPartDescriptors) { }

	/// <summary>
	/// Creates a LegSegmentDescriptor object.
	/// </summary>
	/// <param name="segmentDistance">The distance form the previous segment.</param>
	/// <param name="skinRadius">The radius of the skin at the segment.</param>
	/// <param name="minAngle">The minimum angle of the joint.</param>
	/// <param name="maxAngle">The maximum angle of the joint.</param>
	public LegSegmentDescriptor(int segmentDistance, int skinRadius, double minAngle, double maxAngle)
		: this(segmentDistance, skinRadius, minAngle, maxAngle, []) { }

	/// <summary>
	/// Mirrors the position one LegSegmentDescriptor.
	/// </summary>
	/// <param name="descriptor">The LegSegmentDescriptor to mirror.</param>
	/// <returns>A new LegSegmentDescriptor object with mirrored coordinates.</returns>
	public static LegSegmentDescriptor Mirror(LegSegmentDescriptor descriptor)
	{
		return new LegSegmentDescriptor(
			-descriptor.segmentDistance,
			descriptor.skinRadius,
			-descriptor.maxAngle,
			-descriptor.minAngle,
			descriptor.bodyPartDescriptors
		);
	}
}