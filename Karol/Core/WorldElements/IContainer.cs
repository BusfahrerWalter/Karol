namespace Karol.Core.WorldElements
{
    internal interface IContainer
    {
        WorldElement Content { get; set; }
        bool IsEmpty { get; }
    }
}
