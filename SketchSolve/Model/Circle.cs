namespace SketchSolve.Model;

using System.Collections;

public class Circle : IEnumerable<Parameter>
{
  public readonly Point Center;
  public readonly Parameter Rad;

  public Circle(Point center, Parameter rad)
  {
    Center = center;
    Rad = rad;
  }

  /// <summary>
  /// Returns a line normal to the circle and normal
  /// to the line 
  /// </summary>
  /// <returns></returns>
  /// <param name="line"></param>
  public Line CenterTo(Line line)
  {
    var pCircCenter = Center;
    var pLineP1 = line.P1;
    var vLine = line.Vector;

    var vLineStartToCircCenter = pCircCenter - pLineP1;

    var pProjection = pLineP1 + vLineStartToCircCenter.ProjectOnto(vLine);

    return new Line(Center, pProjection);
  }

  public override string ToString()
  {
    return "[c " + Center + ", r " + Rad.Value + "]";
  }
  
  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    foreach (var p in Center)
    {
      yield return p;
    }

    yield return Rad;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
