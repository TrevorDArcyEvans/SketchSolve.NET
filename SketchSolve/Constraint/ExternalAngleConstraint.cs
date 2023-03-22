namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ExternalAngleConstraint : BaseConstraint
{
  public readonly Line Line1;
  public readonly Line Line2;
  public readonly Parameter Angle;

  public ExternalAngleConstraint(Line line1, Line line2, Parameter angle)
  {
    Line1 = line1;
    Line2 = line2;
    Angle = angle;
  }

  public override double CalculateError()
  {
    var angleP = Angle.Value;
    var temp = Line1.Vector.Cosine(Line2.Vector);
    var temp2 = Math.Cos(Math.PI - angleP);
    return (temp - temp2) * (temp - temp2);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      Line1,
      Line2,
      new[] {Angle}
    };
  }
}
