using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.CivilObjects;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment class. Alignment objects can represent centerlines,
    /// lanes, shoulders, right-of-ways, or construction baselines.
    /// </summary>
    [RegisterForTrace]
    public sealed class Alignment : CivilEntity
    {
        internal C3dDb.Alignment AeccAlignment => AcObject as C3dDb.Alignment;

        internal Alignment(C3dDb.Alignment alignment, bool isDynamoOwned = false)
            : base(alignment, isDynamoOwned) { }

        [SupressImportIntoVM]
        internal static Alignment GetByObjectId(ObjectId alignId)
            => CivilObjectSupport.Get<Alignment, C3dDb.Alignment>
                (alignId, (align) => new Alignment(align));

        /// <summary>
        /// Gets the alignment type: Centerline,
        /// Offset, CurbReturn, Utility, Rail
        /// </summary>
        public string AlignmentType => GetString();

        /// <summary>
        /// Gets the Alignment creation mode: RuleBasedCreation or ManuallyCreation
        /// </summary>
        public string CreationMode => GetString();

        /// <summary>
        /// Gets the criteria file name for the current alignment.
        /// The critertia file must keep consistent between the offset alignment and parent alignment.
        /// </summary>
        public string CriteriaFileName => GetString();

        /// <summary>
        /// Sets the criteria file name for the current alignment.
        /// The critertia file must keep consistent between the offset alignment and parent alignment.
        /// </summary>
        /// <param name="value"></param>
        public void SetCriteriaFileName(string value) => SetValue(value);

        /// <summary>
        /// Gets the name of of design check set that is used in the alignment.
        /// Return "" when there is no design check set applied in the current alignment.
        /// </summary>
        public string DesignCheckSetName => GetString();

        /// <summary>
        /// Sets the name of of design check set that is used in the alignment.
        /// </summary>
        /// <param name="value"></param>
        public void SetDesignCheckSetName(string value) => SetValue(value);

        /// <summary>
        /// Gets the Alignment's start station.
        /// </summary>
        public double StartingStation => GetDouble();

        /// <summary>
        /// Gets the Alignment's end station.
        /// </summary>
        public double EndingStation => GetDouble();

        /// <summary>
        /// Gets the Alignment's end station with equations.
        /// </summary>
        public double EndingStationWithEquations => GetDouble();

        /// <summary>
        /// Gets whether this Alignment has a Roundabout.
        /// </summary>
        public bool HasRoundabout => GetBool();

        /// <summary>
        /// Gets whether this Alignment is a connected alignment.
        /// </summary>
        public bool IsConnectedAlignment => GetBool();

        /// <summary>
        /// Gets whether this alignment is an offset alignment.
        /// </summary>
        public bool IsOffsetAlignment => GetBool();

        /// <summary>
        /// Gets a bool value that indicates whether this Alignment is a siteless Alignment.
        /// </summary>
        public bool IsSiteless => GetBool();

        /// <summary>
        /// Gets the Alignment's length.
        /// </summary>
        public double Length => GetDouble();

        /// <summary>
        /// Gets the Alignment reference point.
        /// </summary>
        public Point ReferencePoint
            => PointData.FromPointObject(AeccAlignment.ReferencePoint).CreateDynamoPoint();

        /// <summary>
        /// Sets the Alignment reference point.
        /// </summary>
        /// <param name="value"></param>
        public void SetReferencePoint(Point value) => SetValue(new Point2d(value.X, value.Y));

        struct RealStationData
        {
            public C3dDb.Station StationData;
            public double RealStation;

            public RealStationData(C3dDb.Alignment alignment, C3dDb.Station data)
            {
                StationData = data;
                double station = 0.0, offset = 0.0;
                alignment.StationOffset
                    (data.Location.X,
                    data.Location.Y,
                    ref station,
                    ref offset);
                RealStation = station;
            }
        }

        /// <summary>
        /// Gets the Alignment PI points
        /// </summary>
        public IList<Point> PIPoints
        {
            get
            {
                List<Point2d> piPts = new List<Point2d>();
                RealStationData[] allPIStations = AeccAlignment
                    .GetStationSet(C3dDb.StationTypes.PIPoint)
                    .Select(item => new RealStationData(AeccAlignment, item))
                    .ToArray();

                foreach (AlignmentCurve curve in GetCurves())
                {
                    C3dDb.Station[] curveStats = allPIStations
                        .Where(item => curve.StartStation <= item.RealStation
                            && item.RealStation <= curve.EndStation)
                        .Select(item => item.StationData)
                        .ToArray();

                    if (curveStats.Length > 0)
                    {
                        if (curve.SubEntityCount <= 1)
                        {
                            piPts.Add(curveStats[0].Location);
                        }
                        else
                        {
                            var piStat = curveStats.FirstOrDefault
                                (item => item.GeometryStationType
                                == C3dDb.AlignmentGeometryPointStationType.PI);
                            if (piStat != null)
                            {
                                piPts.Add(piStat.Location);
                            }
                        }
                    }
                }

                return piPts
                     .Select(item => PointData.FromPointObject(item).CreateDynamoPoint())
                     .ToList();
            }
        }

        /// <summary>
        /// Gets the Alignment reference point station.
        /// </summary>
        public double ReferencePointStation => GetDouble();

        /// <summary>
        /// Sets the Alignment reference point station.
        /// </summary>
        /// <param name="value"></param>
        public void SetReferencePointStation(double value) => SetValue(value);

        /// <summary>
        /// Gets the name of the Site to which this Alignment belongs.
        /// a string of "" for a siteless alignment.
        /// </summary>
        public string SiteName => GetString();

        /// <summary>
        /// Gets the Alignment station index increment.
        /// </summary>
        public double StationIndexIncrement => GetDouble();

        /// <summary>
        /// Sets the Alignment station index increment.
        /// </summary>
        /// <param name="value"></param>
        public void SetStationIndexIncrement(double value) => SetValue(value);

        /// <summary>
        /// Get the Alignment's style name.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the style name is invalid.
        /// </exception>
        public string StyleName => GetString();

        /// <summary>
        /// Sets the Alignment's style name.
        /// </summary>
        /// <param name="value"></param>
        public void SetStyleName(string value) => SetValue(value);

        /// <summary>
        /// Gets whether the Alignment uses the design check set.
        /// </summary>
        public bool UseDesignCheckSet => GetBool();       

        /// <summary>
        /// Sets whether the Alignment uses the design check set.
        /// </summary>
        /// <param name="value"></param>
        public void SetUseDesignCheckSet(bool value) => SetValue(value);

        /// <summary>
        /// Gets whether the alignment uses the design criteria file.
        /// </summary>
        public bool UseDesignCriteriaFile => GetBool();

        /// <summary>
        /// Sets whether the alignment uses the design criteria file.
        /// </summary>
        /// <param name="value"></param>
        public void SetUseDesignCriteriaFile(bool value) => SetValue(value);

        /// <summary>
        /// Gets a bool value that indicates whether this Alignment uses degign speed.
        /// </summary>
        public bool UseDesignSpeed => GetBool();

        /// <summary>
        /// Sets a bool value that indicates whether this Alignment uses degign speed.
        /// </summary>
        /// <param name="value"></param>
        public void SetUseDesignSpeed(bool value) => SetValue(value);

        /// <summary>
        /// Copies the Alignment to a specified Site. Specifying "" to move it to siteless.
        /// Calling this method copies all children profiles, profile views and sample line
        /// group with this alignment as well.
        /// </summary>
        /// <param name="siteName">The destination site name.</param>       
        public bool CopyToSite(string siteName)
        {
            try
            {
                AeccAlignment.CopyToSite(siteName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the alignment's curves data
        /// </summary>        
        /// <returns>Alignment's curves data list</returns>
        public IList<AlignmentCurve> GetCurves()
        {
            var curves = new List<AlignmentCurve>();
            foreach (C3dDb.AlignmentEntity ent in AeccAlignment.Entities)
            {
                AlignmentCurve res;
                if (ent is C3dDb.AlignmentArc arc)
                {
                    res = new AlignmentArc(arc);
                }
                else if (ent is C3dDb.AlignmentCCRC ccrc)
                {
                    res = new AlignmentCCRC(ccrc);
                }
                else if (ent is C3dDb.AlignmentCRC crc)
                {
                    res = new AlignmentCRC(crc);
                }
                else if (ent is C3dDb.AlignmentCTC ctc)
                {
                    res = new AlignmentCTC(ctc);
                }
                else if (ent is C3dDb.AlignmentLine line)
                {
                    res = new AlignmentLine(line);
                }
                else if (ent is C3dDb.AlignmentMultipleSegments ms)
                {
                    res = new AlignmentMultipleSegments(ms);
                }
                else if (ent is C3dDb.AlignmentSCS scs)
                {
                    res = new AlignmentSCS(scs);
                }
                else if (ent is C3dDb.AlignmentSCSCS scscs)
                {
                    res = new AlignmentSCSCS(scscs);
                }
                else if (ent is C3dDb.AlignmentSCSSCS scsscs)
                {
                    res = new AlignmentSCSSCS(scsscs);
                }
                else if (ent is C3dDb.AlignmentSpiral spiral)
                {
                    res = new AlignmentSpiral(spiral);
                }
                else if (ent is C3dDb.AlignmentSSCSS sscss)
                {
                    res = new AlignmentSSCSS(sscss);
                }
                else if (ent is C3dDb.AlignmentSTS sts)
                {
                    res = new AlignmentSTS(sts);
                }
                else if (ent is C3dDb.AlignmentCurve curve)
                {
                    res = new AlignmentCurve(curve);
                }
                else
                {
                    continue;
                }
                curves.Add(res);
            }
            return curves.OrderBy(item => item.StartStation).ToList();
        }

        /// <summary>
        /// Get the alignment's curves data
        /// </summary>        
        /// <returns>Alignment's curves data list</returns>
        [MultiReturn
            ("ArcCurves",
            "CCRCCurves",
            "CRCCurves",
            "CTCCurves",
            "LineCurves",
            "MultiSegmentCurves",
            "SCSCurves",
            "SCSCSCurves",
            "SCSSCSCurves",
            "SpiralCurves",
            "SSCSSCurves",
            "STSCurves",
            "UndefinedCurves")]
        public Dictionary<string, List<AlignmentCurve>> GetSortedCurves()
        {
            List<AlignmentCurve>
                ArcCurves = new List<AlignmentCurve>(),
                CCRCCurves = new List<AlignmentCurve>(),
                CRCCurves = new List<AlignmentCurve>(),
                CTCCurves = new List<AlignmentCurve>(),
                LineCurves = new List<AlignmentCurve>(),
                MultiSegmentCurves = new List<AlignmentCurve>(),
                SCSCurves = new List<AlignmentCurve>(),
                SCSCSCurves = new List<AlignmentCurve>(),
                SCSSCSCurves = new List<AlignmentCurve>(),
                SpiralCurves = new List<AlignmentCurve>(),
                SSCSSCurves = new List<AlignmentCurve>(),
                STSCurves = new List<AlignmentCurve>(),
                UndefinedCurves = new List<AlignmentCurve>();

            Dictionary<string, List<AlignmentCurve>> ret
                = new Dictionary<string, List<AlignmentCurve>>
            {
                { "ArcCurves", ArcCurves },
                { "CCRCCurves", CCRCCurves},
                { "CRCCurves", CRCCurves},
                { "CTCCurves", CTCCurves},
                { "LineCurves", LineCurves},
                { "MultiSegmentCurves", MultiSegmentCurves },
                { "SCSCurves", SCSCurves },
                { "SCSCSCurves", SCSCSCurves },
                { "SCSSCSCurves", SCSSCSCurves },
                { "SpiralCurves", SpiralCurves },
                { "SSCSSCurves", SSCSSCurves },
                { "STSCurves", STSCurves },
                { "UndefinedCurves", UndefinedCurves }
            };

            foreach (C3dDb.AlignmentEntity ent in AeccAlignment.Entities)
            {
                if (ent is C3dDb.AlignmentArc arc)
                {
                    ArcCurves.Add(new AlignmentArc(arc));
                }
                else if (ent is C3dDb.AlignmentCCRC ccrc)
                {
                    CCRCCurves.Add(new AlignmentCCRC(ccrc));
                }
                else if (ent is C3dDb.AlignmentCRC crc)
                {
                    CRCCurves.Add(new AlignmentCRC(crc));
                }
                else if (ent is C3dDb.AlignmentCTC ctc)
                {
                    CTCCurves.Add(new AlignmentCTC(ctc));
                }
                else if (ent is C3dDb.AlignmentLine line)
                {
                    LineCurves.Add(new AlignmentLine(line));
                }
                else if (ent is C3dDb.AlignmentMultipleSegments ms)
                {
                    MultiSegmentCurves.Add(new AlignmentMultipleSegments(ms));
                }
                else if (ent is C3dDb.AlignmentSCS scs)
                {
                    SCSCurves.Add(new AlignmentSCS(scs));
                }
                else if (ent is C3dDb.AlignmentSCSCS scscs)
                {
                    SCSCSCurves.Add(new AlignmentSCSCS(scscs));
                }
                else if (ent is C3dDb.AlignmentSCSSCS scsscs)
                {
                    SCSSCSCurves.Add(new AlignmentSCSSCS(scsscs));
                }
                else if (ent is C3dDb.AlignmentSpiral spiral)
                {
                    SpiralCurves.Add(new AlignmentSpiral(spiral));
                }
                else if (ent is C3dDb.AlignmentSSCSS sscss)
                {
                    SSCSSCurves.Add(new AlignmentSSCSS(sscss));
                }
                else if (ent is C3dDb.AlignmentSTS sts)
                {
                    STSCurves.Add(new AlignmentSTS(sts));
                }
                else if (ent is C3dDb.AlignmentCurve curve)
                {
                    UndefinedCurves.Add(new AlignmentCurve(curve));
                }
                else
                {
                    continue;
                }
            }
            return ret;
        }

        /// <summary>
        /// Select a alignment on the drawing
        /// </summary>       
        /// <returns>Selected alignment</returns>
        public static Alignment SelectOnDwg()
        {
            Document document = Document.Current;
            Editor ed = document.AcDocument.Editor;
            PromptEntityOptions alignSelOpt
                = new PromptEntityOptions("\nSelect an alignment:");
            alignSelOpt.SetRejectMessage("\nIt is not an alignment!");
            alignSelOpt.AddAllowedClass(typeof(C3dDb.Alignment), true);
            PromptEntityResult alignSelRes = ed.GetEntity(alignSelOpt);
            return alignSelRes.Status == PromptStatus.OK
                ? GetByObjectId(alignSelRes.ObjectId)
                : null;
        }
        
        /// <summary>
        /// Gets the geometry points of the alignment
        /// </summary>       
        /// <returns>List of points</returns>
        public IList<Point> GetPoints()
        {
            List<Point> ret = new List<Point>();
            C3dDb.Station[] stationSet = AeccAlignment
                .GetStationSet(C3dDb.StationTypes.GeometryPoint);
            foreach (C3dDb.Station data in stationSet)
            {
                Point point = Point.ByCoordinates
                    (data.Location.X, data.Location.Y);
                ret.Add(point);
            }
            return ret;
        }

        /// <summary>
        /// Gets the stations in the geometry points of the alignment
        /// </summary>       
        /// <returns>List of stations</returns>
        public IList<double> GetStations()
        {
            List<double> ret = new List<double>();
            C3dDb.Station[] stationSet = AeccAlignment
                .GetStationSet(C3dDb.StationTypes.GeometryPoint);
            foreach (C3dDb.Station data in stationSet)
            {
                ret.Add(data.RawStation);
            }
            return ret;
        }

        /// <summary>
        /// Get all alignments in the drawing
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="allowReference">Add referenced alignments to result</param>
        /// <returns></returns>
        public static IList<Alignment>
            GetAllAlignments(Document document, bool allowReference)
        {
            if (document is null) throw new ArgumentNullException("document is null!");

            IList<Alignment> alignments = new List<Alignment>();

            using (var context = new DocumentContext(document.AcDocument))
            {
                CivilDocument cdoc = CivilDocument
                    .GetCivilDocument(context.Database);

                Transaction tr = context.Transaction;

                ObjectIdCollection alignIds = cdoc.GetAlignmentIds();

                foreach (ObjectId alignId in alignIds)
                {
                    if (!alignId.IsValid
                        || alignId.IsErased
                        || alignId.IsEffectivelyErased) continue;

                    if (tr.GetObject(alignId, OpenMode.ForRead, false, true)
                        is C3dDb.Alignment align)
                    {
                        if (allowReference
                            || (!align.IsReferenceObject && !align.IsReferenceSubObject))
                        {
                            alignments.Add(new Alignment(align));
                        }
                    }
                }
            }

            return alignments;
        }

        /// <summary>
        /// Get an alignments in the drawing by name
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="alignmentName">The name of an alignment</param>
        /// <returns></returns>
        public static Alignment GetAlignmentByName(Document document, string alignmentName)
        {
            if (document is null) throw new ArgumentNullException("document is null!");
            if (alignmentName is null) throw new ArgumentNullException("alignmentName is null!");

            using (var context = new DocumentContext(document.AcDocument))
            {
                CivilDocument cdoc = CivilDocument
                    .GetCivilDocument(context.Database);

                Transaction tr = context.Transaction;

                ObjectIdCollection alignIds = cdoc.GetAlignmentIds();

                foreach (ObjectId alignId in alignIds)
                {
                    if (!alignId.IsValid
                        || alignId.IsErased
                        || alignId.IsEffectivelyErased) continue;

                    if (tr.GetObject(alignId, OpenMode.ForRead, false, true)
                        is C3dDb.Alignment align)
                    {
                        if (align.Name.Equals(alignmentName, StringComparison.OrdinalIgnoreCase))
                        {
                            return new Alignment(align);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the names of all alignments in the drawing
        /// </summary>
        /// <param name="document">Drawing document</param>
        /// <param name="allowReference">Include the referenced alignment to the result</param>
        /// <returns>List of alignments names</returns>
        public static IList<string> GetAllAlignmentNames(Document document, bool allowReference)
        {
            if (document is null) throw new ArgumentNullException("document is null!");

            IList<string> alignNames = new List<string>();

            using (var context = new DocumentContext(document.AcDocument))
            {
                CivilDocument cdoc = CivilDocument
                    .GetCivilDocument(context.Database);

                Transaction tr = context.Transaction;

                ObjectIdCollection alignIds = cdoc.GetAlignmentIds();

                foreach (ObjectId alignId in alignIds)
                {
                    if (!alignId.IsValid
                        || alignId.IsErased
                        || alignId.IsEffectivelyErased) continue;

                    if (tr.GetObject(alignId, OpenMode.ForRead, false, true)
                        is C3dDb.Alignment align)
                    {
                        if (allowReference
                            || (!align.IsReferenceObject && !align.IsReferenceSubObject))
                        {
                            alignNames.Add(align.Name);
                        }
                    }
                }
            }

            return alignNames;
        }
    }
}
