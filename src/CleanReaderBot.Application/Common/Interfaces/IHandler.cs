using System.Threading.Tasks;

namespace CleanReaderBot.Application.Common.Interfaces
{
    public interface IHandler<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult> {
         Task<TQueryResult> Execute(TQuery query);
    }
}