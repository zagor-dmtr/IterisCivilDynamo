using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace Iteris.Civil.Dynamo.Alignments
{
    /// <summary>
    /// Данные о кривой трассы не добавленного в библиотеку типа
    /// </summary>
    [RegisterForTrace]
    public class AlignmentAnyCurve : AlignmentCurve
    {
        internal AlignmentAnyCurve(C3dDb.AlignmentCurve curve) : base(curve)
        {
        }
    }
}
