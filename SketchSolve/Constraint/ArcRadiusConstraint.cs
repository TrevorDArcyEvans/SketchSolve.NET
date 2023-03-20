namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ArcRadiusConstraint : BaseConstraint
{
  private readonly Arc _arc1;
  private readonly Parameter _parameter;

  public ArcRadiusConstraint(Arc arc1, Parameter parameter)
  {
    _arc1 = arc1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var radius = _parameter.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var rad2 = Hypot(a1CenterX - a1EndX, a1CenterY - a1EndY);
    var temp = rad1 - radius;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1,
      new[] {_parameter}
    };
  }
}
