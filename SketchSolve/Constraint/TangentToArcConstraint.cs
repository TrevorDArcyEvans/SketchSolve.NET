namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class TangentToArcConstraint : BaseConstraint
{
  public readonly Line Line;
  public readonly Arc Arc;
  public override IEnumerable<object> Items => new object[] { Line, Arc };

  public TangentToArcConstraint(Line line, Arc arc)
  {
    Line = line;
    Arc = arc;
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

    var l1P1X = Line.P1.X.Value;
    var l1P1Y = Line.P1.Y.Value;
    var l1P2X = Line.P2.X.Value;
    var l1P2Y = Line.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1StartA = Arc.StartAngle.Value;
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
      Line,
      Arc
    };
  }
}
