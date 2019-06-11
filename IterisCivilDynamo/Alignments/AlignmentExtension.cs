using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using DynamoServices;
using System;
using System.Collections.Generic;
using System.Linq;
using C3dAlignment = Autodesk.Civil.DatabaseServices.Alignment;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace Iteris.Civil.Dynamo.Alignments
{
    /// <summary>
    /// Методы для работы с трассами
    /// </summary>
    [RegisterForTrace]
    public static class AlignmentExtension
    {
        internal static C3dAlignment GetC3dObject(this Alignment alignment)
        {
            return alignment.InternalDBObject as C3dAlignment;
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
            alignSelOpt.AddAllowedClass(typeof(C3dAlignment), true);
            PromptEntityResult alignSelRes = ed.GetEntity(alignSelOpt);
            if (alignSelRes.Status != PromptStatus.OK) return null;
            string name;
            using (DocumentContext context
                = new DocumentContext(document.AcDocument))
            {
                C3dAlignment align = context.Transaction.GetObject
                    (alignSelRes.ObjectId, OpenMode.ForRead) as C3dAlignment;
                name = align.Name;
            }

            return Selection.Alignments(document)
                .FirstOrDefault(item => item.Name.Equals(name));
        }

        /// <summary>
        /// Получить точки изменения геометрии трассы
        /// </summary>
        /// <param name="alignment">Трасса</param>
        /// <returns>Список точек изменения геометрии трассы</returns>
        public static IList<Point> GetPoints(Alignment alignment)
        {
            List<Point> ret = new List<Point>();
            C3dDb.Station[] stationSet = alignment.GetC3dObject().GetStationSet(C3dDb.StationTypes.GeometryPoint);
            foreach (C3dDb.Station data in stationSet)
            {
                Point point = Point.ByCoordinates(data.Location.X, data.Location.Y);
                ret.Add(point);
            }
            return ret;
        }

        /// <summary>
        /// Получение пикетов в точках изменения геометрии трассы
        /// </summary>
        /// <param name="alignment">Трасса</param>
        /// <returns>Список значений пикетов в точках изменения геометрии трассы</returns>
        public static IList<double> GetStations(Alignment alignment)
        {
            List<double> ret = new List<double>();
            C3dDb.Station[] stationSet = alignment.GetC3dObject()
                .GetStationSet(C3dDb.StationTypes.GeometryPoint);
            foreach (C3dDb.Station data in stationSet)
            {
                ret.Add(data.RawStation);
            }
            return ret;
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

                    if (tr.GetObject(alignId, OpenMode.ForRead, false, true) is C3dAlignment align)
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

        /// <summary>
        /// Получение данных о кривых, из которых состоит трасса
        /// </summary>
        /// <param name="alignment">Трасса</param>
        /// <returns>Список из данных о кривых трассы</returns>
        public static IList<AlignmentCurve> GetCurves(Alignment alignment)
        {
            var curves = new List<AlignmentCurve>();
            C3dAlignment c3DAlign = alignment.GetC3dObject();
            foreach (C3dDb.AlignmentEntity ent in c3DAlign.Entities)
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
                    AlignmentAnyCurve curve
                        = new AlignmentAnyCurve(c3dAlignCurve);
                    curves.Add(curve);
                }
            }
            return curves;
        }
    }
}
