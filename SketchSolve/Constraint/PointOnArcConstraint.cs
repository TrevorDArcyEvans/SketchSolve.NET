namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnArcConstraint : BaseConstraint
{
  public readonly Point Point;
  public readonly Arc Arc;

  public PointOnArcConstraint(Point point, Arc arc)
  {
    Point = point;
    Arc = arc;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1StartA = Arc.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var p1X = Point.X.Value;
    var p1Y = Point.Y.Value;
    var rad1 = Hypot(a1CenterX - p1X, a1CenterY - p1Y);
    var rad2 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - rad2;
    return temp * temp;
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
