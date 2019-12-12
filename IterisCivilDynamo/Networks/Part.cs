using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.CivilObjects;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;
using DynAlignment = IterisCivilDynamo.Alignments.Alignment;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// The Part class
    /// </summary>
    [RegisterForTrace]
    public abstract class Part : CivilEntity
    { 
        internal C3dDb.Part AeccPart => AcObject as C3dDb.Part;

        internal Part(C3dDb.Entity entity, bool isDynamoOwned = false)
            : base(entity, isDynamoOwned)
        {
        }

        /// <summary>
        /// Gets the number of parts that connects to the part
        /// </summary>
        public int ConnectedPartCount => GetInt();

        /// <summary>
        /// Gets the part's domain: Pipe or Structure.
        /// </summary>
        public string Domain => GetString();

        /// <summary>
        /// Gets the network to which this part belongs
        /// </summary>
        /// <returns></returns>
        public Network Network => Network.GetByObjectId(AeccPart.NetworkId);

        /// <summary>
        /// Gets the part size name
        /// </summary>
        public string PartSizeName
            => AeccPart.PartType != C3dDb.PartType.StructNull
            ? GetString()
            : "Null Structure";

        /// <summary>
        /// Gets the part’s subtype.
        /// </summary>
        public string PartSubType => GetString();

        /// <summary>
        /// Gets the type of the network part.
        /// UndefinedPartType, Pipe, Channel, Wire, Conduit, StructNull,
        /// StructJunction, StructInletOutlet, StructGeneral, StructEquipment
        /// </summary>       
        public string PartType => GetString();

        /// <summary>
        /// Gets the position of the network part.
        /// </summary>
        public Point Position
            => PointData.FromPointObject(AeccPart.Position).CreateDynamoPoint();

        /// <summary>
        /// Sets the position of the network part.
        /// </summary>
        /// <param name="value"></param>
        public void SetPosition(Point value)
            => SetValue(new Point3d(value.X, value.Y, value.Z));

        /// <summary>
        /// Gets the alignment which this part references.
        /// </summary>
        public DynAlignment RefAlignment
            => DynAlignment.GetByObjectId(AeccPart.RefAlignmentId);

        /// <summary>
        /// Sets the alignment which this part references.
        /// </summary>
        /// <param name="value"></param>
        public void SetRefAlignment(DynAlignment value)
            => SetValue(value?.InternalObjectId ?? ObjectId.Null);

        /// <summary>
        /// Gets RuleSetStyle by name.
        /// </summary>
        public string RuleSetStyleName => GetString();

        /// <summary>
        /// Sets RuleSetStyle by name.
        /// </summary>
        /// <param name="value"></param>
        public void SetRuleSetStyleName(string value) => SetValue(value);

        /// <summary>
        /// Gets the Part's style name.
        /// </summary>
        public string StyleName => GetString();

        /// <summary>
        /// Sets the Part's style name
        /// </summary>
        /// <param name="value"></param>
        public void SetStyleName(string value) => SetValue(value);

        /// <summary>
        /// Gets the wall thickness for this part
        /// </summary>
        public double WallThickness => GetDouble();

       
    }
}
