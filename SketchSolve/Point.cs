namespace SketchSolve;

using System.Collections;

public class Point : IEnumerable<Parameter>
{
  public readonly Parameter X;
  public readonly Parameter Y;

  public Point(double x, double y, bool freex, bool freey)
  {
    X = new Parameter(x, freex);
    Y = new Parameter(y, freey);
  }

  public Point(double x, double y, bool free = true)
  {
    X = new Parameter(x, free);
    Y = new Parameter(y, free);
  }

  public override string ToString()
  {
    return X.Value + ";" + Y.Value;
  }

  public static Point operator +(Point a, Vector b)
  {
    return new Point(a.X.Value + b.dX.Value, a.Y.Value + b.dY.Value);
  }

  public static Point operator -(Point a, Vector b)
  {
    return new Point(a.X.Value - b.dX.Value, a.Y.Value - b.dY.Value);
  }

  public static Vector operator -(Point a, Point b)
  {
    return new Vector(a.X.Value - b.X.Value, a.Y.Value - b.Y.Value);
  }

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    yield return X;
    yield return Y;
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
