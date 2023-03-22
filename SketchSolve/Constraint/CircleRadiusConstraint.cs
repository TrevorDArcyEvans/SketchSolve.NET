namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class CircleRadiusConstraint : BaseConstraint
{
  public readonly Circle Circle;
  public readonly Parameter Radius;

  public CircleRadiusConstraint(Circle circle, Parameter radius)
  {
    Circle = circle;
    Radius = radius;
  }

  public override double CalculateError()
  {
    var c1Rad = Circle.Rad.Value;
    var radius = Radius.Value;
    return (c1Rad - radius) * (c1Rad - radius);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Circle,
      new[] {Radius}
    };
  }
}
