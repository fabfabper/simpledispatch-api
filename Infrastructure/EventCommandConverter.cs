using SimpleDispatch.SharedModels.Commands;
using SimpleDispatch.SharedModels.CommandTypes;
using SimpleDispatch.SharedModels.Dtos;

namespace SimpleDispatch.Infrastructure
{
    public static class EventCommandConverter
    {
        public static EventCommand ConvertToCommand(Event eventDto, EventCommandType commandType)
        {
            var command = new EventCommand
            {
                Id = Guid.NewGuid(),
                Payload = eventDto,
                Command = commandType,
            };

            return command;
        }
    }
}