namespace SketchSolve.UI.Web.Pages;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SketchSolve.Model;
using SketchSolve.UI.Web.Drawing;
using SketchSolve.UI.Web.Drawing.Model;
using Point = System.Drawing.Point;

public partial class Index
{
  private const string DefaultCursor = "default";
  private const string HorizontalCursor = "ew-resize";
  private const string VerticalCursor = "ns-resize";
  private const string CoincidentCursor = "cell";

  private const string CoincidentImageElementName = "coincident";
  private const string CollinearImageElementName = "collinear";
  private const string ConcentricImageElementName = "concentric";
  private const string EqualImageElementName = "equal";
  private const string FixedImageElementName = "fixed";
  private const string HorizontalImageElementName = "horizontal";
  private const string ParallelImageElementName = "parallel";
  private const string PerpendicularImageElementName = "perpendicular";
  private const string SmoothG2ImageElementName = "smooth_g2";
  private const string SymmetricImageElementName = "symmetric";
  private const string TangentImageElementName = "tangent";
  private const string VerticalImageElementName = "vertical";

  [Inject]
  private IJSRuntime _js { get; set; }

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

  private Point _mouseDown = new(0, 0);
  private Point _currMouse = new(0, 0);

  private bool _isMouseDown = false;

  private ApplicationMode _appMode = ApplicationMode.Select;
  private DrawableEntity _drawEnt;

  private string _cursorStyle = DefaultCursor;

  private readonly List<IDrawable> _drawables = new();

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      CanvasDims = new Point(int.Parse((string)_canvas.AdditionalAttributes["width"]), int.Parse((string)_canvas.AdditionalAttributes["height"]));

      _context = await _canvas.GetContext2DAsync();

      // this retrieves the top left corner of the canvas _container (which is equivalent to the top left corner of the canvas, as we don't have any margins / padding)
      // NOTE: coordinates are returned as doubles
      var pos = await _js.InvokeAsync<PointD>("eval", $"let e = document.querySelector('[_bl_{_container.Id}=\"\"]'); e = e.getBoundingClientRect(); e = {{ 'X': e.x, 'Y': e.y }}; e");
      CanvasPos = new((int)pos.X, (int)pos.Y);
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
    _mouseDown.X = _currMouse.X = (int)(e.ClientX - CanvasPos.X);
    _mouseDown.Y = _currMouse.Y = (int)(e.ClientY - CanvasPos.Y);
    _isMouseDown = true;

    if (_appMode == ApplicationMode.Select)
    {
      // select points under mouse
      _drawables
        .SelectMany(draw => draw.SelectionPoints)
        .ToList()
        .ForEach(pt => pt.IsSelected = pt.Point.IsNear(_currMouse));

      // only select entities under mouse which do not have any points selected
      _drawables
        .Where(draw => !draw.SelectionPoints.Any(pt => pt.IsSelected))
        .ToList()
        .ForEach(draw => draw.IsSelected = draw.IsNear(_currMouse));
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
        var arcDraw = new ArcDrawer(arc);
        _drawables.Add(arcDraw);

        // reset arc creation
        _drawables.Remove(_tempLine);
        _drawables.Remove(_tempArc);
        _tempLine = null;
        _tempArc = null;
        _arcCentre = Point.Empty;
        _arcStart = Point.Empty;

        _appMode = ApplicationMode.Select;
      }
    }
  }

  private void MouseMoveCanvasAsync(MouseEventArgs e)
  {
    _currMouse.X = (int)(e.ClientX - CanvasPos.X);
    _currMouse.Y = (int)(e.ClientY - CanvasPos.Y);

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

      _appMode = ApplicationMode.Select;
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

    _cursorStyle = DefaultCursor;
  }

  private class PointD
  {
    public double X { get; set; }
    public double Y { get; set; }
  }
}
