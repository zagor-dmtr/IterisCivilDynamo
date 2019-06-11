using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// Данные о кривой трассы типа спираль-дуга-спираль
    /// </summary>
    [RegisterForTrace]
    public class AlignmentSCS : AlignmentCurve
    {
        /// <summary>
        /// Данные о дуге
        /// </summary>
        public AlignmentArc Arc { get; }

        /// <summary>
        /// Данные о входной спирали
        /// </summary>
        public AlignmentSpiral SpiralIn { get; }

        /// <summary>
        /// Данные о выходной спирали
        /// </summary>
        public AlignmentSpiral SpiralOut { get; }

        internal AlignmentSCS(C3dDb.AlignmentSCS alignmentSCS) : base(alignmentSCS)
        {
            Arc = new AlignmentArc(alignmentSCS.Arc);
            SpiralIn = new AlignmentSpiral(alignmentSCS.SpiralIn);
            SpiralOut = new AlignmentSpiral(alignmentSCS.SpiralOut);
        }
    }
}
