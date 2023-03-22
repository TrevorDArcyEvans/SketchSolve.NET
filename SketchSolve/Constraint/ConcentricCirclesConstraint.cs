namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricCirclesConstraint : BaseConstraint
{
  public readonly Circle Circle1;
  public readonly Circle Circle2;

  public ConcentricCirclesConstraint(Circle circle1, Circle circle2)
  {
    Circle1 = circle1;
    Circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1CenterX = Circle1.Center.X.Value;
    var c1CenterY = Circle1.Center.Y.Value;
    var c2CenterX = Circle2.Center.X.Value;
    var c2CenterY = Circle2.Center.Y.Value;
    var temp = Hypot(c1CenterX - c2CenterX, c1CenterY - c2CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Circle1,
      Circle2
    };
  }
}
