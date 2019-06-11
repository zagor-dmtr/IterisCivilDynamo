using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// Базовый объект для не прямолинейной кривой трассы
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public abstract class AlignmentNotLinearCurve : AlignmentCurve
    {
        /// <summary>
        /// Направление в начале
        /// </summary>
        public virtual double StartDirection { get; private set; }

        /// <summary>
        /// Направление в конце
        /// </summary>
        public virtual double EndDirection { get; private set; }

        /// <summary>
        /// Gets or sets the AlignmentNotLinearCurve's delta
        /// </summary>
        public virtual double Delta { get; private set; }

        internal AlignmentNotLinearCurve(C3dDb.AlignmentCurve curve) : base(curve)
        {
            SetProps(curve);
        }

        internal AlignmentNotLinearCurve(C3dDb.AlignmentSubEntity subEntity) : base(subEntity)
        {
            SetProps(subEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve"></param>
        protected override void SetProps(object curve)
        {
            base.SetProps(curve);
            StartDirection = ReflectionSupport
                .GetProperty(curve, "StartDirection", double.NaN);
            EndDirection = ReflectionSupport
                .GetProperty(curve, "EndDirection", double.NaN);
            Delta = ReflectionSupport.GetProperty(curve, "Delta", double.NaN);
        }
    }
}
