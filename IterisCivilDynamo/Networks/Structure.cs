using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// 
    /// </summary>
    [RegisterForTrace]
    public sealed class Structure : Part
    {
        internal C3dDb.Structure AeccStructure => AeccPart as C3dDb.Structure;

        internal Structure(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        [SupressImportIntoVM]
        internal static Structure GetByObjectId(ObjectId structId)
            => CivilObjectSupport.Get<Structure, C3dDb.Structure>
                (structId, (structure) => new Structure(structure));

        /// <summary>
        /// Resize the structure by pipe depths.
        /// </summary>
        /// <returns>True if success, false if failed to resize.</returns>
        public bool ResizeByPipeDepths()
            => AeccStructure.PartType == C3dDb.PartType.StructJunction
            ? AeccStructure.ResizeByPipeDepths()
            : false;

        /// <summary>
        /// Gets whether the rim should be automatically adjusted.
        /// </summary>
        public bool AutomaticRimSurfaceAdjustment => GetBool();

        /// <summary>
        /// Sets whether the rim should be automatically adjusted.
        /// </summary>
        /// <param name="value"></param>
        public void SetAutomaticRimSurfaceAdjustment(bool value) => SetValue(value);

        /// <summary>
        /// Gets the structure bounding shape: Undefined, Cylinder, Box or Sphere
        /// </summary>
        public string BoundingShape => GetString();

        /// <summary>
        /// Gets the count of the pipes connected to the structure.
        /// </summary>
        public int ConnectedPipesCount => GetInt();

        /// <summary>
        /// Gets the pipes connected to the structure.
        /// </summary>
        /// <returns></returns>
        public IList<Pipe> GetConnectedPipes()
        {
            List<Pipe> conPipes = new List<Pipe>();
            for (int i = 0; i < ConnectedPipesCount; i++)
            {
                ObjectId conPipeId = AeccStructure.get_ConnectedPipe(i);
                Pipe pipe = Pipe.GetByObjectId(conPipeId);
                conPipes.Add(pipe);
            }
            return conPipes
                .OrderBy(item => item.InnerDiameterOrWidth)
                .ToList();
        }

        /// <summary>
        /// Gets the structure size properties.
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new string[]
        {
            "InnerDiameterOrWidth",
            "InnerLength",
            "DiameterOrWidth",
            "Length",
            "WallThickness",
            "FloorThickness",
            "VerticalPipeClearance",
            "BarrelPipeClearance",
            "ConeHeight",
            "FrameDiameter",
            "HeadwallBaseThickness",
            "HeadwallBaseWidth",
        })]
        public Dictionary<string, object> GetSizeProperties()
        {
            return new Dictionary<string, object>
            {
                { "InnerDiameterOrWidth", GetDouble("InnerDiameterOrWidth") },
                { "InnerLength" , GetDouble("InnerLength") },
                { "DiameterOrWidth", GetDouble("DiameterOrWidth") },
                { "Length", GetDouble("Length") },
                { "WallThickness", WallThickness },
                { "FloorThickness", GetDouble("FloorThickness") },
                { "VerticalPipeClearance", GetDouble("VerticalPipeClearance") },
                { "BarrelPipeClearance", GetDouble("BarrelPipeClearance") },
                { "ConeHeight", GetDouble("ConeHeight") },
                { "FrameDiameter", GetDouble("FrameDiameter") },
                { "FrameHeight", GetDouble("FrameHeight") },
                { "HeadwallBaseThickness", GetDouble("HeadwallBaseThickness") },
                { "HeadwallBaseWidth", GetDouble("HeadwallBaseWidth") },
            };
        }

        /// <summary>
        /// Gets how the sump should be adjusted.
        /// </summary>
        public string ControlSumpBy => GetString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">True: by depth, false: by elevation</param>
        public void SetControlSumpByDepth(bool value)
        {
            var newVal = value
                ? C3dDb.StructureControlSumpType.ByDepth
                : C3dDb.StructureControlSumpType.ByElevation;

            SetValue("ControlSumpBy", newVal);
        }

        /// <summary>
        /// Sets how the sump should be adjusted.
        /// </summary>
        /// <param name="value">ByDepth or ByElevation</param>
        public void SetControlSumpBy(string value)
        {
            if (Enum.TryParse(value, true, out C3dDb.StructureControlSumpType res))
            {
                SetValue(res);
            }
        }

        /// <summary>
        /// Gets the model or type of grate used for a structure intended to be used as a catchbasin.
        /// </summary>
        public string Cover => GetString();

        /// <summary>
        /// Gets the model or type of frame used for a structure.
        /// </summary>
        public string Frame => GetString();

        /// <summary>
        /// Gets the grate of the structure.
        /// </summary>
        public string Grate => GetString();

        /// <summary>
        /// Gets the structure height.
        /// </summary>
        public double Height => GetDouble();

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location
            => PointData.FromPointObject(AeccStructure.Location).CreateDynamoPoint();

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="value"></param>
        public void SetLocation(Point value) => SetValue(new Point3d(value.X, value.Y, value.Z));

        /// <summary>
        /// Gets the rim elevation.
        /// </summary>
        public double RimElevation => GetDouble();

        /// <summary>
        /// Sets the rim elevation.
        /// </summary>
        /// <param name="value"></param>
        public void SetRimElevation(double value)
        {
            bool old = AutomaticRimSurfaceAdjustment;
            SetAutomaticRimSurfaceAdjustment(false);
            SetValue(value);
            SetAutomaticRimSurfaceAdjustment(old);
        }

        /// <summary>
        /// Gets the distance between the sump to the structure’s rim.
        /// </summary>
        public double RimToSumpHeight => GetDouble();

        /// <summary>
        /// Sets the distance between the sump to the structure’s rim.
        /// </summary>
        /// <param name="value"></param>
        public void SetRimToSumpHeight(double value) => SetValue(value);

        /// <summary>
        /// Gets the structure rotation.
        /// </summary>
        public double Rotation => GetDouble();

        /// <summary>
        /// Sets the structure rotation.
        /// </summary>
        /// <param name="value"></param>
        public void SetRotation(double value) => SetValue(value);

        /// <summary>
        /// Gets the sump depth.
        /// </summary>
        public double SumpDepth => GetDouble();

        /// <summary>
        /// Sets the sump depth.
        /// </summary>
        public void SetSumpDepth(double value)
        {
            string old = ControlSumpBy;
            SetControlSumpByDepth(true);
            SetValue(value);
            SetControlSumpBy(old);
        }

        /// <summary>
        /// Gets the sump elevation.
        /// </summary>
        public double SumpElevation => GetDouble();

        /// <summary>
        /// Sets the sump elevation.
        /// </summary>
        /// <param name="value"></param>
        public void SetSumpElevation(double value)
        {
            string old = ControlSumpBy;
            SetControlSumpByDepth(false);
            SetValue(value);
            SetControlSumpBy(old);
        }

        /// <summary>
        /// Gets the surface adjustment value.
        /// </summary>
        public double SurfaceAdjustmentValue => GetDouble();

        /// <summary>
        /// Sets the surface adjustment value.
        /// </summary>
        /// <param name="value"></param>
        public void SetSurfaceAdjustmentValue(double value)
        {
            bool isOn = AutomaticRimSurfaceAdjustment;
            SetAutomaticRimSurfaceAdjustment(true);
            SetValue(value);
            SetAutomaticRimSurfaceAdjustment(isOn);
        }

        /// <summary>
        /// The elevation of the referenced surface at the location of the structure.
        /// </summary>
        public double SurfaceElevationAtInsertionPoint => GetDouble();      
    }
}
