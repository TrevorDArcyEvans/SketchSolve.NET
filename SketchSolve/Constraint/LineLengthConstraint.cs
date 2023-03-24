namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class LineLengthConstraint : BaseConstraint
{
  public readonly Line Line;
  public readonly Parameter Length;
  public override IEnumerable<object> Items => new[] { Line };

  public LineLengthConstraint(Line line, Parameter length)
  {
    Line = line;
    Length = length;
  }

  public override double CalculateError()
  {
    var l1P1X = Line.P1.X.Value;
    var l1P1Y = Line.P1.Y.Value;
    var l1P2X = Line.P2.X.Value;
    var l1P2Y = Line.P2.Y.Value;
    var temp = Math.Sqrt(Math.Pow(l1P2X - l1P1X, 2) + Math.Pow(l1P2Y - l1P1Y, 2)) - Length.Value;
    return temp * temp * 100;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line,
      new[] {Length}
    };
  }
}
