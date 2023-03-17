namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class HorizontalConstraint : Constraint
{
  private readonly Line _line1;

  public HorizontalConstraint(Line line1)
  {
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var ody = l1P2Y - l1P1Y;
    return ody * ody;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1
    };
  }
}
