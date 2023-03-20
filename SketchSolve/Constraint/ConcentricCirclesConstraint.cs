namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricCirclesConstraint : BaseConstraint
{
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public ConcentricCirclesConstraint(Circle circle1, Circle circle2)
  {
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var c2CenterX = _circle2.Center.X.Value;
    var c2CenterY = _circle2.Center.Y.Value;
    var temp = Hypot(c1CenterX - c2CenterX, c1CenterY - c2CenterY);
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
