namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualRadiusCirclesConstraint : BaseConstraint
{
  public readonly Circle Circle1;
  public readonly Circle Circle2;

  public EqualRadiusCirclesConstraint(Circle circle1, Circle circle2)
  {
    Circle1 = circle1;
    Circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1Rad = Circle1.Rad.Value;
    var c2Rad = Circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
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
