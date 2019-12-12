using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.Civil.ApplicationServices;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using IterisCivilDynamo.CivilObjects;
using IterisCivilDynamo.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.Networks
{
    /// <summary>
    /// The Network class
    /// </summary>
    [RegisterForTrace]
    public sealed class Network : CivilEntity
    {
        internal C3dDb.Network AeccNetwork => AcObject as C3dDb.Network;

        internal Network(C3dDb.Entity entity, bool isDynamoOwned = false) : base(entity, isDynamoOwned)
        {
        }

        [SupressImportIntoVM]
        internal static Network GetByObjectId(ObjectId networkId)
        => CivilObjectSupport.Get<Network, C3dDb.Network>
                (networkId, (net) => new Network(net));

        /// <summary>
        /// Get all networks in the drawing
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="allowReference">Add referenced networks to result</param>
        /// <returns></returns>
        public static IList<Network> GetNetworks(Document document, bool allowReference)
        {
            if (document is null) throw new ArgumentNullException("document is null!");

            IList<Network> nets = new List<Network>();

            using (var context = new DocumentContext(document.AcDocument))
            {
                CivilDocument cdoc = CivilDocument
                    .GetCivilDocument(context.Database);

                Transaction tr = context.Transaction;

                using (ObjectIdCollection netIds = cdoc.GetPipeNetworkIds())
                {
                    foreach (ObjectId netId in netIds)
                    {
                        if (!netId.IsValid
                            || netId.IsErased
                            || netId.IsEffectivelyErased) continue;

                        if (tr.GetObject(netId, OpenMode.ForRead, false, true)
                        is C3dDb.Network net)
                        {
                            if (allowReference
                                || (!net.IsReferenceObject && !net.IsReferenceSubObject))
                            {
                                nets.Add(new Network(net));
                            }
                        }
                    }
                }
            }

            return nets;
        }

        /// <summary>
        /// Get a network in the drawing by name
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="networkName">The name of a network</param>
        /// <returns></returns>
        public static Network GetNetworkByName(Document document, string networkName)
        {
            if (networkName is null)
            {
                throw new ArgumentNullException("networkName is null!");
            }

            return GetNetworks(document, true)
                .FirstOrDefault(item => item.Name.Equals
                (networkName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the names of all networks in the drawing
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="allowReference">Add referenced networks to result</param>
        /// <returns></returns>
        public static IList<string> GetAllNetworksNames
            (Document document, bool allowReference)
            => GetNetworks(document, allowReference)
                .Select(item => item.Name)
                .ToList();

        /// <summary>
        /// Get the network pipes
        /// </summary>
        public IList<Pipe> Pipes
        {
            get
            {
                List<Pipe> pipes = new List<Pipe>();
                using (ObjectIdCollection pipeIds = AeccNetwork.GetPipeIds())
                {
                    foreach (ObjectId pipeId in pipeIds)
                    {
                        Pipe pipe = Pipe.GetByObjectId(pipeId);
                        pipes.Add(pipe);
                    }
                }
                return pipes;
            }
        }

        /// <summary>
        /// Get the network structures
        /// </summary>
        public IList<Structure> Structures
        {
            get
            {
                List<Structure> structures = new List<Structure>();
                using (ObjectIdCollection structIds = AeccNetwork.GetStructureIds())
                {
                    foreach (ObjectId structId in structIds)
                    {
                        Structure structure = Structure.GetByObjectId(structId);
                        structures.Add(structure);
                    }
                }
                return structures;
            }
        }        

        /// <summary>
        /// Gets the part list name.
        /// </summary>
        public string PartsListName => GetString();

        /// <summary>
        /// Sets the part list name.
        /// </summary>
        /// <param name="value"></param>
        public void SetPartsListName(string value) => SetValue(value);

        /// <summary>
        /// Gets the reference surface name.
        /// </summary>
        public string ReferenceSurfaceName => GetString();

        /// <summary>
        /// Sets the reference surface name.
        /// </summary>
        /// <param name="value"></param>
        public void SetReferenceSurfaceName(string value) => SetValue(value);

        /// <summary>
        /// Gets the reference alignment name.
        /// </summary>
        public string ReferenceAlignmentName => GetString();

        /// <summary>
        /// Sets the reference alignment name.
        /// </summary>
        /// <param name="value"></param>
        public void SetReferenceAlignmentName(string value) => SetValue(value);
    }
}
