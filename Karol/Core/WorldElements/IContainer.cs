namespace Karol.Core.WorldElements
{
    public interface IContainer
    {
        WorldElement Content { get; set; }
        bool IsEmpty { get; }
    }
}
