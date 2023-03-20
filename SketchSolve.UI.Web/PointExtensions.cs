namespace SketchSolve.UI.Web;

using System.Drawing;

public static class PointExtensions
{
  private const int ToleranceRadius = 16;

  public static bool IsNear(this Point pt, Point other)
  {
    var diff = pt - new Size(other);
    var distSq = diff.X * diff.X + diff.Y * diff.Y;
    return distSq < ToleranceRadius;
  }

  public static bool IsNear(this SketchSolve.Model.Point pt, Point other)
  {
    var dX = pt.X.Value - other.X;
    var dY = pt.Y.Value - other.Y;
    var distSq = dX * dX + dY * dY;
    return distSq < ToleranceRadius;
  }
}
