namespace ReaderBot.Webhooks.Services
{
    public interface ISpecificBotService<TClientType, TSettingsType> : IBotService
    {
         TClientType Client { get; }
         TSettingsType Settings { get; }
    }
}