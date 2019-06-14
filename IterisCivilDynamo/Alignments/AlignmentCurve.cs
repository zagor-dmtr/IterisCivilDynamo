using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using System;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The AlignmentCurve class. This is an abstract base class
    /// for other Alignment Entity classes, such as AlignmentArc,
    /// AlignmentSCS, and AlignmentLine.
    /// </summary>
    [RegisterForTrace]
    public class AlignmentCurve
    {
        private int entityAfter;
        private int entityBefore;
        private int entityId;
        private double highestDesignSpeed;
        private int subEntityCount;
        
        private protected PointData _startPoint;
       
        private protected PointData _endPoint;

        /// <summary>
        /// Gets the AlignmentCurve constraint type, either Fixed, Float or Free.
        /// </summary>
        public string ConstraintType { get; private set; }

        /// <summary>
        /// Gets the start point of the AlignmentCurve.
        /// </summary>
        public Point StartPoint => _startPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the end point of the AlignmentCurve.
        /// </summary>
        public Point EndPoint => _endPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the start station of the AlignmentCurve.
        /// </summary>
        public double StartStation { get; private set; }

        /// <summary>
        /// Gets the end station of the AlignmentCurve.
        /// </summary>
        public double EndStation { get; private set; }

        /// <summary>
        /// Gets the length of the AlignmentCurve.
        /// </summary>
        public double Length { get; protected set; }

        /// <summary>
        /// Gets the AlignmentCurve type: Line, Arc, Spiral, SpiralCurveSpiral,
        /// SpiralLineSpiral, SpiralLine, LineSpiral, SpiralCurve, CurveSpiral,
        /// SpiralSpiralCurveSpiralSpiral, SpiralCurveSpiralCurveSpiral,
        /// SpiralCurveSpiralSpiralCurveSpiral, SpiralSpiral, SpiralSpiralCurve,
        /// CurveSpiralSpiral, MultipleSegments, CurveLineCurve, CurveReverseCurve,
        /// CurveCurveReverseCurve
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the AlignmentEntity id, which is the unique representation of this alignment entity.
        /// </summary>
        public int EntityId
        {
            get
            {
                CheckForSubEntity(nameof(EntityId));
                return entityId;
            }

            private set => entityId = value;
        }

        /// <summary>
        /// Gets the ID of the AlignmentEntity after this one.
        /// If the value is -1, the curve is in end of the alignment.
        /// </summary>
        public int EntityAfter
        {
            get
            {
                CheckForSubEntity(nameof(EntityAfter));
                return entityAfter;
            }

            private set => entityAfter = value;
        }

        /// <summary>
        /// Gets the ID of the AlignmentEntity before this one.
        /// If the value is -1, the curve is in start of the alignment.
        /// </summary>
        public int EntityBefore
        {
            get
            {
                CheckForSubEntity(nameof(EntityBefore));
                return entityBefore;
            }

            private set => entityBefore = value;
        }

        /// <summary>
        /// Gets the highest design speed of the AlignmentCurve.
        /// </summary>
        public double HighestDesignSpeed
        {
            get
            {
                CheckForSubEntity(nameof(HighestDesignSpeed));
                return highestDesignSpeed;
            }

            private set => highestDesignSpeed = value;
        }

        /// <summary>
        /// Gets the number of subentities that make up the AlignmentCurve.
        /// </summary>
        public int SubEntityCount
        {
            get
            {
                CheckForSubEntity(nameof(SubEntityCount));
                return subEntityCount;
            }

            private set => subEntityCount = value;
        }

        /// <summary>
        /// AlignmentCurves's group index.
        /// </summary>
        public string CurveGroupIndex { get; private set; }

        /// <summary>
        /// AlignmentCurves's group subentity index
        /// </summary>
        public string CurveGroupSubEntityIndex { get; private set; }

        /// <summary>
        /// Alignment curve is subentity
        /// </summary>
        public bool IsSubEntity { get; }

        internal AlignmentCurve(object curve)
        {
            SetProps(curve);
        }
        
        protected private AlignmentCurve(C3dDb.AlignmentCurve curve) : this(curve as object)
        {
            IsSubEntity = false;
            Type = ReflectionSupport.GetProperty
                (curve, "EntityType", (object)"NO_TYPE").ToString();
            EntityId = ReflectionSupport.GetProperty(curve, "EntityId", -1);
            EntityAfter = ReflectionSupport.GetProperty(curve, "EntityAfter", -1);
            EntityBefore = ReflectionSupport.GetProperty(curve, "EntityBefore", -1);
            HighestDesignSpeed = ReflectionSupport.GetProperty
                (curve, "HighestDesignSpeed", double.NaN);
            SubEntityCount = ReflectionSupport.GetProperty(curve, "SubEntityCount", 0);
        }
       
        protected private AlignmentCurve(C3dDb.AlignmentSubEntity subEntity)
            : this(subEntity as object)
        {
            IsSubEntity = true;
            Type = ReflectionSupport.GetProperty
                (subEntity, "SubEntityType", (object)"NO_TYPE").ToString();
        }
        
        protected private virtual void SetProps(object entityObject)
        {
            ConstraintType = ReflectionSupport.GetProperty
                (entityObject, "Constraint1", (object)"NO_TYPE").ToString();
            _startPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(entityObject, "StartPoint", null));
            _endPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(entityObject, "EndPoint", null));
            StartStation = ReflectionSupport.GetProperty
                (entityObject, "StartStation", double.NaN);
            EndStation = ReflectionSupport.GetProperty
                (entityObject, "EndStation", double.NaN);
            Length = ReflectionSupport.GetProperty
                (entityObject, "Length", double.NaN);
            CurveGroupIndex = ReflectionSupport.GetProperty
                (entityObject, "CurveGroupIndex", string.Empty);
            CurveGroupSubEntityIndex = ReflectionSupport.GetProperty
                (entityObject, "CurveGroupSubEntityIndex", string.Empty);
        }

        private protected string GetConstraint2(object obj)
            => ReflectionSupport.GetProperty
                (obj, "Constraint2", (object)"NO_TYPE").ToString();

        private protected void SafeAction(Action forTry, Action forCatch)
        {
            try
            {
                forTry();
            }
            catch
            {
                forCatch();
            }
        }

        private void CheckForSubEntity(string propName)
        {
            if (IsSubEntity)
            {
                throw new InvalidOperationException
                    ($"This property is not awaylable for sub entity: {propName}");
            }
        }
    }
}
