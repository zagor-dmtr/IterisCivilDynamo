using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment SpiralSpiralCurveSpiralSpiral class.
    /// This class represents an AlignmentEntity made up
    /// of the following subentities: spiral, spiral, arc,
    /// spiral, spiral.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentSSCSS : AlignmentCurve
    {
        /// <summary>
        /// Gets the arc from the SSCSS group.
        /// </summary>
        public AlignmentArc Arc { get; private set; }

        /// <summary>
        /// Gets the first spiral from the SSCSS group.
        /// </summary>
        public AlignmentSpiral Spiral1 { get; private set; }

        /// <summary>
        /// Gets the second spiral from the SSCSS group.
        /// </summary>
        public AlignmentSpiral Spiral2 { get; private set; }

        /// <summary>
        /// Gets the third spiral from the SSCSS group.
        /// </summary>
        public AlignmentSpiral Spiral3 { get; private set; }

        /// <summary>
        /// Gets the fourth spiral from the SSCSS group.
        /// </summary>
        public AlignmentSpiral Spiral4 { get; private set; }

        /// <summary>
        /// Defines the underlying Spiral-Spiral-Curve-SpiralSpiral entity
        /// constraint type: kSp1LenSp2LenRadiusPt, kSp1LenSp2LenPt1Pt2,
        /// kSp1AValSp2AValRadiusPt, kSp1AValSp2AValPt1Pt2
        /// </summary>
        public string AlignmentSSCSSConstraintType { get; }

        internal AlignmentSSCSS(C3dDb.AlignmentSSCSS obj) : base(obj)
        {
            SafeAction
                (() => Arc = new AlignmentArc(obj.Arc),
                () => Arc = null);
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

            AlignmentSSCSSConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
