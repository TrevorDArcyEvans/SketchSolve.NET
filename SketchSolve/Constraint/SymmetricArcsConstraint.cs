﻿namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class SymmetricArcsConstraint : Constraint
{
  private readonly Line _symLine;
  private readonly Arc _arc1;
  private readonly Arc _arc2;

  public SymmetricArcsConstraint(Line symLine, Arc arc1, Arc arc2)
  {
    _symLine = symLine;
    _arc1 = arc1;
    _arc2 = arc2;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;

    var t = -(dy * a1StartX - dx * a1StartY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = a1StartX + dy * t * 2;
    var ey = a1StartY - dx * t * 2;
    var a2Radius = _arc2.Rad.Value;
    var a2StartA = _arc2.StartAngle.Value;
    var a2StartX = a1CenterX + a2Radius * Math.Cos(a2StartA);
    var a2StartY = a1CenterY + a2Radius * Math.Sin(a2StartA);
    var tempX = ex - a2StartX;
    var tempY = ey - a2StartY;
    error += tempX * tempX + tempY * tempY;

    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    t = -(dy * a1EndX - dx * a1EndY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    ex = a1EndX + dy * t * 2;
    ey = a1EndY - dx * t * 2;
    var a2EndA = _arc2.EndAngle.Value;
    var a2EndX = a1CenterX + a2Radius * Math.Cos(a2EndA);
    var a2EndY = a1CenterY + a2Radius * Math.Sin(a2EndA);
    tempX = ex - a2EndX;
    tempY = ey - a2EndY;
    error += tempX * tempX + tempY * tempY;

    t = -(dy * a1CenterX - dx * a1CenterY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    ex = a1CenterX + dy * t * 2;
    ey = a1CenterY - dx * t * 2;
    var a2CenterX = _arc2.Center.X.Value;
    var a2CenterY = _arc2.Center.Y.Value;
    tempX = ex - a2CenterX;
    tempY = ey - a2CenterY;
    error += tempX * tempX + tempY * tempY;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _symLine,
      _arc1,
      _arc2
    };
  }
}