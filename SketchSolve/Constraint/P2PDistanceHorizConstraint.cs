namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2PDistanceHorizConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Point Point2;
  public readonly Parameter Distance;
  public override IEnumerable<object> Items => new[] { Point1, Point2 };

  public P2PDistanceHorizConstraint(Point point1, Point point2, Parameter distance)
  {
    Point1 = point1;
    Point2 = point2;
    Distance = distance;
  }

  public override double CalculateError()
  {
    var p1X = Point1.X.Value;
    var p2X = Point2.X.Value;
    var distance = Distance.Value;
    return (p1X - p2X) * (p1X - p2X) - distance * distance;
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
