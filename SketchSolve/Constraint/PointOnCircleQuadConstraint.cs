namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnCircleQuadConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Circle _circle1;
  private readonly Parameter _parameter;

  public PointOnCircleQuadConstraint(Point point1, Circle circle1, Parameter parameter)
  {
    _point1 = point1;
    _circle1 = circle1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var ex = c1CenterX;
    var ey = c1CenterY;
    var c1Rad = _circle1.Rad.Value;
    var quadIndex = _parameter.Value;
    switch ((int) quadIndex)
    {
      case 0:
        ex += c1Rad;
        break;
      case 1:
        ey += c1Rad;
        break;
      case 2:
        ex -= c1Rad;
        break;
      case 3:
        ey -= c1Rad;
        break;
    }

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
      _circle1,
      new[] {_parameter}
    };
  }
}
