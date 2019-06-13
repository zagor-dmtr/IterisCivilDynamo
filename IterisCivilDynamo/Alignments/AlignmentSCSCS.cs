using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment SpiralCurveSpiralCurveSpiral class. This class represents
    /// an AlignmentCurve made up of the following subentities: an in spiral,
    /// then an arc, spiral, arc, and out spiral.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentSCSCS : AlignmentCurve
    {
        /// <summary>
        /// Gets the first arc of the SCSCS group.
        /// </summary>
        public AlignmentArc Arc1 { get; }

        /// <summary>
        /// Gets the second arc of the SCSCS group.
        /// </summary>
        public AlignmentArc Arc2 { get; }

        /// <summary>
        /// Gets the AlignmentSCSCS entity constraint type: SpLenAndTanLenIn,
        /// SpLenAndStartPoint, SpLenAndArc1Angle, SpLenAndArc1PassPt,
        /// SpAValAndTanLenIn, SpAValAndStartPoint, SpAValAndArc1Angle,
        /// SpAValAndArc1PassPt, SpLenAndTanLenOut, SpLenAndEndPoint,
        /// SpLenAndArc2Angle, SpLenAndArc2PassPt, SpAValAndTanLenOut,
        /// SpAValAndEndPoint, SpAValAndArc2Angle, SpAValAndArc2PassPt,
        /// SpLenAndArc1Length, SpLenAndArc2Length, SpAValAndArc1Length,
        /// SpAValAndArc2Length
        /// </summary>
        public string AlignmentSCSCSConstraintType { get; }

        /// <summary>
        /// Gets the first spiral of the SCSCS group.
        /// </summary>
        public AlignmentSpiral Spiral1 { get; }

        /// <summary>
        /// Gets the second spiral of the SCSCS group.
        /// </summary>
        public AlignmentSpiral Spiral2 { get; }

        /// <summary>
        /// Gets the third spiral of the SCSCS group.
        /// </summary>
        public AlignmentSpiral Spiral3 { get; }

        internal AlignmentSCSCS(C3dDb.AlignmentSCSCS alignmentSCSCS) : base(alignmentSCSCS)
        {
            Arc1 = new AlignmentArc(alignmentSCSCS.Arc1);
            Arc2 = new AlignmentArc(alignmentSCSCS.Arc2);
            Spiral1 = new AlignmentSpiral(alignmentSCSCS.Spiral1);
            Spiral2 = new AlignmentSpiral(alignmentSCSCS.Spiral2);
            Spiral3 = new AlignmentSpiral(alignmentSCSCS.Spiral3);
            AlignmentSCSCSConstraintType
                = alignmentSCSCS.Constraint2.ToString();
        }
    }
}
