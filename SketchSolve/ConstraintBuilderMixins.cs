namespace SketchSolve;

public static class ConstraintBuilderMixins
{
  public static Constraint IsHorizontal(this Line line)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Horizontal,
      Line1 = line
    };
  }

  public static Constraint IsVertical(this Line line)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Vertical,
      Line1 = line
    };
  }

  public static Constraint IsColocated(this Point pt, Point other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.PointOnPoint,
      Point1 = pt,
      Point2 = other
    };
  }

  public static Constraint HasInternalAngle(this Line line, Line other, Parameter angle)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.InternalAngle,
      Line1 = line,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint HasExternalAngle(this Line line, Line other, Parameter angle)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.ExternalAngle,
      Line1 = line,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint IsPerpendicularTo(this Line line, Line other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Perpendicular,
      Line1 = line,
      Line2 = other,
    };
  }

  public static Constraint IsTangentTo(this Line line, Circle other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.TangentToCircle,
      Line1 = line,
      Circle1 = other,
    };
  }
}
