using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using AeccAlignment = Autodesk.Civil.DatabaseServices.Alignment;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Alignments
{
    /// <summary>
    /// Alignment data
    /// </summary>
    [RegisterForTrace]
    public sealed class Alignment : CivilObject
    {
        internal AeccAlignment AeccAlignment => AcObject as AeccAlignment;

        internal Alignment(AeccAlignment alignment, bool isDynamoOwned = false)
            : base(alignment, isDynamoOwned) { }

        /// <summary>
        /// Получение данных о кривых, из которых состоит трасса
        /// </summary>        
        /// <returns>Список из данных о кривых трассы</returns>
        public IList<AlignmentCurve> GetCurves()
        {
            var curves = new List<AlignmentCurve>();
            foreach (C3dDb.AlignmentEntity ent in AeccAlignment.Entities)
            {
                if (ent is C3dDb.AlignmentLine c3dAlignLine)
                {
                    AlignmentLine lineData = new AlignmentLine(c3dAlignLine);
                    curves.Add(lineData);
                }
                else if (ent is C3dDb.AlignmentArc c3dAlignArc)
                {
                    AlignmentArc alignmentArc = new AlignmentArc(c3dAlignArc);
                    curves.Add(alignmentArc);
                }
                else if (ent is C3dDb.AlignmentSpiral c3dAlignSpiral)
                {
                    AlignmentSpiral alignmentSpiral = new AlignmentSpiral(c3dAlignSpiral);
                    curves.Add(alignmentSpiral);
                }
                else if (ent is C3dDb.AlignmentSCS c3dAlignSCS)
                {
                    AlignmentSCS alignmentSCS = new AlignmentSCS(c3dAlignSCS);
                    curves.Add(alignmentSCS);
                }
                else if (ent is C3dDb.AlignmentCurve c3dAlignCurve)
                {
                    AlignmentCurve curve
                        = new AlignmentCurve(c3dAlignCurve);
                    curves.Add(curve);
                }
            }
            return curves;
        }

        /// <summary>
        /// Выбрать трассу на чертеже
        /// </summary>       
        /// <returns>Выбранная на чертеже трасса</returns>
        public static Alignment SelectOnDwg()
        {
            Document document = Document.Current;
            Editor ed = document.AcDocument.Editor;
            PromptEntityOptions alignSelOpt
                = new PromptEntityOptions("\nSelect an alignment:");
            alignSelOpt.SetRejectMessage("\nIt is not an alignment!");
            alignSelOpt.AddAllowedClass(typeof(AeccAlignment), true);
            PromptEntityResult alignSelRes = ed.GetEntity(alignSelOpt);
            return alignSelRes.Status == PromptStatus.OK
                ? Get(alignSelRes.ObjectId)
                : null;
        }

        [IsVisibleInDynamoLibrary(false)]
        internal static Alignment Get(ObjectId alignId)
            => CivilObjectSupport.Get<Alignment, AeccAlignment>
                (alignId, (align) => new Alignment(align));

        /// <summary>
        /// Получить точки изменения геометрии трассы
        /// </summary>       
        /// <returns>Список точек изменения геометрии трассы</returns>
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
        /// Получение пикетов в точках изменения геометрии трассы
        /// </summary>       
        /// <returns>Список значений пикетов в точках изменения геометрии трассы</returns>
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
            if (document is null) throw new ArgumentNullException("document");

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
                        is AeccAlignment align)
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
        /// Получение имён всех трасс в чертеже
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="allowReference">Включить в результат трассы из быстрых ссылок</param>
        /// <returns>Список имён трасс в чертеже</returns>
        public static IList<string> GetAllAlignmentNames(Document document, bool allowReference)
        {
            if (document is null) throw new ArgumentNullException("document");

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
                        is AeccAlignment align)
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
