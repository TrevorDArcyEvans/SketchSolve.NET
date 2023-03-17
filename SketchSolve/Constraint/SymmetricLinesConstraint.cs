namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricLinesConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;
  private readonly Line _symLine;

  public SymmetricLinesConstraint(Line line1, Line line2, Line symLine)
  {
    _line1 = line1;
    _line2 = line2;
    _symLine = symLine;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var t = -(dy * l1P1X - dx * l1P1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = l1P1X + dy * t * 2;
    var ey = l1P1Y - dx * t * 2;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var l2P1X = _line2.P1.X.Value;
    var l2P1Y = _line2.P1.Y.Value;
    var l2P2X = _line2.P2.X.Value;
    var l2P2Y = _line2.P2.Y.Value;
    var tempX = ex - l2P1X;
    var tempY = ey - l2P1Y;
    error += tempX * tempX + tempY * tempY;

    t = -(dy * l1P2X - dx * l1P2Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    ex = l1P2X + dy * t * 2;
    ey = l1P2Y - dx * t * 2;
    tempX = ex - l2P2X;
    tempY = ey - l2P2Y;
    error += tempX * tempX + tempY * tempY;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2,
      _symLine
    };
  }
}
