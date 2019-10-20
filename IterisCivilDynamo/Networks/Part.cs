using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;
using ThisAlignment = IterisCivilDynamo.Alignments.Alignment;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// The Part class
    /// </summary>
    [RegisterForTrace]
    public abstract class Part : CivilObject
    {
        internal C3dDb.Part AeccPart => AcObject as C3dDb.Part;

        internal Part(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        /// <summary>
        /// Gets the number of parts that connects to the part
        /// </summary>
        public int ConnectedPartCount => AeccPart.ConnectedPartCount;

        /// <summary>
        /// Gets the part's domain: Pipe or Structure.
        /// </summary>
        public string Domain => AeccPart.Domain.ToString();

        /// <summary>
        /// Gets the network to which this part belongs
        /// </summary>
        /// <returns></returns>
        public Network Network => Network.GetByObjectId(AeccPart.NetworkId);

        /// <summary>
        /// Gets the part size name
        /// </summary>
        public string PartSizeName => AeccPart.PartSizeName;

        /// <summary>
        /// Gets the part’s subtype.
        /// </summary>
        public string PartSubType => AeccPart.PartSubType;

        /// <summary>
        /// Gets the type of the network part.
        /// UndefinedPartType, Pipe, Channel, Wire, Conduit, StructNull,
        /// StructJunction, StructInletOutlet, StructGeneral, StructEquipment
        /// </summary>       
        public string PartType => AeccPart.PartType.ToString();

        /// <summary>
        /// Gets or sets the position of the network part.
        /// </summary>
        public Point Position
        {
            get => PointData.FromPointObject(AeccPart.Position).CreateDynamoPoint();
            set => AeccPart.Position = new Point3d(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Gets or sets the alignment which this part references.
        /// </summary>
        public ThisAlignment RefAlignment
        {
            get => ThisAlignment.GetByObjectId(AeccPart.RefAlignmentId);
            set => AeccPart.RefAlignmentId = value?.InternalObjectId ?? ObjectId.Null;
        }

        /// <summary>
        /// Gets or sets the Part's style name.
        /// </summary>
        public string StyleName
        {
            get => AeccPart.StyleName;
            set
            {
                try
                {
                    AeccPart.StyleName = value;
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Gets the wall thickness for this part
        /// </summary>
        public double WallThickness => AeccPart.WallThickness;
    }
}
