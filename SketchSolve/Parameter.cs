namespace SketchSolve;

public class Parameter
{
  public double Value = 0;
  public readonly double Max = 1000;
  public readonly double Min = -1000;

  // true if the parameter is free to be adjusted by the solver
  public readonly bool Free;

  public Parameter(double v, bool free = true)
  {
    Value = v;
    Free = free;
  }
}
