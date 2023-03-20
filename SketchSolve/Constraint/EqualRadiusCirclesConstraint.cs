namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualRadiusCirclesConstraint : BaseConstraint
{
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public EqualRadiusCirclesConstraint(Circle circle1, Circle circle2)
  {
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1Rad = _circle1.Rad.Value;
    var c2Rad = _circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _circle2
    };
  }
}
