namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnCircleConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Circle Circle;

  public PointOnCircleConstraint(Point point, Circle circle)
  {
    Point = point;
    Circle = circle;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var c1Rad = Circle.Rad.Value;
    var c1CenterX = Circle.Center.X.Value;
    var c1CenterY = Circle.Center.Y.Value;
    var p1X = Point.X.Value;
    var p1Y = Point.Y.Value;
    var rad1 = Hypot(c1CenterX - p1X, c1CenterY - p1Y);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point,
      Circle
    };
  }
}
