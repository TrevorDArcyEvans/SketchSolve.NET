namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ParallelConstraint : BaseConstraint
{
  public readonly Line Line1;
  public readonly Line Line2;

  public ParallelConstraint(Line line1, Line line2)
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
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;
    var dx2 = l2P2X - l2P1X;
    var dy2 = l2P2Y - l2P1Y;

    var hyp1 = Hypot(dx, dy);
    var hyp2 = Hypot(dx2, dy2);

    dx = dx / hyp1;
    dy = dy / hyp1;
    dx2 = dx2 / hyp2;
    dy2 = dy2 / hyp2;

    var temp = dy * dx2 - dx * dy2;
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
