namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualRadiusArcsConstraint : BaseConstraint
{
  public readonly Arc Arc1;
  public readonly Arc Arc2;
  public override IEnumerable<object> Items => new[] { Arc1, Arc2 };

  public EqualRadiusArcsConstraint(Arc arc1, Arc arc2)
  {
    Arc1 = arc1;
    Arc2 = arc2;
  }

  public override double CalculateError()
  {
    var a2Radius = Arc2.Rad.Value;
    var a2StartA = Arc2.StartAngle.Value;
    var a1CenterX = Arc1.Center.X.Value;
    var a1CenterY = Arc1.Center.Y.Value;
    var a2StartX = a1CenterX + a2Radius * Math.Cos(a2StartA);
    var a2StartY = a1CenterY + a2Radius * Math.Sin(a2StartA);
    var a1Radius = Arc1.Rad.Value;
    var a1StartA = Arc1.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a2CenterX = Arc2.Center.X.Value;
    var a2CenterY = Arc2.Center.Y.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var rad2 = Hypot(a2CenterX - a2StartX, a2CenterY - a2StartY);
    var temp = rad1 - rad2;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Arc1,
      Arc2
    };
  }
}
