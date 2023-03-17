namespace SketchSolve;

using System.Collections;

public class Arc : IEnumerable<Parameter>
{
  public readonly Parameter StartAngle;
  public readonly Parameter EndAngle;
  public readonly Parameter Rad;
  public readonly Point Center;

  public Arc(Point center, Parameter rad, Parameter startAngle, Parameter endAngle)
  {
    Center = center;
    Rad = rad;
    EndAngle = endAngle;
    StartAngle = startAngle;
  }

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    yield return StartAngle;
    yield return EndAngle;
    yield return Rad;
    foreach (var p in Center)
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
