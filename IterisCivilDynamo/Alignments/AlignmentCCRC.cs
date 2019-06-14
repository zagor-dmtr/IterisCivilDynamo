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
        public AlignmentArc Arc1 { get; private set; }

        /// <summary>
        /// Gets the arc2 of the CCRC group.
        /// </summary>
        public AlignmentArc Arc2 { get; private set; }

        /// <summary>
        /// Gets the arc3 of the CCRC group.
        /// </summary>
        public AlignmentArc Arc3 { get; private set; }

        /// <summary>
        /// Defines the underlying Curve-Curve-ReverseCurve entity
        /// constraint type: TransitionLengthRadius1Radius2Radius3
        /// </summary>
        public string AlignmentCCRCConstraintType { get; }
       
        internal AlignmentCCRC(C3dDb.AlignmentCCRC obj) : base(obj)
        {
            SafeAction
                (() => Arc1 = new AlignmentArc(obj.Arc1),
                () => Arc1 = null);
            SafeAction
                (() => Arc2 = new AlignmentArc(obj.Arc2),
                () => Arc2 = null);
            SafeAction
                (() => Arc3 = new AlignmentArc(obj.Arc3),
                () => Arc3 = null);

            AlignmentCCRCConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
