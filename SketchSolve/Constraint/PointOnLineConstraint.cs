namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnLineConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Line _line1;

  public PointOnLineConstraint(Point point1, Line line1)
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
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var m = dy / dx; // Slope
    var n = dx / dy; // 1/Slope

    if (m <= 1 && m >= -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var p1X = _point1.X.Value;
      var p1Y = _point1.Y.Value;
      var ey = l1P1Y + m * (p1X - l1P1X);
      return (ey - p1Y) * (ey - p1Y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var p1X = _point1.X.Value;
      var p1Y = _point1.Y.Value;
      var ex = l1P1X + n * (p1Y - l1P1Y);
      return (ex - p1X) * (ex - p1X);
    }
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
