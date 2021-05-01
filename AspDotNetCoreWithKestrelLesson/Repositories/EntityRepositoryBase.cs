using AspDotNetCoreWithKestrelLesson.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreWithKestrelLesson.Repositories
{
	public class EntityRepositoryBase<T> : IEntityRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;

		public EntityRepositoryBase(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<T> Add(T model)
		{
			var result = await _dbContext.AddAsync(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> Get(int id) => await _dbContext.FindAsync<T>(id);

		public async Task<IEnumerable<T>> Get() => _dbContext.Set<T>().AsEnumerable();

		public async Task<T> Update(T model)
		{
			var result = _dbContext.Update(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> Delete(T model)
		{
			var result = _dbContext.Remove(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}
	}
}