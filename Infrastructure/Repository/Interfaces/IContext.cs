using MongoDB.Driver;

namespace Infrastructure.Repository.Interfaces
{
    public interface IContext
    {
        public IMongoCollection<T> DbMongoCollectionSet<T>(string collection);
    }
}