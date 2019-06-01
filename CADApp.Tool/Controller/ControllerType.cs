using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADApp.Tool.Controller
{
    public enum ControllerType
    {
        Select,
        SketchLine,
        SketchPrimitive,
        SketchRectangle,
        SketchCurvature,
        BuildCube,
        BuildIcosahedron
    }

    public enum GeometryType
    {
        Bezier,
        Spline,
        Circle
    }

    public enum SelectMode
    {
        Geometry,
        Point,
        Line,
        Triangle,
    }

}
