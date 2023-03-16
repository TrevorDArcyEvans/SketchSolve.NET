namespace SketchSolve;

using System.Collections;

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

  public static double Calculate(IEnumerable<Constraint> constraints)
  {
    double error = 0;

    foreach (var constraint in constraints)
    {
      // Crappy hack but it will get us going
      var P1_x = constraint.Point1 == null ? 0 : constraint.Point1.X.Value;
      var P1_y = constraint.Point1 == null ? 0 : constraint.Point1.Y.Value;
      var P2_x = constraint.Point2 == null ? 0 : constraint.Point2.X.Value;
      var P2_y = constraint.Point2 == null ? 0 : constraint.Point2.Y.Value;

      var L1_P1_x = constraint.Line1 == null ? 0 : constraint.Line1.P1.X.Value;
      var L1_P1_y = constraint.Line1 == null ? 0 : constraint.Line1.P1.Y.Value;
      var L1_P2_x = constraint.Line1 == null ? 0 : constraint.Line1.P2.X.Value;
      var L1_P2_y = constraint.Line1 == null ? 0 : constraint.Line1.P2.Y.Value;
      var L2_P1_x = constraint.Line2 == null ? 0 : constraint.Line2.P1.X.Value;
      var L2_P1_y = constraint.Line2 == null ? 0 : constraint.Line2.P1.Y.Value;
      var L2_P2_x = constraint.Line2 == null ? 0 : constraint.Line2.P2.X.Value;
      var L2_P2_y = constraint.Line2 == null ? 0 : constraint.Line2.P2.Y.Value;

      var C1_Center_x = constraint.Circle1 == null ? 0 : constraint.Circle1.Center.X.Value;
      var C1_Center_y = constraint.Circle1 == null ? 0 : constraint.Circle1.Center.Y.Value;
      var C1_rad = constraint.Circle1 == null ? 0 : constraint.Circle1.Rad.Value;
      var C2_Center_x = constraint.Circle2 == null ? 0 : constraint.Circle2.Center.X.Value;
      var C2_Center_y = constraint.Circle2 == null ? 0 : constraint.Circle2.Center.Y.Value;
      var C2_rad = constraint.Circle2 == null ? 0 : constraint.Circle2.Rad.Value;

      var A1_startA = constraint.Arc1 == null ? 0 : constraint.Arc1.StartAngle.Value;
      var A1_endA = constraint.Arc1 == null ? 0 : constraint.Arc1.EndAngle.Value;
      var A1_radius = constraint.Arc1 == null ? 0 : constraint.Arc1.Rad.Value;
      var A1_Center_x = constraint.Arc1 == null ? 0 : constraint.Arc1.Center.X.Value;
      var A1_Center_y = constraint.Arc1 == null ? 0 : constraint.Arc1.Center.Y.Value;
      var A2_startA = constraint.Arc2 == null ? 0 : constraint.Arc2.StartAngle.Value;
      var A2_endA = constraint.Arc2 == null ? 0 : constraint.Arc2.EndAngle.Value;
      var A2_radius = constraint.Arc2 == null ? 0 : constraint.Arc2.Rad.Value;
      var A2_Center_x = constraint.Arc2 == null ? 0 : constraint.Arc2.Center.X.Value;
      var A2_Center_y = constraint.Arc2 == null ? 0 : constraint.Arc2.Center.Y.Value;

      var A1_Start_x = A1_Center_x + A1_radius * Math.Cos(A1_startA);
      var A1_Start_y = A1_Center_y + A1_radius * Math.Sin(A1_startA);
      var A1_End_x = A1_Center_x + A1_radius * Math.Cos(A1_endA);
      var A1_End_y = A1_Center_y + A1_radius * Math.Sin(A1_endA);
      var A2_Start_x = A1_Center_x + A2_radius * Math.Cos(A2_startA);
      var A2_Start_y = A1_Center_y + A2_radius * Math.Sin(A2_startA);
      var A2_End_x = A1_Center_x + A2_radius * Math.Cos(A2_endA);
      var A2_End_y = A1_Center_y + A2_radius * Math.Sin(A2_endA);


      var length = constraint.Parameter == null ? 0 : constraint.Parameter.Value;
      var distance = length;
      var radius = length;

      var Sym_P1_x = constraint.SymLine == null ? 0 : constraint.SymLine.P1.X.Value;
      var Sym_P1_y = constraint.SymLine == null ? 0 : constraint.SymLine.P1.Y.Value;

      var Sym_P2_x = constraint.SymLine == null ? 0 : constraint.SymLine.P2.X.Value;
      var Sym_P2_y = constraint.SymLine == null ? 0 : constraint.SymLine.P2.Y.Value;


      switch (constraint.ContraintType)
      {
        case ConstraintEnum.PointOnPoint:
        {
          //Hopefully avoid this constraint, make coincident points use the same parameters
          var l2 = (constraint.Point1 - constraint.Point2).LengthSquared;
          error += l2;
        }
          break;

        case ConstraintEnum.P2PDistance:
        {
          error += (P1_x - P2_x) * (P1_x - P2_x) + (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
        }
          break;

        case ConstraintEnum.P2PDistanceVert:
        {
          error += (P1_y - P2_y) * (P1_y - P2_y) - distance * distance;
        }
          break;

        case ConstraintEnum.P2PDistanceHoriz:
        {
          error += (P1_x - P2_x) * (P1_x - P2_x) - distance * distance;
        }
          break;

        case ConstraintEnum.PointOnLine:
        {
          var dx = L1_P2_x - L1_P1_x;
          var dy = L1_P2_y - L1_P1_y;

          var m = dy / dx; // Slope
          var n = dx / dy; // 1/Slope

          if (m <= 1 && m >= -1)
          {
            //Calculate the expected y point given the x coordinate of the point
            var Ey = L1_P1_y + m * (P1_x - L1_P1_x);
            error += (Ey - P1_y) * (Ey - P1_y);
          }
          else
          {
            //Calculate the expected x point given the y coordinate of the point
            var Ex = L1_P1_x + n * (P1_y - L1_P1_y);
            error += (Ex - P1_x) * (Ex - P1_x);
          }
        }
          break;

        case ConstraintEnum.P2LDistance:
        {
          var dx = L1_P2_x - L1_P1_x;
          var dy = L1_P2_y - L1_P1_y;

          var t = -(L1_P1_x * dx - P1_x * dx + L1_P1_y * dy - P1_y * dy) / (dx * dx + dy * dy);
          var Xint = L1_P1_x + dx * t;
          var Yint = L1_P1_y + dy * t;
          var temp = Hypot(P1_x - Xint, P1_y - Yint) - distance;
          error += temp * temp / 10;
        }
          break;

        case ConstraintEnum.P2LDistanceVert:
        {
          var dx = L1_P2_x - L1_P1_x;
          var dy = L1_P2_y - L1_P1_y;

          var t = (P1_x - L1_P1_x) / dx;
          var Yint = L1_P1_y + dy * t;
          var temp = Math.Abs(P1_y - Yint) - distance;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.P2LDistanceHoriz:
        {
          var dx = L1_P2_x - L1_P1_x;
          var dy = L1_P2_y - L1_P1_y;

          var t = (P1_y - L1_P1_y) / dy;
          var Xint = L1_P1_x + dx * t;
          var temp = Math.Abs(P1_x - Xint) - distance;
          error += temp * temp / 10;
        }
          break;

        case ConstraintEnum.Vertical:
        {
          var odx = L1_P2_x - L1_P1_x;
          error += odx * odx;
        }
          break;

        case ConstraintEnum.Horizontal:
        {
          var ody = L1_P2_y - L1_P1_y;
          error += ody * ody;
        }
          break;

        case ConstraintEnum.TangentToCircle:
        {
          var line = constraint.Line1;
          var circle = constraint.Circle1;
          var temp = circle.CenterTo(line).Vector.Length - circle.Rad.Value;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.TangentToArc:
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

          var dx = L1_P2_x - L1_P1_x;
          var dy = L1_P2_y - L1_P1_y;

          var radsq = (A1_Center_x - A1_Start_x) * (A1_Center_x - A1_Start_x) + (A1_Center_y - A1_Start_y) * (A1_Center_y - A1_Start_y);
          var t = -(L1_P1_x * dx - A1_Center_x * dx + L1_P1_y * dy - A1_Center_y * dy) / (dx * dx + dy * dy);
          var Xint = L1_P1_x + dx * t;
          var Yint = L1_P1_y + dy * t;
          var temp = (A1_Center_x - Xint) * (A1_Center_x - Xint) + (A1_Center_y - Yint) * (A1_Center_y - Yint) - radsq;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.ArcRules:
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
          break;

        case ConstraintEnum.LineLength:
        {
          var temp = Math.Sqrt(Math.Pow(L1_P2_x - L1_P1_x, 2) + Math.Pow(L1_P2_y - L1_P1_y, 2)) - length;
          //temp=Hypot(L1_P2_x - L1_P1_x , L1_P2_y - L1_P1_y) - length;
          error += temp * temp * 100;
        }
          break;

        case ConstraintEnum.EqualLength:
        {
          var temp = Hypot(L1_P2_x - L1_P1_x, L1_P2_y - L1_P1_y) - Hypot(L2_P2_x - L2_P1_x, L2_P2_y - L2_P1_y);
          error += temp * temp;
        }
          break;

        case ConstraintEnum.ArcRadius:
        {
          var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
          var rad2 = Hypot(A1_Center_x - A1_End_x, A1_Center_y - A1_End_y);
          var temp = rad1 - radius;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.EqualRadiusArcs:
        {
          var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
          var rad2 = Hypot(A2_Center_x - A2_Start_x, A2_Center_y - A2_Start_y);
          var temp = rad1 - rad2;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.EqualRadiusCircles:
        {
          var temp = C1_rad - C2_rad;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.EqualRadiusCircArc:
        {
          var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
          var temp = rad1 - C1_rad;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.ConcentricArcs:
        {
          var temp = Hypot(A1_Center_x - A2_Center_x, A1_Center_y - A2_Center_y);
          error += temp * temp;
        }
          break;

        case ConstraintEnum.ConcentricCircles:
        {
          var temp = Hypot(C1_Center_x - C2_Center_x, C1_Center_y - C2_Center_y);
          error += temp * temp;
        }
          break;

        case ConstraintEnum.ConcentricCircArc:
        {
          var temp = Hypot(A1_Center_x - C1_Center_x, A1_Center_y - C1_Center_y);
          error += temp * temp;
        }
          break;

        case ConstraintEnum.CircleRadius:
        {
          error += (C1_rad - radius) * (C1_rad - radius);
        }
          break;

        case ConstraintEnum.InternalAngle:
        {
          var angleP = length;
          var temp = constraint.Line1.Vector.Cosine(constraint.Line2.Vector);
          var temp2 = Math.Cos(angleP);
          error += (temp - temp2) * (temp - temp2);
        }
          break;

        case ConstraintEnum.ExternalAngle:
        {
          var angleP = length;
          var temp = constraint.Line1.Vector.Cosine(constraint.Line2.Vector);
          var temp2 = Math.Cos(Math.PI - angleP);
          error += (temp - temp2) * (temp - temp2);
        }
          break;

        case ConstraintEnum.Perpendicular:
        {
          var temp = constraint.Line1.Vector.Dot(constraint.Line2.Vector);
          error += temp * temp;
        }
          break;

        case ConstraintEnum.Parallel:
        {
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
          error += temp * temp;
        }
          break;

        case ConstraintEnum.Collinear:
        {
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
            var Ey = L1_P1_y + m * (L2_P1_x - L1_P1_x);
            error += (Ey - L2_P1_y) * (Ey - L2_P1_y);

            Ey = L1_P1_y + m * (L2_P2_x - L1_P1_x);
            error += (Ey - L2_P2_y) * (Ey - L2_P2_y);
          }
          else
          {
            //Calculate the expected x point given the y coordinate of the point
            var Ex = L1_P1_x + n * (L2_P1_y - L1_P1_y);
            error += (Ex - L2_P1_x) * (Ex - L2_P1_x);

            Ex = L1_P1_x + n * (L2_P2_y - L1_P1_y);
            error += (Ex - L2_P2_x) * (Ex - L2_P2_x);
          }
        }
          break;

        case ConstraintEnum.PointOnCircle:
        {
          //see what the current radius to the point is
          var rad1 = Hypot(C1_Center_x - P1_x, C1_Center_y - P1_y);
          //Compare this radius to the radius of the circle, return the error squared
          var temp = rad1 - C1_rad;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.PointOnArc:
        {
          //see what the current radius to the point is
          var rad1 = Hypot(A1_Center_x - P1_x, A1_Center_y - P1_y);
          var rad2 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
          //Compare this radius to the radius of the circle, return the error squared
          var temp = rad1 - rad2;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.PointOnLineMidpoint:
        {
          var Ex = (L1_P1_x + L1_P2_x) / 2;
          var Ey = (L1_P1_y + L1_P2_y) / 2;
          var tempX = Ex - P1_x;
          var tempY = Ey - P1_y;
          error += tempX * tempX + tempY * tempY;
        }
          break;

        case ConstraintEnum.PointOnArcMidpoint:
        {
          var rad1 = Hypot(A1_Center_x - A1_Start_x, A1_Center_y - A1_Start_y);
          var tempStart = Math.Atan2(A1_Start_y - A1_Center_y, A1_Start_x - A1_Center_x);
          var tempEnd = Math.Atan2(A1_End_y - A1_Center_y, A1_End_x - A1_Center_x);
          var Ex = A1_Center_x + rad1 * Math.Cos((tempEnd + tempStart) / 2);
          var Ey = A1_Center_y + rad1 * Math.Sin((tempEnd + tempStart) / 2);
          var tempX = Ex - P1_x;
          var tempY = Ey - P1_y;
          error += tempX * tempX + tempY * tempY;
        }
          break;

        case ConstraintEnum.PointOnCircleQuad:
        {
          var Ex = C1_Center_x;
          var Ey = C1_Center_y;
          var quadIndex = length;
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

          var tempX = Ex - P1_x;
          var tempY = Ey - P1_y;
          error += tempX * tempX + tempY * tempY;
        }

          break;

        case ConstraintEnum.SymmetricPoints:
        {
          var dx = Sym_P2_x - Sym_P1_x;
          var dy = Sym_P2_y - Sym_P1_y;
          var t = -(dy * P1_x - dx * P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          var Ex = P1_x + dy * t * 2;
          var Ey = P1_y - dx * t * 2;
          var tempX = Ex - P2_x;
          var tempY = Ey - P2_y;
          error += tempX * tempX + tempY * tempY;
        }
          break;

        case ConstraintEnum.SymmetricLines:
        {
          var dx = Sym_P2_x - Sym_P1_x;
          var dy = Sym_P2_y - Sym_P1_y;
          var t = -(dy * L1_P1_x - dx * L1_P1_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          var Ex = L1_P1_x + dy * t * 2;
          var Ey = L1_P1_y - dx * t * 2;
          var tempX = Ex - L2_P1_x;
          var tempY = Ey - L2_P1_y;
          error += tempX * tempX + tempY * tempY;

          t = -(dy * L1_P2_x - dx * L1_P2_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          Ex = L1_P2_x + dy * t * 2;
          Ey = L1_P2_y - dx * t * 2;
          tempX = Ex - L2_P2_x;
          tempY = Ey - L2_P2_y;
          error += tempX * tempX + tempY * tempY;
        }
          break;

        case ConstraintEnum.SymmetricCircles:
        {
          var dx = Sym_P2_x - Sym_P1_x;
          var dy = Sym_P2_y - Sym_P1_y;
          var t = -(dy * C1_Center_x - dx * C1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          var Ex = C1_Center_x + dy * t * 2;
          var Ey = C1_Center_y - dx * t * 2;
          var tempX = Ex - C2_Center_x;
          var tempY = Ey - C2_Center_y;
          error += tempX * tempX + tempY * tempY;

          var temp = C1_rad - C2_rad;
          error += temp * temp;
        }
          break;

        case ConstraintEnum.SymmetricArcs:
        {
          var dx = Sym_P2_x - Sym_P1_x;
          var dy = Sym_P2_y - Sym_P1_y;

          var t = -(dy * A1_Start_x - dx * A1_Start_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          var Ex = A1_Start_x + dy * t * 2;
          var Ey = A1_Start_y - dx * t * 2;
          var tempX = Ex - A2_Start_x;
          var tempY = Ey - A2_Start_y;
          error += tempX * tempX + tempY * tempY;

          t = -(dy * A1_End_x - dx * A1_End_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          Ex = A1_End_x + dy * t * 2;
          Ey = A1_End_y - dx * t * 2;
          tempX = Ex - A2_End_x;
          tempY = Ey - A2_End_y;
          error += tempX * tempX + tempY * tempY;

          t = -(dy * A1_Center_x - dx * A1_Center_y - dy * Sym_P1_x + dx * Sym_P1_y) / (dx * dx + dy * dy);
          Ex = A1_Center_x + dy * t * 2;
          Ey = A1_Center_y - dx * t * 2;
          tempX = Ex - A2_Center_x;
          tempY = Ey - A2_Center_y;
          error += tempX * tempX + tempY * tempY;
        }
          break;

        default:
          throw new ArgumentOutOfRangeException($"Unknown constraint type: {constraint.ContraintType}");
      }
    }

    // Prevent symmetry errors
    return error;
  }
};
