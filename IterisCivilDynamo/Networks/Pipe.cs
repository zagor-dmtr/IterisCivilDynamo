using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using System.Collections.Generic;
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

        [SupressImportIntoVM]
        internal static Pipe GetByObjectId(ObjectId pipeId)
            => CivilObjectSupport.Get<Pipe, C3dDb.Pipe>
                (pipeId, (pipe) => new Pipe(pipe));

        /// <summary>
        /// Gets the end point's cover of pipe.
        /// </summary>
        public double CoverOfEndpoint => GetDouble();

        /// <summary>
        /// Gets the start point's cover of pipe.
        /// </summary>
        public double CoverOfStartPoint => GetDouble();

        /// <summary>
        /// Gets the pipe’s cross sectional shape, such as circular, egg-shaped, elliptical, or rectangular.
        /// </summary>
        public string CrossSectionalShape => GetString();

        /// <summary>
        /// Gets the offset of the starting point for the pipe object.
        /// </summary>
        public double StartOffset => GetDouble();

        /// <summary>
        /// Gets the startpoint of the Pipe.
        /// </summary>
        public Point StartPoint
            => PointData.FromPointObject(AeccPipe.StartPoint).CreateDynamoPoint();

        /// <summary>
        /// Sets the startpoint of the Pipe.
        /// </summary>
        /// <param name="value"></param>
        public void SetStartPoint(Point value)
            => SetValue(new Point3d(value.X, value.Y, value.Z));

        /// <summary>
        /// Gets the station of the starting point for the pipe object.
        /// </summary>
        public double StartStation => GetDouble();

        /// <summary>
        /// Gets the start structure of the Pipe. If no connected structure, return Null.
        /// </summary>
        public Structure StartStructure
            => Structure.GetByObjectId(AeccPipe.StartStructureId);

        /// <summary>
        /// Gets the offset of the ending point for the pipe object.
        /// </summary>
        public double EndOffset => GetDouble();

        /// <summary>
        /// Gets the endpoint of the Pipe.
        /// </summary>
        public Point EndPoint
            => PointData.FromPointObject(AeccPipe.EndPoint).CreateDynamoPoint();

        /// <summary>
        /// Sets the endpoint of the Pipe.
        /// </summary>
        /// <param name="value"></param>
        public void SetEndPoint(Point value)
            => SetValue(new Point3d(value.X, value.Y, value.Z));

        /// <summary>
        /// Gets the station of the ending point for the pipe object.
        /// </summary>
        public double EndStation => GetDouble();

        /// <summary>
        /// Gets the end structure of the Pipe. If no connected structure, return Null.
        /// </summary>
        public Structure EndStructure => Structure.GetByObjectId(AeccPipe.EndStructureId);

        /// <summary>
        /// Gets the pipe flow rate.
        /// </summary>
        public double FlowRate => GetDouble();        

        /// <summary>
        /// Sets the pipe flow rate.
        /// </summary>
        /// <param name="value"></param>
        public void SetFlowRate(double value) => SetValue(value);

        /// <summary>
        /// Gets how pipe should hold when resized.
        /// </summary>
        public string HoldOnResizeType => GetString();

        /// <summary>
        /// Sets how pipe should hold when resized.
        /// </summary>
        /// <param name="value">0 - invert, 1 - crown, 2 - centerline</param>
        public void SetHoldOnResizeType(int value)
            => SetValue((C3dDb.HoldOnResizeType)value);

        /// <summary>
        /// Gets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        public double EnergyGradeLineDown => GetDouble();

        /// <summary>
        /// Sets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        /// <param name="value"></param>
        public void SetEnergyGradeLineDown(double value)
            => SetValue(value);

        /// <summary>
        /// Gets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        public double EnergyGradeLineUp => GetDouble();

        /// <summary>
        /// Sets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        /// <param name="value"></param>
        public void SetEnergyGradeLineUp(double value) => SetValue(value);

        /// <summary>
        /// Gets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        public double HydraulicGradeLineDown => GetDouble();

        /// <summary>
        /// Sets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        /// <param name="value"></param>
        public void SetHydraulicGradeLineDown(double value) => SetValue(value);

        /// <summary>
        /// Gets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        public double HydraulicGradeLineUp => GetDouble();

        /// <summary>
        /// Sets the elevation of the hydraulic grade line for pipe
        /// networks flowing in a downstream direction and that contain
        /// hydraulic property data.
        /// </summary>
        /// <param name="value"></param>
        public void SetHydraulicGradeLineUp(double value) => SetValue(value);

        /// <summary>
        /// Gets the inner diameter or inner width of the pipe.
        /// </summary>
        public double InnerDiameterOrWidth => GetDouble();

        /// <summary>
        /// Gets the inner height of the pipe in drawing units.
        /// </summary>
        public double InnerHeight => GetDouble();

        /// <summary>
        /// Gets the outer diameter or outer width of the pipe.
        /// </summary>
        public double OuterDiameterOrWidth => GetDouble();

        /// <summary>
        /// Gets the outer height of the pipe in drawing units.
        /// </summary>
        public double OuterHeight => GetDouble();

        /// <summary>
        /// Gets the pipe size properties.
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new string[]
        {
            "InnerDiameterOrWidth",
            "InnerHeight",
            "OuterDiameterOrWidth",
            "OuterHeight",
            "Radius"
        })]
        public Dictionary<string, object> GetSizeProperties()
        {
            return new Dictionary<string, object>
            {
                { "InnerDiameterOrWidth", InnerDiameterOrWidth },
                { "InnerHeight", InnerHeight },
                { "OuterDiameterOrWidth", OuterDiameterOrWidth },
                { "OuterHeight", OuterHeight },
                { "Radius", Radius },
            };
        }

        /// <summary>
        /// Gets the pipe junction loss.
        /// </summary>
        public double JunctionLoss => GetDouble();

        /// <summary>
        /// Sets the pipe junction loss.
        /// </summary>
        /// <param name="value"></param>
        public void SetJunctionLoss(double value) => SetValue(value);

        /// <summary>
        /// Gets the two-dimensional length of the pipe, measured from the center of the
        /// connected starting structure to the center of the connected ending structure.
        /// </summary>
        public double Length2DCenterToCenter => GetDouble();

        /// <summary>
        /// Gets the two-dimensional length of the pipe, measured from the inside edge of the
        /// connected starting structure to the inside edge of the connected ending structure.
        /// </summary>
        public double Length2DToInsideEdge => GetDouble();

        /// <summary>
        /// Gets the three-dimensional length of the pipe, measured from the center of the
        /// connected starting structure to the center of the connected ending structure.
        /// </summary>
        public double Length3DCenterToCenter => GetDouble();

        /// <summary>
        /// Gets the three-dimensional length of the pipe,measured from the inside edge of the
        /// connected starting structure to the inside edge of the connected ending structure.
        /// </summary>
        public double Length3DToInsideEdge => GetDouble();

        /// <summary>
        /// Gets the maximum depth of cover along the entire length of pipe, from
        /// the top outside of the pipe to the reference surface.
        /// </summary>
        public double MaximumCover => GetDouble();

        /// <summary>
        /// Gets the minimum depth of cover along the entire length of pipe, from
        /// the top outside of the pipe to the reference surface.
        /// </summary>
        public double MinimumCover => GetDouble();

        /// <summary>
        /// Gets the radius of the pipe.
        /// </summary>
        public double Radius => GetDouble();

        /// <summary>
        /// Gets the pipe return period.
        /// </summary>
        public int ReturnPeriod => GetInt();

        /// <summary>
        /// Sets the pipe return period.
        /// </summary>
        /// <param name="value"></param>
        public void SetReturnPeriod(int value) => SetValue(value);

        /// <summary>
        /// Gets the pipe’s slope in absolute value.
        /// </summary>
        public double Slope => GetDouble();
    }
}
