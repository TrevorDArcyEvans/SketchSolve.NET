namespace SketchSolve.UI.Web.Drawing.Model;

using System;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public sealed class UpdatableArcDrawer : ArcDrawer
{
  public UpdatableArcDrawer(Arc arc) :
    base(arc)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    // update underlying Arc
    // NOTE:  Arc is defined by StartAngle+EndAngle, so Start+End will not sit on Arc
    var startAngle = Math.Atan2(Start.Point.Y.Value - Centre.Point.Y.Value, Start.Point.X.Value - Centre.Point.X.Value);
    var endAngle = Math.Atan2(End.Point.Y.Value - Centre.Point.Y.Value, End.Point.X.Value - Centre.Point.X.Value);
    Arc.StartAngle.Value = startAngle;
    Arc.EndAngle.Value = endAngle;

    await base.DrawAsyncInternal(batch);
  }
}
