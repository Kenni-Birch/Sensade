namespace Interface
{
    public interface IDaoAsync<T>
    {
        public Task<int> AddAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(int id);
        public Task<T> GetAsync(int id);
        public Task<T> GetAllAsync();
    }
}