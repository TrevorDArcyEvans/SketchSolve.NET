namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualRadiusCircArcConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Arc _arc1;

  public EqualRadiusCircArcConstraint(Circle circle1, Arc arc1)
  {
    _circle1 = circle1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var c1Rad = _circle1.Rad.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _arc1
    };
  }
}
