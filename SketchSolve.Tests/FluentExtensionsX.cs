namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Numeric;

public static class FluentExtensionsX
{
  public static AndConstraint<NumericAssertions<double>> BeApproximately
    (this NumericAssertions<double> number, double val, double eps)
  {
    return number.BeInRange(val - eps / 2, val + eps / 2);
  }
}
