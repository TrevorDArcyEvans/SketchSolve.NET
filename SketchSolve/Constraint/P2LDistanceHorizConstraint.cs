namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2LDistanceHorizConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Line Line;
  public readonly Parameter Distance;

  public P2LDistanceHorizConstraint(Point point, Line line, Parameter distance)
  {
    Point = point;
    Line = line;
    Distance = distance;
  }

  public override double CalculateError()
  {
    var l1P1X = Line.P1.X.Value;
    var l1P1Y = Line.P1.Y.Value;
    var l1P2X = Line.P2.X.Value;
    var l1P2Y = Line.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var p1X = Point.X.Value;
    var p1Y = Point.Y.Value;
    var t = (p1Y - l1P1Y) / dy;
    var xint = l1P1X + dx * t;
    var distance = Distance.Value;
    var temp = Math.Abs(p1X - xint) - distance;
    return temp * temp / 10;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point,
      Line,
      new[] {Distance}
    };
  }
}
