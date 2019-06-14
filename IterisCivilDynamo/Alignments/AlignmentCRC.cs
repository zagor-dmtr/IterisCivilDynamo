using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment MultipleArcs class. This class represents an
    /// AlignmentEntity made up of Curve and reverse curve subentities.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentCRC : AlignmentCurve
    {
        /// <summary>
        /// Gets the arc1 of the CRC group.
        /// </summary>
        public AlignmentArc Arc1 { get; private set; }

        /// <summary>
        /// Gets the arc2 of the CRC group.
        /// </summary>
        public AlignmentArc Arc2 { get; private set; }

        /// <summary>
        /// Gets the AlignmentCRC entity constraint type: Radius1 or Radius2
        /// </summary>
        public string AlignmentCRCConstraintType { get; }

        internal AlignmentCRC(C3dDb.AlignmentCRC crc) : base(crc)
        {
            SafeAction
               (() => Arc1 = new AlignmentArc(crc.Arc1),
               () => Arc1 = null);
            SafeAction
                (() => Arc2 = new AlignmentArc(crc.Arc2),
                () => Arc2 = null);
            AlignmentCRCConstraintType
                = crc.Constraint2.ToString();
        }
    }
}
