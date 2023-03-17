namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2PDistanceVertConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Parameter _parameter;

  public P2PDistanceVertConstraint(Point point1, Point point2, Parameter parameter)
  {
    _point1 = point1;
    _point2 = point2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var p1Y = _point1.Y.Value;
    var p2Y = _point2.Y.Value;
    var distance = _parameter.Value;
    return (p1Y - p2Y) * (p1Y - p2Y) - distance * distance;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      new[] {_parameter}
    };
  }
}
