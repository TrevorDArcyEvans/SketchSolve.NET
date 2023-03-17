namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class VerticalConstraint : Constraint
{
  private readonly Line _line1;

  public VerticalConstraint(Line line1)
  {
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P2X = _line1.P2.X.Value;
    var odx = l1P2X - l1P1X;
    return odx * odx;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1
    };
  }
}
