namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnArcConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Arc _arc1;

  public PointOnArcConstraint(Point point1, Arc arc1)
  {
    _point1 = point1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var rad1 = Hypot(a1CenterX - p1X, a1CenterY - p1Y);
    var rad2 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - rad2;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _arc1
    };
  }
}
