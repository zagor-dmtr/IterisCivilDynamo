using DynamoServices;
using Iteris.Civil.Dynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace Iteris.Civil.Dynamo.Alignments
{
    /// <summary>
    /// Базовый объект для не прямолинейной кривой трассы
    /// </summary>
    [RegisterForTrace]
    public abstract class AlignmentNotLinearCurve : AlignmentCurve
    {
        /// <summary>
        /// Направление в начале
        /// </summary>
        public double StartDirection { get; private set; }

        /// <summary>
        /// Направление в конце
        /// </summary>
        public double EndDirection { get; private set; }

        /// <summary>
        /// Gets or sets the AlignmentNotLinearCurve's delta
        /// </summary>
        public double Delta { get; private set; }

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
