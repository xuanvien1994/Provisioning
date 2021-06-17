using System;
using System.Collections.Generic;
using System.Text;

namespace Covergo.AADProvisioning.Domain.User
{
    public class Individual : Entity
    {
        public string Salutation { get; set; }

        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string ChineseFirstName { get; set; }
        public string ChineseLastName { get; set; }
        public string OtherName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string CountryOfResidency { get; set; }

        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public string PreferredCommunicationChannel { get; set; }
        public string PreferredLanguage { get; set; }
        public Range IncomeRange { get; set; }
        public IndividualTypes Type { get; set; }

        public bool? AcceptsMarketing { get; set; }
    }

    public class CreateIndividualCommand : CreateEntityCommand
    {
        public string Salutation { get; set; }
        public string Gender { get; set; }
        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string ChineseFirstName { get; set; }
        public string ChineseLastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string CountryOfResidency { get; set; }
        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public string PreferredCommunicationChannel { get; set; }
        public string PreferredLanguage { get; set; }
        public Range IncomeRange { get; set; }
        public IndividualTypes Type { get; set; }

        public bool? AcceptsMarketing { get; set; }
    }

    public enum IndividualTypes
    {
        Undefined,
        Customer,
        Lead,
    }

    public class UpdateIndividualCommand : UpdateEntityCommand
    {
        public string Salutation { get; set; }
        public bool IsSalutationChanged { get; set; }
        public string Gender { get; set; }
        public bool IsGenderChanged { get; set; }
        public string EnglishFirstName { get; set; }
        public bool IsEnglishFirstNameChanged { get; set; }
        public string EnglishLastName { get; set; }
        public bool IsEnglishLastNameChanged { get; set; }
        public string ChineseFirstName { get; set; }
        public bool IsChineseFirstNameChanged { get; set; }
        public string ChineseLastName { get; set; }
        public bool IsChineseLastNameChanged { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsDateOfBirthChanged { get; set; }
        public string CountryOfResidency { get; set; }
        public bool IsCountryOfResidencyChanged { get; set; }
        public string EmploymentStatus { get; set; }
        public bool IsEmploymentStatusChanged { get; set; }
        public string Occupation { get; set; }
        public bool IsOccupationChanged { get; set; }
        public string MaritalStatus { get; set; }
        public bool IsMaritalStatusChanged { get; set; }
        public string PreferredCommunicationChannel { get; set; }
        public bool IsPreferredCommunicationChannelChanged { get; set; }
        public string PreferredLanguage { get; set; }
        public bool IsPreferredLanguageChanged { get; set; }
        public Range IncomeRange { get; set; }
        public bool IsIncomeRangeChanged { get; set; }
        public bool? AcceptsMarketing { get; set; }
        public bool IsAcceptsMarketingChanged { get; set; }
        public IndividualTypes Type { get; set; }
        public bool IsTypeChanged { get; set; }
    }

    public class Range
    {
        public double Minimum { get; set; } = double.NegativeInfinity;
        public double Maximum { get; set; } = double.PositiveInfinity;
        public bool ExclusiveMinimum { get; set; }
        public bool ExclusiveMaximum { get; set; }
    }
}
