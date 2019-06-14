using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment SpiralTangentSpiral class. This class
    /// represents an AlignmentEntity made up of a spiral,
    /// then tangent, then spiral subentities.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentSTS : AlignmentCurve
    {
        /// <summary>
        /// Gets the in spiral of the STS group.
        /// </summary>
        public AlignmentSpiral SpiralIn { get; private set; }

        /// <summary>
        /// Gets the out spiral of the STS group.
        /// </summary>
        public AlignmentSpiral SpiralOut { get; private set; }

        /// <summary>
        /// Gets the tangent of the STS group.
        /// </summary>
        public AlignmentLine Tangent { get; private set; }

        /// <summary>
        /// Gets the AlignmentSTS entity constraint type: Spiral1LengthSpiral2Length,
        /// TangentLength, Spiral1AValSpiral2AVal, SpiralLengthTangentPassPt,
        /// SpiralLengthTangentLength, SpiralAValTangentPassPt, SpiralAValTangentLength
        /// </summary>
        public string AlignmentSTSConstraintType { get; }

        internal AlignmentSTS(C3dDb.AlignmentSTS obj) : base(obj)
        {
            SafeAction
               (() => SpiralIn = new AlignmentSpiral(obj.SpiralIn),
               () => SpiralIn = null);
            SafeAction
               (() => SpiralOut = new AlignmentSpiral(obj.SpiralOut),
               () => SpiralOut = null);
            SafeAction
                (() => Tangent = new AlignmentLine(obj.Tangent),
                () => Tangent = null);

            AlignmentSTSConstraintType
                = obj.Constraint2.ToString();
        }
    }
}
