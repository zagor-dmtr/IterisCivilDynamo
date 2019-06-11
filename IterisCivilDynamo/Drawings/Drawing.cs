using DynamoServices;
using AcApplication = Autodesk.AutoCAD.ApplicationServices.Application;

namespace IterisCivilDynamo.Drawings
{
    /// <summary>
    /// Drawing methods and properties
    /// </summary>
    [RegisterForTrace]
    public static class Drawing
    {
        /// <summary>
        /// Current annotationscale factor
        /// </summary>
        public static double CurrentScaleFactor
            => 1 / (double)AcApplication.GetSystemVariable("cannoscalevalue");

    }
}
