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
    // update StartAngle+EndAngle based on new Start+End
    var startAngle = Math.Atan2(Start.Point.Y.Value - Centre.Point.Y.Value, Start.Point.X.Value - Centre.Point.X.Value);
    var endAngle = Math.Atan2(End.Point.Y.Value - Centre.Point.Y.Value, End.Point.X.Value - Centre.Point.X.Value);
    Arc.StartAngle.Value = startAngle;
    Arc.EndAngle.Value = endAngle;

    // update Rad as Start or End might have changed
    const double Tolerance = 1e-5;
    var startRadVec = Start.Point - Centre.Point;
    var startRad = startRadVec.Length;
    var endRadVec = End.Point - Centre.Point;
    var endRad = endRadVec.Length;
    if (Math.Abs(Arc.Rad.Value - startRad) > Tolerance)
    {
      Arc.Rad.Value = startRad;
    }
    else if (Math.Abs(Arc.Rad.Value - endRad) > Tolerance)
    {
      Arc.Rad.Value = endRad;
    }

    // update Start+End to sit on Arc
    var startVec = new Vector(Math.Cos(Arc.StartAngle.Value), Math.Sin(Arc.StartAngle.Value));
    var startPt = Arc.Center + Arc.Rad.Value * startVec;
    Start.Point.X.Value = startPt.X.Value;
    Start.Point.Y.Value = startPt.Y.Value;

    var endVec = new Vector(Math.Cos(Arc.EndAngle.Value), Math.Sin(Arc.EndAngle.Value));
    var endPt = Arc.Center + Arc.Rad.Value * endVec;
    End.Point.X.Value = endPt.X.Value;
    End.Point.Y.Value = endPt.Y.Value;

    await base.DrawAsyncInternal(batch);
  }
}
