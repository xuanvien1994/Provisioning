using System;
using System.Collections.Generic;
using System.Text;

namespace Covergo.AADProvisioning.Domain.User
{
    public class Internal : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string ChineseFirstName { get; set; }
        public string ChineseLastName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateInternalCommand : CreateEntityCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string ChineseFirstName { get; set; }
        public string ChineseLastName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateInternalCommand : UpdateEntityCommand
    {
        public string Title { get; set; }
        public bool IsTitleChanged { get; set; }
        public string Description { get; set; }
        public bool IsDescriptionChanged { get; set; }
        public string EnglishFirstName { get; set; }
        public bool IsEnglishFirstNameChanged { get; set; }
        public string EnglishLastName { get; set; }
        public bool IsEnglishLastNameChanged { get; set; }
        public string ChineseFirstName { get; set; }
        public bool IsChineseFirstNameChanged { get; set; }
        public string ChineseLastName { get; set; }
        public bool IsChineseLastNameChanged { get; set; }
        public string Gender { get; set; }
        public bool IsGenderChanged { get; set; }
        public bool IsActive { get; set; }
        public bool IsIsActiveChanged { get; set; }
    }
}
