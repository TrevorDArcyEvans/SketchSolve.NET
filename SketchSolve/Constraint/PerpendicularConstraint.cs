namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PerpendicularConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public PerpendicularConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var temp = _line1.Vector.Dot(_line2.Vector);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2
    };
  }
}
