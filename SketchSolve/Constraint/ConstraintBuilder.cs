namespace SketchSolve.Constraint;

using SketchSolve.Model;

public static class ConstraintBuilder
{
  public static BaseConstraint IsHorizontal(this Line line) => new HorizontalConstraint(line);
  public static BaseConstraint IsVertical(this Line line) => new VerticalConstraint(line);
  public static BaseConstraint IsCoincidentWith(this Point pt, Point other) => new PointOnPointConstraint(pt, other);
  public static BaseConstraint HasInternalAngle(this Line line, Line other, Parameter angle) => new InternalAngleConstraint(line, other, angle);
  public static BaseConstraint HasExternalAngle(this Line line, Line other, Parameter angle) => new ExternalAngleConstraint(line, other, angle);
  public static BaseConstraint IsPerpendicularTo(this Line line, Line other) => new PerpendicularConstraint(line, other);
  public static BaseConstraint IsParallelTo(this Line line, Line other) => new ParallelConstraint(line, other);
  public static BaseConstraint IsCollinearTo(this Line line, Line other) => new CollinearConstraint(line, other);
  public static BaseConstraint IsEqualInLengthTo(this Line line, Line other) => new EqualLengthConstraint(line, other);
  public static BaseConstraint IsTangentTo(this Line line, Circle other) => new TangentToCircleConstraint(line, other);
  public static BaseConstraint IsTangentTo(this Line line, Arc other) => new TangentToArcConstraint(line, other);
  public static BaseConstraint IsConcentricWith(this Circle circ, Circle other) => new ConcentricCirclesConstraint(circ, other);
  public static BaseConstraint IsConcentricWith(this Circle circ, Arc other) => new ConcentricCircArcConstraint(circ, other);
  public static BaseConstraint IsConcentricWith(this Arc arc, Arc other) => new ConcentricArcsConstraint(arc, other);
  public static BaseConstraint IsEqualInRadiusTo(this Circle circ, Circle other) => new EqualRadiusCirclesConstraint(circ, other);
  public static BaseConstraint IsEqualInRadiusTo(this Circle circ, Arc other) => new EqualRadiusCircArcConstraint(circ, other);
  public static BaseConstraint IsEqualInRadiusTo(this Arc arc, Arc other) => new EqualRadiusArcsConstraint(arc, other);
}
