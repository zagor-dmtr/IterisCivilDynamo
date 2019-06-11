using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// Данные об отрезке трассы
    /// </summary>
    [RegisterForTrace]
    public class AlignmentLine : AlignmentCurve
    {
        PointData _midPoint;

        /// <summary>
        /// Угол направления
        /// </summary>
        public double Direction { get; private set; }

        /// <summary>
        /// Средняя точка
        /// </summary>
        public Point MidPoint => _midPoint.CreateDynamoPoint();      

        internal AlignmentLine(C3dDb.AlignmentLine line) : base(line)
        {           
        }

        internal AlignmentLine(C3dDb.AlignmentSubEntityLine subEntityLine) : base(subEntityLine)
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected override void SetProps(object line)
        {
            base.SetProps(line);
            Direction = ReflectionSupport.GetProperty(line, "Direction", double.NaN);
            _midPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(line, "MidPoint", null));
            Length = _startPoint.DistanceTo(_endPoint);
        }
    }
}