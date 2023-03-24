namespace SketchSolve.UI.Web.Drawing;

using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.UI.Web.Drawing.Model;

public interface IDrawable
{
  bool ShowPreview { get; set; }
  bool IsSelected { get; set; }
  IEnumerable<PointDrawer> SelectionPoints { get; }
  object Entity { get; }
  bool IsNear(Point pt);
  Task DrawAsync(Batch2D batch);
}
