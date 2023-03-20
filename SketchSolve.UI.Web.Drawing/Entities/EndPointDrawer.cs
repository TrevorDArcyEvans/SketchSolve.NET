namespace SketchSolve.UI.Web.Drawing.Entities;

using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public sealed class EndPointDrawer : PointDrawer
{
  public EndPointDrawer(Point point) :
    base(point)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await batch.RectAsync(Point.X.Value - RectSize / 2,  Point.Y.Value - RectSize / 2, RectSize, RectSize);
  }
}
