# SketchSolve

A geometric constraints solver for use in CAD software

## Background

SketchSolve can be used to solve solve geometric constraints problems found in CAD software.
This project is one of the first geometric constraints solver intended to be used in open source CAD software.
Another similar project here on Google code is [psketcher](https://code.google.com/archive/p/psketcher/). In these problems profiles can be created from
primitive objects like points, lines, circles, and arcs. These primitives are subjected to geometric
constraints like equal length, concentric arcs and so forth. The solver then solves for a set of primitive
parameters that satisfy the sketch constraints.

Currently only 2d sketch problems are supported. However I hope to soon also have a set of 3D part Assembly
constraints working. This would make SketchSolve be able to solve complex Assemblies. However, I only plan
to write that section of the code if people actually use the 2d solver.

## Implementation

The solution method used is actually a optimization method. The sum of the constraint violations are the
objective of the optimization problem. The optimization routine used is a BFGS update Newtons method.
An Optimization routine was selected because there are often more or fewer constraint equations than unknowns.

## Supported constraints

The constraints that are currently supported are the following:
* pointOnPoint
* pointToLine
* pointOnCurve
* horizontal
* vertical
* radiusValue
* tangentToArc
* tangentToCircle
* arcRules
* Point to point Distance
* Point to point vertical Distance
* Point to point horizontal Distance
* Point to line Distance
* Point to line vertical Distance
* Point to line horizontal Distance
* lineLength
* equalLegnth
* arcRadius
* equalRadiusArcs
* equalRadiusCircles
* equalRadiusCircArc
* concentricArcs
* concentricCircles
* concentricCircArc
* circleRadius
* angle ( between two lines )
* parallel
* Perpendicular
* Colinear Lines
* Point On Circle
* Point On Arc
* Point On midpoint of a line
* Point on midpoint of an arc
* Point on a quadrant point of a circle
  * +x (parameter = 0)
  * +y (parameter = 1)
  * -x (parameter = 2)
  * -y (parameter = 3)
* Points Symmetric about a line
* lines Symmetric about a line
* Circles Symmetric about a line
* Arcs Symmetric about a line

The constraints that will be implemented soon are the following:
* others I can't think of right now
Let me know if there are constraints that you use that are not on these lists !!!!

Thanks!

## Prerequisites
* .NET Core 6

## Getting started
```bash
$ git clone https://github.com/TrevorDArcyEvans/SketchSolve.NET.git
$ cd SketchSolve.NET
$ dotnet restore
$ dotnet build
$ dotnet test
```

## Further work
* ~~refactor to class based constraints~~
* ConstraintBuilder support for all constraints
* unit tests for constraint errors
* unit tests for constraint parameters
* fix unit tests for solver
* more unit tests for solver
* portable UI aka test harness

## Acknowledgements

This repository was cloned from [SketchSolve.NET](https://github.com/bradphelan/SketchSolve.NET)
which in turn was cloned from [SketchSolve](http://code.google.com/p/sketchsolve/.)

## License

The [LICENSE](LICENSE) file was inferred from the license field on the google code project,
but was not inserted by the original creator.

