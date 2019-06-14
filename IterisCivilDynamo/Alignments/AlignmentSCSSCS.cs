using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment SpiralCurveSpiralSpiralCurveSpiral class.
    /// This class represents an AlignmentEntity made up of the
    /// following subentities: an in spiral, then an arc, spiral,
    /// spiral, arc, and an out spiral.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentSCSSCS : AlignmentCurve
    {
        /// <summary>
        /// Gets the first arc of the SCSSCS group.
        /// </summary>
        public AlignmentArc Arc1 { get; private set; }

        /// <summary>
        /// Gets the second arc of the SCSSCS group.
        /// </summary>
        public AlignmentArc Arc2 { get; private set; }

        /// <summary>
        /// Gets the first spiral of the SCSSCS group.
        /// </summary>
        public AlignmentSpiral Spiral1 { get; private set; }

        /// <summary>
        /// Gets the second spiral of the SCSSCS group.
        /// </summary>
        public AlignmentSpiral Spiral2 { get; private set; }

        /// <summary>
        /// Gets the third spiral of the SCSSCS group.
        /// </summary>
        public AlignmentSpiral Spiral3 { get; private set; }

        /// <summary>
        /// Gets the fourth spiral of the SCSSCS group.
        /// </summary>
        public AlignmentSpiral Spiral4 { get; private set; }

        /// <summary>
        /// Gets the AlignmentSCSSCS entity constraint type: SpLenAndStartPoint,
        /// SpLenAndArc1Angle, SpLenAndArc1PassPt, SpAValAndStartPoint,
        /// SpAValAndArc1Angle, SpAValAndArc1PassPt, SpLenAndEndPoint,
        /// SpLenAndArc2Angle, SpLenAndArc2PassPt, SpAValAndEndPoint,
        /// SpAValAndArc2Angle, SpAValAndArc2PassPt, SpLenAndArc1Length,
        /// SpLenAndArc2Length, SpAValAndArc1Length, SpAValAndArc2Length
        /// </summary>
        public string AlignmentSCSSCSConstraintType { get; }

        internal AlignmentSCSSCS(C3dDb.AlignmentSCSSCS obj) : base(obj)
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
            SafeAction
                (() => Spiral4 = new AlignmentSpiral(obj.Spiral4),
                () => Spiral4 = null);

            AlignmentSCSSCSConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
