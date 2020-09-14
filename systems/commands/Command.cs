using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class Command
    {
        Delegate command;
        object[] commandArgs;
        public bool IsSet { get; private set; } = false;

        public void SetCommand(Delegate _command, params object[] _commandArgs)
        {
            command = _command;
            commandArgs = _commandArgs;
            IsSet = true;
        }

        public void Execute()
        {
            command.DynamicInvoke(commandArgs);
            IsSet = false;
        }
    }
}