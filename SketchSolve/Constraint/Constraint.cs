using System.Collections;
namespace SketchSolve.Constraint;

using SketchSolve.Model;

public abstract class Constraint : IEnumerable<Parameter>
{
  public abstract double CalculateError();

  protected static double Hypot(double a, double b)
  {
    return Math.Sqrt(a * a + b * b);
  }

  protected abstract IEnumerable<IEnumerable<Parameter>> GetParameters();

  public IEnumerator<Parameter> GetEnumerator()
  {
    return GetParameters()
      .Where(p => p != null)
      .SelectMany(p => p)
      .Where(p => p != null)
      .GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}

public sealed class PointOnPointConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;

  public PointOnPointConstraint(Point point1, Point point2)
  {
    _point1 = point1;
    _point2 = point2;
  }

  public override double CalculateError()
  {
    //Hopefully avoid this constraint, make coincident points use the same parameters
    var lengthSquared = (_point1 - _point2).LengthSquared;
    return lengthSquared;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2
    };
  }
}

public sealed class HorizontalConstraint : Constraint
{
  private readonly Line _line1;

  public HorizontalConstraint(Line line1)
  {
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var ody = l1P2Y - l1P1Y;
    return ody * ody;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1
    };
  }
}

public sealed class VerticalConstraint : Constraint
{
  private readonly Line _line1;

  public VerticalConstraint(Line line1)
  {
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P2X = _line1.P2.X.Value;
    var odx = l1P2X - l1P1X;
    return odx * odx;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1
    };
  }
}

public sealed class InternalAngleConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;
  private readonly Parameter _parameter;

  public InternalAngleConstraint(Line line1, Line line2, Parameter parameter)
  {
    _line1 = line1;
    _line2 = line2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var angleP = _parameter.Value;
    var temp = _line1.Vector.Cosine(_line2.Vector);
    var temp2 = Math.Cos(angleP);
    return (temp - temp2) * (temp - temp2);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2,
      new[] {_parameter}
    };
  }
}

public sealed class ExternalAngleConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;
  private readonly Parameter _parameter;

  public ExternalAngleConstraint(Line line1, Line line2, Parameter parameter)
  {
    _line1 = line1;
    _line2 = line2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var angleP = _parameter.Value;
    var temp = _line1.Vector.Cosine(_line2.Vector);
    var temp2 = Math.Cos(Math.PI - angleP);
    return (temp - temp2) * (temp - temp2);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2,
      new[] {_parameter}
    };
  }
}

public sealed class PerpendicularConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public PerpendicularConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var temp = _line1.Vector.Dot(_line2.Vector);
    return temp * temp;
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

public sealed class TangentToCircleConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Circle _circle1;

  public TangentToCircleConstraint(Line line1, Circle circle1)
  {
    _line1 = line1;
    _circle1 = circle1;
  }

  public override double CalculateError()
  {
    var temp = _circle1.CenterTo(_line1).Vector.Length - _circle1.Rad.Value;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _circle1
    };
  }
}

public sealed class P2PDistanceConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Parameter _parameter;

  public P2PDistanceConstraint(Point point1, Point point2, Parameter parameter)
  {
    _point1 = point1;
    _point2 = point2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var p2X = _point2.X.Value;
    var p2Y = _point2.Y.Value;
    var distance = _parameter.Value;
    return (p1X - p2X) * (p1X - p2X) + (p1Y - p2Y) * (p1Y - p2Y) - distance * distance;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      new[] {_parameter}
    };
  }
}

public sealed class P2PDistanceVertConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Parameter _parameter;

  public P2PDistanceVertConstraint(Point point1, Point point2, Parameter parameter)
  {
    _point1 = point1;
    _point2 = point2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var p1Y = _point1.Y.Value;
    var p2Y = _point2.Y.Value;
    var distance = _parameter.Value;
    return (p1Y - p2Y) * (p1Y - p2Y) - distance * distance;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      new[] {_parameter}
    };
  }
}

public sealed class P2PDistanceHorizConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Parameter _parameter;

  public P2PDistanceHorizConstraint(Point point1, Point point2, Parameter parameter)
  {
    _point1 = point1;
    _point2 = point2;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var p1X = _point1.X.Value;
    var p2X = _point2.X.Value;
    var distance = _parameter.Value;
    return (p1X - p2X) * (p1X - p2X) - distance * distance;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      new[] {_parameter}
    };
  }
}

public sealed class PointOnLineConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Line _line1;

  public PointOnLineConstraint(Point point1, Line line1)
  {
    _point1 = point1;
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var m = dy / dx; // Slope
    var n = dx / dy; // 1/Slope

    if (m <= 1 && m >= -1)
    {
      //Calculate the expected y point given the x coordinate of the point
      var p1X = _point1.X.Value;
      var p1Y = _point1.Y.Value;
      var ey = l1P1Y + m * (p1X - l1P1X);
      return (ey - p1Y) * (ey - p1Y);
    }
    else
    {
      //Calculate the expected x point given the y coordinate of the point
      var p1X = _point1.X.Value;
      var p1Y = _point1.Y.Value;
      var ex = l1P1X + n * (p1Y - l1P1Y);
      return (ex - p1X) * (ex - p1X);
    }
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1
    };
  }
}

public sealed class P2LDistanceConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public P2LDistanceConstraint(Point point1, Line line1, Parameter parameter)
  {
    _point1 = point1;
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var t = -(l1P1X * dx - p1X * dx + l1P1Y * dy - p1Y * dy) / (dx * dx + dy * dy);
    var xint = l1P1X + dx * t;
    var yint = l1P1Y + dy * t;
    var distance = _parameter.Value;
    var temp = Hypot(p1X - xint, p1Y - yint) - distance;
    return temp * temp / 10;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1,
      new[] {_parameter}
    };
  }
}

public sealed class P2LDistanceVertConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public P2LDistanceVertConstraint(Point point1, Line line1, Parameter parameter)
  {
    _point1 = point1;
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var p1X = _point1.X.Value;
    var t = (p1X - l1P1X) / dx;
    var yint = l1P1Y + dy * t;
    var distance = _parameter.Value;
    var p1Y = _point1.Y.Value;
    var temp = Math.Abs(p1Y - yint) - distance;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1,
      new[] {_parameter}
    };
  }
}

public sealed class P2LDistanceHorizConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public P2LDistanceHorizConstraint(Point point1, Line line1, Parameter parameter)
  {
    _point1 = point1;
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;

    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var t = (p1Y - l1P1Y) / dy;
    var xint = l1P1X + dx * t;
    var distance = _parameter.Value;
    var temp = Math.Abs(p1X - xint) - distance;
    return temp * temp / 10;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1,
      new[] {_parameter}
    };
  }
}

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

public sealed class ArcRulesConstraint : Constraint
{
  private readonly Arc _arc1;

  public ArcRulesConstraint(Arc arc1)
  {
    _arc1 = arc1;
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
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1Endx2 = a1EndX * a1EndX;
    var a1Endy2 = a1EndY * a1EndY;
    var a1Startx2 = a1StartX * a1StartX;
    var a1Starty2 = a1StartY * a1StartY;
    var num = -2 * a1CenterX * a1EndX + a1Endx2 - 2 * a1CenterY * a1EndY + a1Endy2 + 2 * a1CenterX * a1StartX - a1Startx2 + 2 * a1CenterY * a1StartY - a1Starty2;
    return num * num / (4.0 * a1Endx2 + a1Endy2 - 2 * a1EndX * a1StartX + a1Startx2 - 2 * a1EndY * a1StartY + a1Starty2);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1
    };
  }
}

public sealed class LineLengthConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Parameter _parameter;

  public LineLengthConstraint(Line line1, Parameter parameter)
  {
    _line1 = line1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var temp = Math.Sqrt(Math.Pow(l1P2X - l1P1X, 2) + Math.Pow(l1P2Y - l1P1Y, 2)) - _parameter.Value;
    return temp * temp * 100;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      new[] {_parameter}
    };
  }
}

public sealed class EqualLengthConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public EqualLengthConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var l2P1X = _line2.P1.X.Value;
    var l2P1Y = _line2.P1.Y.Value;
    var l2P2X = _line2.P2.X.Value;
    var l2P2Y = _line2.P2.Y.Value;
    var temp = Hypot(l1P2X - l1P1X, l1P2Y - l1P1Y) - Hypot(l2P2X - l2P1X, l2P2Y - l2P1Y);
    return temp * temp;
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

public sealed class ArcRadiusConstraint : Constraint
{
  private readonly Arc _arc1;
  private readonly Parameter _parameter;

  public ArcRadiusConstraint(Arc arc1, Parameter parameter)
  {
    _arc1 = arc1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var radius = _parameter.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var rad2 = Hypot(a1CenterX - a1EndX, a1CenterY - a1EndY);
    var temp = rad1 - radius;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1,
      new[] {_parameter}
    };
  }
}

public sealed class EqualRadiusArcsConstraint : Constraint
{
  private readonly Arc _arc1;
  private readonly Arc _arc2;

  public EqualRadiusArcsConstraint(Arc arc1, Arc arc2)
  {
    _arc1 = arc1;
    _arc2 = arc2;
  }

  public override double CalculateError()
  {
    var a2Radius = _arc2.Rad.Value;
    var a2StartA = _arc2.StartAngle.Value;
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a2StartX = a1CenterX + a2Radius * Math.Cos(a2StartA);
    var a2StartY = a1CenterY + a2Radius * Math.Sin(a2StartA);
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a2CenterX = _arc2.Center.X.Value;
    var a2CenterY = _arc2.Center.Y.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var rad2 = Hypot(a2CenterX - a2StartX, a2CenterY - a2StartY);
    var temp = rad1 - rad2;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1,
      _arc2
    };
  }
}

public sealed class EqualRadiusCirclesConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public EqualRadiusCirclesConstraint(Circle circle1, Circle circle2)
  {
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1Rad = _circle1.Rad.Value;
    var c2Rad = _circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _circle2
    };
  }
}

public sealed class EqualRadiusCircArcConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Arc _arc1;

  public EqualRadiusCircArcConstraint(Circle circle1, Arc arc1)
  {
    _circle1 = circle1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var c1Rad = _circle1.Rad.Value;
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _arc1
    };
  }
}

public sealed class ConcentricArcsConstraint : Constraint
{
  private readonly Arc _arc1;
  private readonly Arc _arc2;

  public ConcentricArcsConstraint(Arc arc1, Arc arc2)
  {
    _arc1 = arc1;
    _arc2 = arc2;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a2CenterY = _arc2.Center.Y.Value;
    var a2CenterX = _arc2.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var temp = Hypot(a1CenterX - a2CenterX, a1CenterY - a2CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _arc1,
      _arc2
    };
  }
}

public sealed class ConcentricCirclesConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public ConcentricCirclesConstraint(Circle circle1, Circle circle2)
  {
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var c2CenterX = _circle2.Center.X.Value;
    var c2CenterY = _circle2.Center.Y.Value;
    var temp = Hypot(c1CenterX - c2CenterX, c1CenterY - c2CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _circle2
    };
  }
}

public sealed class ConcentricCircArcConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Arc _arc1;

  public ConcentricCircArcConstraint(Circle circle1, Arc arc1)
  {
    _circle1 = circle1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var temp = Hypot(a1CenterX - c1CenterX, a1CenterY - c1CenterY);
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      _arc1
    };
  }
}

public sealed class CircleRadiusConstraint : Constraint
{
  private readonly Circle _circle1;
  private readonly Parameter _parameter;

  public CircleRadiusConstraint(Circle circle1, Parameter parameter)
  {
    _circle1 = circle1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var c1Rad = _circle1.Rad.Value;
    var radius = _parameter.Value;
    return (c1Rad - radius) * (c1Rad - radius);
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _circle1,
      new[] {_parameter}
    };
  }
}

public sealed class ParallelConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;

  public ParallelConstraint(Line line1, Line line2)
  {
    _line1 = line1;
    _line2 = line2;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var l2P1X = _line2.P1.X.Value;
    var l2P1Y = _line2.P1.Y.Value;
    var l2P2X = _line2.P2.X.Value;
    var l2P2Y = _line2.P2.Y.Value;
    var dx = l1P2X - l1P1X;
    var dy = l1P2Y - l1P1Y;
    var dx2 = l2P2X - l2P1X;
    var dy2 = l2P2Y - l2P1Y;

    var hyp1 = Hypot(dx, dy);
    var hyp2 = Hypot(dx2, dy2);

    dx = dx / hyp1;
    dy = dy / hyp1;
    dx2 = dx2 / hyp2;
    dy2 = dy2 / hyp2;

    var temp = dy * dx2 - dx * dy2;
    return temp * temp;
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

public sealed class PointOnCircleConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Circle _circle1;

  public PointOnCircleConstraint(Point point1, Circle circle1)
  {
    _point1 = point1;
    _circle1 = circle1;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var c1Rad = _circle1.Rad.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var rad1 = Hypot(c1CenterX - p1X, c1CenterY - p1Y);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - c1Rad;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _circle1
    };
  }
}

public sealed class PointOnArcConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Arc _arc1;

  public PointOnArcConstraint(Point point1, Arc arc1)
  {
    _point1 = point1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    //see what the current radius to the point is
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var rad1 = Hypot(a1CenterX - p1X, a1CenterY - p1Y);
    var rad2 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    //Compare this radius to the radius of the circle, return the error squared
    var temp = rad1 - rad2;
    return temp * temp;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _arc1
    };
  }
}

public sealed class PointOnLineMidpointConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Line _line1;

  public PointOnLineMidpointConstraint(Point point1, Line line1)
  {
    _point1 = point1;
    _line1 = line1;
  }

  public override double CalculateError()
  {
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var ex = (l1P1X + l1P2X) / 2;
    var ey = (l1P1Y + l1P2Y) / 2;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _line1
    };
  }
}

public sealed class PointOnArcMidpointConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Arc _arc1;

  public PointOnArcMidpointConstraint(Point point1, Arc arc1)
  {
    _point1 = point1;
    _arc1 = arc1;
  }

  public override double CalculateError()
  {
    var a1CenterX = _arc1.Center.X.Value;
    var a1CenterY = _arc1.Center.Y.Value;
    var a1Radius = _arc1.Rad.Value;
    var a1EndA = _arc1.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = _arc1.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var rad1 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    var tempStart = Math.Atan2(a1StartY - a1CenterY, a1StartX - a1CenterX);
    var tempEnd = Math.Atan2(a1EndY - a1CenterY, a1EndX - a1CenterX);
    var ex = a1CenterX + rad1 * Math.Cos((tempEnd + tempStart) / 2);
    var ey = a1CenterY + rad1 * Math.Sin((tempEnd + tempStart) / 2);
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _arc1
    };
  }
}

public sealed class PointOnCircleQuadConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Circle _circle1;
  private readonly Parameter _parameter;

  public PointOnCircleQuadConstraint(Point point1, Circle circle1, Parameter parameter)
  {
    _point1 = point1;
    _circle1 = circle1;
    _parameter = parameter;
  }

  public override double CalculateError()
  {
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var ex = c1CenterX;
    var ey = c1CenterY;
    var c1Rad = _circle1.Rad.Value;
    var quadIndex = _parameter.Value;
    switch ((int) quadIndex)
    {
      case 0:
        ex += c1Rad;
        break;
      case 1:
        ey += c1Rad;
        break;
      case 2:
        ex -= c1Rad;
        break;
      case 3:
        ey -= c1Rad;
        break;
    }

    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var tempX = ex - p1X;
    var tempY = ey - p1Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _circle1,
      new[] {_parameter}
    };
  }
}

public sealed class SymmetricPointsConstraint : Constraint
{
  private readonly Point _point1;
  private readonly Point _point2;
  private readonly Line _symLine;

  public SymmetricPointsConstraint(Point point1, Point point2, Line symLine)
  {
    _point1 = point1;
    _point2 = point2;
    _symLine = symLine;
  }

  public override double CalculateError()
  {
    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var p1X = _point1.X.Value;
    var p1Y = _point1.Y.Value;
    var t = -(dy * p1X - dx * p1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = p1X + dy * t * 2;
    var ey = p1Y - dx * t * 2;
    var p2X = _point2.X.Value;
    var p2Y = _point2.Y.Value;
    var tempX = ex - p2X;
    var tempY = ey - p2Y;
    return tempX * tempX + tempY * tempY;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _point1,
      _point2,
      _symLine
    };
  }
}

public sealed class SymmetricLinesConstraint : Constraint
{
  private readonly Line _line1;
  private readonly Line _line2;
  private readonly Line _symLine;

  public SymmetricLinesConstraint(Line line1, Line line2, Line symLine)
  {
    _line1 = line1;
    _line2 = line2;
    _symLine = symLine;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var l1P1X = _line1.P1.X.Value;
    var l1P1Y = _line1.P1.Y.Value;
    var t = -(dy * l1P1X - dx * l1P1Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = l1P1X + dy * t * 2;
    var ey = l1P1Y - dx * t * 2;
    var l1P2X = _line1.P2.X.Value;
    var l1P2Y = _line1.P2.Y.Value;
    var l2P1X = _line2.P1.X.Value;
    var l2P1Y = _line2.P1.Y.Value;
    var l2P2X = _line2.P2.X.Value;
    var l2P2Y = _line2.P2.Y.Value;
    var tempX = ex - l2P1X;
    var tempY = ey - l2P1Y;
    error += tempX * tempX + tempY * tempY;

    t = -(dy * l1P2X - dx * l1P2Y - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    ex = l1P2X + dy * t * 2;
    ey = l1P2Y - dx * t * 2;
    tempX = ex - l2P2X;
    tempY = ey - l2P2Y;
    error += tempX * tempX + tempY * tempY;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _line1,
      _line2,
      _symLine
    };
  }
}

public sealed class SymmetricCirclesConstraint : Constraint
{
  private readonly Line _symLine;
  private readonly Circle _circle1;
  private readonly Circle _circle2;

  public SymmetricCirclesConstraint(Line symLine, Circle circle1, Circle circle2)
  {
    _symLine = symLine;
    _circle1 = circle1;
    _circle2 = circle2;
  }

  public override double CalculateError()
  {
    var error = 0d;

    var symP1X = _symLine.P1.X.Value;
    var symP1Y = _symLine.P1.Y.Value;
    var symP2X = _symLine.P2.X.Value;
    var symP2Y = _symLine.P2.Y.Value;
    var c1CenterX = _circle1.Center.X.Value;
    var c1CenterY = _circle1.Center.Y.Value;
    var dx = symP2X - symP1X;
    var dy = symP2Y - symP1Y;
    var t = -(dy * c1CenterX - dx * c1CenterY - dy * symP1X + dx * symP1Y) / (dx * dx + dy * dy);
    var ex = c1CenterX + dy * t * 2;
    var ey = c1CenterY - dx * t * 2;
    var c2CenterX = _circle2.Center.X.Value;
    var c2CenterY = _circle2.Center.Y.Value;
    var tempX = ex - c2CenterX;
    var tempY = ey - c2CenterY;
    error += tempX * tempX + tempY * tempY;

    var c1Rad = _circle1.Rad.Value;
    var c2Rad = _circle2.Rad.Value;
    var temp = c1Rad - c2Rad;
    error += temp * temp;

    return error;
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    return new List<IEnumerable<Parameter>>
    {
      _symLine,
      _circle1,
      _circle2
    };
  }
}

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

public sealed class RadiusValueConstraint : Constraint
{
  public override double CalculateError()
  {
    throw new NotImplementedException();
  }

  protected override IEnumerable<IEnumerable<Parameter>> GetParameters()
  {
    throw new NotImplementedException();
  }
}
