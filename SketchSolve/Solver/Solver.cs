namespace SketchSolve.Solver;

using Accord.Math.Differentiation;
using Accord.Math.Optimization;
using SketchSolve.Constraint;
using SketchSolve.Model;

public static class Solver
{
  public static double Solve(double maxError = 1e5, params BaseConstraint[] constraints)
  {
    return Solve(maxError, (IEnumerable<BaseConstraint>)constraints);
  }

  private static Func<double[], double[]> Grad(int n, Func<double[], double> fn)
  {
    var gradient = new FiniteDifferences(n, fn);
    return a => gradient.Gradient(a);
  }

  private static double Solve(double maxError, IEnumerable<BaseConstraint> cons)
  {
    const int MaxIterations = 10;

    var currIteration = 0;

    var constraints = cons.ToArray();

    // Get the parameters that need solving by selecting "free" ones
    var freeParameters = constraints.SelectMany(p => p)
      .Distinct()
      .Where(p => p.Free)
      .ToArray();

    // Wrap our constraint error function for Accord.NET
    Func<double[], double> objective = args =>
    {
      var i = 0;
      foreach (var arg in args)
      {
        // if we can't solve it the first time, then increasingly, randomly perturb parameter for subsequent attempts
        var sign = Random.Shared.NextDouble() >= 0.5 ? 1d : -1d;
        freeParameters[i].Value = arg * (1 + currIteration * sign * 0.001);
        i++;
      }

      return CalculateError(constraints);
    };


    // Finally, we create the non-linear programming solver 
    var solver = new AugmentedLagrangian(freeParameters.Length, Enumerable.Empty<IConstraint>())
    {
      Function = objective,
      Gradient = Grad(freeParameters.Length, objective)
    };

    // Copy in the initial conditions
    freeParameters.Select(v => v.Value).ToArray().CopyTo(solver.Solution, 0);

    do
    {
      // And attempt to solve the problem 
      _ = solver.Minimize();
    } while (
      currIteration++ < MaxIterations &&
      solver.Value > maxError);

    // we have either solved it or run out of attempts
    return solver.Value;
  }

  private static double CalculateError(IEnumerable<BaseConstraint> constraints)
  {
    // Prevent symmetry errors
    return constraints.Sum(constraint => constraint.CalculateError());
  }

  // We don't use this at the moment
  private static IEnumerable<NonlinearConstraint> CreateConstraints(IEnumerable<Parameter> parameters, NonlinearObjectiveFunction func)
  {
    // Now we can start stating the constraints 
    var nlConstraints = parameters.SelectMany((p, i) =>
    {
      Func<double[], double> constraintFunc = args => p.Value;
      return new[]
      {
        new NonlinearConstraint(func, function: constraintFunc, shouldBe: ConstraintType.GreaterThanOrEqualTo, value: p.Min, gradient: Grad(parameters.Count(), constraintFunc)),
        new NonlinearConstraint(func, function: constraintFunc, shouldBe: ConstraintType.LesserThanOrEqualTo, value: p.Max, gradient: Grad(parameters.Count(), constraintFunc)),
      };
    });
    return nlConstraints;
  }
}
