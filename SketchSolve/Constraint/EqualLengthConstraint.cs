namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class EqualLengthConstraint : BaseConstraint
{
  public readonly Line Line1;
  public readonly Line Line2;

  public EqualLengthConstraint(Line line1, Line line2)
  {
    Line1 = line1;
    Line2 = line2;
  }

  public override double CalculateError()
  {
    var l1P1X = Line1.P1.X.Value;
    var l1P1Y = Line1.P1.Y.Value;
    var l1P2X = Line1.P2.X.Value;
    var l1P2Y = Line1.P2.Y.Value;
    var l2P1X = Line2.P1.X.Value;
    var l2P1Y = Line2.P1.Y.Value;
    var l2P2X = Line2.P2.X.Value;
    var l2P2Y = Line2.P2.Y.Value;
    var temp = Hypot(l1P2X - l1P1X, l1P2Y - l1P1Y) - Hypot(l2P2X - l2P1X, l2P2Y - l2P1Y);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line1,
      Line2
    };
  }
}
