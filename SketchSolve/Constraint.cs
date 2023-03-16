namespace SketchSolve;

using System.Collections;

public class Constraint : IEnumerable<Parameter>
{
  public ConstraintEnum ContraintType;

  public Point Point1;
  public Point Point2;
  public Line Line1;
  public Line Line2;
  public Line SymLine;
  public Circle Circle1;
  public Circle Circle2;
  public Arc Arc1;
  public Arc Arc2;

  public Parameter Parameter = null;
  //radius, length, angle etc...

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    var list = new List<IEnumerable<Parameter>>
    {
      Point1,
      Point2,
      Line1,
      Line2,
      SymLine,
      Circle1,
      Circle2,
      Arc1,
      Arc2,
      new[] {Parameter}
    };
    return list
      .Where(p => p != null)
      .SelectMany(p => p)
      .Where(p => p != null)
      .GetEnumerator();
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
