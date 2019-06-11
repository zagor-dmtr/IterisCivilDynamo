using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using AcApplication = Autodesk.AutoCAD.ApplicationServices.Application;
using AeccProfileView = Autodesk.Civil.DatabaseServices.ProfileView;
using IterisAlignment = IterisCivilDynamo.Alignments.Alignment;

namespace IterisCivilDynamo.ProfileViews
{
    /// <summary>
    /// Данные о виде профиля
    /// </summary>
    [RegisterForTrace]
    public sealed class ProfileView : CivilObject
    {
        private readonly ObjectId AlignmentId;

        internal AeccProfileView AeccProfileView => AcObject as AeccProfileView;       

        internal ProfileView(AeccProfileView pView, bool isDynamoOwned = false)
            : base(pView, isDynamoOwned)
        {
            AlignmentId = pView.AlignmentId;
        }
        
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
        public IterisAlignment Alignment => IterisAlignment.Get(AlignmentId);

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
        public static IList<ProfileView> GetProfileViews(IterisAlignment alignment)
        {
            if (alignment is null) throw new ArgumentNullException("Alignment is null!");

            IList<ProfileView> ret = new List<ProfileView>();           

            ObjectIdCollection pViewIds
                = alignment.AeccAlignment.GetProfileViewIds();

            foreach (ObjectId pViewId in pViewIds)
            {
                if (!pViewId.IsValid
                    || pViewId.IsErased
                    || pViewId.IsEffectivelyErased) continue;

                ProfileView pView = CivilObjectSupport
                    .Get<ProfileView, AeccProfileView>
                    (pViewId, (pV) => new ProfileView(pV));               
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
