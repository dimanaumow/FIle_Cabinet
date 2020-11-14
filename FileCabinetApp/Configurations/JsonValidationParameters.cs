using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Configurations
{
    public class JsonValidationParameters
    {
        public int FirstNameMaxLenght { get; set; }

        public int FirstNameMinLength { get; set; }

        public int LastNameMaxLength { get; set; }

        public int LastNameMinLength { get; set; }

        public DateTime DateOfBirthFrom { get; set; }

        public DateTime DateOfBirthTo { get; set; }

        public short ExperienceMaxValue { get; set; }

        public short ExperienceMinValue { get; set; }

        public int BalanceMinValue { get; set; }

        public string EnglishLevels { get; set; }
    }
}
