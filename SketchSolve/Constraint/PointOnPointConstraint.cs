namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PointOnPointConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;

  public PointOnPointConstraint(Point point1, Point point2)
  {
    _point1 = point1;
    _point2 = point2;
  }

  public override double CalculateError()
  {
    //Hopefully avoid this constraint, make coincident points use the same parameters
    var lengthSquared = (_point1 - _point2).LengthSquared;
    return lengthSquared;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2
    };
  }
}
