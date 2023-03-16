﻿namespace SketchSolve;

using System.Collections;

public class Arc : IEnumerable<Parameter>
{
  public readonly Parameter StartAngle = new(0);
  public readonly Parameter EndAngle = new(0);
  public readonly Parameter Rad = new(0);
  public readonly Point Center = new(0, 0);

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
