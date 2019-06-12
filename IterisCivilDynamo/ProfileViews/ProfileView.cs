using Autodesk.AutoCAD.DatabaseServices;
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
using AlignmentNode = IterisCivilDynamo.Alignments.Alignment;
using C3dProfileView = Autodesk.Civil.DatabaseServices.ProfileView;

namespace IterisCivilDynamo.ProfileViews
{
    /// <summary>
    /// Profile view data
    /// </summary>
    [RegisterForTrace]
    public sealed class ProfileView : CivilObject
    {
        private readonly ObjectId AlignmentId;

        internal C3dProfileView PView => AcObject as C3dProfileView;       

        internal ProfileView(C3dProfileView pView, bool isDynamoOwned = false)
            : base(pView, isDynamoOwned)
        {
            AlignmentId = pView.AlignmentId;
            AlignmentName = pView.AlignmentName;
        }

        /// <summary>
        /// Gets the location of the profile view.
        /// </summary>
        public Point Location
        {
            get
            {
                Point3d location = PView.Location;
                return Point.ByCoordinates
                    (location.X, location.Y, location.Z);
            }
        }

        /// <summary>
        /// Gets the minimum elevation of the profile view.
        /// </summary>
        public double ElevationMin => PView.ElevationMin;

        /// <summary>
        /// Gets the maximum elevation of the profile view.
        /// </summary>
        public double ElevationMax => PView.ElevationMax;

        /// <summary>
        /// Gets the start station of the profile view.
        /// </summary>
        public double StationStart => PView.StationStart;

        /// <summary>
        /// Gets the end station of the profile view.
        /// </summary>
        public double StationEnd => PView.StationEnd;

        /// <summary>
        /// Gets or sets the ProfileView's style by name.
        /// </summary>
        public string StyleName
        {
            get => PView.StyleName;
            set => PView.StyleName = value;
        }

        /// <summary>
        /// Gets the alignment from which the profile view was created.
        /// </summary>
        public AlignmentNode Alignment => AlignmentNode.Get(AlignmentId);

        /// <summary>
        /// Gets the name of the alignment from which the profile view was created.
        /// </summary>
        public string AlignmentName { get; }

        /// <summary>
        /// Gets how to specify the vertical range of the profile view.
        /// </summary>
        /// <remarks>
        /// Automatic or UserSpecified
        /// </remarks>
        public string ElevationRangeMode => PView.ElevationRangeMode.ToString();

        /// <summary>
        /// Select a profile view on drawing
        /// </summary>        
        /// <returns></returns>
        public static ProfileView SelectOnDwg()
        {
            Editor ed = AcApplication
                .DocumentManager.MdiActiveDocument.Editor;

            PromptEntityOptions selOpt
                = new PromptEntityOptions("\nSelect a profile view:");
            selOpt.SetRejectMessage("\nIt's not a profile view!");
            selOpt.AddAllowedClass(typeof(C3dProfileView), true);
            PromptEntityResult selRes = ed.GetEntity(selOpt);
            if (selRes.Status != PromptStatus.OK) return null;

            return Get(selRes.ObjectId);
        }

        /// <summary>
        /// Gets all the alignment's profile views
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IList<ProfileView> GetProfileViews(AlignmentNode alignment)
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

                ProfileView pView = Get(pViewId);               
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
                = PView.FindXYAtStationAndElevation
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
                = PView.FindStationAndElevationAtXY
                (x, y, ref station, ref elevation);
            return new Dictionary<string, object>
            {
                { "Station", station },
                { "Elevation", elevation },
                { "IsOnPView", isOn }
            };
        }

        static ProfileView Get(ObjectId pViewId)
            => CivilObjectSupport
            .Get<ProfileView, C3dProfileView>
            (pViewId, (pV) => new ProfileView(pV));
    }
}
