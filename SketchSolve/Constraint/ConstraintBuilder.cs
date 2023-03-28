namespace SketchSolve.Constraint;

using SketchSolve.Model;

public static class ConstraintBuilder
{
  public static BaseConstraint IsHorizontal(this Line line) => new HorizontalConstraint(line);
  public static BaseConstraint IsVertical(this Line line) => new VerticalConstraint(line);

  public static BaseConstraint IsCoincidentWith(this Point pt, Point other) => new PointOnPointConstraint(pt, other);
  public static BaseConstraint IsCoincidentWith(this Point pt, Line other) => new PointOnLineConstraint(pt, other);
  public static BaseConstraint IsCoincidentWith(this Point pt, Circle other) => new PointOnCircleConstraint(pt, other);
  public static BaseConstraint IsCoincidentWith(this Point pt, Arc other) => new PointOnArcConstraint(pt, other);

  public static BaseConstraint IsCoincidentWithMidPoint(this Point pt, Line other) => new PointOnLineMidpointConstraint(pt, other);
  public static BaseConstraint IsCoincidentWithMidPoint(this Point pt, Arc other) => new PointOnArcMidpointConstraint(pt, other);

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

  public static BaseConstraint HasLength(this Line line, int length) => new LineLengthConstraint(line, new Parameter(length, false));
  public static BaseConstraint HasRadius(this Circle circ, int radius) => new CircleRadiusConstraint(circ, new Parameter(radius, false));
  public static BaseConstraint HasRadius(this Arc arc, int radius) => new ArcRadiusConstraint(arc, new Parameter(radius, false));

  public static BaseConstraint HasDistance(this Point pt, Point other, int dist) => new P2PDistanceConstraint(pt, other, new Parameter(dist, false));
  public static BaseConstraint HasDistance(this Point pt, Line other, int dist) => new P2LDistanceConstraint(pt, other, new Parameter(dist, false));
  public static BaseConstraint HasDistanceHorizontal(this Point pt, Point other, int dist) => new P2PDistanceHorizConstraint(pt, other, new Parameter(dist, false));
  public static BaseConstraint HasDistanceHorizontal(this Point pt, Line other, int dist) => new P2LDistanceHorizConstraint(pt, other, new Parameter(dist, false));
  public static BaseConstraint HasDistanceVertical(this Point pt, Point other, int dist) => new P2PDistanceVertConstraint(pt, other, new Parameter(dist, false));
  public static BaseConstraint HasDistanceVertical(this Point pt, Line other, int dist) => new P2LDistanceVertConstraint(pt, other, new Parameter(dist, false));
}
