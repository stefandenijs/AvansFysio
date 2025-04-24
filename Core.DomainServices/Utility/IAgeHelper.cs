using System;

namespace Core.DomainServices.Utility
{
    public interface IAgeHelper
    {
        public bool CheckAge(DateTime birthDate);
        public int Age(DateTime birthDate);
    }
}
