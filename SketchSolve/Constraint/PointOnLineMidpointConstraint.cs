namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnLineMidpointConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Line Line;

  public PointOnLineMidpointConstraint(Point point, Line line)
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
    var ex = (l1P1X + l1P2X) / 2;
    var ey = (l1P1Y + l1P2Y) / 2;
    var p1X = Point.X.Value;
    var p1Y = Point.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
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
