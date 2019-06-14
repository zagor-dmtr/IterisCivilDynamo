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
        public AlignmentArc Arc1 { get; private set; }

        /// <summary>
        /// Gets the second arc of the SCSCS group.
        /// </summary>
        public AlignmentArc Arc2 { get; private set; }

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
        public AlignmentSpiral Spiral1 { get; private set; }

        /// <summary>
        /// Gets the second spiral of the SCSCS group.
        /// </summary>
        public AlignmentSpiral Spiral2 { get; private set; }

        /// <summary>
        /// Gets the third spiral of the SCSCS group.
        /// </summary>
        public AlignmentSpiral Spiral3 { get; private set; }

        internal AlignmentSCSCS(C3dDb.AlignmentSCSCS obj) : base(obj)
        {
            SafeAction
                (() => Arc1 = new AlignmentArc(obj.Arc1),
                () => Arc1 = null);
            SafeAction
                (() => Arc2 = new AlignmentArc(obj.Arc2),
                () => Arc2 = null);
            SafeAction
               (() => Spiral1 = new AlignmentSpiral(obj.Spiral1),
               () => Spiral1 = null);
            SafeAction
                (() => Spiral2 = new AlignmentSpiral(obj.Spiral2),
                () => Spiral2 = null);
            SafeAction
                (() => Spiral3 = new AlignmentSpiral(obj.Spiral3),
                () => Spiral3 = null);
            AlignmentSCSCSConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
