namespace SketchSolve.UI.Web.Drawing.Entities;

using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public sealed class ArcDrawer : EntityDrawer
{
  private Arc Arc { get; }
  private StartPointDrawer Start { get; }
  private CentrePointDrawer Centre { get; }
  private EndPointDrawer End { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] { Start, Centre, End };

  public ArcDrawer(Arc arc)
  {
    var startVec = new Vector(Math.Cos(Arc.StartAngle.Value), Math.Sin(Arc.StartAngle.Value));
    var startPt = Arc.Center + Arc.Rad.Value * startVec;
    var endVec = new Vector(Math.Cos(Arc.EndAngle.Value), Math.Sin(Arc.EndAngle.Value));
    var endPt = Arc.Center + Arc.Rad.Value * endVec;
    Arc = arc;
    Start = new StartPointDrawer(startPt);
    Centre = new CentrePointDrawer(Arc.Center);
    End = new EndPointDrawer(endPt);
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await Centre.DrawAsync(batch);
    await Start.DrawAsync(batch);

    await batch.BeginPathAsync();
    await Initialise(batch);

    var drawCCW = DrawArcCounterClockwise(Arc.StartAngle.Value, Arc.EndAngle.Value);
    await batch.ArcAsync(Arc.Center.X.Value, Arc.Center.Y.Value, Arc.Rad.Value, Arc.StartAngle.Value, Arc.EndAngle.Value, drawCCW);
    await batch.StrokeAsync();

    await End.DrawAsync(batch);
  }

  private static bool DrawArcCounterClockwise(double startAngle, double endAngle)
  {
    // Quadrants are defined here:
    //
    //    https://www.w3schools.com/tags/canvas_arc.asp
    //
    //             |
    //          3  |  4
    //        -----------
    //          2  |  1
    //             |
    //
    // However, Math.atan2() returns angles as:
    //
    //                      |
    //         [-pi/2, -pi] |  [0, -pi/2]
    //       ------------------------------
    //         [+pi/2, +pi] |  [0, +pi/2]
    //                      |
    //
    // rather than in the range [0, 2pi]
    //
    // This leads to chirality issues when the endpoint crosses the X axis
    // and the endAngle flips from -pi to +pi

    // quadrant 1
    if (0 < startAngle && startAngle <= Math.PI / 2)
    {
      if (endAngle + Math.PI < startAngle)
      {
        return false;
      }

      return (endAngle - startAngle) < 0;
    }

    // quadrant 2
    if (Math.PI / 2 < startAngle && startAngle <= Math.PI)
    {
      if (endAngle < Math.PI &&
          endAngle > startAngle)
      {
        return false;
      }

      if (endAngle < startAngle - Math.PI)
      {
        return false;
      }

      return startAngle - endAngle < Math.PI || endAngle < startAngle;
    }

    // quadrant 3
    if (-Math.PI < startAngle && startAngle <= -Math.PI / 2)
    {
      if (endAngle > startAngle + Math.PI)
      {
        return true;
      }

      if (endAngle < startAngle && endAngle < 0)
      {
        return true;
      }

      return false;
    }

    // quadrant 4
    if (-Math.PI / 2 < startAngle && startAngle <= 0)
    {
      if (startAngle + Math.PI > endAngle &&
          endAngle > startAngle)
      {
        return false;
      }

      if (endAngle < startAngle)
      {
        return true;
      }

      return true;
    }

    return default;
  }
}
