namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricCircArcConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Arc _arc1;

  public ConcentricCircArcConstraint(Circle circle1, Arc arc1)
  {
    _circle1 = circle1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var temp = Hypot(a1CenterX - c1CenterX, a1CenterY - c1CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _arc1
    };
  }
}
