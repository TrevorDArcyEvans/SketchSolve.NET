namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricCirclesConstraint : BaseConstraint
{
  public readonly Line SymLine;
  public readonly Circle Circle1;
  public readonly Circle Circle2;
  public override IEnumerable<object> Items => new object[] { SymLine, Circle1, Circle2 };

  public SymmetricCirclesConstraint(Line symLine, Circle circle1, Circle circle2)
  {
    SymLine = symLine;
    Circle1 = circle1;
    Circle2 = circle2;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = SymLine.P1.X.Value;
    var symP1Y = SymLine.P1.Y.Value;
    var symP2X = SymLine.P2.X.Value;
    var symP2Y = SymLine.P2.Y.Value;
    var c1CenterX = Circle1.Center.X.Value;
    var c1CenterY = Circle1.Center.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var t = -(dy * c1CenterX - dx * c1CenterY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = c1CenterX + dy * t * 2;
    var ey = c1CenterY - dx * t * 2;
    var c2CenterX = Circle2.Center.X.Value;
    var c2CenterY = Circle2.Center.Y.Value;
    var tempX = ex - c2CenterX;
    var tempY = ey - c2CenterY;
    error += tempX * tempX + tempY * tempY;

    var c1Rad = Circle1.Rad.Value;
    var c2Rad = Circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
    error += temp * temp;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      SymLine,
      Circle1,
      Circle2
    };
  }
}
