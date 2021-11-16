using System.Drawing;

namespace Karol.Core.WorldElements
{

    /// <summary>
    /// Markiert Elemente die ein anderes beinhalten können...
    /// </summary>
    internal interface IContainer
    {
        /// <summary>
        /// Enthaltenes Element
        /// </summary>
        WorldElement Content { get; set; }

        /// <summary>
        /// Ist der Container leer
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Offset um das der Content versetzt werden soll
        /// </summary>
        Point ContentOffset { get; }
    }
}
