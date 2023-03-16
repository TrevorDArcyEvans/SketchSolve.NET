namespace SketchSolve;

using System.Collections;

public class Line : IEnumerable<Parameter>
{
  public readonly Point P1;
  public readonly Point P2;
  public readonly Vector V1;

  public Line(Point p1, Point p2)
  {
    P1 = p1;
    P2 = p2;
  }

  public Line(Point p1, Vector v)
  {
    P1 = p1;
    V1 = v;
  }

  public override string ToString()
  {
    return "l " + P1 + " : " + P2;
  }

  public Vector Vector
  {
    get
    {
      if (V1 == null)
      {
        return new Vector(dx, dy, false, false);
      }
      else
      {
        return V1;
      }
    }
  }

  private double dx => P2.X.Value - P1.X.Value;

  private double dy => P2.Y.Value - P1.Y.Value;

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    return new[] { P1, P2 }.SelectMany(p => p).GetEnumerator();
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
