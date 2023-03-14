using System.Collections;

namespace SketchSolve;

public class Arc : IEnumerable<Parameter>
{
  public Parameter startAngle = new(0);
  public Parameter endAngle = new(0);
  public Parameter rad = new(0);
  public Point center = new(0, 0);

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    yield return startAngle;
    yield return endAngle;
    yield return rad;
    foreach (var p in center)
    {
      yield return p;
    }
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
