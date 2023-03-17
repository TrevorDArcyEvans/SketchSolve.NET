namespace SketchSolve.Model;

using System.Collections;

public sealed class Line : IEnumerable<Parameter>
{
  public readonly Point P1;
  public readonly Point P2;
  public Vector Vector => new (dX, dY, false, false);

  public Line(Point p1, Point p2)
  {
    P1 = p1;
    P2 = p2;
  }

  public override string ToString()
  {
    return "L " + P1 + " : " + P2;
  }

  private double dX => P2.X.Value - P1.X.Value;

  private double dY => P2.Y.Value - P1.Y.Value;

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    return new[] {P1, P2}.SelectMany(p => p).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
