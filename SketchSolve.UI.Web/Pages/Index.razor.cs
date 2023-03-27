namespace SketchSolve.UI.Web.Pages;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SketchSolve.Constraint;
using SketchSolve.Model;
using SketchSolve.UI.Web.Drawing;
using SketchSolve.UI.Web.Drawing.Model;
using Point = System.Drawing.Point;

public partial class Index
{
  [Inject]
  private IJSRuntime _js { get; set; }

  [Inject]
  private IMatToaster _toaster { get; set; }

  private ElementReference _container;
  private Canvas _canvas;
  private Context2D _context;
  private Point CanvasPos = new(0, 0);
  private Point CanvasDims = new(0, 0);

  private Point _lineStart = Point.Empty;
  private LineDrawer _tempLine;

  private Point _arcCentre = Point.Empty;
  private Point _arcStart = Point.Empty;
  private ArcDrawer _tempArc;

  private Point _circCentre = Point.Empty;
  private CircleDrawer _tempCirc;

  private Point _mouseDown = new(0, 0);
  private Point _currMouse = new(0, 0);

  private bool _isMouseDown;

  private ApplicationMode _appMode = ApplicationMode.Draw;
  private DrawableEntity _drawEnt;

  private ConstraintType _selConstraintType; // = ConstraintType.Fixed;
  private readonly List<BaseConstraint> _constraints = new();

  private readonly List<IDrawable> _drawables = new();

  private bool _canShowPointConstraints;
  private bool _isPtFixed;

  private bool _canShowEntityConstraints;
  private readonly List<BaseConstraint> _selConstraints = new();

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      CanvasDims = new Point(int.Parse((string) _canvas.AdditionalAttributes["width"]), int.Parse((string) _canvas.AdditionalAttributes["height"]));

      _context = await _canvas.GetContext2DAsync();

      // this retrieves the top left corner of the canvas _container (which is equivalent to the top left corner of the canvas, as we don't have any margins / padding)
      // NOTE: coordinates are returned as doubles
      var pos = await _js.InvokeAsync<PointD>("eval", $"let e = document.querySelector('[_bl_{_container.Id}=\"\"]'); e = e.getBoundingClientRect(); e = {{ 'X': e.x, 'Y': e.y }}; e");
      CanvasPos = new((int) pos.X, (int) pos.Y);
    }

    await DrawAsync();
  }

  private async Task DrawAsync()
  {
    await using var batch = _context.CreateBatch();
    await batch.ClearRectAsync(0, 0, CanvasDims.X, CanvasDims.Y);

    // draw what we already have
    foreach (var draw in _drawables)
    {
      await draw.DrawAsync(batch);
    }
  }

  private void MouseDownCanvas(MouseEventArgs e)
  {
    _mouseDown.X = _currMouse.X = (int) (e.ClientX - CanvasPos.X);
    _mouseDown.Y = _currMouse.Y = (int) (e.ClientY - CanvasPos.Y);
    _isMouseDown = true;

    if (_appMode == ApplicationMode.Select)
    {
      // get points under mouse
      var selPtsNearMouse = _drawables
        .SelectMany(draw => draw.SelectionPoints)
        .Where(pt => pt.Point.IsNear(_currMouse))
        .ToList();
      if (selPtsNearMouse.Any())
      {
        // add points under mouse to current selections
        selPtsNearMouse.ForEach(pt => pt.IsSelected = true);
      }


      // only select entities under mouse which do not have any points selected
      var selDrawsNearMouse = _drawables
        .Where(draw => !draw.SelectionPoints.Any(pt => pt.IsSelected))
        .Where(draw => draw.IsNear(_currMouse))
        .ToList();
      if (selDrawsNearMouse.Any())
      {
        // add entities under mouse to current selections
        selDrawsNearMouse.ForEach(draw => draw.IsSelected = true);
      }


      // nothing under mouse, so clear all selections
      if (!selPtsNearMouse.Any() && !selDrawsNearMouse.Any())
      {
        _drawables
          .SelectMany(draw => draw.SelectionPoints)
          .ToList()
          .ForEach(pt => pt.IsSelected = false);
        _drawables
          .ToList()
          .ForEach(draw => draw.IsSelected = false);
      }


      // update point constraints which depends on selection
      var selPts = _drawables
        .SelectMany(draw => draw.SelectionPoints)
        .Where(pt => pt.IsSelected)
        .ToList();
      if (selPts.Count == 1)
      {
        var selPt = selPts.Single();
        _isPtFixed = !selPt.Point.X.Free && !selPt.Point.X.Free;
      }

      _canShowPointConstraints = selPts.Count == 1;


      // update entity constraints which depends on selection
      var selDraws = _drawables
        .Where(draw => draw.IsSelected)
        .ToList();
      if (selDraws.Count == 1)
      {
        // get entity
        var selDrawEnt = selDraws.Single().Entity;

        // get all constraints associate with this entity
        var selDrawEntCons = _constraints
          .Where(cons => cons.Items.Contains(selDrawEnt));
        _selConstraints.Clear();
        _selConstraints.AddRange(selDrawEntCons);
      }

      _canShowEntityConstraints = selDraws.Count == 1;
    }


    // drawing line
    // MouseDown[StartPt] --> drag [update preview] --> MouseUp[EndPt]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Line)
    {
      _lineStart = _mouseDown;
      var startPt = new Model.Point(_lineStart.X, _lineStart.Y);
      var endPt = new Model.Point(e.ClientX - CanvasPos.X, e.ClientY - CanvasPos.Y);
      var line = new Line(startPt, endPt);
      _tempLine = new LineDrawer(line)
      {
        ShowPreview = true
      };
      _drawables.Add(_tempLine);
    }


    // drawing arc
    // MouseDown[CentrePt] --> drag [update line preview] --> MouseUp[StartPt] --> move [update arc preview] --> MouseDown[EndPt]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      if (_arcCentre == Point.Empty)
      {
        _arcCentre = _mouseDown;

        var centrePt = _arcCentre.ToModel();
        var startPt = new Model.Point(e.ClientX - CanvasPos.X, e.ClientY - CanvasPos.Y);
        var line = new Line(centrePt, startPt);
        _tempLine = new LineDrawer(line)
        {
          ShowPreview = true
        };
        _drawables.Add(_tempLine);
      }

      if (_arcCentre != Point.Empty &&
          _arcStart != Point.Empty &&
          _tempArc is not null)
      {
        // finish arc
        var centre = _arcCentre.ToModel();
        var radPt = _arcCentre - new Size(_arcStart);
        var rad = Math.Sqrt(radPt.X * radPt.X + radPt.Y * radPt.Y);
        var radParam = new Parameter(rad);
        var startAngle = Math.Atan2(_arcStart.Y - _arcCentre.Y, _arcStart.X - _arcCentre.X);
        var endAngle = Math.Atan2(_currMouse.Y - _arcCentre.Y, _currMouse.X - _arcCentre.X);
        var startAngleParam = new Parameter(startAngle);
        var endAngleParam = new Parameter(endAngle);
        var arc = new Arc(centre, radParam, startAngleParam, endAngleParam);
        var arcDraw = new UpdatableArcDrawer(arc);
        _drawables.Add(arcDraw);

        // reset arc creation
        _drawables.Remove(_tempLine);
        _drawables.Remove(_tempArc);
        _tempLine = null;
        _tempArc = null;
        _arcCentre = Point.Empty;
        _arcStart = Point.Empty;
      }
    }


    // drawing circle
    // MouseDown[CentrePt] --> drag [update preview] --> MouseUp[Radius]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Circle)
    {
      _circCentre = _mouseDown;
      var circle = new Circle(_circCentre.ToModel(), new Parameter(0));
      _tempCirc = new CircleDrawer(circle)
      {
        ShowPreview = true
      };
      _drawables.Add(_tempCirc);
    }
  }

  private void MouseMoveCanvasAsync(MouseEventArgs e)
  {
    _currMouse.X = (int) (e.ClientX - CanvasPos.X);
    _currMouse.Y = (int) (e.ClientY - CanvasPos.Y);

    // highlight points under mouse
    _drawables
      .SelectMany(draw => draw.SelectionPoints)
      .ToList()
      .ForEach(pt => pt.ShowPreview = pt.Point.IsNear(_currMouse));

    // only highlight entities under mouse which do not have any points highlighted
    _drawables
      .Where(draw => !draw.SelectionPoints.Any(pt => pt.ShowPreview))
      .ToList()
      .ForEach(draw => draw.ShowPreview = draw.IsNear(_currMouse));

    // drawing line
    // MouseDown[StartPt] --> drag [update preview] --> MouseUp[EndPt]
    if (_isMouseDown && _appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Line)
    {
      _tempLine.Line.P2.X.Value = _currMouse.X;
      _tempLine.Line.P2.Y.Value = _currMouse.Y;
    }

    // drawing arc
    // MouseDown[CentrePt] --> drag [update line preview] --> MouseUp[StartPt] --> move [update arc preview] --> MouseDown[EndPt]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      // dragging to arc start point
      if (_isMouseDown && _tempLine is not null)
      {
        _tempLine.Line.P2.X.Value = _currMouse.X;
        _tempLine.Line.P2.Y.Value = _currMouse.Y;
      }

      if (_arcCentre != Point.Empty &&
          _arcStart != Point.Empty &&
          _tempArc is null)
      {
        var centre = _arcCentre.ToModel();
        var radPt = _arcCentre - new Size(_arcStart);
        var rad = Math.Sqrt(radPt.X * radPt.X + radPt.Y * radPt.Y);
        var radParam = new Parameter(rad);
        var startAngle = Math.Atan2(_arcStart.Y - _arcCentre.Y, _arcStart.X - _arcCentre.X);
        var endAngle = Math.Atan2(_currMouse.Y - _arcCentre.Y, _currMouse.X - _arcCentre.X);
        var startAngleParam = new Parameter(startAngle);
        var endAngleParam = new Parameter(endAngle);
        var arc = new Arc(centre, radParam, startAngleParam, endAngleParam);
        _tempArc = new ArcDrawer(arc);
        _drawables.Add(_tempArc);
      }

      if (_arcCentre != Point.Empty &&
          _arcStart != Point.Empty &&
          _tempArc is not null)
      {
        var endAngle = Math.Atan2(_currMouse.Y - _arcCentre.Y, _currMouse.X - _arcCentre.X);
        _tempArc.Arc.EndAngle.Value = endAngle;
      }
    }

    // drawing circle
    // MouseDown[CentrePt] --> drag [update preview] --> MouseUp[Radius]
    if (_isMouseDown && _appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Circle)
    {
      var currMouse = new Point(_currMouse.X, _currMouse.Y);
      var radVec = _tempCirc.Circle.Center - currMouse.ToModel();
      _tempCirc.Circle.Rad.Value = radVec.Length;
    }

    // drag selected points
    if (_isMouseDown && _appMode == ApplicationMode.Select)
    {
      _drawables
        .SelectMany(draw => draw.SelectionPoints)
        .Where(pt => pt.IsSelected)
        .ToList()
        .ForEach(pt =>
        {
          pt.Point.X.Value = _currMouse.X;
          pt.Point.Y.Value = _currMouse.Y;
        });
    }
  }

  private void MouseUpCanvas(MouseEventArgs e)
  {
    _isMouseDown = false;

    // clear previews
    _drawables
      .ToList()
      .ForEach(draw => draw.ShowPreview = false);
    _drawables
      .SelectMany(draw => draw.SelectionPoints)
      .ToList()
      .ForEach(pt => pt.ShowPreview = false);

    // drawing line
    // MouseDown[StartPt] --> drag [update preview] --> MouseUp[EndPt]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Line)
    {
      // finish line
      var startPt = _lineStart.ToModel();
      var endPt = new Model.Point(e.ClientX - CanvasPos.X, e.ClientY - CanvasPos.Y);
      var line = new Line(startPt, endPt);
      var lineDrawer = new LineDrawer(line);
      _drawables.Add(lineDrawer);

      // reset line creation
      _drawables.Remove(_tempLine);
      _tempLine = null;
      _lineStart = Point.Empty;
    }

    // drawing arc
    // MouseDown[CentrePt] --> drag [update line preview] --> MouseUp[StartPt] --> move [update arc preview] --> MouseDown[EndPt]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      if (_arcStart == Point.Empty)
      {
        _arcStart = _currMouse;
      }
    }

    // drawing circle
    // MouseDown[CentrePt] --> drag [update preview] --> MouseUp[Radius]
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Circle)
    {
      // finish circle
      var currMouse = new Point(_currMouse.X, _currMouse.Y);
      var radVec = _tempCirc.Circle.Center - currMouse.ToModel();
      var circle = new Circle(_circCentre.ToModel(), new Parameter(radVec.Length));
      var circDrawer = new UpdatableCircleDrawer(circle);
      _drawables.Add(circDrawer);

      // reset circle creation
      _drawables.Remove(_tempCirc);
      _tempCirc = null;
      _circCentre = Point.Empty;
    }
  }

  private void OnDelete()
  {
    var selected = _drawables
      .Where(draw => draw.IsSelected)
      .ToList();

    foreach (var draw in selected)
    {
      _drawables.Remove(draw);
    }

    _constraints.Clear();
  }

  private void OnApply()
  {
    switch (_selConstraintType)
    {
      case ConstraintType.Free:
      case ConstraintType.Fixed:
      {
        var isFree = _selConstraintType == ConstraintType.Free;
        _drawables
          .SelectMany(draw => draw.SelectionPoints)
          .Where(pt => pt.IsSelected)
          .ToList()
          .ForEach(pt => pt.Point.X.Free = pt.Point.Y.Free = isFree);
      }
        break;

      case ConstraintType.Vertical:
      case ConstraintType.Horizontal:
      {
        var constraints = _drawables
          .OfType<LineDrawer>()
          .Where(ine => ine.IsSelected)
          .Select(line => _selConstraintType == ConstraintType.Vertical ? line.Line.IsVertical() : line.Line.IsHorizontal());
        _constraints.AddRange(constraints);
      }
        break;

      case ConstraintType.Parallel:
      case ConstraintType.Perpendicular:
      case ConstraintType.Collinear:
      case ConstraintType.EqualLength:
      {
        var selDraws = _drawables
          .OfType<LineDrawer>()
          .Where(line => line.IsSelected)
          .ToList();
        if (selDraws.Count != 2)
        {
          return;
        }

        var line1 = selDraws[0].Line;
        var line2 = selDraws[1].Line;
        var cons = _selConstraintType switch
        {
          ConstraintType.Parallel => line1.IsParallelTo(line2),
          ConstraintType.Perpendicular => line1.IsPerpendicularTo(line2),
          ConstraintType.Collinear => line1.IsCollinearTo(line2),
          ConstraintType.EqualLength => line1.IsEqualInLengthTo(line2),
          _ => throw new ArgumentOutOfRangeException()
        };
        _constraints.Add(cons);
      }
        break;

      case ConstraintType.Tangent:
      {
        var selLines = _drawables
          .OfType<LineDrawer>()
          .Where(line => line.IsSelected)
          .ToList();
        var selCircs = _drawables
          .OfType<CircleDrawer>()
          .Where(circ => circ.IsSelected)
          .ToList();
        var selArcs = _drawables
          .OfType<ArcDrawer>()
          .Where(arc => arc.IsSelected)
          .ToList();

        if (selLines.Count != 1)
        {
          return;
        }

        BaseConstraint cons = null;

        if (selCircs.Count == 1 && selArcs.Count != 1)
        {
          cons = selLines[0].Line.IsTangentTo(selCircs[0].Circle);
        }

        if (selCircs.Count != 1 && selArcs.Count == 1)
        {
          cons = selLines[0].Line.IsTangentTo(selArcs[0].Arc);
        }

        if (cons is null)
        {
          return;
        }

        _constraints.Add(cons);
      }
        break;

      case ConstraintType.Coincident:
      {
        var selPts = _drawables
          .SelectMany(draw => draw.SelectionPoints)
          .Where(pt => pt.IsSelected)
          .ToList();
        if (selPts.Count != 2)
        {
          return;
        }

        var cons = selPts[0].Point.IsCoincidentWith(selPts[1].Point);
        _constraints.Add(cons);
      }
        break;

      case ConstraintType.Concentric:
      {
        var selCircs = _drawables
          .OfType<CircleDrawer>()
          .Where(circ => circ.IsSelected)
          .ToList();
        var selArcs = _drawables
          .OfType<ArcDrawer>()
          .Where(arc => arc.IsSelected)
          .ToList();

        BaseConstraint cons = null;

        if (selCircs.Count == 2 && selArcs.Count == 0)
        {
          cons = selCircs[0].Circle.IsConcentricWith(selCircs[1].Circle);
        }

        if (selCircs.Count == 1 && selArcs.Count == 1)
        {
          cons = selCircs[0].Circle.IsConcentricWith(selArcs[0].Arc);
        }

        if (selCircs.Count == 0 && selArcs.Count == 2)
        {
          cons = selArcs[0].Arc.IsConcentricWith(selArcs[1].Arc);
        }

        if (cons is null)
        {
          return;
        }

        _constraints.Add(cons);
      }
        break;

      default:
        throw new ArgumentOutOfRangeException();
    }

    _toaster.Add(_selConstraintType.ToString(), MatToastType.Info, "Added constraint");
  }

  private void OnClearAll()
  {
    _constraints.Clear();
    _toaster.Add("No constraints in system", MatToastType.Info, "Cleared constraints");
  }

  private void OnSolve()
  {
    var error = Solver.Solver.Solve(constraints: _constraints.ToArray());
    _toaster.Add($"Error: {error:E3}", MatToastType.Info, "Solver completed");
  }

  private void OnDeleteSelectedPointConstraint()
  {
    var selPt = _drawables
      .SelectMany(draw => draw.SelectionPoints)
      .Single(pt => pt.IsSelected);
    selPt.Point.X.Free = selPt.Point.Y.Free = true;
    _isPtFixed = false;
  }

  private void OnDeleteSelectedEntityConstraint(BaseConstraint cons)
  {
    _ = _constraints.Remove(cons);
    _ = _selConstraints.Remove(cons);
  }

  private class PointD
  {
    public double X { get; set; }
    public double Y { get; set; }
  }
}
