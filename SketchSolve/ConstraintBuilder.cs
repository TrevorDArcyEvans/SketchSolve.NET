namespace SketchSolve;

public static class ConstraintBuilder
{
  public static Constraint IsHorizontal(this Line line)
  {
    return new HorizontalConstraint
    {
      Line1 = line
    };
  }

  public static Constraint IsVertical(this Line line)
  {
    return new VerticalConstraint
    {
      Line1 = line
    };
  }

  public static Constraint IsColocated(this Point pt, Point other)
  {
    return new PointOnPointConstraint
    {
      Point1 = pt,
      Point2 = other
    };
  }

  public static Constraint HasInternalAngle(this Line line, Line other, Parameter angle)
  {
    return new InternalAngleConstraint
    {
      Line1 = line,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint HasExternalAngle(this Line line, Line other, Parameter angle)
  {
    return new ExternalAngleConstraint
    {
      Line1 = line,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint IsPerpendicularTo(this Line line, Line other)
  {
    return new PerpendicularConstraint
    {
      Line1 = line,
      Line2 = other,
    };
  }

  public static Constraint IsTangentTo(this Line line, Circle other)
  {
    return new TangentToCircleConstraint
    {
      Line1 = line,
      Circle1 = other,
    };
  }
}
