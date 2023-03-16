namespace SketchSolve;

public enum ConstraintEnum
{
  PointOnPoint,
  PointOnLine,

  Horizontal,
  Vertical,

  InternalAngle,
  ExternalAngle,

  RadiusValue,

  TangentToArc,
  TangentToCircle,

  ArcRules,

  P2PDistance,
  P2PDistanceVert,
  P2PDistanceHoriz,
  P2LDistance,
  P2LDistanceVert,
  P2LDistanceHoriz,

  LineLength,
  EqualLength,

  ArcRadius,
  EqualRadiusArcs,
  EqualRadiusCircles,
  EqualRadiusCircArc,

  ConcentricArcs,
  ConcentricCircles,
  ConcentricCircArc,

  CircleRadius,

  Parallel,
  Perpendicular,
  Collinear,

  PointOnCircle,
  PointOnArc,
  PointOnLineMidpoint,
  PointOnArcMidpoint,
  PointOnCircleQuad,

  SymmetricPoints,
  SymmetricLines,
  SymmetricCircles,
  SymmetricArcs,
}
