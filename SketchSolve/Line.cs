using System.Collections;

namespace SketchSolve;

public class Line : IEnumerable<Parameter>
{
  public Point p1 = null;
  public Point p2 = null;
  public Vector v1 = null;

  public Line(Point p1, Point p2)
  {
    this.p1 = p1;
    this.p2 = p2;
  }

  public Line(Point p1, Vector v)
  {
    this.p1 = p1;
    v1 = v;
  }

  public override string ToString()
  {
    return "l " + p1 + " : " + p2;
  }

  public Vector Vector
  {
    get
    {
      if (v1 == null)
      {
        return new Vector(dx, dy, false, false);
      }
      else
      {
        return v1;
      }
    }
  }

  private double dx
  {
    get
    {
      return p2.x.Value - p1.x.Value;
    }
  }

  private double dy
  {
    get
    {
      return p2.y.Value - p1.y.Value;
    }
  }


  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    return new[] { p1, p2 }.SelectMany(p => p).GetEnumerator();
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
