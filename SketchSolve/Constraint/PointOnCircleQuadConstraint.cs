namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnCircleQuadConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Circle Circle1;
  public readonly Parameter QuadIndex;

  public PointOnCircleQuadConstraint(Point point1, Circle circle1, Parameter quadIndex)
  {
    Point1 = point1;
    Circle1 = circle1;
    QuadIndex = quadIndex;
  }

  public override double CalculateError()
  {
    var c1CenterX = Circle1.Center.X.Value;
    var c1CenterY = Circle1.Center.Y.Value;
    var ex = c1CenterX;
    var ey = c1CenterY;
    var c1Rad = Circle1.Rad.Value;
    var quadIndex = QuadIndex.Value;
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

    var p1X = Point1.X.Value;
    var p1Y = Point1.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point1,
      Circle1,
      new[] {QuadIndex}
    };
  }
}
