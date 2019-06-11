using Autodesk.DesignScript.Geometry;
using DynamoServices;
using Iteris.Civil.Dynamo.Support;
using C3d = Autodesk.Civil;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace Iteris.Civil.Dynamo.Alignments
{
    /// <summary>
    /// Alignment spirale data
    /// </summary>
    [RegisterForTrace]
    public class AlignmentSpiral : AlignmentNotLinearCurve
    {
        private PointData _radialPoint, _sPIPoint;

        /// <summary>
        /// Spirale parameter A
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// Spirale parameter K
        /// </summary>
        public double K { get; private set; }

        /// <summary>
        /// Spirale parameter P
        /// </summary>
        public double P { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity Simple/Compound Flag
        /// </summary>
        public bool Compound { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral entity Incurve/Outcurve type
        /// </summary>
        public string CurveType { get; private set; }  

        /// <summary>
        /// Left or right
        /// </summary>
        public string Direction { get; private set; }

        /// <summary>
        /// Gets the spiral long tangent.
        /// </summary>
        public double LongTangent { get; private set; }

        /// <summary>
        /// Gets the minimum transition length according to the design speed check.
        /// </summary>
        public double MinimumTransitionLength { get; private set; }

        /// <summary>
        /// Входной радиус
        /// </summary>
        public double RadiusIn { get; private set; }

        /// <summary>
        /// Выходной радиус
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
        /// Способ построения спирали (клотоида, синусоида и т.п.)
        /// </summary>
        public string SpiralDefinition { get; private set; }

        /// <summary>
        /// Gets the spiral total X value.
        /// </summary>
        public double TotalX { get; private set; }

        /// <summary>
        /// Gets the spiral total Y value.
        /// </summary>
        public double TotalY { get; private set; }

        /// <summary>
        /// Gets the AlignmentSpiral subentity radial Point2D coordinate.
        /// </summary>
        public Point RadialPoint => _radialPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentSpiral entity SPI Point2D coordinate.
        /// </summary>
        public Point SPIPoint => _sPIPoint.CreateDynamoPoint();

        /// <summary>
        /// Gets the AlignmentSpiral entity SPI station.
        /// </summary>
        public double SPIStation { get; private set; }

        internal AlignmentSpiral(C3dDb.AlignmentSpiral spiral)
            : base(spiral)
        {           
        }

        internal AlignmentSpiral
            (C3dDb.AlignmentSubEntitySpiral subEntitySpiral)
            : base(subEntitySpiral)
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spiral"></param>
        protected override void SetProps(object spiral)
        {
            base.SetProps(spiral);
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
