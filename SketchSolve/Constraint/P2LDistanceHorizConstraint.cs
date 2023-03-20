namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class P2LDistanceHorizConstraint : BaseConstraint
{
  private readonly Point _point1;
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public P2LDistanceHorizConstraint(Point point1, Line line1, Parameter parameter)
  {
    _point1 = point1;
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var t = (p1Y - l1P1Y) / dy;
    var xint = l1P1X + dx * t;
    var distance = _parameter.Value;
    var temp = Math.Abs(p1X - xint) - distance;
    return temp * temp / 10;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1,
      new[] {_parameter}
    };
  }
}
