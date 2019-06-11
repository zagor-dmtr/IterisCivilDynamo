using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using System;

namespace IterisCivilDynamo.Support
{
    /// <summary>
    /// Data for point coordinates
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    internal struct PointData
    {
        /// <summary>
        /// Coordinate X
        /// </summary>
        public double X;
        /// <summary>
        /// Coordinate Y
        /// </summary>
        public double Y;
        /// <summary>
        /// Coordinate Z
        /// </summary>
        public double Z;

        private readonly bool _isConstructed;

        /// <summary>
        /// Create data for 2D point
        /// </summary>
        /// <param name="x">coordinate X</param>
        /// <param name="y">coordinate Y</param>
        public PointData(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0.0;
            _isConstructed = true;
        }

        /// <summary>
        /// Create data for 3D point
        /// </summary>
        /// <param name="x">coordinate X</param>
        /// <param name="y">coordinate Y</param>
        /// <param name="z">coordinate Z</param>
        public PointData(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            _isConstructed = true;
        }

        /// <summary>
        /// Create data from object with double properties: x and y or x, y and z
        /// </summary>
        /// <param name="point2dOr3d">
        /// Object with double properties: x and y or x, y and z
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Object "point3d" have to has two or tree double
        /// propeties with names "x", "y" and "z".
        /// Method ignore case. Method use reflection.
        /// </remarks>
        public static PointData FromPointObject(object point2dOr3d) => new PointData
                (ReflectionSupport.GetProperty(point2dOr3d, "X", double.NaN),
                ReflectionSupport.GetProperty(point2dOr3d, "Y", double.NaN),
                ReflectionSupport.GetProperty(point2dOr3d, "Z", 0.0));

        /// <summary>
        /// Convert data to dynamo point.
        /// </summary>
        /// <returns>
        /// Autodesk.DesignScript.Geometry.Point or null if data is not valid
        /// </returns>
        public Point CreateDynamoPoint() =>
            _isConstructed
            && !double.IsNaN(X)
            && !double.IsNaN(Y)
            && !double.IsNaN(Z)
            ? Point.ByCoordinates(X, Y, Z)
            : null; 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceTo(PointData other)
        {
            return Math.Sqrt
                (Math.Pow(other.X - X, 2)
                + Math.Pow(other.Y - Y, 2)
                + Math.Pow(other.Z - Z, 2));
        }
    }
}
