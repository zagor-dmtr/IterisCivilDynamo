using DynamoServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisAlignment
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
