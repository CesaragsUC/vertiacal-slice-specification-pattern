using MongoDB.Driver;
namespace Application.Specifications.MongoDb;
public abstract class MongoSpecification<T>{
    public FilterDefinition<T> Filter {get; protected set;} = Builders<T>.Filter.Empty; 
    public SortDefinition<T>? Sort {get; protected set;} 
    public int? Skip {get; protected set;}
    public int? Take {get; protected set;}
}