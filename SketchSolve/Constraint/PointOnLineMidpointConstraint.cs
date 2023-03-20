namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnLineMidpointConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Line _line1;

  public PointOnLineMidpointConstraint(Point point1, Line line1)
  {
    _point1 = point1;
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var ex = (l1P1X + l1P2X) / 2;
    var ey = (l1P1Y + l1P2Y) / 2;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1
    };
  }
}
