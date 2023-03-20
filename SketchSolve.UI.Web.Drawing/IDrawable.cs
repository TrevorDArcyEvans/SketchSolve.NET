namespace SketchSolve.UI.Web.Drawing;

using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using SketchSolve.UI.Web.Drawing.Model;

public interface IDrawable
{
  bool ShowPreview { get; set; }
  bool IsSelected { get; set; }
  IEnumerable<PointDrawer> SelectionPoints { get; }
  Task DrawAsync(Batch2D batch);
}
