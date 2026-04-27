namespace Interfaces;

public record ControlMessage(ControlMessageCommands CommandName);

public enum ControlMessageCommands
{
    Colder,
    Warmer
}
