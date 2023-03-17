﻿namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class TangentToArcConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Arc _arc1;

  public TangentToArcConstraint(Line line1, Arc arc1)
  {
    _line1 = line1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    /*
    double dx,dy,Rpx,Rpy,RpxN,RpyN,hyp,error1,error2,rad;
    dx = L1_P2_x - L1_P1_x;
    dy = L1_P2_y - L1_P1_y;

    hyp=Hypot(dx,dy);

    double u = (A1_Center_x - L1_P1_x) * (L1_P2_x - L1_P1_x) + (A1_Center_y - L1_P1_y) * (L1_P2_y - L1_P1_y);
    u/=hyp*hyp;

    double x = L1_P1_x + u *(L1_P2_x - L1_P1_x);
    double y = L1_P1_y + u *(L1_P2_y - L1_P1_y);

    double dcsx = A1_Center_x - A1_Start_x;
    double dcsy = A1_Center_y - A1_Start_y;
    double dcex = A1_Center_x - A1_End_x;
    double dcey = A1_Center_y - A1_End_y;
    rad=(dcsx*dcsx + dcsy * dcsy);
    //  rad+=(dcex*dcex + dcey * dcey)/4;

    double dcx = A1_Center_x-x;
    double dcy = A1_Center_y-y;
    temp = (dcx * dcx + dcy * dcy) - rad;
    error += temp*temp*100;
    */

    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var radsq = (a1CenterX - a1StartX) * (a1CenterX - a1StartX) + (a1CenterY - a1StartY) * (a1CenterY - a1StartY);
    var t = -(l1P1X * dx - a1CenterX * dx + l1P1Y * dy - a1CenterY * dy) / (dx * dx + dy * dy);
    var xint = l1P1X + dx * t;
    var yint = l1P1Y + dy * t;
    var temp = (a1CenterX - xint) * (a1CenterX - xint) + (a1CenterY - yint) * (a1CenterY - yint) - radsq;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _arc1
    };
  }
}