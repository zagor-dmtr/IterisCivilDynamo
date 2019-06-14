using DynamoServices;
using System.Collections.Generic;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment MultipleArcs class. This class represents an
    /// AlignmentEntity made up of several arc or line subentities.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentMultipleSegments : AlignmentCurve
    {
        /// <summary>
        /// Arc or line subentities
        /// </summary>
        public IList<AlignmentCurve> SubEntities { get; } = new List<AlignmentCurve>();

        /// <summary>
        /// Gets the AlignmentMultipleArcs entity constraint
        /// type: RadiiAndLengths, RatiosAndLengths, KeyPoints
        /// </summary>
        public string AlignmentMultipleSegmentsConstraintType { get; }

        internal AlignmentMultipleSegments(C3dDb.AlignmentMultipleSegments mseg)
            : base(mseg)
        {
            for (int i = 0; i < mseg.SubEntityCount; i++)
            {
                C3dDb.AlignmentSubEntity curve = mseg[i];
                if (curve is C3dDb.AlignmentSubEntityLine line)
                {
                    SubEntities.Add(new AlignmentLine(line));
                }
                else if (curve is C3dDb.AlignmentSubEntityArc arc)
                {
                    SubEntities.Add(new AlignmentArc(arc));
                }
            }
        }
    }
}
