namespace SketchSolve.Model;

using System.Collections;

public class Line : IEnumerable<Parameter>
{
  public readonly Point P1;
  public readonly Point P2;
  public Vector Vector { get; }

  public Line(Point p1, Point p2)
  {
    P1 = p1;
    P2 = p2;
    Vector = new Vector(dX, dY, false, false);
  }

  public override string ToString()
  {
    return "l " + P1 + " : " + P2;
  }

  private double dX => P2.X.Value - P1.X.Value;

  private double dY => P2.Y.Value - P1.Y.Value;

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    return new[] {P1, P2}.SelectMany(p => p).GetEnumerator();
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
