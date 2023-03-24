namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnArcMidpointConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Arc Arc;
  public override IEnumerable<object> Items => new object[] { Point, Arc};

  public PointOnArcMidpointConstraint(Point point, Arc arc)
  {
    Point = point;
    Arc = arc;
  }

  public override double CalculateError()
  {
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1EndA = Arc.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = Arc.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var tempStart = Math.Atan2(a1StartY - a1CenterY, a1StartX - a1CenterX);
    var tempEnd = Math.Atan2(a1EndY - a1CenterY, a1EndX - a1CenterX);
    var ex = a1CenterX + rad1 * Math.Cos((tempEnd + tempStart) / 2);
    var ey = a1CenterY + rad1 * Math.Sin((tempEnd + tempStart) / 2);
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
      Arc
    };
  }
}
