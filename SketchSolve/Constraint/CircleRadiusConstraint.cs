namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class CircleRadiusConstraint : BaseConstraint
{
  private readonly Circle _circle1;
  private readonly Parameter _parameter;

  public CircleRadiusConstraint(Circle circle1, Parameter parameter)
  {
    _circle1 = circle1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var c1Rad = _circle1.Rad.Value;
    var radius = _parameter.Value;
    return (c1Rad - radius) * (c1Rad - radius);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      new[] {_parameter}
    };
  }
}
