
namespace Core.Common.Contracts
{
    public interface IParentableEntity<T> where T : class
    {
        int? Id_Parent { get; set; }
        T Parent { get; set; }
    }
}
