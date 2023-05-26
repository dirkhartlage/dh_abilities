using System;

namespace DHAbilities.AbilitySystem
{
    public sealed class IncorrectUsageException : Exception
    {
        public IncorrectUsageException(string s) : base(s)
        {
        }
    }
}