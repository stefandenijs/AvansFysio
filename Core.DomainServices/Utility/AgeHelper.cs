using System;

namespace Core.DomainServices.Utility
{
    public class AgeHelper : IAgeHelper
    {
        public bool CheckAge(DateTime birthDate)
        {
            var age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.AddYears(-age) > birthDate)
            {
                age++;
            }

            return age >= 16;
        }

        public int Age(DateTime birthDate)
        {
            var age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.AddYears(-age) > birthDate)
            {
                age++;
            }

            return age;
        }
    }
}
