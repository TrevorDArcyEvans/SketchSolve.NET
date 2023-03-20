namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class InternalAngleConstraint : BaseConstraint
{
  private readonly Line _line1;
  private readonly Line _line2;
  private readonly Parameter _parameter;

  public InternalAngleConstraint(Line line1, Line line2, Parameter parameter)
  {
    _line1 = line1;
    _line2 = line2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var angleP = _parameter.Value;
    var temp = _line1.Vector.Cosine(_line2.Vector);
    var temp2 = Math.Cos(angleP);
    return (temp - temp2) * (temp - temp2);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2,
      new[] {_parameter}
    };
  }
}
