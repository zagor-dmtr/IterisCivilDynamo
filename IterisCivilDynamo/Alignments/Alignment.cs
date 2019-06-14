using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// The Alignment class. Alignment objects can represent centerlines,
    /// lanes, shoulders, right-of-ways, or construction baselines.
    /// </summary>
    [RegisterForTrace]
    public sealed class Alignment : CivilObject
    {
        internal C3dDb.Alignment AeccAlignment => AcObject as C3dDb.Alignment;

        internal Alignment(C3dDb.Alignment alignment, bool isDynamoOwned = false)
            : base(alignment, isDynamoOwned) { }

        /// <summary>
        /// Gets the alignment type: Centerline,
        /// Offset, CurbReturn, Utility, Rail
        /// </summary>
        public string AlignmentType => AeccAlignment.AlignmentType.ToString();

        /// <summary>
        /// Gets the Alignment creation mode: RuleBasedCreation or ManuallyCreation
        /// </summary>
        public string CreationMode => AeccAlignment.CreationMode.ToString();

        /// <summary>
        /// Gets or sets the criteria file name for the current alignment.
        /// The critertia file must keep consistent between the offset alignment and parent alignment.
        /// </summary>
        public string CriteriaFileName
        {
            get => AeccAlignment.CriteriaFileName;
            set => AeccAlignment.CriteriaFileName = value;
        }

        /// <summary>
        /// Gets or sets the name of of design check set that is used in the alignment.
        /// Return "" when there is no design check set applied in the current alignment.
        /// </summary>
        public string DesignCheckSetName
        {
            get => AeccAlignment.DesignCheckSetName;
            set => AeccAlignment.DesignCheckSetName = value;
        }

        /// <summary>
        /// Gets the Alignment's start station.
        /// </summary>
        public double StartingStation => AeccAlignment.StartingStation;

        /// <summary>
        /// Gets the Alignment's end station.
        /// </summary>
        public double EndingStation => AeccAlignment.EndingStation;

        /// <summary>
        /// Gets the Alignment's end station with equations.
        /// </summary>
        public double EndingStationWithEquations => AeccAlignment.EndingStationWithEquations;

        /// <summary>
        /// Gets whether this Alignment has a Roundabout.
        /// </summary>
        public bool HasRoundabout => AeccAlignment.HasRoundabout;

        /// <summary>
        /// Gets whether this Alignment is a connected alignment.
        /// </summary>
        public bool IsConnectedAlignment => AeccAlignment.IsConnectedAlignment;

        /// <summary>
        /// Gets whether this alignment is an offset alignment.
        /// </summary>
        public bool IsOffsetAlignment => AeccAlignment.IsOffsetAlignment;

        /// <summary>
        /// Gets a bool value that indicates whether this Alignment is a siteless Alignment.
        /// </summary>
        public bool IsSiteless => AeccAlignment.IsSiteless;

        /// <summary>
        /// Gets the Alignment's length.
        /// </summary>
        public double Length => AeccAlignment.Length;

        /// <summary>
        /// Gets or sets the Alignment reference point.
        /// </summary>
        public Point ReferencePoint
        {
            get => PointData.FromPointObject(AeccAlignment.ReferencePoint).CreateDynamoPoint();
            set => AeccAlignment.ReferencePoint = new Point2d(value.X, value.Y);
        }

        /// <summary>
        /// Gets or sets the Alignment reference point station.
        /// </summary>
        public double ReferencePointStation
        {
            get => AeccAlignment.ReferencePointStation;
            set => AeccAlignment.ReferencePointStation = value;
        }

        /// <summary>
        /// Gets the name of the Site to which this Alignment belongs.
        /// a string of "" for a siteless alignment.
        /// </summary>
        public string SiteName => AeccAlignment.SiteName;

        /// <summary>
        /// Gets or sets the Alignment station index increment.
        /// </summary>
        public double StationIndexIncrement
        {
            get => AeccAlignment.StationIndexIncrement;
            set => AeccAlignment.StationIndexIncrement = value;
        }

        /// <summary>
        /// Get or sets the Alignment's style name.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the style name is invalid.
        /// </exception>
        public string StyleName
        {
            get => AeccAlignment.StyleName;
            set => AeccAlignment.StyleName = value;
        }

        /// <summary>
        /// Gets or sets whether the Alignment uses the design check set.
        /// </summary>
        public bool UseDesignCheckSet
        {
            get => AeccAlignment.UseDesignCheckSet;
            set => AeccAlignment.UseDesignCheckSet = value;
        }

        /// <summary>
        /// Gets or sets whether the alignment uses the design criteria file.
        /// </summary>
        public bool UseDesignCriteriaFile
        {
            get => AeccAlignment.UseDesignCriteriaFile;
            set => AeccAlignment.UseDesignCriteriaFile = value;
        }

        /// <summary>
        /// Gets or sets a bool value that indicates whether this Alignment uses degign speed.
        /// </summary>
        public bool UseDesignSpeed
        {
            get => AeccAlignment.UseDesignSpeed;
            set => AeccAlignment.UseDesignSpeed = value;
        }

        /// <summary>
        /// Copies the Alignment to a specified Site. Specifying "" to move it to siteless.
        /// Calling this method copies all children profiles, profile views and sample line
        /// group with this alignment as well.
        /// </summary>
        /// <param name="siteName">The destination site name.</param>
        /// <exception cref="System.ArgumentException">Thrown when the Site name is invalid.</exception>
        public void CopyToSite(string siteName)
        {
            AeccAlignment.CopyToSite(siteName);
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
            return curves;
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
                ? Get(alignSelRes.ObjectId)
                : null;
        }

        [IsVisibleInDynamoLibrary(false)]
        internal static Alignment Get(ObjectId alignId)
            => CivilObjectSupport.Get<Alignment, C3dDb.Alignment>
                (alignId, (align) => new Alignment(align));

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
