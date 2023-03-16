namespace SketchSolve;

public static class ConstraintBuilderMixins
{
  public static Constraint IsHorizontal(this Line This)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Horizontal,
      Line1 = This
    };
  }

  public static Constraint IsVertical(this Line This)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Vertical,
      Line1 = This
    };
  }

  public static Constraint IsColocated(this Point This, Point other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.PointOnPoint,
      Point1 = This,
      Point2 = other
    };
  }

  public static Constraint HasInternalAngle(this Line This, Line other, Parameter angle)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.InternalAngle,
      Line1 = This,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint HasExternalAngle(this Line This, Line other, Parameter angle)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.ExternalAngle,
      Line1 = This,
      Line2 = other,
      Parameter = angle
    };
  }

  public static Constraint IsPerpendicularTo(this Line This, Line other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.Perpendicular,
      Line1 = This,
      Line2 = other,
    };
  }

  public static Constraint IsTangentTo(this Line This, Circle other)
  {
    return new Constraint
    {
      ContraintType = ConstraintEnum.TangentToCircle,
      Line1 = This,
      Circle1 = other,
    };
  }
}
