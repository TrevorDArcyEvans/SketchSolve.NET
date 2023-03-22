namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2PDistanceConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Point Point2;
  public readonly Parameter Distance;

  public P2PDistanceConstraint(Point point1, Point point2, Parameter distance)
  {
    Point1 = point1;
    Point2 = point2;
    Distance = distance;
  }

  public override double CalculateError()
  {
    var p1X = Point1.X.Value;
    var p1Y = Point1.Y.Value;
    var p2X = Point2.X.Value;
    var p2Y = Point2.Y.Value;
    var distance = Distance.Value;
    return (p1X - p2X) * (p1X - p2X) + (p1Y - p2Y) * (p1Y - p2Y) - distance * distance;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point1,
      Point2,
      new[] {Distance}
    };
  }
}
