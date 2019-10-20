using Autodesk.Civil.DynamoNodes;
using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.DesignScript.Runtime;
using IterisCivilDynamo.Support;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// The Network class
    /// </summary>
    [RegisterForTrace]
    public sealed class Network : CivilObject
    {
        internal C3dDb.Network AeccNetwork => AcObject as C3dDb.Network;

        internal Network(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        [IsVisibleInDynamoLibrary(false)]
        internal static Network GetByObjectId(ObjectId networkId)
        => CivilObjectSupport.Get<Network, C3dDb.Network>
                (networkId, (net) => new Network(net));        
    }
}
