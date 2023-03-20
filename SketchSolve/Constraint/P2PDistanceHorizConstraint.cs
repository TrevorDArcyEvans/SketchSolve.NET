namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2PDistanceHorizConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Parameter _parameter;

  public P2PDistanceHorizConstraint(Point point1, Point point2, Parameter parameter)
  {
    _point1 = point1;
    _point2 = point2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var p1X = _point1.X.Value;
    var p2X = _point2.X.Value;
    var distance = _parameter.Value;
    return (p1X - p2X) * (p1X - p2X) - distance * distance;
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
