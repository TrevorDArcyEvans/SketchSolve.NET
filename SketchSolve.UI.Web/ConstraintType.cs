namespace SketchSolve.UI.Web;

public enum ConstraintType
{
  // one or more points
  Fixed,
  Free,

  // one or more lines
  Horizontal,
  Vertical,
  
  // exactly two lines
  Parallel,
  Perpendicular,
  Collinear,
  EqualLength,
  
  // one line + one circle
  TangentToCircle,
  
  // one line + one arc
  TangentToArc,
  
  // exactly two points
  Coincident,
  
  // exactly two circles
  // exactly two arcs
  // one circle + one arc
  Concentric
}
