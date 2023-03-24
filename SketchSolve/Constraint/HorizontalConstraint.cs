namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class HorizontalConstraint : BaseConstraint
{
  public readonly Line Line;
  public override IEnumerable<object> Items => new[] { Line };

  public HorizontalConstraint(Line line)
  {
    Line = line;
  }

  public override double CalculateError()
  {
    var l1P1Y = Line.P1.Y.Value;
    var l1P2Y = Line.P2.Y.Value;
    var ody = l1P2Y - l1P1Y;
    return ody * ody;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line
    };
  }
}
