namespace SketchSolve.Tests.Solver;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SketchSolve.Constraint;
using SketchSolve.Model;

[TestFixture]
public sealed class Solver_Tests
{
  [Test]
  public void HorizontalConstraint_should_work()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, false, true));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsHorizontal());

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line.P1.Y.Value.Should().BeApproximately(line.P2.Y.Value, 0.001);
    }
  }

  [Test]
  public void VerticalConstraint_should_work()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, true, false));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsVertical());

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line.P1.X.Value.Should().BeApproximately(line.P2.X.Value, 0.001);
    }
  }

  [Test]
  public void PointOnPointConstraint_should_work()
  {
    var line1 = new Line(new Point(0, 1), new Point(2, 3, false));
    var line2 = new Line(new Point(10, 100, false), new Point(200, 300, false));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line1.P1.IsCoincidentWith(line2.P1));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line1.P1.X.Value.Should().BeApproximately(line2.P1.X.Value, 0.001);
      line1.P1.Y.Value.Should().BeApproximately(line2.P1.Y.Value, 0.001);
    }
  }

  [Test]
  public void InternalAngleConstraint_should_work()
  {
    var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
    var line2 = new Line(new Point(0, 0, false), new Point(10, -1, false));

    const double angle = Math.PI / 2 / 3; // 30 deg

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line1.HasInternalAngle(line2, new Parameter(angle, false)));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line1
        .Vector
        .Cosine(line2.Vector)
        .Should().BeApproximately(Math.Cos(angle), 0.001);
    }
  }

  [Test]
  public void ExternalAngleConstraint_should_work()
  {
    var line1 = new Line(new Point(0, 0, false), new Point(10, -10, false, true));
    var line2 = new Line(new Point(0, 0, false), new Point(10, 0, false));

    const double angle = Math.PI / 2 / 3; // 30 deg

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line1.HasInternalAngle(line2, new Parameter(angle, false)));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line1
        .Vector
        .Cosine(line2.Vector)
        .Should().BeApproximately(-Math.Cos(Math.PI - angle), 0.001);
    }
  }

  [Test]
  public void PerpendicularLineConstraint_should_work()
  {
    var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, false));
    var line2 = new Line(new Point(0, 0, false), new Point(10, 10, true, false));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line1.IsPerpendicularTo(line2));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line1
        .Vector
        .Dot(line2.Vector)
        .Should().BeApproximately(0, 0.001);
    }
  }

  [Test]
  public void TangentToCircleConstraint_should_work()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, -v, false, false), new Point(1.0001 * v, 0, true, false));

    // TODO   objective function gets sent double.NaN
    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line.P2.X.Value.Should().BeApproximately(v, 0.001);
    }
  }

  [Test]
  public void TangentToCircleConstraint_ill_conditioned_should_work()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, -v, false, false), new Point(v, 0, true, false));

    // TODO   objective function gets sent double.NaN
    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);

      // NOTE:  looser tolerance because problem is ill conditioned
      line.P2.X.Value.Should().BeApproximately(v, 0.01);
    }
  }

  [Test]
  public void TangentToCircleConstraint_with_line_initially_through_center()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, -v, false, false), new Point(0.00444, v, true, false));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - 1)
        .Should().BeApproximately(0, 0.001);
    }
  }

  [Test]
  public void TangentToCircleConstraint_ill_conditioned_with_line_initially_through_center()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, -v, false, false), new Point(0, v, true, false));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);

      // NOTE:  looser tolerance because problem is ill conditioned
      (circle.CenterTo(line).Vector.LengthSquared - 1)
        .Should().BeApproximately(0, 0.01);
    }
  }

  [Test]
  public void TangentToCircleConstraint_with_line_initially_horizontal()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, v, false, false), new Point(v, 1.000001 * v, false, true));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - 1)
        .Should().BeApproximately(0, 0.001);
    }
  }

  [Test]
  public void TangentToCircleConstraint_ill_conditioned_with_line_initially_horizontal()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(1, false));

    var v = 1 / Math.Sin(Math.PI / 4); // sqrt(2)

    // ill conditioned
    var line = new Line(new Point(0, v, false, false), new Point(v, v, false, true));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);

      // NOTE:  looser tolerance because problem is ill conditioned
      (circle.CenterTo(line).Vector.LengthSquared - 1)
        .Should().BeApproximately(0, 0.01);
    }
  }

  [Test]
  public void TangentToCircleConstraint_with_line_initially_horizontal_should_work()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle(new Point(0, 0, false), new Parameter(10, false));

    const int v = -15;

    var line = new Line(new Point(-100, -v, false, true), new Point(100, -v * 2.1, false, true));

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line.IsTangentTo(circle), line.IsHorizontal());

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - circle.Rad.Value * circle.Rad.Value)
        .Should().BeApproximately(0, 0.001);
    }
  }

  [Test]
  public void Square_around_circle()
  {
    var circle = new Circle(new Point(0, 0, false), new Parameter(10, false));

    // We want a box around the circle where all lines touch the 
    // circle and line0 is vertical. We arrange the lines roughly
    // in the correct placement to get the search off to a good
    // start
    //
    // ill conditioned
    var line0 = new Line(new Point(-11, 0), new Point(0, 13));
    var line1 = new Line(new Point(0, 12), new Point(12, 0));
    var line2 = new Line(new Point(11, 0), new Point(0, -12));
    var line3 = new Line(new Point(0, -12), new Point(-12, 0));

    var angle = new Parameter(Math.PI / 2, false);

    var error = SketchSolve.Solver.Solver.Solve(0.0001, line0.IsTangentTo(circle),
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

    error.Should().BeApproximately(0, 0.0001);
  }
}
