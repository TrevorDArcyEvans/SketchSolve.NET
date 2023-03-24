namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricArcsConstraint : BaseConstraint
{
  public readonly Arc Arc1;
  public readonly Arc Arc2;
  public override IEnumerable<object> Items => new[] { Arc1, Arc2 };

  public ConcentricArcsConstraint(Arc arc1, Arc arc2)
  {
    Arc1 = arc1;
    Arc2 = arc2;
  }

  public override double CalculateError()
  {
    var a1CenterX = Arc1.Center.X.Value;
    var a2CenterY = Arc2.Center.Y.Value;
    var a2CenterX = Arc2.Center.X.Value;
    var a1CenterY = Arc1.Center.Y.Value;
    var temp = Hypot(a1CenterX - a2CenterX, a1CenterY - a2CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Arc1,
      Arc2
    };
  }
}
