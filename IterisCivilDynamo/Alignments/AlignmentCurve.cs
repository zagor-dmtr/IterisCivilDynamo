using Autodesk.DesignScript.Geometry;
using Iteris.Civil.Dynamo.Support;
using System;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace Iteris.Civil.Dynamo.Alignments
{
    /// <summary>
    /// Базовый объект для всех типов кривых,
    /// из которых может состоять трасса
    /// </summary>
    public abstract class AlignmentCurve
    {
        private int entityAfter;
        private int entityBefore;
        private int entityId;
        private double highestDesignSpeed;
        private int subEntityCount;

        private PointData _startPoint, _endPoint;

        /// <summary>
        /// Начальная точка кривой
        /// </summary>
        public Point StartPoint => _startPoint.CreateDynamoPoint();

        /// <summary>
        /// Конечная точка кривой
        /// </summary>
        public Point EndPoint => _endPoint.CreateDynamoPoint();

        /// <summary>
        /// Начальный пикет аж кривой
        /// </summary>
        public double StartStation { get; private set; }

        /// <summary>
        /// Конечный пикетаж кривой
        /// </summary>
        public double EndStation { get; private set; }

        /// <summary>
        /// Длина кривой
        /// </summary>
        public double Length { get; protected set; }

        /// <summary>
        /// Тип кривой
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Уникальный номер кривой внутри трассы
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
        /// Номер следующей кривой внутри трассы.
        /// Если -1 - следующая кривая отсутствует,
        /// то есть кривая находится в конце трассы
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
        /// Номер предыдущей кривой внутри трассы.
        /// Если -1 - предыдущая кривая отсутствует,
        /// то есть кривая находится в начале трассы
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
        /// Максимальная проектная скорость на кривой
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
        /// Количество вложенных кривых - для сложных участков
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
        /// Является ли кривая вложенной в другую кривую
        /// </summary>
        public bool IsSubEntity { get; }

        private AlignmentCurve(object curve)
        {
            SetProps(curve);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve"></param>
        protected AlignmentCurve(C3dDb.AlignmentCurve curve) : this(curve as object)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subEntity"></param>
        protected AlignmentCurve(C3dDb.AlignmentSubEntity subEntity) : this(subEntity as object)
        {
            IsSubEntity = true;
            Type = ReflectionSupport.GetProperty
                (subEntity, "SubEntityType", (object)"NO_TYPE").ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityObject"></param>
        protected virtual void SetProps(object entityObject)
        {
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
