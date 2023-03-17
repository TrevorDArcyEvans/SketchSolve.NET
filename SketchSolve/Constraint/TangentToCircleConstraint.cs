namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class TangentToCircleConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Circle _circle1;

  public TangentToCircleConstraint(Line line1, Circle circle1)
  {
    _line1 = line1;
    _circle1 = circle1;
  }

  public override double CalculateError()
  {
    var temp = _circle1.CenterTo(_line1).Vector.Length - _circle1.Rad.Value;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _circle1
    };
  }
}
