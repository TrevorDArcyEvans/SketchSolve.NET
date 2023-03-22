namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class PerpendicularConstraint : BaseConstraint
{
  public readonly Line Line1;
  public readonly Line Line2;

  public PerpendicularConstraint(Line line1, Line line2)
  {
    Line1 = line1;
    Line2 = line2;
  }

  public override double CalculateError()
  {
    var temp = Line1.Vector.Dot(Line2.Vector);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line1,
      Line2
    };
  }
}
