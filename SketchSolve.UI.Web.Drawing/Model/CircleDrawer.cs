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
    const double Tolerance = 1e-5;
    var nVec = North.Point - Circle.Center;
    var sVec = South.Point - Circle.Center;
    var eVec = East.Point - Circle.Center;
    var wVec = West.Point - Circle.Center;

    // if Center has moved, then *all* of NSEW will be inside/outside a Rad from Center
    if (!(Math.Abs(Circle.Rad.Value - nVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Rad.Value - sVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Rad.Value - eVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Rad.Value - wVec.Length) > Tolerance))
    {
      // update Rad based on new NSEW
      if (Math.Abs(Circle.Rad.Value - nVec.Length) > Tolerance)
      {
        Circle.Rad.Value = nVec.Length;
      }
      else if (Math.Abs(Circle.Rad.Value - sVec.Length) > Tolerance)
      {
        Circle.Rad.Value = sVec.Length;
      }
      else if (Math.Abs(Circle.Rad.Value - eVec.Length) > Tolerance)
      {
        Circle.Rad.Value = eVec.Length;
      }
      else if (Math.Abs(Circle.Rad.Value - wVec.Length) > Tolerance)
      {
        Circle.Rad.Value = wVec.Length;
      }
    }

    // update NSEW to sit on Circle
    North.Point.X.Value = Circle.Center.X.Value;
    North.Point.Y.Value = Circle.Center.Y.Value + Circle.Rad.Value;
    South.Point.X.Value = Circle.Center.X.Value;
    South.Point.Y.Value = Circle.Center.Y.Value - Circle.Rad.Value;
    East.Point.X.Value = Circle.Center.X.Value + Circle.Rad.Value;
    East.Point.Y.Value = Circle.Center.Y.Value;
    West.Point.X.Value = Circle.Center.X.Value - Circle.Rad.Value;
    West.Point.Y.Value = Circle.Center.Y.Value;

    await North.DrawAsync(batch);
    await South.DrawAsync(batch);
    await East.DrawAsync(batch);
    await West.DrawAsync(batch);

    await base.DrawAsyncInternal(batch);
  }
}
