using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The AlignmentArc class.
    /// </summary>
    [RegisterForTrace]
    public sealed class AlignmentArc : AlignmentNotLinearCurve
    {
        private PointData _centerPoint;
        private readonly PointData _pIPoint;

        /// <summary>
        /// Gets the AlignmentArc's center point.
        /// </summary>
        public Point CenterPoint => _centerPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentArc's chord direction.
        /// </summary>
        public double ChordDirection { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's chord length.
        /// </summary>
        public double ChordLength { get; private set; }

        /// <summary>
        /// Gets a bool value that specifies whether the curve entity is a reverse curve.
        /// </summary>
        public bool Clockwise { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc entity constraint type: ThreePoints,
        /// CenterRadius, CenterPassThroughPoint, PassThroughRadius,
        /// PassThroughHoldEnd, PassThroughDirection, Radius, PassThrough,
        /// RadiusAndLength, BestFitArc
        /// </summary>
        public string AlignmentArcConstraintType { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's deflected angle.
        /// </summary>
        public double DeflectedAngle { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's external secant.
        /// </summary>
        public double ExternalSecant { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's external tangent.
        /// </summary>
        public double ExternalTangent { get; private set; }

        /// <summary>
        /// Gets or sets a bool value indicating whether the Arc solution angle is > 180 Degrees.
        /// </summary>
        public bool GreaterThan180 { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's mid ordinate.
        /// </summary>
        public double MidOrdinate { get; private set; }

        /// <summary>
        /// Gets the minimum radius according to the design speed check.
        /// </summary>
        public double MinimumRadius { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Point PIPoint => _pIPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentArc's PI station.
        /// </summary>
        public double PIStation { get; private set; }

        /// <summary>
        /// Gets the AlignmentArc's radius.
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// Gets a bool value that specifies whether the curve entity is a reverse curve.
        /// </summary>
        public bool ReverseCurve { get; private set; }

        /// <summary>
        /// Direction in start
        /// </summary>
        public override double StartDirection => base.StartDirection;

        /// <summary>
        /// Direction in end
        /// </summary>
        public override double EndDirection => base.EndDirection;

        /// <summary>
        /// Gets the AlignmentCurve's delta
        /// </summary>
        public override double Delta => base.Delta;

        internal AlignmentArc(C3dDb.AlignmentArc arc) : base(arc)
        {
            _pIPoint = default;            
        }

        internal AlignmentArc(C3dDb.AlignmentSubEntityArc subEntityArc) : base(subEntityArc)
        {
            _pIPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(subEntityArc, "PIPoint", null));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arc"></param>
        protected private override void SetProps(object arc)
        {
            base.SetProps(arc);
            _centerPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(arc, "CenterPoint", null));
            ChordDirection = ReflectionSupport.GetProperty(arc, "ChordDirection", double.NaN);
            ChordLength = ReflectionSupport.GetProperty(arc, "ChordLength", double.NaN);
            Clockwise = ReflectionSupport.GetProperty(arc, "Clockwise", false);
            AlignmentArcConstraintType = GetConstraint2(arc);
            DeflectedAngle = ReflectionSupport.GetProperty(arc, "DeflectedAngle", double.NaN);
            ExternalSecant = ReflectionSupport.GetProperty(arc, "ExternalSecant", double.NaN);
            ExternalTangent = ReflectionSupport.GetProperty(arc, "ExternalTangent", double.NaN);
            GreaterThan180 = ReflectionSupport.GetProperty(arc, "GreaterThan180", false);
            MidOrdinate = ReflectionSupport.GetProperty(arc, "MidOrdinate", double.NaN);
            MinimumRadius = ReflectionSupport.GetProperty(arc, "MinimumRadius", double.NaN);
            PIStation = ReflectionSupport.GetProperty(arc, "PIStation", double.NaN);
            Radius = ReflectionSupport.GetProperty(arc, "Radius", double.NaN);
            ReverseCurve = ReflectionSupport.GetProperty(arc, "ReverseCurve", false);
        }
    }
}
