namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2PDistanceVertConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Point Point2;
  public readonly Parameter Distance;
  public override IEnumerable<object> Items => new[] { Point1, Point2 };

  public P2PDistanceVertConstraint(Point point1, Point point2, Parameter distance)
  {
    Point1 = point1;
    Point2 = point2;
    Distance = distance;
  }

  public override double CalculateError()
  {
    var p1Y = Point1.Y.Value;
    var p2Y = Point2.Y.Value;
    var distance = Distance.Value;
    return (p1Y - p2Y) * (p1Y - p2Y) - distance * distance;
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
