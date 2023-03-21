namespace SketchSolve.UI.Web.Drawing.Model;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.Model;

public class CircleDrawer : EntityDrawer
{
  public Circle Circle { get; }
  public CentrePointDrawer Centre { get; }

  public CircleDrawer(Circle circle)
  {
    Circle = circle;
    Centre = new CentrePointDrawer(Circle.Center);
  }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Centre};

  public override bool IsNear(System.Drawing.Point pt)
  {
    var arc = new Arc(Circle.Center, Circle.Rad, new Parameter(0), new Parameter(2 * Math.PI));
    return pt.IsNear(arc);
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await Centre.DrawAsync(batch);

    await batch.BeginPathAsync();
    await Initialise(batch);

    await batch.ArcAsync(Circle.Center.X.Value, Circle.Center.Y.Value, Circle.Rad.Value, 0d, 2 * Math.PI);
    await batch.StrokeAsync();
  }
}

public sealed class UpdatableCircleDrawer : CircleDrawer
{
  public StartPointDrawer North { get; }
  public StartPointDrawer South { get; }
  public StartPointDrawer East { get; }
  public StartPointDrawer West { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Centre, North, South, East, West};

  public UpdatableCircleDrawer(Circle circle) :
    base(circle)
  {
    North = new StartPointDrawer(new Point(Circle.Center.X.Value, Circle.Center.Y.Value + Circle.Rad.Value));
    South = new StartPointDrawer(new Point(Circle.Center.X.Value, Circle.Center.Y.Value - Circle.Rad.Value));
    East = new StartPointDrawer(new Point(Circle.Center.X.Value + Circle.Rad.Value, Circle.Center.Y.Value));
    West = new StartPointDrawer(new Point(Circle.Center.X.Value - Circle.Rad.Value, Circle.Center.Y.Value));
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await North.DrawAsync(batch);
    await South.DrawAsync(batch);
    await East.DrawAsync(batch);
    await West.DrawAsync(batch);

    await base.DrawAsyncInternal(batch);
  }
}
