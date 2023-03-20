namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnArcMidpointConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Arc _arc1;

  public PointOnArcMidpointConstraint(Point point1, Arc arc1)
  {
    _point1 = point1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var tempStart = Math.Atan2(a1StartY - a1CenterY, a1StartX - a1CenterX);
    var tempEnd = Math.Atan2(a1EndY - a1CenterY, a1EndX - a1CenterX);
    var ex = a1CenterX + rad1 * Math.Cos((tempEnd + tempStart) / 2);
    var ey = a1CenterY + rad1 * Math.Sin((tempEnd + tempStart) / 2);
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
      _arc1
    };
  }
}
