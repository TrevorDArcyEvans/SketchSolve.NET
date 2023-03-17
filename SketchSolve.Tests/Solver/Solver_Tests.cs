namespace SketchSolve.Tests.Solver;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SketchSolve.Constraint;
using SketchSolve.Model;

[TestFixture]
public class Solver_Tests
{
  [Test]
  public void HorizontalConstraintShouldWork()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, false, true));

    var error = SketchSolve.Solver.Solver.Solve(line.IsHorizontal());

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately(line.P1.Y.Value.Should(), line.P2.Y.Value, 0.001);
    }
  }

  [Test]
  public void VerticalConstraintShouldWork()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, true, false));

    var r = SketchSolve.Solver.Solver.Solve(line.IsVertical());

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(r.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately(line.P1.X.Value.Should(), line.P2.X.Value, 0.001);
    }
  }

  [Test]
  public void PointOnPointConstraintShouldWork()
  {
    var line1 = new Line(new Point(0, 1), new Point(2, 3, false));
    var line2 = new Line(new Point(10, 100, false), new Point(200, 300, false));

    var error = SketchSolve.Solver.Solver.Solve(line1.P1.IsCoincidentWith(line2.P1));

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately(line1.P1.X.Value.Should(), line2.P1.X.Value, 0.001);
      NumericAssertionsExtensions.BeApproximately(line1.P1.Y.Value.Should(), line2.P1.Y.Value, 0.001);
    }
  }

  [Test]
  public void InternalAngleConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, -1, false));

      const double angle = Math.PI / 2 / 3; // 30 deg

      var error = SketchSolve.Solver.Solver.Solve(line1.HasInternalAngle(line2, new Parameter(angle, false)));

      using (new AssertionScope())
      {
        NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
        NumericAssertionsExtensions.BeApproximately(line1
            .Vector
            .Cosine(line2.Vector)
            .Should(), Math.Cos(angle), 0.001);
      }
    }
  }

  [Test]
  public void ExternalAngleConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, -1, false));

      const double angle = Math.PI / 2 / 3; // 30 deg

      var error = SketchSolve.Solver.Solver.Solve(line1.HasExternalAngle(line2, new Parameter(angle, false)));

      using (new AssertionScope())
      {
        NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
        NumericAssertionsExtensions.BeApproximately(line1
            .Vector
            .Cosine(line2.Vector)
            .Should(), Math.Cos(Math.PI - angle), 0.001);
      }
    }
  }

  [Test]
  public void PerpendicularLineConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, 10, true, false));

      var error = SketchSolve.Solver.Solver.Solve(line1.IsPerpendicularTo(line2));

      using (new AssertionScope())
      {
        NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
        NumericAssertionsExtensions.BeApproximately(line1
            .Vector
            .Dot(line2.Vector)
            .Should(), 0, 0.001);
      }
    }
  }

  [Test]
  public void TangentToCircleConstraintShouldWork()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(35, 0, true, false));

    var error = SketchSolve.Solver.Solver.Solve(line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately(line.P2.X.Value.Should(), v, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Weird problem when only one degree of freedom with horizontal initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyThroughCenter()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(0, v, true, false));

    var error = SketchSolve.Solver.Solver.Solve(line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately((circle.CenterTo(line).Vector.LengthSquared - 1).Should(), 0, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Weird problem when only one degree of freedom with horizontal initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyHorizontal()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(10, -v, true, false));

    var error = SketchSolve.Solver.Solver.Solve(line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately((circle.CenterTo(line).Vector.LengthSquared - 1).Should(), 0, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Not sure how to fix this one. My guess is that we have a local maximum due to the initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyHorizontalShouldWork()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(10, false));

    const int v = -15;

    var line = new Line(new Point(-100, -v, false, true), new Point(100, -v * 2.1, false, true));

    var error = SketchSolve.Solver.Solver.Solve(line.IsTangentTo(circle), line.IsHorizontal());

    using (new AssertionScope())
    {
      NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
      NumericAssertionsExtensions.BeApproximately((circle.CenterTo(line).Vector.LengthSquared - circle.Rad.Value * circle.Rad.Value)
          .Should(), 0, 0.001);
    }
  }

  [Test]
  public void SquareAroundCircle()
  {
    var circle = new Circle(new Point(0, 0, false), new Parameter(10, false));

    // We want a box around the circle where all lines touch the 
    // circle and line0 is vertical. We arrange the lines roughly
    // in the correct placement to get the search off to a good
    // start
    var line0 = new Line(new Point(-21, 0), new Point(0, 23));
    var line1 = new Line(new Point(0, 22), new Point(22, 0));
    var line2 = new Line(new Point(21, 0), new Point(0, -29));
    var line3 = new Line(new Point(0, -27), new Point(-25, 0));

    var angle = new Parameter(Math.PI / 2, false);

    var error = SketchSolve.Solver.Solver.Solve(line0.IsTangentTo(circle),
      line1.IsTangentTo(circle),
      line2.IsTangentTo(circle),
      line3.IsTangentTo(circle),
      line0.HasInternalAngle(line1, angle),
      line1.HasInternalAngle(line2, angle),
      line2.HasInternalAngle(line3, angle),
      line3.HasInternalAngle(line0, angle),
      line0.P2.IsCoincidentWith(line1.P1),
      line1.P2.IsCoincidentWith(line2.P1),
      line2.P2.IsCoincidentWith(line3.P1),
      line3.P2.IsCoincidentWith(line0.P1),
      line0.IsVertical());

    NumericAssertionsExtensions.BeApproximately(error.Should(), 0, 0.0001);
  }
}
