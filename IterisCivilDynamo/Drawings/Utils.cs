using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using DynamoServices;
using System.Collections.Generic;
using System.Linq;
using Dyn = Autodesk.AutoCAD.DynamoNodes;

namespace IterisCivilDynamo.Drawings
{
    /// <summary>
    /// Utilites for the autocad objects
    /// </summary>
    [RegisterForTrace]
    public static class Utils
    {
        /// <summary>
        /// Create an anonymous group from objects
        /// </summary>
        /// <param name="objects"></param>
        public static void CreateAnonymousGroup(IList<Dyn.Object> objects)
        {
            if (objects is null) throw new System.ArgumentNullException("Objects");

            var first = objects.FirstOrDefault();

            if (first is null) return;

            Database db = first.InternalObjectId.Database;
            Document adoc = Application.DocumentManager.GetDocument(db);

            using (DocumentContext context = new DocumentContext(adoc))
            {
                Transaction tr = context.Transaction;

                DBDictionary groupDic = tr.GetObject
                    (db.GroupDictionaryId, OpenMode.ForWrite)
                    as DBDictionary;

                Group anonyGroup = new Group();
                groupDic.SetAt("*", anonyGroup);

                foreach (var obj in objects)
                {
                    anonyGroup.Append(obj.InternalObjectId);
                }
                tr.AddNewlyCreatedDBObject(anonyGroup, true);
            }
        }
    }
}
