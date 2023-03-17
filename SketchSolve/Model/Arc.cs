namespace SketchSolve.Model;

using System.Collections;

public class Arc : IEnumerable<Parameter>
{
  public readonly Point Center;
  public readonly Parameter Rad;
  public readonly Parameter StartAngle;
  public readonly Parameter EndAngle;

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
    foreach (var p in Center)
    {
      yield return p;
    }
    yield return Rad;
    yield return StartAngle;
    yield return EndAngle;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
