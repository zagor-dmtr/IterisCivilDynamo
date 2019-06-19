using DynamoServices;
using AcDbHatch = Autodesk.AutoCAD.DatabaseServices.Hatch;
using DynAcObject = Autodesk.AutoCAD.DynamoNodes.Object;

namespace IterisCivilDynamo.AutoCAD
{
    /// <summary>
    /// Dynamo representation of the AutoCAD Hatch object 
    /// </summary>
    [RegisterForTrace]
    public class Hatch : DynAcObject
    {
        internal AcDbHatch DbHatch => AcObject as AcDbHatch;

        internal Hatch(AcDbHatch hatch, bool isDynamoOwned = false)
            : base(hatch, isDynamoOwned)
        {
        }

        /// <summary>
        /// The area of the hatch
        /// </summary>
        public double Area => DbHatch.Area;

        /// <summary>
        /// Get hatch from object
        /// </summary>
        /// <param name="dynAcObject"></param>
        public Hatch(DynAcObject dynAcObject) : this(dynAcObject.InternalDBObject as AcDbHatch)
        {            
        }
    }
}
