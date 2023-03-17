namespace SketchSolve.Solver;

using Accord.Math.Differentiation;
using Accord.Math.Optimization;
using SketchSolve.Model;

public static class Solver
{
  public static double Solve(params Constraint.Constraint[] cons)
  {
    return Solve((IEnumerable<Constraint.Constraint>) cons);
  }

  private static Func<double[], double> LogWrap(Func<double[], double> fn)
  {
    return args =>
    {
      var v = fn(args);
      Console.WriteLine(v);
      return v;
    };
  }

  private static Func<double[], double[]> LogWrap(Func<double[], double[]> fn)
  {
    return args =>
    {
      var v = fn(args);
      Console.WriteLine("[" + String.Join(", ", v) + "]");
      return v;
    };
  }

  private static Func<double[], double[]> Grad(int n, Func<double[], double> fn)
  {
    var gradient = new FiniteDifferences(n, fn);
    return a => gradient.Compute(a);
  }

  private static double Solve(IEnumerable<Constraint.Constraint> cons)
  {
    var constraints = cons.ToArray();

    // Get the parameters that need solving by selecting "free" ones
    var freeParameters = constraints.SelectMany(p => p)
      .Distinct()
      .Where(p => p.Free)
      .ToArray();

    Console.WriteLine("Number of free parameters is " + freeParameters.Length);

    // Wrap our constraint error function for Accord.NET
    Func<double[], double> objective = args =>
    {
      var i = 0;
      foreach (var arg in args)
      {
        freeParameters[i].Value = arg;
        i++;
      }

      return CalculateError(constraints);
    };


    // Finally, we create the non-linear programming solver 
    var solver = new AugmentedLagrangian(freeParameters.Length, Enumerable.Empty<IConstraint>())
    {
      Function = LogWrap(objective),
      Gradient = LogWrap(Grad(freeParameters.Length, objective))
    };

    // Copy in the initial conditions
    freeParameters.Select(v => v.Value).ToArray().CopyTo(solver.Solution, 0);

    // And attempt to solve the problem 
    _ = solver.Minimize();
    return solver.Value;
  }

  private static double CalculateError(IEnumerable<Constraint.Constraint> constraints)
  {
    // Prevent symmetry errors
    return constraints.Sum(constraint => constraint.CalculateError());
  }

  // We don't use this at the moment
  private static List<NonlinearConstraint> CreateConstraints(Parameter[] x, NonlinearObjectiveFunction f)
  {
    // Now we can start stating the constraints 
    var nlConstraints = x.SelectMany((p, i) =>
    {
      Func<double[], double> cfn = args => x[i].Value;
      return new[]
      {
        new NonlinearConstraint
        (f
          , function: cfn
          , shouldBe: ConstraintType.GreaterThanOrEqualTo
          , value: p.Min
          , gradient: Grad(x.Length, cfn)),
        new NonlinearConstraint
        (f
          , function: cfn
          , shouldBe: ConstraintType.LesserThanOrEqualTo
          , value: p.Max
          , gradient: Grad(x.Length, cfn)),
      };
    }).ToList();
    return nlConstraints;
  }
}