namespace SketchSolve.UI.Web.Pages;

using System.Collections.Generic;
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

  private Point _lineStart;
  private LineDrawer _tempLine;

  private Point _arcCentre;
  private Point _arcStart;
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
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      if (_arcCentre == Point.Empty)
      {
        _arcCentre = _mouseDown;
      }
      else
      {
        _arcStart = _mouseDown;
      }
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
    if (_isMouseDown && _appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Line)
    {
      _tempLine.Line.P2.X.Value = _currMouse.X;
      _tempLine.Line.P2.Y.Value = _currMouse.Y;
    }

    // drawing arc
    if (_isMouseDown && _appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      if (_arcCentre != Point.Empty && _arcStart != Point.Empty && _tempArc is null)
      {
        // TODO   create ArcDrawer
        _drawables.Add(_tempArc);
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
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Line)
    {
      _drawables.Remove(_tempLine);

      var startPt = _lineStart.ToModel();
      var endPt = new Model.Point(e.ClientX - CanvasPos.X, e.ClientY - CanvasPos.Y);
      var line = new Line(startPt, endPt);
      var lineDrawer = new LineDrawer(line);
      _drawables.Add(lineDrawer);

      _appMode = ApplicationMode.Select;
    }

    // drawing arc
    if (_appMode == ApplicationMode.Draw && _drawEnt == DrawableEntity.Arc)
    {
      _drawables.Remove(_tempArc);

      // TODO   finish arc
      _arcCentre = Point.Empty;
      _arcStart = Point.Empty;
    }

    _cursorStyle = DefaultCursor;
  }

  private class PointD
  {
    public double X { get; set; }
    public double Y { get; set; }
  }
}
