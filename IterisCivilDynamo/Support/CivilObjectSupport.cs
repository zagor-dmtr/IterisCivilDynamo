using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.Civil.DynamoNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AeccEntity = Autodesk.Civil.DatabaseServices.Entity;

namespace IterisCivilDynamo.Support
{
    internal static class CivilObjectSupport
    {
        public static T Get<T, U>(ObjectId id, Func<U,T> creator)
            where T : CivilObject
            where U : AeccEntity
        {
            Document document = Document.Current;
            using (DocumentContext context = new DocumentContext(document.AcDocument))
            {
                U aeccObject = context.Transaction
                    .GetObject(id, OpenMode.ForWrite) as U;
                return creator(aeccObject);
            }
        }
    }
}
