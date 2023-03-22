namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class VerticalConstraint : BaseConstraint
{
  public readonly Line Line;

  public VerticalConstraint(Line line)
  {
    Line = line;
  }

  public override double CalculateError()
  {
    var l1P1X = Line.P1.X.Value;
    var l1P2X = Line.P2.X.Value;
    var odx = l1P2X - l1P1X;
    return odx * odx;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line
    };
  }
}
