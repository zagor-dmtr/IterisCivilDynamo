using Autodesk.Civil.DynamoNodes;
using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// 
    /// </summary>
    [RegisterForTrace]
    public sealed class Structure : Part
    {
        internal C3dDb.Structure AeccStructure => AeccPart as C3dDb.Structure;

        internal Structure(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        internal static Structure GetByObjectId(ObjectId endStructureId)
        {
            throw new NotImplementedException();
        }
    }
}
