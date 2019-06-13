using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment CurveCurveReverseCurve class. This class represents
    /// an AlignmentCurve made up of Curve, Curve and reverse Curve subentities.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentCCRC : AlignmentCurve
    {
        /// <summary>
        /// Gets the arc1 of the CCRC group.
        /// </summary>
        public AlignmentArc Arc1 { get; }

        /// <summary>
        /// Gets the arc2 of the CCRC group.
        /// </summary>
        public AlignmentArc Arc2 { get; }

        /// <summary>
        /// Gets the arc3 of the CCRC group.
        /// </summary>
        public AlignmentArc Arc3 { get; }

        /// <summary>
        /// Defines the underlying Curve-Curve-ReverseCurve entity constraint type.
        /// </summary>
        public string AlignmentCCRCConstraintType { get; }
       
        internal AlignmentCCRC(C3dDb.AlignmentCCRC alignmentCCRC) : base(alignmentCCRC)
        {
            Arc1 = new AlignmentArc(alignmentCCRC.Arc1);
            Arc2 = new AlignmentArc(alignmentCCRC.Arc2);
            Arc3 = new AlignmentArc(alignmentCCRC.Arc3);
            AlignmentCCRCConstraintType
                = alignmentCCRC.Constraint2.ToString();
        }
    }
}
