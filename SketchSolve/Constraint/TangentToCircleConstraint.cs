namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class TangentToCircleConstraint : BaseConstraint
{
  public readonly Line Line;
  public readonly Circle Circle;
  public override IEnumerable<object> Items => new object[] { Line, Circle };

  public TangentToCircleConstraint(Line line, Circle circle)
  {
    Line = line;
    Circle = circle;
  }

  public override double CalculateError()
  {
    var temp = Circle.CenterTo(Line).Vector.Length - Circle.Rad.Value;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line,
      Circle
    };
  }
}
