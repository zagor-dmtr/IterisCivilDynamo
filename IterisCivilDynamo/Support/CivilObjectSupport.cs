﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DynamoApp.Services;
using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.Civil.DynamoNodes;
using Autodesk.DesignScript.Runtime;
using System;
using AeccEntity = Autodesk.Civil.DatabaseServices.Entity;

namespace IterisCivilDynamo.Support
{
    [SupressImportIntoVM]
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

        public static void CheckForReference(AeccEntity entity)
        {
            if (entity.IsReferenceObject || entity.IsReferenceSubObject)
            {
                throw new InvalidOperationException("Object is referenced and cannot be changed!");
            }
        }        
    }
}
