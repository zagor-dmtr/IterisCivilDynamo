using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using System;
using System.Collections.Generic;
using System.Linq;
using AcApplication = Autodesk.AutoCAD.ApplicationServices.Application;
using AeccAlignment = Autodesk.Civil.DatabaseServices.Alignment;
using AeccProfileView = Autodesk.Civil.DatabaseServices.ProfileView;

namespace Iteris.Civil.Dynamo.ProfileViews
{
    /// <summary>
    /// Данные о виде профиля
    /// </summary>
    [RegisterForTrace]
    public class ProfileView : CivilObject
    {
        Alignment _alignment;

        internal AeccProfileView AeccProfileView => AcObject as AeccProfileView;

        internal ProfileView(AeccProfileView pView, bool isDynamoOwned)
            : base(pView, isDynamoOwned) { }

        /// <summary>
        /// Текущий коэффициент масштабирования анннотаций вида профиля
        /// </summary>
        public static double CurrentScaleFactor
            => 1 / (double)AcApplication.GetSystemVariable("cannoscalevalue");

        /// <summary>
        /// Положение точки вставки вида профиля
        /// </summary>
        public Point Location
        {
            get
            {
                Point3d location = AeccProfileView.Location;
                return Point.ByCoordinates
                    (location.X, location.Y, location.Z);
            }
        }

        /// <summary>
        /// Трасса вида профиля
        /// </summary>
        public Alignment Alignment
        {
            get
            {
                Document document = Document.Current;
                return _alignment ??
                    (_alignment = Selection.Alignments(document).FirstOrDefault
                    (item => item.Name.Equals(AeccProfileView.AlignmentName)));
            }
        }

        /// <summary>
        /// Выбор вида профиля на чертеже
        /// </summary>        
        /// <returns></returns>
        public static ProfileView SelectOnDwg()
        {
            Editor ed = AcApplication
                .DocumentManager.MdiActiveDocument.Editor;

            PromptEntityOptions selOpt
                = new PromptEntityOptions("\nSelect a profile view:");
            selOpt.SetRejectMessage("\nIt's not a profile view!");
            selOpt.AddAllowedClass(typeof(AeccProfileView), true);
            PromptEntityResult selRes = ed.GetEntity(selOpt);
            if (selRes.Status != PromptStatus.OK) return null;

            return Get(selRes.ObjectId);
        }

        /// <summary>
        /// Получение всех видов профилей трассы
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IList<ProfileView> GetProfileViews(Alignment alignment)
        {
            if (alignment is null) throw new ArgumentNullException("Alignment is null!");

            IList<ProfileView> ret = new List<ProfileView>();
            AeccAlignment align = alignment.InternalDBObject as AeccAlignment;

            ObjectIdCollection pViewIds = align.GetProfileViewIds();

            foreach (ObjectId pViewId in pViewIds)
            {
                if (!pViewId.IsValid
                    || pViewId.IsErased
                    || pViewId.IsEffectivelyErased) continue;

                ProfileView pView = Get(pViewId);
                pView._alignment = alignment;
                ret.Add(pView);
            }

            return ret;
        }

        /// <summary>
        /// Find coordinates X and Y for station and elevation.
        /// </summary>
        /// <param name="station"></param>
        /// <param name="elevation"></param>
        /// <returns>
        /// 1. Coordinate X (double),
        /// 2. Coordinate Y (double),
        /// 3. Coordinates is on this profile view (bool)
        /// </returns>
        [MultiReturn(new string[] { "X", "Y", "IsOnPView" })]
        public Dictionary<string, object> FindXYAtStationAndElevation(double station, double elevation)
        {
            double x = 0.0, y = 0.0;
            bool isOn
                = AeccProfileView.FindXYAtStationAndElevation
                (station, elevation, ref x, ref y);

            return new Dictionary<string, object>
            {
                { "X", x },
                { "Y", y },
                { "IsOnPView", isOn }
            };
        }
        
        /// <summary>
        /// Find station and elevation for coordinates X and Y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>
        /// 1. Station (double)
        /// 2. Elevation (double)
        /// 3. Station and elevation is on this profile view (bool)
        /// </returns>
        [MultiReturn(new string[] { "Station", "Elevation", "IsOnPView" })]
        public Dictionary<string, object> FindStationAndElevationAtXY(double x, double y)
        {
            double station = 0.0, elevation = 0.0;
            bool isOn
                = AeccProfileView.FindStationAndElevationAtXY
                (x, y, ref station, ref elevation);
            return new Dictionary<string, object>
            {
                { "Station", station },
                { "Elevation", elevation },
                { "IsOnPView", isOn }
            };
        }

        static ProfileView Get(ObjectId pViewId)
        {
            Document document = Document.Current;
            using (DocumentContext context = new DocumentContext(document.AcDocument))
            {
                AeccProfileView pView = context.Transaction
                    .GetObject(pViewId, OpenMode.ForWrite) as AeccProfileView;
                return new ProfileView(pView, false);
            }
        }
    }
}
