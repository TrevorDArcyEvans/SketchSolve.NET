namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ArcRadiusConstraint : BaseConstraint
{
  public readonly Arc Arc;
  public readonly Parameter Radius;
  public override IEnumerable<object> Items => new[] { Arc };

  public ArcRadiusConstraint(Arc arc, Parameter radius)
  {
    Arc = arc;
    Radius = radius;
  }

  public override double CalculateError()
  {
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1EndA = Arc.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = Arc.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var radius = Radius.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var rad2 = Hypot(a1CenterX - a1EndX, a1CenterY - a1EndY);
    var temp = rad1 - radius;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Arc,
      new[] {Radius}
    };
  }
}
