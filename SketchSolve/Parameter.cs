namespace SketchSolve;

public class Parameter
{
  public double Value = 0;
  public double Max = 1000;
  public double Min = -1000;

  // true if the parameter is free to be adjusted by the
  // solver
  public bool free;

  public Parameter(double v, bool free = true)
  {
    Value = v;
    this.free = free;
  }
}
