namespace SketchSolve.Model;

using System.Collections;

public class Vector : IEnumerable<Parameter>
{
  public readonly Parameter dX;
  public readonly Parameter dY;

  public Vector(double dx, double dy, bool freex, bool freey)
  {
    dX = new Parameter(dx, freex);
    dY = new Parameter(dy, freey);
  }

  public Vector(double dx, double dy, bool free = true)
  {
    dX = new Parameter(dx, free);
    dY = new Parameter(dy, free);
  }

  public override string ToString()
  {
    return "-> " + dX.Value + ";" + dY.Value;
  }

  public double LengthSquared => dX.Value * dX.Value + dY.Value * dY.Value;

  public double Length => Math.Sqrt(LengthSquared);

  // The cosine of the angle between
  // the lines
  public double Cosine(Vector other)
  {
    return Dot(other) /
           Length /
           other.Length;
  }

  public double Dot(Vector other)
  {
    return dX.Value * other.dX.Value + dY.Value * other.dY.Value;
  }

  public Vector ProjectOnto(Vector other)
  {
    var unit = other.Unit;
    return Dot(unit) * unit;
  }

  #region basic operators

  public static Vector operator *(Vector a, double b)
  {
    return new Vector(a.dX.Value * b, a.dY.Value * b, false);
  }

  public static Vector operator *(double b, Vector a)
  {
    return a * b;
  }

  public static Vector operator /(Vector a, double b)
  {
    return a * (1 / b);
  }

  #endregion

  public Vector Unit
  {
    get
    {
      var length = Length;
      return new Vector(dX.Value / length, dY.Value / length);
    }
  }

  public Vector UnitNormal
  {
    get
    {
      var length = Length;
      return new Vector(-dY.Value / length, dX.Value / length);
    }
  }

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    yield return dX;
    yield return dY;
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
