namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricCircArcConstraint : BaseConstraint
{
  public readonly Circle Circle;
  public readonly Arc Arc;

  public ConcentricCircArcConstraint(Circle circle, Arc arc)
  {
    Circle = circle;
    Arc = arc;
  }

  public override double CalculateError()
  {
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var c1CenterX = Circle.Center.X.Value;
    var c1CenterY = Circle.Center.Y.Value;
    var temp = Hypot(a1CenterX - c1CenterX, a1CenterY - c1CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Circle,
      Arc
    };
  }
}
