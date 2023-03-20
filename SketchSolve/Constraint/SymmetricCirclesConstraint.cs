namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricCirclesConstraint : BaseConstraint
{
  private readonly Line _symLine;
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public SymmetricCirclesConstraint(Line symLine, Circle circle1, Circle circle2)
  {
    _symLine = symLine;
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var t = -(dy * c1CenterX - dx * c1CenterY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = c1CenterX + dy * t * 2;
    var ey = c1CenterY - dx * t * 2;
    var c2CenterX = _circle2.Center.X.Value;
    var c2CenterY = _circle2.Center.Y.Value;
    var tempX = ex - c2CenterX;
    var tempY = ey - c2CenterY;
    error += tempX * tempX + tempY * tempY;

    var c1Rad = _circle1.Rad.Value;
    var c2Rad = _circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
    error += temp * temp;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _symLine,
      _circle1,
      _circle2
    };
  }
}
