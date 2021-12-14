namespace AspDotNetCoreWithKestrelLesson.Repositories;

public interface IEntityRepository<T>
{
	public Task<T> Add(T model);

	public Task<T> Get(int id);

	public Task<IEnumerable<T>> Get();

	public Task<T> Update(T model);

	public Task<T> Delete(T model);
}