namespace SketchSolve.Constraint;

using System.Collections;
using SketchSolve.Model;

public abstract class Constraint : IEnumerable<Parameter>
{
  public abstract double CalculateError();

  protected static double Hypot(double a, double b)
  {
    return Math.Sqrt(a * a + b * b);
  }

  protected abstract IEnumerable<IEnumerable<Parameter>> GetParameters();

  public IEnumerator<Parameter> GetEnumerator()
  {
    return GetParameters()
      .Where(p => p != null)
      .SelectMany(p => p)
      .Where(p => p != null)
      .GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
