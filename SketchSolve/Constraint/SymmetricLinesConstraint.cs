namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricLinesConstraint : BaseConstraint
{
  public readonly Line Line1;
  public readonly Line Line2;
  public readonly Line SymLine;

  public SymmetricLinesConstraint(Line line1, Line line2, Line symLine)
  {
    Line1 = line1;
    Line2 = line2;
    SymLine = symLine;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = SymLine.P1.X.Value;
    var symP1Y = SymLine.P1.Y.Value;
    var symP2X = SymLine.P2.X.Value;
    var symP2Y = SymLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var l1P1X = Line1.P1.X.Value;
    var l1P1Y = Line1.P1.Y.Value;
    var t = -(dy * l1P1X - dx * l1P1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = l1P1X + dy * t * 2;
    var ey = l1P1Y - dx * t * 2;
    var l1P2X = Line1.P2.X.Value;
    var l1P2Y = Line1.P2.Y.Value;
    var l2P1X = Line2.P1.X.Value;
    var l2P1Y = Line2.P1.Y.Value;
    var l2P2X = Line2.P2.X.Value;
    var l2P2Y = Line2.P2.Y.Value;
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
      Line1,
      Line2,
      SymLine
    };
  }
}
