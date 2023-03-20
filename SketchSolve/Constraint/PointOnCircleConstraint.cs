namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnCircleConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Circle _circle1;

  public PointOnCircleConstraint(Point point1, Circle circle1)
  {
    _point1 = point1;
    _circle1 = circle1;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var c1Rad = _circle1.Rad.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var rad1 = Hypot(c1CenterX - p1X, c1CenterY - p1Y);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _circle1
    };
  }
}
