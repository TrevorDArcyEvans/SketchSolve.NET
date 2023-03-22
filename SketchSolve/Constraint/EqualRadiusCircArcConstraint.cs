namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualRadiusCircArcConstraint : BaseConstraint
{
  public readonly Circle Circle;
  public readonly Arc Arc;

  public EqualRadiusCircArcConstraint(Circle circle, Arc arc)
  {
    Circle = circle;
    Arc = arc;
  }

  public override double CalculateError()
  {
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1StartA = Arc.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var c1Rad = Circle.Rad.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Circle,
      Arc
    };
  }
}
