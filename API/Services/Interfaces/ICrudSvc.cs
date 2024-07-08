namespace API.Services.Interfaces
{
    public interface IReadable<T>
    {
        Task<IEnumerable<T>> ReadDatas();
    }
    public interface IAddable<T>
    {
        Task<bool> AddNewData(T entity);
    }
    public interface IEditable<T>
    {
        Task<bool> EditData(T entity);
    }
    public interface IDeletable<Type, Entity>
    {
        Task<bool> DeleteData(Type key);
    }
    public interface IReadableHasWhere<Key, Result>
    {
        Task<IEnumerable<Result>> ReadDatasHasW(Key key);
    }
}
