﻿namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class CollinearConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public CollinearConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var m = dy / dx;
    var n = dx / dy;
    // Calculate the error between the expected intersection point
    // and the true point of the second lines two end points on the
    // first line
    if (m <= 1 && m > -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var l2P1X = _line2.P1.X.Value;
      var l2P1Y = _line2.P1.Y.Value;
      var l2P2X = _line2.P2.X.Value;
      var l2P2Y = _line2.P2.Y.Value;
      var ey = l1P1Y + m * (l2P1X - l1P1X);
      error += (ey - l2P1Y) * (ey - l2P1Y);

      ey = l1P1Y + m * (l2P2X - l1P1X);
      error += (ey - l2P2Y) * (ey - l2P2Y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var l2P1X = _line2.P1.X.Value;
      var l2P1Y = _line2.P1.Y.Value;
      var l2P2X = _line2.P2.X.Value;
      var l2P2Y = _line2.P2.Y.Value;
      var ex = l1P1X + n * (l2P1Y - l1P1Y);
      error += (ex - l2P1X) * (ex - l2P1X);

      ex = l1P1X + n * (l2P2Y - l1P1Y);
      error += (ex - l2P2X) * (ex - l2P2X);
    }

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2
    };
  }
}