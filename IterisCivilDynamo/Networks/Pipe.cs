using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// The Pipe class
    /// </summary>
    [RegisterForTrace]
    public sealed class Pipe : Part
    {
        internal C3dDb.Pipe AeccPipe => AeccPart as C3dDb.Pipe;

        internal Pipe(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        [IsVisibleInDynamoLibrary(false)]
        internal static Pipe GetByObjectId(ObjectId pipeId)
            => CivilObjectSupport.Get<Pipe, C3dDb.Pipe>
                (pipeId, (pipe) => new Pipe(pipe));

        /// <summary>
        /// Gets the end point's cover of pipe.
        /// </summary>
        public double CoverOfEndpoint => AeccPipe.CoverOfEndpoint;

        /// <summary>
        /// Gets the start point's cover of pipe.
        /// </summary>
        public double CoverOfStartPoint => AeccPipe.CoverOfStartPoint;

        /// <summary>
        /// Gets the pipe’s cross sectional shape, such as circular, egg-shaped, elliptical, or rectangular.
        /// </summary>
        public string CrossSectionalShape => AeccPipe.CrossSectionalShape.ToString();

        /// <summary>
        /// Gets the offset of the starting point for the pipe object.
        /// </summary>
        public double StartOffset => AeccPipe.StartOffset;

        /// <summary>
        /// Gets or sets the startpoint of the Pipe.
        /// </summary>
        public Point StartPoint
        {
            get => PointData.FromPointObject(AeccPipe.StartPoint).CreateDynamoPoint();
            set => AeccPipe.StartPoint = new Point3d(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Gets the station of the starting point for the pipe object.
        /// </summary>
        public double StartStation => AeccPipe.StartStation;

        /// <summary>
        /// Gets the start structure of the Pipe. If no connected structure, return Null.
        /// </summary>
        public Structure StartStructure => Structure.GetByObjectId(AeccPipe.StartStructureId);

        /// <summary>
        /// Gets the offset of the ending point for the pipe object.
        /// </summary>
        public double EndOffset => AeccPipe.EndOffset;

        /// <summary>
        /// Gets or sets the endpoint of the Pipe.
        /// </summary>
        public Point EndPoint
        {
            get => PointData.FromPointObject(AeccPipe.EndPoint).CreateDynamoPoint();
            set => AeccPipe.EndPoint = new Point3d(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Gets the station of the ending point for the pipe object.
        /// </summary>
        public double EndStation => AeccPipe.EndStation;

        /// <summary>
        /// Gets the end structure of the Pipe. If no connected structure, return Null.
        /// </summary>
        public Structure EndStructure => Structure.GetByObjectId(AeccPipe.EndStructureId);

        /// <summary>
        /// Gets the inner diameter or inner width of the pipe.
        /// </summary>
        public double InnerDiameterOrWidth => AeccPipe.InnerDiameterOrWidth;

        /// <summary>
        /// Gets the inner height of the pipe in drawing units.
        /// </summary>
        public double InnerHeight => AeccPipe.InnerHeight;

        /// <summary>
        /// Gets the outer diameter or outer width of the pipe.
        /// </summary>
        public double OuterDiameterOrWidth => AeccPipe.OuterDiameterOrWidth;

        /// <summary>
        /// Gets the outer height of the pipe in drawing units.
        /// </summary>
        public double OuterHeight => AeccPipe.OuterHeight;

        /// <summary>
        /// Gets the two-dimensional length of the pipe, measured from the center of the
        /// connected starting structure to the center of the connected ending structure.
        /// </summary>
        public double Length2DCenterToCenter => AeccPipe.Length2DCenterToCenter;

        /// <summary>
        /// Gets the two-dimensional length of the pipe, measured from the inside edge of the
        /// connected starting structure to the inside edge of the connected ending structure.
        /// </summary>
        public double Length2DToInsideEdge => AeccPipe.Length2DToInsideEdge;

        /// <summary>
        /// Gets the three-dimensional length of the pipe, measured from the center of the
        /// connected starting structure to the center of the connected ending structure.
        /// </summary>
        public double Length3DCenterToCenter => AeccPipe.Length3DCenterToCenter;

        /// <summary>
        /// Gets the three-dimensional length of the pipe,measured from the inside edge of the
        /// connected starting structure to the inside edge of the connected ending structure.
        /// </summary>
        public double Length3DToInsideEdge => AeccPipe.Length3DToInsideEdge;

        /// <summary>
        /// Gets the maximum depth of cover along the entire length of pipe, from
        /// the top outside of the pipe to the reference surface.
        /// </summary>
        public double MaximumCover => AeccPipe.MaximumCover;

        /// <summary>
        /// Gets the minimum depth of cover along the entire length of pipe, from
        /// the top outside of the pipe to the reference surface.
        /// </summary>
        public double MinimumCover => AeccPipe.MinimumCover;

        /// <summary>
        /// Gets the pipe’s slope in absolute value.
        /// </summary>
        public double Slope => AeccPipe.Slope;
    }
}
