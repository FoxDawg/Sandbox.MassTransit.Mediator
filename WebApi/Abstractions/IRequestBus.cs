using System.Threading.Tasks;

namespace WebApi.Abstractions;

public interface IRequestBus
{
    Task<RequestResult<TResult>> GetResponse<TRequest, TResult>(TRequest message)
        where TRequest : class where TResult : class;
}