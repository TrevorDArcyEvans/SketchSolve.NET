namespace SketchSolve;

using Accord.Math.Optimization;
using Accord.Math.Differentiation;

public static class Solver
{
  public static double Solve(params Constraint[] cons)
  {
    return Solve((IEnumerable<Constraint>)cons);
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

  private static double Solve(IEnumerable<Constraint> cons)
  {
    var constraints = cons.ToArray();

    // Get the parameters that need solving by selecting "free" ones
    var freeConstraints = constraints.SelectMany(p => p)
      .Distinct()
      .Where(p => p.free == true)
      .ToArray();

    Console.WriteLine("Number of free vars is " + freeConstraints.Length);

    // Wrap our constraint error function for Accord.NET
    Func<double[], double> objective = args =>
    {
      var i = 0;
      foreach (var arg in args)
      {
        freeConstraints[i].Value = arg;
        i++;
      }

      return Constraint.Calc(constraints);
    };


    var nlConstraints = new List<NonlinearConstraint>();

    // Finally, we create the non-linear programming solver 
    var solver = new AugmentedLagrangian(freeConstraints.Length, nlConstraints)
    {
      Function = LogWrap(objective),
      Gradient = LogWrap(Grad(freeConstraints.Length, objective))
    };

    // Copy in the initial conditions
    freeConstraints.Select(v => v.Value).ToArray().CopyTo(solver.Solution, 0);

    // And attempt to solve the problem 
    _ = solver.Minimize();
    return solver.Value;
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
