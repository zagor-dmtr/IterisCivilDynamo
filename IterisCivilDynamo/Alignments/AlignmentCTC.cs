using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment MultipleArcs class. This class represents
    /// an AlignmentEntity made up of Curve, Line and Curve subentities.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentCTC : AlignmentCurve
    {
        /// <summary>
        /// Gets the arc1 of the CTC group.
        /// </summary>
        public AlignmentArc Arc1 { get; private set; }

        /// <summary>
        /// Gets the arc2 of the CTC group.
        /// </summary>
        public AlignmentArc Arc2 { get; private set; }

        /// <summary>
        /// Gets the tangent of the CTC group.
        /// </summary>
        public AlignmentLine Tangent { get; private set; }

        /// <summary>
        /// Gets the AlignmentCTC curve constraint type: RadiusRadiusLength
        /// </summary>
        public string AlignmentCTCConstraintType { get; }
        
        internal AlignmentCTC(C3dDb.AlignmentCTC obj) : base(obj)
        {
            SafeAction
               (() => Arc1 = new AlignmentArc(obj.Arc1),
               () => Arc1 = null);
            SafeAction
                (() => Arc2 = new AlignmentArc(obj.Arc2),
                () => Arc2 = null);
            SafeAction
                (() => Tangent = new AlignmentLine(obj.Tangent),
                () => Tangent = null);
            
            AlignmentCTCConstraintType = obj.Constraint2.ToString();
        }
    }
}
