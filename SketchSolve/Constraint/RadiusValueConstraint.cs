﻿namespace SketchSolve.Constraint;

using SketchSolve.Model;

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