using System;

namespace SketchSolve.UI.Web.Drawing.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public sealed class LineDrawer : EntityDrawer
{
  public Line Line { get; }
  private StartPointDrawer Start { get; }
  private EndPointDrawer End { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Start, End};

  public override bool IsNear(System.Drawing.Point pt)
  {
    return pt.IsNear(Line);
  }

  public LineDrawer(Line line)
  {
    Line = line;
    Start = new StartPointDrawer(Line.P1);
    End = new EndPointDrawer(Line.P2);
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await Start.DrawAsync(batch);

    await batch.BeginPathAsync();
    await Initialise(batch);

    await batch.MoveToAsync(Line.P1.X.Value, Line.P1.Y.Value);
    await batch.LineToAsync(Line.P2.X.Value, Line.P2.Y.Value);
    await batch.StrokeAsync();

    await End.DrawAsync(batch);
  }
}
