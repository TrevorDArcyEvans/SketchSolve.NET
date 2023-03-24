namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricPointsConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Point Point2;
  public readonly Line SymLine;
  public override IEnumerable<object> Items => new object[] { Point1, Point2, SymLine };

  public SymmetricPointsConstraint(Point point1, Point point2, Line symLine)
  {
    Point1 = point1;
    Point2 = point2;
    SymLine = symLine;
  }

  public override double CalculateError()
  {
    var symP1X = SymLine.P1.X.Value;
    var symP1Y = SymLine.P1.Y.Value;
    var symP2X = SymLine.P2.X.Value;
    var symP2Y = SymLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var p1X = Point1.X.Value;
    var p1Y = Point1.Y.Value;
    var t = -(dy * p1X - dx * p1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = p1X + dy * t * 2;
    var ey = p1Y - dx * t * 2;
    var p2X = Point2.X.Value;
    var p2Y = Point2.Y.Value;
    var tempX = ex - p2X;
    var tempY = ey - p2Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point1,
      Point2,
      SymLine
    };
  }
}
