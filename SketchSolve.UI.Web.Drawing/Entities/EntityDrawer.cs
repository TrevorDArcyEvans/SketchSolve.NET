namespace SketchSolve.UI.Web.Drawing.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;

public abstract class EntityDrawer : IDrawable
{
  public const string DefaultClr = "black";
  public const string PreviewClr = "blue";
  public const string SelectedClr = "orange";

  public const double CircleRadius = 8d;
  public const double RectSize = 2 * CircleRadius;

  public bool IsSelected { get; set; }
  public bool ShowPreview { get; set; }

  public abstract IEnumerable<PointDrawer> SelectionPoints { get; }

  public async Task DrawAsync(Batch2D batch)
  {
    await batch.BeginPathAsync();
    await Initialise(batch);
    await DrawAsyncInternal(batch);
    await batch.StrokeAsync();
  }

  protected async Task Initialise(Batch2D batch)
  {
    await batch.GlobalCompositeOperationAsync(CompositeOperation.Source_Over);
    await SetLine(batch);
    await SetColour(batch);
  }

  protected virtual async Task SetLine(Batch2D batch)
  {
    if (ShowPreview)
    {
      await batch.LineWidthAsync(2);
    }
    else if (IsSelected)
    {
      await batch.LineWidthAsync(2);
    }
    else
    {
      await batch.LineWidthAsync(1);
    }
    await batch.LineJoinAsync(LineJoin.Round);
    await batch.LineCapAsync(LineCap.Round);
  }

  protected virtual async Task SetColour(Batch2D batch)
  {
    if (ShowPreview)
    {
      await batch.StrokeStyleAsync(PreviewClr);
    }
    else if (IsSelected)
    {
      await batch.StrokeStyleAsync(SelectedClr);
    }
    else
    {
      await batch.StrokeStyleAsync(DefaultClr);
    }
  }

  protected abstract Task DrawAsyncInternal(Batch2D batch);
}