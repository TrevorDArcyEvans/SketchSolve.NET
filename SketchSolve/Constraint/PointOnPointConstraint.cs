namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnPointConstraint : BaseConstraint
{
  public readonly Point Point1;
  public readonly Point Point2;

  public PointOnPointConstraint(Point point1, Point point2)
  {
    Point1 = point1;
    Point2 = point2;
  }

  public override double CalculateError()
  {
    //Hopefully avoid this constraint, make coincident points use the same parameters
    var lengthSquared = (Point1 - Point2).LengthSquared;
    return lengthSquared;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Point1,
      Point2
    };
  }
}
