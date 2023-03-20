namespace SketchSolve.UI.Web.Drawing.Model;

using System;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public sealed class StartPointDrawer : PointDrawer
{
  public StartPointDrawer(Point point) :
    base(point)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await batch.ArcAsync(Point.X.Value, Point.Y.Value, CircleRadius, 0, 2 * Math.PI);
  }
}
