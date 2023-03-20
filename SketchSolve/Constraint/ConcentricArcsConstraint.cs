namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ConcentricArcsConstraint : BaseConstraint
{
  private readonly Arc _arc1;
  private readonly Arc _arc2;

  public ConcentricArcsConstraint(Arc arc1, Arc arc2)
  {
    _arc1 = arc1;
    _arc2 = arc2;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a2CenterY = _arc2.Center.Y.Value;
    var a2CenterX = _arc2.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var temp = Hypot(a1CenterX - a2CenterX, a1CenterY - a2CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1,
      _arc2
    };
  }
}
