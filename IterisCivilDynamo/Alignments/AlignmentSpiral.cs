using Autodesk.DesignScript.Geometry;
using DynamoServices;
using IterisCivilDynamo.Support;
using C3d = Autodesk.Civil;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The AlignmentSpiral Class. AlignmentSpiral derives from the
    /// AlignmentCurve class, and represents an AlignmentEntity made
    /// up of a single spiral 
    /// </summary>
    [RegisterForTrace]
    public class AlignmentSpiral : AlignmentNotLinearCurve
    {
        private PointData _radialPoint, _sPIPoint;

        /// <summary>
        /// Gets the AlignmentSpiral entity constraint type:
        /// StartPtAndDirRadiusLength or StartPtAndDirStartAndEndRadiusLength
        /// </summary>
        public string AlignmentSpiralConstraintType { get; private set; }

        /// <summary>
        /// Gets the spiral A value.
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity Simple/Compound Flag
        /// </summary>
        public bool Compound { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity Incurve/Outcurve type
        /// </summary>
        public string CurveType { get; private set; }

        /// <summary>
        /// Gets the AlignmentCurve's delta
        /// </summary>
        public override double Delta => base.Delta;

        /// <summary>
        /// Left or right
        /// </summary>
        public string Direction { get; private set; }

        /// <summary>
        /// Gets the direction value at the end point.
        /// </summary>
        public override double EndDirection => base.EndDirection;

        /// <summary>
        /// Gets the spiral K value.
        /// </summary>
        public double K { get; private set; }

        /// <summary>
        /// Gets the spiral long tangent.
        /// </summary>
        public double LongTangent { get; private set; }

        /// <summary>
        /// Gets the minimum transition length according to the design speed check.
        /// </summary>
        public double MinimumTransitionLength { get; private set; }

        /// <summary>
        /// Gets the spiral P value.
        /// </summary>
        public double P { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral subentity radial Point2D coordinate.
        /// </summary>
        public Point RadialPoint => _radialPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the incoming curve radius.
        /// </summary>
        public double RadiusIn { get; private set; }

        /// <summary>
        /// Gets the outgoing curve radius.
        /// </summary>
        public double RadiusOut { get; private set; }

        /// <summary>
        /// Gets the spiral short tangent.
        /// </summary>
        public double ShortTangent { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity SPI angle.
        /// </summary>
        public double SPIAngle { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity SPI Point2D coordinate.
        /// </summary>
        public Point SPIPoint => _sPIPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentSpiral entity spiral type: Clothoid, JapaneseCubic,
        /// SineHalfWave, Bloss, CubicParabola, Sinusoidal, BiQuadratic, OffsetClothoid,
        /// OffsetHalfWaveLenDimnTangent, OffsetJapaneseCubic, OffsetBloss, OffsetCubicParabola,
        /// OffsetSinusoidal,OffsetBiQuadratic, OffsetHalfWaveLenDimnTangent2, OffsetInvalidSpiralType
        /// </summary>
        public string SpiralDefinition { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity SPI station.
        /// </summary>
        public double SPIStation { get; private set; }

        /// <summary>
        /// Gets the direction value at the start point.
        /// </summary>
        public override double StartDirection => base.StartDirection;

        /// <summary>
        /// Gets the spiral total X value.
        /// </summary>
        public double TotalX { get; private set; }

        /// <summary>
        /// Gets the spiral total Y value.
        /// </summary>
        public double TotalY { get; private set; }        

        internal AlignmentSpiral(C3dDb.AlignmentSpiral spiral)
            : base(spiral)
        {           
        }

        internal AlignmentSpiral
            (C3dDb.AlignmentSubEntitySpiral subEntitySpiral)
            : base(subEntitySpiral)
        {            
        }
        
        protected private override void SetProps(object spiral)
        {
            base.SetProps(spiral);
            AlignmentSpiralConstraintType = GetConstraint2(spiral);
            A = ReflectionSupport.GetProperty(spiral, "A", double.NaN);
            K = ReflectionSupport.GetProperty(spiral, "K", double.NaN);
            P = ReflectionSupport.GetProperty(spiral, "P", double.NaN);
            Compound = ReflectionSupport.GetProperty(spiral, "Compound", false);
            CurveType = ReflectionSupport.GetProperty
                (spiral, "CurveType", C3dDb.SpiralCurveType.InCurve).ToString();
            Direction = ReflectionSupport.GetProperty
                (spiral, "Direction", C3dDb.SpiralDirectionType.DirectionRight).ToString();
            ShortTangent = ReflectionSupport.GetProperty(spiral, "ShortTangent", double.NaN);
            LongTangent = ReflectionSupport.GetProperty(spiral, "LongTangent", double.NaN);
            MinimumTransitionLength = ReflectionSupport.GetProperty
                (spiral, "MinimumTransitionLength", double.NaN);
            _radialPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(spiral, "RadialPoint", null));
            RadiusIn = ReflectionSupport.GetProperty(spiral, "RadiusIn", double.NaN);
            RadiusOut = ReflectionSupport.GetProperty(spiral, "RadiusOut", double.NaN);
            SPIAngle = ReflectionSupport.GetProperty(spiral, "SPIAngle", double.NaN);
            _sPIPoint = PointData.FromPointObject
                (ReflectionSupport.GetProperty(spiral, "SPIPoint", null));
            SpiralDefinition = ReflectionSupport.GetProperty
                (spiral, "SpiralDefinition", C3d.SpiralType.Clothoid).ToString();
            SPIStation = ReflectionSupport.GetProperty(spiral, "SPIStation", double.NaN);
            TotalX = ReflectionSupport.GetProperty(spiral, "TotalX", double.NaN);
            TotalY = ReflectionSupport.GetProperty(spiral, "TotalY", double.NaN);
        }
    }
}
