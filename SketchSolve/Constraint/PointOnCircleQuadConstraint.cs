namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnCircleQuadConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Circle Circle;
  public readonly Parameter QuadIndex;
  public override IEnumerable<object> Items => new object[] {Point, Circle};

  public PointOnCircleQuadConstraint(Point point, Circle circle, Parameter quadIndex)
  {
    Point = point;
    Circle = circle;
    QuadIndex = quadIndex;
  }

  public override double CalculateError()
  {
    var c1CenterX = Circle.Center.X.Value;
    var c1CenterY = Circle.Center.Y.Value;
    var ex = c1CenterX;
    var ey = c1CenterY;
    var c1Rad = Circle.Rad.Value;
    var quadIndex = QuadIndex.Value;
    switch ((int) quadIndex)
    {
      // East
      case 0:
        ex += c1Rad;
        break;

      // North
      case 1:
        ey += c1Rad;
        break;

      // West
      case 2:
        ex -= c1Rad;
        break;

      // South
      case 3:
        ey -= c1Rad;
        break;

      default:
        throw new ArgumentOutOfRangeException();
    }

    var p1X = Point.X.Value;
    var p1Y = Point.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point,
      Circle,
      new[] {QuadIndex}
    };
  }
}
