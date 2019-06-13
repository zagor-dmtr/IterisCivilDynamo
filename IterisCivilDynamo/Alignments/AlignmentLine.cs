using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The AlignmentLine class. This class represents an AlignmentEntity made up of a single line.
    /// </summary>
    [RegisterForTrace]
    public class AlignmentLine : AlignmentCurve
    {
        PointData _midPoint;

        /// <summary>
        /// Gets the direction of the AlignmentLine.
        /// </summary>
        public double Direction { get; private set; }

        /// <summary>
        /// Gets the AlignmentLine middle point coordinate.
        /// </summary>
        public Point MidPoint => _midPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentLine entity constraint type: TwoPoints,
        /// ThroughPoint, Length, NoConstraint, BestFit
        /// </summary>
        public string AlignmentLineConstraintType { get; private set; }

        internal AlignmentLine(C3dDb.AlignmentLine line) : base(line)
        {           
        }

        internal AlignmentLine(C3dDb.AlignmentSubEntityLine subEntityLine) : base(subEntityLine)
        {            
        }
       
        protected private override void SetProps(object line)
        {
            base.SetProps(line);
            Direction = ReflectionSupport.GetProperty(line, "Direction", double.NaN);
            _midPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(line, "MidPoint", null));
            Length = _startPoint.DistanceTo(_endPoint);
            AlignmentLineConstraintType = GetConstraint2(line);
        }
    }
}