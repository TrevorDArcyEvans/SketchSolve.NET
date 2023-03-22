namespace SketchSolve.Constraint;

using SketchSolve.Model;

public sealed class ArcRulesConstraint : BaseConstraint
{
  public readonly Arc Arc;

  public ArcRulesConstraint(Arc arc)
  {
    Arc = arc;
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
    var a1CenterX = Arc.Center.X.Value;
    var a1CenterY = Arc.Center.Y.Value;
    var a1Radius = Arc.Rad.Value;
    var a1EndA = Arc.EndAngle.Value;
    var a1EndX = a1CenterX + a1Radius * Math.Cos(a1EndA);
    var a1EndY = a1CenterY + a1Radius * Math.Sin(a1EndA);
    var a1StartA = Arc.StartAngle.Value;
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
      Arc
    };
  }
}
