using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using Accord.Math;
using Accord.Math.Optimization;
using Accord.Math.Differentiation;

namespace SketchSolve
{
  public enum Result
  {
    error = -1,
    succsess = 0,
    noSolution = 1,
  }

  public static class Solver
  {
    public static double solve(bool isFine, params Constraint[] cons)
    {
      return solve(isFine, (IEnumerable<Constraint>)cons);
    }

    static Func<double[], double> LogWrap(Func<double[], double> fn)
    {
      return args =>
      {
        var v = fn(args);
        Console.WriteLine(v);
        return v;
      };
    }

    static Func<double[], double[]> LogWrap(Func<double[], double[]> fn)
    {
      return args =>
      {
        var v = fn(args);
        Console.WriteLine("[" + String.Join(", ", v) + "]");
        return v;
      };
    }

    static Func<double[], double[]> Grad(int n, Func<double[], double> fn)
    {
      var gradient = new FiniteDifferences(n, fn);
      return a => gradient.Compute(a);
    }

    public static double solve(bool isFine, IEnumerable<Constraint> cons)
    {
      var constraints = cons.ToArray();

      // Get the parameters that need solving by selecting "free" ones
      Parameter[] x = constraints.SelectMany(p => p)
        .Distinct()
        .Where(p => p.free == true)
        .ToArray();

      Console.WriteLine("Number of free vars is " + x.Length);

      // Wrap our constraint error function for Accord.NET
      Func<double[], double> objective = args =>
      {
        int i = 0;
        foreach (var arg in args)
        {
          x[i].Value = arg;
          i++;
        }

        return Constraint.calc(constraints);
      };


      var nlConstraints = new List<NonlinearConstraint>();

      // Finally, we create the non-linear programming solver 
      var solver = new AugmentedLagrangian(x.Length, nlConstraints)
      {
        Function = LogWrap(objective),
        Gradient = LogWrap(Grad(x.Length, objective))
      };

      // Copy in the initial conditions
      x.Select(v => v.Value).ToArray().CopyTo(solver.Solution, 0);

      // And attempt to solve the problem 
      _ = solver.Minimize();
      return solver.Value;
    }

    // We don't use this at the moment
    static List<NonlinearConstraint> CreateConstraints(Parameter[] x, NonlinearObjectiveFunction f)
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
}
