namespace SketchSolve.Constraint;

using SketchSolve.Model;

public static class ConstraintBuilder
{
  public static Constraint IsHorizontal(this Line line) => new HorizontalConstraint(line);
  public static Constraint IsVertical(this Line line) => new VerticalConstraint(line);
  public static Constraint IsCoincidentWith(this Point pt, Point other) => new PointOnPointConstraint(pt, other);
  public static Constraint HasInternalAngle(this Line line, Line other, Parameter angle) => new InternalAngleConstraint(line, other, angle);
  public static Constraint HasExternalAngle(this Line line, Line other, Parameter angle) => new ExternalAngleConstraint(line, other, angle);
  public static Constraint IsPerpendicularTo(this Line line, Line other) => new PerpendicularConstraint(line, other);
  public static Constraint IsTangentTo(this Line line, Circle other) => new TangentToCircleConstraint(line, other);
}
