namespace ProcedurallyGeneratedAnimals;

/// <summary>
/// Describes a point in the 2D space.
/// </summary>
public class Point
{
	private double x;
	private double y;

	public double X => x;
	public double Y => y;

	/// <summary>
	/// Creates a <see cref="Point"/> object.
	/// </summary>
	/// <param name="x">The X value.</param>
	/// <param name="y">The Y value.</param>
	public Point(double x, double y)
	{
		this.x = x;
		this.y = y;
	}

	/// <summary>
	/// Adds two points.
	/// </summary>
	/// <param name="a">The first point.</param>
	/// <param name="b">The second point.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Add(Point a, Point b) => new(a.x + b.x, a.y + b.y);

	/// <summary>
	/// Subtracts two points.
	/// </summary>
	/// <param name="a">The first point.</param>
	/// <param name="b">The second point.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Subtract(Point a, Point b) => new(a.x - b.x, a.y - b.y);

	/// <summary>
	/// Scales a vector by a number.
	/// </summary>
	/// <param name="v">The vector.</param>
	/// <param name="s">The number.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Multiply(Point v, double s) => new(v.x * s, v.y * s);

	/// <summary>
	/// Divides a vector by a number.
	/// </summary>
	/// <param name="v">The vector.</param>
	/// <param name="s">The number.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Divide(Point v, double s) => new(v.x / s, v.y / s);

	/// <summary>
	/// Gets the magnitude of the given vector.
	/// </summary>
	/// <param name="v">The vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static double Magnitude(Point v) => Math.Sqrt(v.x * v.x + v.y * v.y);

	/// <summary>
	/// Gets the distance of the two points.
	/// </summary>
	/// <param name="a">The first point.</param>
	/// <param name="b">The second point.</param>
	/// <returns>The result of the calculation.</returns>
	public static double Distance(Point a, Point b) => Magnitude(Subtract(a, b));

	/// <summary>
	/// Normalizes the given vector.
	/// </summary>
	/// <param name="v">The vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Normalize(Point v) => Divide(v, Magnitude(v));

	/// <summary>
	/// Scales a vector to a length.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <param name="s"> The length.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Scale(Point v, double s) => Multiply(Normalize(v), s);

	/// <summary>
	/// Gets the left normal vector of the given vector.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point NormalLeft(Point v)
	{
		return new Point(-v.y, v.x);
	}

	/// <summary>
	/// Gets the right normal vector of the given vector.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point NormalRight(Point v) => new(v.y, -v.x);

	/// <summary>
	/// Reverses the given vector.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Reverse(Point v) => new(-v.x, -v.y);

	/// <summary>
	/// Rotates the given vector by a radian value.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <param name="radian"> The radian value.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point RotateRadian(Point v, double radian)
	{
		double sinR = Math.Sin(radian);
		double cosR = Math.Cos(radian);

		return new Point(cosR * v.x - sinR * v.y, sinR * v.x + cosR * v.y);
	}

	/// <summary>
	/// Rotates the given vector by a degree value.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <param name="degree"> The degree value.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point RotateDegree(Point v, double degree) => RotateRadian(v, double.DegreesToRadians(degree));

	/// <summary>
	/// Gets the dot product of the given vectors.
	/// </summary>
	/// <param name="v1"> The first vector.</param>
	/// <param name="v2"> The second vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static double Dot(Point v1, Point v2) => v1.x * v2.x + v1.y * v2.y;

	/// <summary>
	/// Projects a vector to another.
	/// </summary>
	/// <param name="v1"> The vector to project.</param>
	/// <param name="v2"> The line of the projection.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point Project(Point v1, Point v2) => Multiply(v2, Dot(v1, v2) / Dot(v1, v2));

	/// <summary>
	/// Gets the cosine of two vectors.
	/// </summary>
	/// <param name="v1"> The first vector.</param>
	/// <param name="v2"> The second vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static double CosOfVectors(Point v1, Point v2) => Dot(v1, v2) / (Magnitude(v1) * Magnitude(v2));

	/// <summary>
	/// Gets the sine of two vectors.
	/// </summary>
	/// <param name="v1"> The first vector.</param>
	/// <param name="v2"> The second vector.</param>
	/// <returns>The result of the calculation.</returns>
	public static double SinOfVectors(Point v1, Point v2)
	{
		v1 = NormalLeft(v1);
		return CosOfVectors(v1, v2);
	}

	/// <summary>
	/// Gets the cosine of three points.
	/// </summary>
	/// <param name="a"> The first point.</param>
	/// <param name="b"> The middle point.</param>
	/// <param name="c"> The last point.</param>
	/// <returns>The result of the calculation.</returns>
	public static double CosOfPoints(Point a, Point b, Point c)
	{
		Point v1 = Subtract(a, b);
		Point v2 = Subtract(c, b);

		return CosOfVectors(v1, v2);
	}

	/// <summary>
	/// Gets the sine of three points.
	/// </summary>
	/// <param name="a"> The first point.</param>
	/// <param name="b"> The middle point.</param>
	/// <param name="c"> The last point.</param>
	/// <returns>The result of the calculation.</returns>
	public static double SinOfPoints(Point a, Point b, Point c)
	{
		Point v1 = Subtract(a, b);
		Point v2 = Subtract(c, b);

		return SinOfVectors(v1, v2);
	}

	/// <summary>
	/// Gets the angle of a vector to the X base line.
	/// </summary>
	/// <param name="v"> The vector.</param>
	/// <returns>The angle in degrees.</returns>
	public static double AngleOfVector(Point v) => double.RadiansToDegrees(Math.Atan2(v.y, v.x));

	/// <summary>
	/// Gets the angle of two vectors.
	/// </summary>
	/// <param name="v1"> The first vector.</param>
	/// <param name="v2"> The second vector.</param>
	/// <returns>The angle in degrees.</returns>
	public static double AngleOfVectors(Point v1, Point v2)
	{
		double angle = double.RadiansToDegrees(Math.Atan2(v2.y, v2.x) - Math.Atan2(v1.y, v1.x));

		return angle > 180 ? angle - 360 : angle < -180 ? angle + 360 : angle;
	}

	/// <summary>
	/// Gets the angle of three points.
	/// </summary>
	/// <param name="a"> The first point.</param>
	/// <param name="b"> The middle point.</param>
	/// <param name="c"> The last point.</param>
	/// <returns>The angle in degrees.</returns>
	public static double AngleOfPoints(Point a, Point b, Point c)
	{
		Point v1 = Subtract(a, b);
		Point v2 = Subtract(c, b);

		return AngleOfVectors(v1, v2);
	}

	/// <summary>
	/// Restricts the angle a given vector.
	/// </summary>
	/// <param name="baseVector"> The base vector.</param>
	/// <param name="directionVector"> The vector to restrict.</param>
	/// <param name="maxAngle"> The maximum angle in degrees.</param>
	/// <param name="minAngle"> The minimum angle in degrees.</param>
	/// <returns>The restricted vector.</returns>
	public static Point RestrictAngleOfRotation(Point baseVector, Point directionVector, double maxAngle, double minAngle)
	{
		double angleBetween = AngleOfVectors(baseVector, directionVector);

		if (minAngle < angleBetween && angleBetween < maxAngle)
			return directionVector;

		double toRotate = (minAngle < angleBetween ? maxAngle : minAngle) - angleBetween;

		return RotateDegree(directionVector, toRotate);
	}
}