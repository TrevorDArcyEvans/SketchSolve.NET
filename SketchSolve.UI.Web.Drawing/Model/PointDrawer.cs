namespace SketchSolve.UI.Web.Drawing.Model;

using System.Collections.Generic;
using SketchSolve.Model;

public abstract class PointDrawer : EntityDrawer
{
  public Point Point { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new [] { this };

  public PointDrawer(Point point)
  {
    Point = point;
  }
}
