namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricPointsConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Line _symLine;

  public SymmetricPointsConstraint(Point point1, Point point2, Line symLine)
  {
    _point1 = point1;
    _point2 = point2;
    _symLine = symLine;
  }

  public override double CalculateError()
  {
    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var t = -(dy * p1X - dx * p1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = p1X + dy * t * 2;
    var ey = p1Y - dx * t * 2;
    var p2X = _point2.X.Value;
    var p2Y = _point2.Y.Value;
    var tempX = ex - p2X;
    var tempY = ey - p2Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      _symLine
    };
  }
}
