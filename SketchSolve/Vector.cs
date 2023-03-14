using System.Collections;

namespace SketchSolve;

public class Vector : IEnumerable<Parameter>
{
  public Parameter dx = new(0);
  public Parameter dy = new(0);

  public Vector(double dx, double dy, bool freex, bool freey)
  {
    this.dx = new Parameter(dx, freex);
    this.dy = new Parameter(dy, freey);
  }

  public Vector(double dx, double dy, bool free = true)
  {
    this.dx = new Parameter(dx, free);
    this.dy = new Parameter(dy, free);
  }

  public override string ToString()
  {
    return "-> " + dx.Value + ";" + dy.Value;
  }

  public double LengthSquared
  {
    get
    {
      return dx.Value * dx.Value + dy.Value * dy.Value;
    }
  }

  public double Length
  {
    get
    {
      return Math.Sqrt(LengthSquared);
    }
  }

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
    return dx.Value * other.dx.Value
           + dy.Value * other.dy.Value;
  }

  public Vector ProjectOnto(Vector other)
  {
    var unit = other.Unit;
    return Dot(unit) * unit;
  }

  #region basic operators

  public static Vector operator *(Vector a, double b)
  {
    return new Vector(a.dx.Value * b, a.dy.Value * b, false);
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
      var l = Length;
      return new Vector(dx.Value / l, dy.Value / l);
    }
  }

  public Vector UnitNormal
  {
    get
    {
      var l = Length;
      return new Vector(-dy.Value / l, dx.Value / l);
    }
  }

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    yield return dx;
    yield return dy;
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}
