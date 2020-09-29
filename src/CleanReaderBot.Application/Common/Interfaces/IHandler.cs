using System.Threading.Tasks;

namespace CleanReaderBot.Application.Common.Interfaces
{
    public interface IHandler<TMessage, TMessageResult> where TMessage : IMessage<TMessageResult> {
         Task<TMessageResult> Execute(TMessage query);
    }
}