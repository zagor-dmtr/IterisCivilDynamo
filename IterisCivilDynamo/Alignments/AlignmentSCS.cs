using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment SpiralCurveSpiral class. This class represents
    /// an AlignmentEntity made up of an in spiral subentity, an arc
    /// subentity, and and out spiral subentity.
    /// </summary>
    [RegisterForTrace]
    public class AlignmentSCS : AlignmentCurve
    {
        /// <summary>
        /// Gets the arc of the SCS group.
        /// </summary>
        public AlignmentArc Arc { get; private set; }

        /// <summary>
        /// Gets the in spiral of the SCS group.
        /// </summary>
        public AlignmentSpiral SpiralIn { get; private set; }

        /// <summary>
        /// Gets the out spiral of the SCS group.
        /// </summary>
        public AlignmentSpiral SpiralOut { get; private set; }

        /// <summary>
        /// Gets the AlignmentSCS entity constraint type: SpiralInRadiusSpiralOut, SpiralLenRadiusPassPt,
        /// SpiralLenRadiusArcLen, SpiralLenRadius, SpiralLength, SpInLenSpOutLen, SpInAValSpOutAVal,
        /// Spiral1AValRadiusSpiral2AVal, SpiralAValRadiusPassPt, SpiralAValRadiusArcLen,
        /// SpiralAValRadius, SpiralAVal, SpiralNoParameter
        /// </summary>
        public string AlignmentSCSConstraintType { get; }

        internal AlignmentSCS(C3dDb.AlignmentSCS obj) : base(obj)
        {
            SafeAction
               (() => Arc = new AlignmentArc(obj.Arc),
               () => Arc = null);
            SafeAction
               (() => SpiralIn = new AlignmentSpiral(obj.SpiralIn),
               () => SpiralIn = null);
            SafeAction
               (() => SpiralOut = new AlignmentSpiral(obj.SpiralOut),
               () => SpiralOut = null);
            
            AlignmentSCSConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
