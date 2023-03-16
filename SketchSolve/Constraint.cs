﻿using System.Collections;

namespace SketchSolve;

public class Constraint : IEnumerable<Parameter>
{
  public ConstraintEnum ContraintType;
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
      new[] { Parameter }
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

  private static double Hypot(double a, double b)
  {
    return Math.Sqrt(a * a + b * b);
  }

  public static double Calc(Constraint[] cons)
  {
    var consLength = cons.Length;
    double error = 0;
    double temp, dx, dy, m, n, Ex, Ey, rad1, rad2, t, Xint, Yint, dx2, dy2, hyp1, hyp2, temp2;

    for (var i = 0; i < consLength; i++)
    {
      // Crappy hack but it will get us going
      var P1_x = cons[i].Point1 == null ? 0 : cons[i].Point1.X.Value;
      var P1_y = cons[i].Point1 == null ? 0 : cons[i].Point1.Y.Value;
      var P2_x = cons[i].Point2 == null ? 0 : cons[i].Point2.X.Value;
      var P2_y = cons[i].Point2 == null ? 0 : cons[i].Point2.Y.Value;
      var L1_P1_x = cons[i].Line1 == null ? 0 : cons[i].Line1.P1.X.Value;
      var L1_P1_y = cons[i].Line1 == null ? 0 : cons[i].Line1.P1.Y.Value;
      var L1_P2_x = cons[i].Line1 == null ? 0 : cons[i].Line1.P2.X.Value;
      var L1_P2_y = cons[i].Line1 == null ? 0 : cons[i].Line1.P2.Y.Value;
      var L2_P1_x = cons[i].Line2 == null ? 0 : cons[i].Line2.P1.X.Value;
      var L2_P1_y = cons[i].Line2 == null ? 0 : cons[i].Line2.P1.Y.Value;
      var L2_P2_x = cons[i].Line2 == null ? 0 : cons[i].Line2.P2.X.Value;
      var L2_P2_y = cons[i].Line2 == null ? 0 : cons[i].Line2.P2.Y.Value;
      var C1_Center_x = cons[i].Circle1 == null ? 0 : cons[i].Circle1.Center.X.Value;
      var C1_Center_y = cons[i].Circle1 == null ? 0 : cons[i].Circle1.Center.Y.Value;
      var C1_rad = cons[i].Circle1 == null ? 0 : cons[i].Circle1.Rad.Value;
      var C2_Center_x = cons[i].Circle2 == null ? 0 : cons[i].Circle2.Center.X.Value;
      var C2_Center_y = cons[i].Circle2 == null ? 0 : cons[i].Circle2.Center.Y.Value;
      var C2_rad = cons[i].Circle2 == null ? 0 : cons[i].Circle2.Rad.Value;

      var A1_startA = cons[i].Arc1 == null ? 0 : cons[i].Arc1.StartAngle.Value;
      var A1_endA = cons[i].Arc1 == null ? 0 : cons[i].Arc1.EndAngle.Value;
      var A1_radius = cons[i].Arc1 == null ? 0 : cons[i].Arc1.Rad.Value;
      var A1_Center_x = cons[i].Arc1 == null ? 0 : cons[i].Arc1.Center.X.Value;
      var A1_Center_y = cons[i].Arc1 == null ? 0 : cons[i].Arc1.Center.Y.Value;
      var A2_startA = cons[i].Arc2 == null ? 0 : cons[i].Arc2.StartAngle.Value;
      var A2_endA = cons[i].Arc2 == null ? 0 : cons[i].Arc2.EndAngle.Value;
      var A2_radius = cons[i].Arc2 == null ? 0 : cons[i].Arc2.Rad.Value;
      var A2_Center_x = cons[i].Arc2 == null ? 0 : cons[i].Arc2.Center.X.Value;
      var A2_Center_y = cons[i].Arc2 == null ? 0 : cons[i].Arc2.Center.Y.Value;

      var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
      var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
      var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
      var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
      var A2_Start_x = A1_Center_x + A2_radius * Math.Cos(A2_startA);
      var A2_Start_y = A1_Center_y + A2_radius * Math.Sin(A2_startA);
      var A2_End_x = A1_Center_x + A2_radius * Math.Cos(A2_endA);
      var A2_End_y = A1_Center_y + A2_radius * Math.Sin(A2_endA);


      var length = cons[i].Parameter == null ? 0 : cons[i].Parameter.Value;
      var distance = length;
      var radius = length;
      var angleP = length;
      var quadIndex = length;

      var Sym_P1_x = cons[i].SymLine == null ? 0 : cons[i].SymLine.P1.X.Value;
      var Sym_P1_y = cons[i].SymLine == null ? 0 : cons[i].SymLine.P1.Y.Value;

      var Sym_P2_x = cons[i].SymLine == null ? 0 : cons[i].SymLine.P2.X.Value;
      var Sym_P2_y = cons[i].SymLine == null ? 0 : cons[i].SymLine.P2.Y.Value;


      if (cons[i].ContraintType == ConstraintEnum.PointOnPoint)
      {
        //Hopefully avoid this constraint, make coincident points use the same parameters
        var l2 = (cons[i].Point1 - cons[i].Point2).LengthSquared;
        error += l2;
      }


      if (cons[i].ContraintType == ConstraintEnum.P2PDistance)
      {
        error += (P1_x - P2_x) * (P1_x - P2_x) + (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
      }

      if (cons[i].ContraintType == ConstraintEnum.P2PDistanceVert)
      {
        error += (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
      }

      if (cons[i].ContraintType == ConstraintEnum.P2PDistanceHoriz)
      {
        error += (P1_x - P2_x) * (P1_x - P2_x) - distance * distance;
      }


      if (cons[i].ContraintType == ConstraintEnum.PointOnLine)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;

        m = dy / dx; // Slope
        n = dx / dy; // 1/Slope

        if (m <= 1 && m >= -1)
        {
          //Calculate the expected y point given the x coordinate of the point
          Ey = L1_P1_y + m * (P1_x - L1_P1_x);
          error += (Ey - P1_y) * (Ey - P1_y);
        }
        else
        {
          //Calculate the expected x point given the y coordinate of the point
          Ex = L1_P1_x + n * (P1_y - L1_P1_y);
          error += (Ex - P1_x) * (Ex - P1_x);
        }
      }

      if (cons[i].ContraintType == ConstraintEnum.P2LDistance)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;

        t = -(L1_P1_x * dx - P1_x * dx + L1_P1_y * dy - P1_y * dy) / (dx * dx + dy * dy);
        Xint = L1_P1_x + dx * t;
        Yint = L1_P1_y + dy * t;
        temp = Hypot(P1_x - Xint, P1_y - Yint) - distance;
        error += temp * temp / 10;
      }

      if (cons[i].ContraintType == ConstraintEnum.P2LDistanceVert)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;

        t = (P1_x - L1_P1_x) / dx;
        Yint = L1_P1_y + dy * t;
        temp = Math.Abs(P1_y - Yint) - distance;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.P2LDistanceHoriz)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;

        t = (P1_y - L1_P1_y) / dy;
        Xint = L1_P1_x + dx * t;
        temp = Math.Abs(P1_x - Xint) - distance;
        error += temp * temp / 10;
      }


      if (cons[i].ContraintType == ConstraintEnum.Vertical)
      {
        var odx = L1_P2_x - L1_P1_x;
        error += odx * odx;
      }

      if (cons[i].ContraintType == ConstraintEnum.Horizontal)
      {
        var ody = L1_P2_y - L1_P1_y;
        error += ody * ody;
      }

      if (cons[i].ContraintType == ConstraintEnum.TangentToCircle)
      {
        var l = cons[i].Line1;
        var c = cons[i].Circle1;
        temp = c.CenterTo(l).Vector.Length - c.Rad.Value;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.TangentToArc)
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

        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;


        var radsq = (A1_Center_x - A1_Start_x) * (A1_Center_x - A1_Start_x) + (A1_Center_y - A1_Start_y) * (A1_Center_y - A1_Start_y);
        t = -(L1_P1_x * dx - A1_Center_x * dx + L1_P1_y * dy - A1_Center_y * dy) / (dx * dx + dy * dy);
        Xint = L1_P1_x + dx * t;
        Yint = L1_P1_y + dy * t;
        temp = (A1_Center_x - Xint) * (A1_Center_x - Xint) + (A1_Center_y - Yint) * (A1_Center_y - Yint) - radsq;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.ArcRules)
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
        var a1endx2 = A1_End_x * A1_End_x;
        var a1endy2 = A1_End_y * A1_End_y;
        var a1startx2 = A1_Start_x * A1_Start_x;
        var a1starty2 = A1_Start_y * A1_Start_y;
        var num = -2 * A1_Center_x * A1_End_x + a1endx2 - 2 * A1_Center_y * A1_End_y + a1endy2 + 2 * A1_Center_x * A1_Start_x - a1startx2 + 2 * A1_Center_y * A1_Start_y - a1starty2;
        error += num * num / (4.0 * a1endx2 + a1endy2 - 2 * A1_End_x * A1_Start_x + a1startx2 - 2 * A1_End_y * A1_Start_y + a1starty2);
      }

      if (cons[i].ContraintType == ConstraintEnum.LineLength)
      {
        temp = Math.Sqrt(Math.Pow(L1_P2_x - L1_P1_x, 2) + Math.Pow(L1_P2_y - L1_P1_y, 2)) - length;
        //temp=Hypot(L1_P2_x - L1_P1_x , L1_P2_y - L1_P1_y) - length;
        error += temp * temp * 100;
      }

      if (cons[i].ContraintType == ConstraintEnum.EqualLength)
      {
        temp = Hypot(L1_P2_x - L1_P1_x, L1_P2_y - L1_P1_y) - Hypot(L2_P2_x - L2_P1_x, L2_P2_y - L2_P1_y);
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.ArcRadius)
      {
        rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
        rad2 = Hypot(A1_Center_x - A1_End_x, A1_Center_y - A1_End_y);
        temp = rad1 - radius;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.EqualRadiusArcs)
      {
        rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
        rad2 = Hypot(A2_Center_x - A2_Start_x, A2_Center_y - A2_Start_y);
        temp = rad1 - rad2;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.EqualRadiusCircles)
      {
        temp = C1_rad - C2_rad;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.EqualRadiusCircArc)
      {
        rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
        temp = rad1 - C1_rad;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.ConcentricArcs)
      {
        temp = Hypot(A1_Center_x - A2_Center_x, A1_Center_y - A2_Center_y);
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.ConcentricCircles)
      {
        temp = Hypot(C1_Center_x - C2_Center_x, C1_Center_y - C2_Center_y);
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.ConcentricCircArc)
      {
        temp = Hypot(A1_Center_x - C1_Center_x, A1_Center_y - C1_Center_y);
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.CircleRadius)
      {
        error += (C1_rad - radius) * (C1_rad - radius);
      }

      if (cons[i].ContraintType == ConstraintEnum.InternalAngle)
      {
        temp = cons[i].Line1.Vector.Cosine(cons[i].Line2.Vector);

        temp2 = Math.Cos(angleP);
        error += (temp - temp2) * (temp - temp2);
      }

      if (cons[i].ContraintType == ConstraintEnum.ExternalAngle)
      {
        temp = cons[i].Line1.Vector.Cosine(cons[i].Line2.Vector);
        temp2 = Math.Cos(Math.PI - angleP);
        error += (temp - temp2) * (temp - temp2);
      }

      if (cons[i].ContraintType == ConstraintEnum.Perpendicular)
      {
        temp = cons[i].Line1.Vector.Dot(cons[i].Line2.Vector);
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.Parallel)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;
        dx2 = L2_P2_x - L2_P1_x;
        dy2 = L2_P2_y - L2_P1_y;

        hyp1 = Hypot(dx, dy);
        hyp2 = Hypot(dx2, dy2);

        dx = dx / hyp1;
        dy = dy / hyp1;
        dx2 = dx2 / hyp2;
        dy2 = dy2 / hyp2;

        temp = dy * dx2 - dx * dy2;
        error += temp * temp;
      }

      // Collinear constraint
      if (cons[i].ContraintType == ConstraintEnum.Collinear)
      {
        dx = L1_P2_x - L1_P1_x;
        dy = L1_P2_y - L1_P1_y;

        m = dy / dx;
        n = dx / dy;
        // Calculate the error between the expected intersection point
        // and the true point of the second lines two end points on the
        // first line
        if (m <= 1 && m > -1)
        {
          //Calculate the expected y point given the x coordinate of the point
          Ey = L1_P1_y + m * (L2_P1_x - L1_P1_x);
          error += (Ey - L2_P1_y) * (Ey - L2_P1_y);

          Ey = L1_P1_y + m * (L2_P2_x - L1_P1_x);
          error += (Ey - L2_P2_y) * (Ey - L2_P2_y);
        }
        else
        {
          //Calculate the expected x point given the y coordinate of the point
          Ex = L1_P1_x + n * (L2_P1_y - L1_P1_y);
          error += (Ex - L2_P1_x) * (Ex - L2_P1_x);

          Ex = L1_P1_x + n * (L2_P2_y - L1_P1_y);
          error += (Ex - L2_P2_x) * (Ex - L2_P2_x);
        }
      }

      // Point on a circle
      if (cons[i].ContraintType == ConstraintEnum.PointOnCircle)
      {
        //see what the current radius to the point is
        rad1 = Hypot(C1_Center_x - P1_x, C1_Center_y - P1_y);
        //Compare this radius to the radius of the circle, return the error squared
        temp = rad1 - C1_rad;
        error += temp * temp;
        //cout<<"Point On circle error"<<temp*temp<<endl;
      }

      if (cons[i].ContraintType == ConstraintEnum.PointOnArc)
      {
        //see what the current radius to the point is
        rad1 = Hypot(A1_Center_x - P1_x, A1_Center_y - P1_y);
        rad2 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
        //Compare this radius to the radius of the circle, return the error squared
        temp = rad1 - rad2;
        error += temp * temp;
        //cout<<"Point On circle error"<<temp*temp<<endl;
      }

      if (cons[i].ContraintType == ConstraintEnum.PointOnLineMidpoint)
      {
        Ex = (L1_P1_x + L1_P2_x) / 2;
        Ey = (L1_P1_y + L1_P2_y) / 2;
        temp = Ex - P1_x;
        temp2 = Ey - P1_y;
        error += temp * temp + temp2 * temp2;
      }

      if (cons[i].ContraintType == ConstraintEnum.PointOnArcMidpoint)
      {
        rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
        temp = Math.Atan2(A1_Start_y - A1_Center_y, A1_Start_x - A1_Center_x);
        temp2 = Math.Atan2(A1_End_y - A1_Center_y, A1_End_x - A1_Center_x);
        Ex = A1_Center_x + rad1 * Math.Cos((temp2 + temp) / 2);
        Ey = A1_Center_y + rad1 * Math.Sin((temp2 + temp) / 2);
        temp = Ex - P1_x;
        temp2 = Ey - P1_y;
        error += temp * temp + temp2 * temp2;
      }

      if (cons[i].ContraintType == ConstraintEnum.PointOnCircleQuad)
      {
        Ex = C1_Center_x;
        Ey = C1_Center_y;
        switch ((int)quadIndex)
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

        temp = Ex - P1_x;
        temp2 = Ey - P1_y;
        error += temp * temp + temp2 * temp2;
      }

      if (cons[i].ContraintType == ConstraintEnum.SymmetricPoints)
      {
        dx = Sym_P2_x - Sym_P1_x;
        dy = Sym_P2_y - Sym_P1_y;
        t = -(dy * P1_x - dx * P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = P1_x + dy * t * 2;
        Ey = P1_y - dx * t * 2;
        temp = Ex - P2_x;
        temp2 = Ey - P2_y;
        error += temp * temp + temp2 * temp2;
      }

      if (cons[i].ContraintType == ConstraintEnum.SymmetricLines)
      {
        dx = Sym_P2_x - Sym_P1_x;
        dy = Sym_P2_y - Sym_P1_y;
        t = -(dy * L1_P1_x - dx * L1_P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = L1_P1_x + dy * t * 2;
        Ey = L1_P1_y - dx * t * 2;
        temp = Ex - L2_P1_x;
        temp2 = Ey - L2_P1_y;
        error += temp * temp + temp2 * temp2;
        t = -(dy * L1_P2_x - dx * L1_P2_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = L1_P2_x + dy * t * 2;
        Ey = L1_P2_y - dx * t * 2;
        temp = Ex - L2_P2_x;
        temp2 = Ey - L2_P2_y;
        error += temp * temp + temp2 * temp2;
      }

      if (cons[i].ContraintType == ConstraintEnum.SymmetricCircles)
      {
        dx = Sym_P2_x - Sym_P1_x;
        dy = Sym_P2_y - Sym_P1_y;
        t = -(dy * C1_Center_x - dx * C1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = C1_Center_x + dy * t * 2;
        Ey = C1_Center_y - dx * t * 2;
        temp = Ex - C2_Center_x;
        temp2 = Ey - C2_Center_y;
        error += temp * temp + temp2 * temp2;
        temp = C1_rad - C2_rad;
        error += temp * temp;
      }

      if (cons[i].ContraintType == ConstraintEnum.SymmetricArcs)
      {
        dx = Sym_P2_x - Sym_P1_x;
        dy = Sym_P2_y - Sym_P1_y;
        t = -(dy * A1_Start_x - dx * A1_Start_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = A1_Start_x + dy * t * 2;
        Ey = A1_Start_y - dx * t * 2;
        temp = Ex - A2_Start_x;
        temp2 = Ey - A2_Start_y;
        error += temp * temp + temp2 * temp2;
        t = -(dy * A1_End_x - dx * A1_End_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = A1_End_x + dy * t * 2;
        Ey = A1_End_y - dx * t * 2;
        temp = Ex - A2_End_x;
        temp2 = Ey - A2_End_y;
        error += temp * temp + temp2 * temp2;
        t = -(dy * A1_Center_x - dx * A1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
        Ex = A1_Center_x + dy * t * 2;
        Ey = A1_Center_y - dx * t * 2;
        temp = Ex - A2_Center_x;
        temp2 = Ey - A2_Center_y;
        error += temp * temp + temp2 * temp2;
      }
    }

    // Prevent symmetry errors
    return error;
  }
};
