using SimpleDispatch.SharedModels.Commands;
using SimpleDispatch.SharedModels.CommandTypes;
using SimpleDispatch.SharedModels.Dtos;

namespace SimpleDispatch.Infrastructure
{
    public static class UnitCommandConverter
    {
        public static UnitCommand ConvertToCommand(Unit unit, UnitCommandType commandType)
        {
            var command = new UnitCommand
            {
                Id = Guid.NewGuid(),
                Payload = unit,
                Command = commandType,
            };

            return command;
        }
    }
}
