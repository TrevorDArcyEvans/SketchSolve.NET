namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class LineLengthConstraint : BaseConstraint
{
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public LineLengthConstraint(Line line1, Parameter parameter)
  {
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var temp = Math.Sqrt(Math.Pow(l1P2X - l1P1X, 2) + Math.Pow(l1P2Y - l1P1Y, 2)) - _parameter.Value;
    return temp * temp * 100;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      new[] {_parameter}
    };
  }
}
