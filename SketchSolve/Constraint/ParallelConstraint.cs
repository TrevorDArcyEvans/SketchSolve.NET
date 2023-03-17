namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ParallelConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public ParallelConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var l2P1X = _line2.P1.X.Value;
    var l2P1Y = _line2.P1.Y.Value;
    var l2P2X = _line2.P2.X.Value;
    var l2P2Y = _line2.P2.Y.Value;
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
      _line1,
      _line2
    };
  }
}
