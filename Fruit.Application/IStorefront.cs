namespace fruit_api.Application;

public interface IStorefront
{
    Task<string?> Read(string kind, string id, CancellationToken token);
    Task<string> ReadCatalog(CancellationToken token);
    Task<object> ReadNews(int page, int size, CancellationToken token);
}
