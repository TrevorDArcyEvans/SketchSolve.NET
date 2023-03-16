namespace SketchSolve;

public static class ConstraintBuilderMixins
{
  public static Constraint IsHorizontal(this Line This)
  {
    return new Constraint
    {
      type = ConstraintEnum.Horizontal,
      line1 = This
    };
  }

  public static Constraint IsVertical(this Line This)
  {
    return new Constraint
    {
      type = ConstraintEnum.Vertical,
      line1 = This
    };
  }

  public static Constraint IsColocated(this Point This, Point other)
  {
    return new Constraint
    {
      type = ConstraintEnum.PointOnPoint,
      point1 = This,
      point2 = other
    };
  }

  public static Constraint HasInternalAngle(this Line This, Line other, Parameter angle)
  {
    return new Constraint
    {
      type = ConstraintEnum.InternalAngle,
      line1 = This,
      line2 = other,
      parameter = angle
    };
  }

  public static Constraint HasExternalAngle(this Line This, Line other, Parameter angle)
  {
    return new Constraint
    {
      type = ConstraintEnum.ExternalAngle,
      line1 = This,
      line2 = other,
      parameter = angle
    };
  }

  public static Constraint IsPerpendicularTo(this Line This, Line other)
  {
    return new Constraint
    {
      type = ConstraintEnum.Perpendicular,
      line1 = This,
      line2 = other,
    };
  }

  public static Constraint IsTangentTo(this Line This, Circle other)
  {
    return new Constraint
    {
      type = ConstraintEnum.TangentToCircle,
      line1 = This,
      circle1 = other,
    };
  }
}
