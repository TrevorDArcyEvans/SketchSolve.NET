namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Numeric;

public static class FluentExtensionsX
{
  public static AndConstraint<NumericAssertions<double>> BeApproximately
    (this NumericAssertions<double> This, double val, double eps)
  {
    return This.BeInRange(val - eps / 2, val + eps / 2);
  }
}
