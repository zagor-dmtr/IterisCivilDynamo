using Autodesk.Civil.DynamoNodes;
using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.DesignScript.Runtime;
using IterisCivilDynamo.Support;
using Autodesk.DesignScript.Geometry;
using Autodesk.AutoCAD.Geometry;

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

        [IsVisibleInDynamoLibrary(false)]
        internal static Structure GetByObjectId(ObjectId structId)
            => CivilObjectSupport.Get<Structure, C3dDb.Structure>
                (structId, (structure) => new Structure(structure));

        /// <summary>
        /// Resize the structure by pipe depths.
        /// </summary>
        /// <returns>True if success, false if failed to resize.</returns>
        public bool ResizeByPipeDepths() => AeccStructure.ResizeByPipeDepths();

        /// <summary>
        /// Gets or sets whether the rim should be automatically adjusted.
        /// </summary>
        public bool AutomaticRimSurfaceAdjustment
        {
            get => AeccStructure.AutomaticRimSurfaceAdjustment;
            set => AeccStructure.AutomaticRimSurfaceAdjustment = value;
        }

        /// <summary>
        /// Gets the clearance of barrel pipe.
        /// </summary>
        public double BarrelPipeClearance => AeccStructure.BarrelPipeClearance;

        /// <summary>
        /// Gets the structure bounding shape: Undefined, Cylinder, Box or Sphere
        /// </summary>
        public string BoundingShape => AeccStructure.BoundingShape.ToString();

        /// <summary>
        /// Gets the count of the pipes connected to the structure.
        /// </summary>
        public int ConnectedPipesCount => AeccStructure.ConnectedPipesCount;

        ///// <summary>
        ///// Gets the structure diameter or width.
        ///// </summary>
        //public double DiameterOrWidth => AeccStructure.DiameterOrWidth;

        ///// <summary>
        ///// Gets the thickness of the bottom of the structure.
        ///// </summary>
        //public double FloorThickness => AeccStructure.FloorThickness;

        /// <summary>
        /// Gets the structure height.
        /// </summary>
        public double Height => AeccStructure.Height;

        ///// <summary>
        ///// Gets the structure inner diameter or width.
        ///// </summary>
        //public double InnerDiameterOrWidth => AeccStructure.InnerDiameterOrWidth;

        ///// <summary>
        ///// Gets the structure inner length.
        ///// </summary>
        //public double InnerLength => AeccStructure.InnerLength;

        ///// <summary>
        ///// 
        ///// </summary>
        //public double Length => AeccStructure.Length;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return PointData.FromPointObject(AeccStructure.Location).CreateDynamoPoint();
            }
            set
            {
                AeccStructure.Location = new Point3d(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the rim elevation.
        /// </summary>
        public double RimElevation
        {
            get => AeccStructure.RimElevation;
            set => AeccStructure.RimElevation = value;
        }

        /// <summary>
        /// Gets or sets the distance between the sump to the structure’s rim.
        /// </summary>
        public double RimToSumpHeight
        {
            get => AeccStructure.RimToSumpHeight;
            set => AeccStructure.RimToSumpHeight = value;
        }

        /// <summary>
        /// Gets or sets the structure rotation.
        /// </summary>
        public double Rotation
        {
            get => AeccStructure.Rotation;
            set => AeccStructure.Rotation = value;
        }

        /// <summary>
        /// Gets or sets the sump depth.
        /// </summary>
        public double SumpDepth
        {
            get => AeccStructure.SumpDepth;
            set => AeccStructure.SumpDepth = value;
        }

        /// <summary>
        /// Gets or sets the sump elevation.
        /// </summary>
        public double SumpElevation
        {
            get => AeccStructure.SumpElevation;
            set => AeccStructure.SumpElevation = value;
        }

        /// <summary>
        /// Gets or sets the surface adjustment value
        /// </summary>
        public double SurfaceAdjustmentValue
        {
            get => AeccStructure.SurfaceAdjustmentValue;
            set => AeccStructure.SurfaceAdjustmentValue = value;
        }

        /// <summary>
        /// The elevation of the referenced surface at the location of the structure 
        /// </summary>
        public double SurfaceElevationAtInsertionPoint
        {
            get => AeccStructure.RefSurfaceId.IsValid
                ? AeccStructure.SurfaceElevationAtInsertionPoint
                : double.NaN;
        }
    }
}
