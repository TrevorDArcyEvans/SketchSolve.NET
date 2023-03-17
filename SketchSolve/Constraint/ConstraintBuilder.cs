namespace SketchSolve;

public static class ConstraintBuilder
{
  public static Constraint IsHorizontal(this Line line)
  {
    return new HorizontalConstraint(line);
  }

  public static Constraint IsVertical(this Line line)
  {
    return new VerticalConstraint(line);
  }

  public static Constraint IsCoincidentWith(this Point pt, Point other)
  {
    return new PointOnPointConstraint(pt, other);
  }

  public static Constraint HasInternalAngle(this Line line, Line other, Parameter angle)
  {
    return new InternalAngleConstraint(line, other, angle);
  }

  public static Constraint HasExternalAngle(this Line line, Line other, Parameter angle)
  {
    return new ExternalAngleConstraint(line, other, angle);
  }

  public static Constraint IsPerpendicularTo(this Line line, Line other)
  {
    return new PerpendicularConstraint(line, other);
  }

  public static Constraint IsTangentTo(this Line line, Circle other)
  {
    return new TangentToCircleConstraint(line, other);
  }
}
