﻿namespace SketchSolve;

using System.Collections;

public abstract class Constraint : IEnumerable<Parameter>
{
  public readonly ConstraintEnum ContraintType;

  public Point Point1;
  public Point Point2;
  public Line Line1;
  public Line Line2;
  public Line SymLine;
  public Circle Circle1;
  public Circle Circle2;
  public Arc Arc1;
  public Arc Arc2;

  public Parameter Parameter = null;
  //radius, length, angle etc...

  protected Constraint(ConstraintEnum constraintType)
  {
    ContraintType = constraintType;
  }

  public abstract double CalculateError();

  protected static double Hypot(double a, double b)
  {
    return Math.Sqrt(a * a + b * b);
  }

  #region IEnumerable implementation

  public IEnumerator<Parameter> GetEnumerator()
  {
    var list = new List<IEnumerable<Parameter>>
    {
      Point1,
      Point2,
      Line1,
      Line2,
      SymLine,
      Circle1,
      Circle2,
      Arc1,
      Arc2,
      new[] {Parameter}
    };
    return list
      .Where(p => p != null)
      .SelectMany(p => p)
      .Where(p => p != null)
      .GetEnumerator();
  }

  #endregion

  #region IEnumerable implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion
}

public sealed class PointOnPointConstraint : Constraint
{
  public PointOnPointConstraint() :
    base(ConstraintEnum.PointOnPoint)
  {
  }

  public override double CalculateError()
  {
    //Hopefully avoid this constraint, make coincident points use the same parameters
    var l2 = (Point1 - Point2).LengthSquared;
    return l2;
  }
}

public sealed class HorizontalConstraint : Constraint
{
  public HorizontalConstraint() :
    base(ConstraintEnum.Horizontal)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var ody = L1_P2_y - L1_P1_y;
    return ody * ody;
  }
}

public sealed class VerticalConstraint : Constraint
{
  public VerticalConstraint() :
    base(ConstraintEnum.Vertical)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var odx = L1_P2_x - L1_P1_x;
    return odx * odx;
  }
}

public sealed class InternalAngleConstraint : Constraint
{
  public InternalAngleConstraint() :
    base(ConstraintEnum.InternalAngle)
  {
  }

  public override double CalculateError()
  {
    var angleP = Parameter == null ? 0 : Parameter.Value;
    var temp = Line1.Vector.Cosine(Line2.Vector);
    var temp2 = Math.Cos(angleP);
    return (temp - temp2) * (temp - temp2);
  }
}

public sealed class ExternalAngleConstraint : Constraint
{
  public ExternalAngleConstraint() :
    base(ConstraintEnum.ExternalAngle)
  {
  }

  public override double CalculateError()
  {
    var angleP = Parameter == null ? 0 : Parameter.Value;
    var temp = Line1.Vector.Cosine(Line2.Vector);
    var temp2 = Math.Cos(Math.PI - angleP);
    return (temp - temp2) * (temp - temp2);
  }
}

public sealed class PerpendicularConstraint : Constraint
{
  public PerpendicularConstraint() :
    base(ConstraintEnum.Perpendicular)
  {
  }

  public override double CalculateError()
  {
    var temp = Line1.Vector.Dot(Line2.Vector);
    return temp * temp;
  }
}

public sealed class TangentToCircleConstraint : Constraint
{
  public TangentToCircleConstraint() :
    base(ConstraintEnum.TangentToCircle)
  {
  }

  public override double CalculateError()
  {
    var line = Line1;
    var circle = Circle1;
    var temp = circle.CenterTo(line).Vector.Length - circle.Rad.Value;
    return temp * temp;
  }
}

public sealed class P2PDistanceConstraint : Constraint
{
  public P2PDistanceConstraint() :
    base(ConstraintEnum.P2PDistance)
  {
  }

  public override double CalculateError()
  {
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var P2_x = Point2 == null ? 0 : Point2.X.Value;
    var P2_y = Point2 == null ? 0 : Point2.Y.Value;
    var distance = Parameter == null ? 0 : Parameter.Value;
    return (P1_x - P2_x) * (P1_x - P2_x) + (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
  }
}

public sealed class P2PDistanceVertConstraint : Constraint
{
  public P2PDistanceVertConstraint() :
    base(ConstraintEnum.P2PDistanceVert)
  {
  }

  public override double CalculateError()
  {
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var P2_y = Point2 == null ? 0 : Point2.Y.Value;
    var distance = Parameter == null ? 0 : Parameter.Value;
    return (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
  }
}

public sealed class P2PDistanceHorizConstraint : Constraint
{
  public P2PDistanceHorizConstraint() :
    base(ConstraintEnum.P2PDistanceHoriz)
  {
  }

  public override double CalculateError()
  {
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P2_x = Point2 == null ? 0 : Point2.X.Value;
    var distance = Parameter == null ? 0 : Parameter.Value;
    return (P1_x - P2_x) * (P1_x - P2_x) - distance * distance;
  }
}

public sealed class PointOnLineConstraint : Constraint
{
  public PointOnLineConstraint() :
    base(ConstraintEnum.PointOnLine)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var m = dy / dx; // Slope
    var n = dx / dy; // 1/Slope

    if (m <= 1 && m >= -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var P1_x = Point1 == null ? 0 : Point1.X.Value;
      var P1_y = Point1 == null ? 0 : Point1.Y.Value;
      var Ey = L1_P1_y + m * (P1_x - L1_P1_x);
      return (Ey - P1_y) * (Ey - P1_y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var P1_x = Point1 == null ? 0 : Point1.X.Value;
      var P1_y = Point1 == null ? 0 : Point1.Y.Value;
      var Ex = L1_P1_x + n * (P1_y - L1_P1_y);
      return (Ex - P1_x) * (Ex - P1_x);
    }
  }
}

public sealed class P2LDistanceConstraint : Constraint
{
  public P2LDistanceConstraint() :
    base(ConstraintEnum.P2LDistance)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var t = -(L1_P1_x * dx - P1_x * dx + L1_P1_y * dy - P1_y * dy) / (dx * dx + dy * dy);
    var Xint = L1_P1_x + dx * t;
    var Yint = L1_P1_y + dy * t;
    var distance = Parameter == null ? 0 : Parameter.Value;
    var temp = Hypot(P1_x - Xint, P1_y - Yint) - distance;
    return temp * temp / 10;
  }
}

public sealed class P2LDistanceVertConstraint : Constraint
{
  public P2LDistanceVertConstraint() :
    base(ConstraintEnum.P2LDistanceVert)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var t = (P1_x - L1_P1_x) / dx;
    var Yint = L1_P1_y + dy * t;
    var distance = Parameter == null ? 0 : Parameter.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var temp = Math.Abs(P1_y - Yint) - distance;
    return temp * temp;
  }
}

public sealed class P2LDistanceHorizConstraint : Constraint
{
  public P2LDistanceHorizConstraint() :
    base(ConstraintEnum.P2LDistanceHoriz)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var t = (P1_y - L1_P1_y) / dy;
    var Xint = L1_P1_x + dx * t;
    var distance = Parameter == null ? 0 : Parameter.Value;
    var temp = Math.Abs(P1_x - Xint) - distance;
    return temp * temp / 10;
  }
}

public sealed class TangentToArcConstraint : Constraint
{
  public TangentToArcConstraint() :
    base(ConstraintEnum.TangentToArc)
  {
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

    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var radsq = (A1_Center_x - A1_Start_x) * (A1_Center_x - A1_Start_x) + (A1_Center_y - A1_Start_y) * (A1_Center_y - A1_Start_y);
    var t = -(L1_P1_x * dx - A1_Center_x * dx + L1_P1_y * dy - A1_Center_y * dy) / (dx * dx + dy * dy);
    var Xint = L1_P1_x + dx * t;
    var Yint = L1_P1_y + dy * t;
    var temp = (A1_Center_x - Xint) * (A1_Center_x - Xint) + (A1_Center_y - Yint) * (A1_Center_y - Yint) - radsq;
    return temp * temp;
  }
}

public sealed class ArcRulesConstraint : Constraint
{
  public ArcRulesConstraint() :
    base(ConstraintEnum.ArcRules)
  {
  }

  public override double CalculateError()
  {
    //rad1=Hypot(A1_Center_x - A1_Start_x , A1_Center_y - A1_Start_y);
    //rad2=Hypot(A1_Center_x - A1_End_x , A1_Center_y - A1_End_y);
    //error += (rad1-rad2)*(rad1-rad2);
    //double dx,dy,Rpx,Rpy,RpxN,RpyN,hyp,error1,error2,rad;
    //dx = A1_End_x - A1_Start_x;
    //dy = A1_End_y - A1_Start_y;
    //
    //hyp=Hypot(dx,dy);
    //
    //double u = (A1_Center_x - A1_Start_x) * (A1_End_x - A1_Start_x) + (A1_Center_y - A1_Start_y) * (A1_End_y - A1_Start_y);
    //u /=hyp*hyp;
    //
    //temp = Math.Sin(u - .5);
    //error+=temp*temp*temp*temp*100000;
    //error+=Math.Pow(-2*A1_Center_x*A1_End_y - 2*A1_Center_y*A1_End_y + A1_End_x*A1_End_y + Math.Pow(A1_End_y,2) + 2*A1_Center_x*A1_Start_x - 2*A1_Center_y*A1_Start_x - A1_End_x*A1_Start_x + 4*A1_End_y*A1_Start_x - 3*Math.Pow(A1_Start_x,2) +  2*A1_Center_y*A1_Start_y + A1_Start_x*A1_Start_y - Math.Pow(A1_Start_y,2),2)/(8*Math.Pow(A1_End_y,2) + 8*Math.Pow(A1_Start_x,2) - 8*A1_End_y*A1_Start_y -  8*A1_Start_x*A1_Start_y + 4*Math.Pow(A1_Start_y,2));
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_endA = Arc1 == null ? 0 : Arc1.EndAngle.Value;
    var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
    var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var a1endx2 = A1_End_x * A1_End_x;
    var a1endy2 = A1_End_y * A1_End_y;
    var a1startx2 = A1_Start_x * A1_Start_x;
    var a1starty2 = A1_Start_y * A1_Start_y;
    var num = -2 * A1_Center_x * A1_End_x + a1endx2 - 2 * A1_Center_y * A1_End_y + a1endy2 + 2 * A1_Center_x * A1_Start_x - a1startx2 + 2 * A1_Center_y * A1_Start_y - a1starty2;
    return num * num / (4.0 * a1endx2 + a1endy2 - 2 * A1_End_x * A1_Start_x + a1startx2 - 2 * A1_End_y * A1_Start_y + a1starty2);
  }
}

public sealed class LineLengthConstraint : Constraint
{
  public LineLengthConstraint() :
    base(ConstraintEnum.LineLength)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var temp = Math.Sqrt(Math.Pow(L1_P2_x - L1_P1_x, 2) + Math.Pow(L1_P2_y - L1_P1_y, 2)) - (Parameter == null ? 0 : Parameter.Value);
    //temp=Hypot(L1_P2_x - L1_P1_x , L1_P2_y - L1_P1_y) - length;
    return temp * temp * 100;
  }
}

public sealed class EqualLengthConstraint : Constraint
{
  public EqualLengthConstraint() :
    base(ConstraintEnum.EqualLength)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var L2_P1_x = Line2 == null ? 0 : Line2.P1.X.Value;
    var L2_P1_y = Line2 == null ? 0 : Line2.P1.Y.Value;
    var L2_P2_x = Line2 == null ? 0 : Line2.P2.X.Value;
    var L2_P2_y = Line2 == null ? 0 : Line2.P2.Y.Value;
    var temp = Hypot(L1_P2_x - L1_P1_x, L1_P2_y - L1_P1_y) - Hypot(L2_P2_x - L2_P1_x, L2_P2_y - L2_P1_y);
    return temp * temp;
  }
}

public sealed class ArcRadiusConstraint : Constraint
{
  public ArcRadiusConstraint() :
    base(ConstraintEnum.ArcRadius)
  {
  }

  public override double CalculateError()
  {
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_endA = Arc1 == null ? 0 : Arc1.EndAngle.Value;
    var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
    var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var radius = Parameter == null ? 0 : Parameter.Value;
    var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
    var rad2 = Hypot(A1_Center_x - A1_End_x, A1_Center_y - A1_End_y);
    var temp = rad1 - radius;
    return temp * temp;
  }
}

public sealed class EqualRadiusArcsConstraint : Constraint
{
  public EqualRadiusArcsConstraint() :
    base(ConstraintEnum.EqualRadiusArcs)
  {
  }

  public override double CalculateError()
  {
    var A2_radius = Arc2 == null ? 0 : Arc2.Rad.Value;
    var A2_startA = Arc2 == null ? 0 : Arc2.StartAngle.Value;
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A2_Start_x = A1_Center_x + A2_radius * Math.Cos(A2_startA);
    var A2_Start_y = A1_Center_y + A2_radius * Math.Sin(A2_startA);
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A2_Center_x = Arc2 == null ? 0 : Arc2.Center.X.Value;
    var A2_Center_y = Arc2 == null ? 0 : Arc2.Center.Y.Value;
    var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
    var rad2 = Hypot(A2_Center_x - A2_Start_x, A2_Center_y - A2_Start_y);
    var temp = rad1 - rad2;
    return temp * temp;
  }
}

public sealed class EqualRadiusCirclesConstraint : Constraint
{
  public EqualRadiusCirclesConstraint() :
    base(ConstraintEnum.EqualRadiusCircles)
  {
  }

  public override double CalculateError()
  {
    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var C2_rad = Circle2 == null ? 0 : Circle2.Rad.Value;
    var temp = C1_rad - C2_rad;
    return temp * temp;
  }
}

public sealed class EqualRadiusCircArcConstraint : Constraint
{
  public EqualRadiusCircArcConstraint() :
    base(ConstraintEnum.EqualRadiusCircArc)
  {
  }

  public override double CalculateError()
  {
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
    var temp = rad1 - C1_rad;
    return temp * temp;
  }
}

public sealed class ConcentricArcsConstraint : Constraint
{
  public ConcentricArcsConstraint() :
    base(ConstraintEnum.ConcentricArcs)
  {
  }

  public override double CalculateError()
  {
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A2_Center_y = Arc2 == null ? 0 : Arc2.Center.Y.Value;
    var A2_Center_x = Arc2 == null ? 0 : Arc2.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var temp = Hypot(A1_Center_x - A2_Center_x, A1_Center_y - A2_Center_y);
    return temp * temp;
  }
}

public sealed class ConcentricCirclesConstraint : Constraint
{
  public ConcentricCirclesConstraint() :
    base(ConstraintEnum.ConcentricCircles)
  {
  }

  public override double CalculateError()
  {
    var C1_Center_x = Circle1 == null ? 0 : Circle1.Center.X.Value;
    var C1_Center_y = Circle1 == null ? 0 : Circle1.Center.Y.Value;
    var C2_Center_x = Circle2 == null ? 0 : Circle2.Center.X.Value;
    var C2_Center_y = Circle2 == null ? 0 : Circle2.Center.Y.Value;
    var temp = Hypot(C1_Center_x - C2_Center_x, C1_Center_y - C2_Center_y);
    return temp * temp;
  }
}

public sealed class ConcentricCircArcConstraint : Constraint
{
  public ConcentricCircArcConstraint() :
    base(ConstraintEnum.ConcentricCircArc)
  {
  }

  public override double CalculateError()
  {
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var C1_Center_x = Circle1 == null ? 0 : Circle1.Center.X.Value;
    var C1_Center_y = Circle1 == null ? 0 : Circle1.Center.Y.Value;
    var temp = Hypot(A1_Center_x - C1_Center_x, A1_Center_y - C1_Center_y);
    return temp * temp;
  }
}

public sealed class CircleRadiusConstraint : Constraint
{
  public CircleRadiusConstraint() :
    base(ConstraintEnum.CircleRadius)
  {
  }

  public override double CalculateError()
  {
    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var radius = Parameter == null ? 0 : Parameter.Value;
    return (C1_rad - radius) * (C1_rad - radius);
  }
}

public sealed class ParallelConstraint : Constraint
{
  public ParallelConstraint() :
    base(ConstraintEnum.Parallel)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var L2_P1_x = Line2 == null ? 0 : Line2.P1.X.Value;
    var L2_P1_y = Line2 == null ? 0 : Line2.P1.Y.Value;
    var L2_P2_x = Line2 == null ? 0 : Line2.P2.X.Value;
    var L2_P2_y = Line2 == null ? 0 : Line2.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;
    var dx2 = L2_P2_x - L2_P1_x;
    var dy2 = L2_P2_y - L2_P1_y;

    var hyp1 = Hypot(dx, dy);
    var hyp2 = Hypot(dx2, dy2);

    dx = dx / hyp1;
    dy = dy / hyp1;
    dx2 = dx2 / hyp2;
    dy2 = dy2 / hyp2;

    var temp = dy * dx2 - dx * dy2;
    return temp * temp;
  }
}

public sealed class CollinearConstraint : Constraint
{
  public CollinearConstraint() :
    base(ConstraintEnum.Collinear)
  {
  }

  public override double CalculateError()
  {
    var error = 0d;

    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var dx = L1_P2_x - L1_P1_x;
    var dy = L1_P2_y - L1_P1_y;

    var m = dy / dx;
    var n = dx / dy;
    // Calculate the error between the expected intersection point
    // and the true point of the second lines two end points on the
    // first line
    if (m <= 1 && m > -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var L2_P1_x = Line2 == null ? 0 : Line2.P1.X.Value;
      var L2_P1_y = Line2 == null ? 0 : Line2.P1.Y.Value;
      var L2_P2_x = Line2 == null ? 0 : Line2.P2.X.Value;
      var L2_P2_y = Line2 == null ? 0 : Line2.P2.Y.Value;
      var Ey = L1_P1_y + m * (L2_P1_x - L1_P1_x);
      error += (Ey - L2_P1_y) * (Ey - L2_P1_y);

      Ey = L1_P1_y + m * (L2_P2_x - L1_P1_x);
      error += (Ey - L2_P2_y) * (Ey - L2_P2_y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var L2_P1_x = Line2 == null ? 0 : Line2.P1.X.Value;
      var L2_P1_y = Line2 == null ? 0 : Line2.P1.Y.Value;
      var L2_P2_x = Line2 == null ? 0 : Line2.P2.X.Value;
      var L2_P2_y = Line2 == null ? 0 : Line2.P2.Y.Value;
      var Ex = L1_P1_x + n * (L2_P1_y - L1_P1_y);
      error += (Ex - L2_P1_x) * (Ex - L2_P1_x);

      Ex = L1_P1_x + n * (L2_P2_y - L1_P1_y);
      error += (Ex - L2_P2_x) * (Ex - L2_P2_x);
    }

    return error;
  }
}

public sealed class PointOnCircleConstraint : Constraint
{
  public PointOnCircleConstraint() :
    base(ConstraintEnum.PointOnCircle)
  {
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var C1_Center_x = Circle1 == null ? 0 : Circle1.Center.X.Value;
    var C1_Center_y = Circle1 == null ? 0 : Circle1.Center.Y.Value;
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var rad1 = Hypot(C1_Center_x - P1_x, C1_Center_y - P1_y);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - C1_rad;
    return temp * temp;
  }
}

public sealed class PointOnArcConstraint : Constraint
{
  public PointOnArcConstraint() :
    base(ConstraintEnum.PointOnArc)
  {
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var rad1 = Hypot(A1_Center_x - P1_x, A1_Center_y - P1_y);
    var rad2 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - rad2;
    return temp * temp;
  }
}

public sealed class PointOnLineMidpointConstraint : Constraint
{
  public PointOnLineMidpointConstraint() :
    base(ConstraintEnum.PointOnLineMidpoint)
  {
  }

  public override double CalculateError()
  {
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var Ex = (L1_P1_x + L1_P2_x) / 2;
    var Ey = (L1_P1_y + L1_P2_y) / 2;
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var tempX = Ex - P1_x;
    var tempY = Ey - P1_y;
    return tempX * tempX + tempY * tempY;
  }
}

public sealed class PointOnArcMidpointConstraint : Constraint
{
  public PointOnArcMidpointConstraint() :
    base(ConstraintEnum.PointOnArcMidpoint)
  {
  }

  public override double CalculateError()
  {
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_endA = Arc1 == null ? 0 : Arc1.EndAngle.Value;
    var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
    var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
    var tempStart = Math.Atan2(A1_Start_y - A1_Center_y, A1_Start_x - A1_Center_x);
    var tempEnd = Math.Atan2(A1_End_y - A1_Center_y, A1_End_x - A1_Center_x);
    var Ex = A1_Center_x + rad1 * Math.Cos((tempEnd + tempStart) / 2);
    var Ey = A1_Center_y + rad1 * Math.Sin((tempEnd + tempStart) / 2);
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var tempX = Ex - P1_x;
    var tempY = Ey - P1_y;
    return tempX * tempX + tempY * tempY;
  }
}

public sealed class PointOnCircleQuadConstraint : Constraint
{
  public PointOnCircleQuadConstraint() :
    base(ConstraintEnum.PointOnCircleQuad)
  {
  }

  public override double CalculateError()
  {
    var C1_Center_x = Circle1 == null ? 0 : Circle1.Center.X.Value;
    var C1_Center_y = Circle1 == null ? 0 : Circle1.Center.Y.Value;
    var Ex = C1_Center_x;
    var Ey = C1_Center_y;
    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var quadIndex = Parameter == null ? 0 : Parameter.Value;
    switch ((int) quadIndex)
    {
      case 0:
        Ex += C1_rad;
        break;
      case 1:
        Ey += C1_rad;
        break;
      case 2:
        Ex -= C1_rad;
        break;
      case 3:
        Ey -= C1_rad;
        break;
    }

    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var tempX = Ex - P1_x;
    var tempY = Ey - P1_y;
    return tempX * tempX + tempY * tempY;
  }
}

public sealed class SymmetricPointsConstraint : Constraint
{
  public SymmetricPointsConstraint() :
    base(ConstraintEnum.SymmetricPoints)
  {
  }

  public override double CalculateError()
  {
    var Sym_P1_x = SymLine == null ? 0 : SymLine.P1.X.Value;
    var Sym_P1_y = SymLine == null ? 0 : SymLine.P1.Y.Value;
    var Sym_P2_x = SymLine == null ? 0 : SymLine.P2.X.Value;
    var Sym_P2_y = SymLine == null ? 0 : SymLine.P2.Y.Value;
    var dx = Sym_P2_x - Sym_P1_x;
    var dy = Sym_P2_y - Sym_P1_y;
    var P1_x = Point1 == null ? 0 : Point1.X.Value;
    var P1_y = Point1 == null ? 0 : Point1.Y.Value;
    var t = -(dy * P1_x - dx * P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    var Ex = P1_x + dy * t * 2;
    var Ey = P1_y - dx * t * 2;
    var P2_x = Point2 == null ? 0 : Point2.X.Value;
    var P2_y = Point2 == null ? 0 : Point2.Y.Value;
    var tempX = Ex - P2_x;
    var tempY = Ey - P2_y;
    return tempX * tempX + tempY * tempY;
  }
}

public sealed class SymmetricLinesConstraint : Constraint
{
  public SymmetricLinesConstraint() :
    base(ConstraintEnum.SymmetricLines)
  {
  }

  public override double CalculateError()
  {
    var error = 0d;
    var Sym_P1_x = SymLine == null ? 0 : SymLine.P1.X.Value;
    var Sym_P1_y = SymLine == null ? 0 : SymLine.P1.Y.Value;
    var Sym_P2_x = SymLine == null ? 0 : SymLine.P2.X.Value;
    var Sym_P2_y = SymLine == null ? 0 : SymLine.P2.Y.Value;
    var dx = Sym_P2_x - Sym_P1_x;
    var dy = Sym_P2_y - Sym_P1_y;
    var L1_P1_x = Line1 == null ? 0 : Line1.P1.X.Value;
    var L1_P1_y = Line1 == null ? 0 : Line1.P1.Y.Value;
    var t = -(dy * L1_P1_x - dx * L1_P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    var Ex = L1_P1_x + dy * t * 2;
    var Ey = L1_P1_y - dx * t * 2;
    var L1_P2_x = Line1 == null ? 0 : Line1.P2.X.Value;
    var L1_P2_y = Line1 == null ? 0 : Line1.P2.Y.Value;
    var L2_P1_x = Line2 == null ? 0 : Line2.P1.X.Value;
    var L2_P1_y = Line2 == null ? 0 : Line2.P1.Y.Value;
    var L2_P2_x = Line2 == null ? 0 : Line2.P2.X.Value;
    var L2_P2_y = Line2 == null ? 0 : Line2.P2.Y.Value;
    var tempX = Ex - L2_P1_x;
    var tempY = Ey - L2_P1_y;
    error += tempX * tempX + tempY * tempY;

    t = -(dy * L1_P2_x - dx * L1_P2_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    Ex = L1_P2_x + dy * t * 2;
    Ey = L1_P2_y - dx * t * 2;
    tempX = Ex - L2_P2_x;
    tempY = Ey - L2_P2_y;
    error += tempX * tempX + tempY * tempY;

    return error;
  }
}

public sealed class SymmetricCirclesConstraint : Constraint
{
  public SymmetricCirclesConstraint() :
    base(ConstraintEnum.SymmetricCircles)
  {
  }

  public override double CalculateError()
  {
    var error = 0d;

    var Sym_P1_x = SymLine == null ? 0 : SymLine.P1.X.Value;
    var Sym_P1_y = SymLine == null ? 0 : SymLine.P1.Y.Value;
    var Sym_P2_x = SymLine == null ? 0 : SymLine.P2.X.Value;
    var Sym_P2_y = SymLine == null ? 0 : SymLine.P2.Y.Value;
    var C1_Center_x = Circle1 == null ? 0 : Circle1.Center.X.Value;
    var C1_Center_y = Circle1 == null ? 0 : Circle1.Center.Y.Value;
    var dx = Sym_P2_x - Sym_P1_x;
    var dy = Sym_P2_y - Sym_P1_y;
    var t = -(dy * C1_Center_x - dx * C1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    var Ex = C1_Center_x + dy * t * 2;
    var Ey = C1_Center_y - dx * t * 2;
    var C2_Center_x = Circle2 == null ? 0 : Circle2.Center.X.Value;
    var C2_Center_y = Circle2 == null ? 0 : Circle2.Center.Y.Value;
    var tempX = Ex - C2_Center_x;
    var tempY = Ey - C2_Center_y;
    error += tempX * tempX + tempY * tempY;

    var C1_rad = Circle1 == null ? 0 : Circle1.Rad.Value;
    var C2_rad = Circle2 == null ? 0 : Circle2.Rad.Value;
    var temp = C1_rad - C2_rad;
    error += temp * temp;

    return error;
  }
}

public sealed class SymmetricArcsConstraint : Constraint
{
  public SymmetricArcsConstraint() :
    base(ConstraintEnum.SymmetricArcs)
  {
  }

  public override double CalculateError()
  {
    var error = 0d;

    var Sym_P1_x = SymLine == null ? 0 : SymLine.P1.X.Value;
    var Sym_P1_y = SymLine == null ? 0 : SymLine.P1.Y.Value;
    var Sym_P2_x = SymLine == null ? 0 : SymLine.P2.X.Value;
    var Sym_P2_y = SymLine == null ? 0 : SymLine.P2.Y.Value;
    var A1_Center_x = Arc1 == null ? 0 : Arc1.Center.X.Value;
    var A1_Center_y = Arc1 == null ? 0 : Arc1.Center.Y.Value;
    var A1_radius = Arc1 == null ? 0 : Arc1.Rad.Value;
    var A1_startA = Arc1 == null ? 0 : Arc1.StartAngle.Value;
    var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
    var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
    var dx = Sym_P2_x - Sym_P1_x;
    var dy = Sym_P2_y - Sym_P1_y;

    var t = -(dy * A1_Start_x - dx * A1_Start_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    var Ex = A1_Start_x + dy * t * 2;
    var Ey = A1_Start_y - dx * t * 2;
    var A2_radius = Arc2 == null ? 0 : Arc2.Rad.Value;
    var A2_startA = Arc2 == null ? 0 : Arc2.StartAngle.Value;
    var A2_Start_x = A1_Center_x + A2_radius * Math.Cos(A2_startA);
    var A2_Start_y = A1_Center_y + A2_radius * Math.Sin(A2_startA);
    var tempX = Ex - A2_Start_x;
    var tempY = Ey - A2_Start_y;
    error += tempX * tempX + tempY * tempY;

    var A1_endA = Arc1 == null ? 0 : Arc1.EndAngle.Value;
    var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
    var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
    t = -(dy * A1_End_x - dx * A1_End_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    Ex = A1_End_x + dy * t * 2;
    Ey = A1_End_y - dx * t * 2;
    var A2_endA = Arc2 == null ? 0 : Arc2.EndAngle.Value;
    var A2_End_x = A1_Center_x + A2_radius * Math.Cos(A2_endA);
    var A2_End_y = A1_Center_y + A2_radius * Math.Sin(A2_endA);
    tempX = Ex - A2_End_x;
    tempY = Ey - A2_End_y;
    error += tempX * tempX + tempY * tempY;

    t = -(dy * A1_Center_x - dx * A1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
    Ex = A1_Center_x + dy * t * 2;
    Ey = A1_Center_y - dx * t * 2;
    var A2_Center_x = Arc2 == null ? 0 : Arc2.Center.X.Value;
    var A2_Center_y = Arc2 == null ? 0 : Arc2.Center.Y.Value;
    tempX = Ex - A2_Center_x;
    tempY = Ey - A2_Center_y;
    error += tempX * tempX + tempY * tempY;

    return error;
  }
}

public sealed class RadiusValueConstraint : Constraint
{
  public RadiusValueConstraint() :
    base(ConstraintEnum.RadiusValue)
  {
  }

  public override double CalculateError()
  {
    throw new NotImplementedException();
  }
}
