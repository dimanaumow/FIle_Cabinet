using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public static class ValidatorExtention
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
            =>
            builder
                .ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
                .Create();

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
            =>
            builder
                .ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
                .ValidateExpirience(0, 10)
                .ValidateBalance(0)
                .ValidateNationality()
                .Create();
    }
}
