namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnLineConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Line Line;

  public PointOnLineConstraint(Point point, Line line)
  {
    Point = point;
    Line = line;
  }

  public override double CalculateError()
  {
    var l1P1X = Line.P1.X.Value;
    var l1P1Y = Line.P1.Y.Value;
    var l1P2X = Line.P2.X.Value;
    var l1P2Y = Line.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var m = dy / dx; // Slope
    var n = dx / dy; // 1/Slope

    if (m <= 1 && m >= -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var p1X = Point.X.Value;
      var p1Y = Point.Y.Value;
      var ey = l1P1Y + m * (p1X - l1P1X);
      return (ey - p1Y) * (ey - p1Y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var p1X = Point.X.Value;
      var p1Y = Point.Y.Value;
      var ex = l1P1X + n * (p1Y - l1P1Y);
      return (ex - p1X) * (ex - p1X);
    }
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point,
      Line
    };
  }
}
