using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoverGo.DomainUtils;
using Covergo.AADProvisioning.Domain.User;

namespace Covergo.AADProvisioning.Domain
{
    public interface IEntityService<T>
        where T : Entity
    {
        Task<IEnumerable<T>> GetAsync(string tenantId, QueryArguments where);
        Task<Result<CreatedStatus>> CreateAsync(string tenantId, CreateIndividualCommand command);

        Task<IEnumerable<Relationships>> GetRelationshipsAsync(string tenantId, QueryArguments queryArguments);
    }

    public class Entity : SystemObject
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class Individual : Entity
    {
        public string PreferredLanguage { get; set; }
    }

    public class IndividualWhere : Where
    {
        public List<IndividualWhere> Or { get; set; }
        public List<IndividualWhere> And { get; set; }

        public IEnumerable<string> Id_in { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int? DateOfBirth_year { get; set; }
        public int? DateOfBirth_month { get; set; }
        public int? DateOfBirth_day { get; set; }
    }

    public class Relationships
    {
        public string EntityId { get; set; }
        public IEnumerable<Link> Links { get; set; }

        public static string ReverseRelationshipType(string type)
        {
            if (type?.StartsWith("opposite of ") ?? false)
                return type.Substring("opposite of ".Length);

            return type switch
            {
                "guardian" => "dependent",
                "dependent" => "guardian",
                "spouse" => "spouse",
                "parent" => "child",
                "child" => "parent",
                "grandparent" => "grandchild",
                "grandchild" => "grandparent",
                "relative" => "relative",
                "domesticPartner" => "domesticPartner",
                "other" => "other",
                "employs" => "worksFor",
                "worksFor" => "employs",
                "advise" => "advisedBy",
                "advisedBy" => "advise",
                "owns" => "ownedBy",
                "ownedBy" => "owns",
                "occupies" => "occupiedBy",
                "occupiedBy" => "occupies",
                "supervise" => "supervisedBy",
                "supervisedBy" => "supervise",
                "sibling" => "sibling",
                null => null,
                _ => $"opposite of {type}",
            };
        }
    }
    public abstract class UpdateEntityCommand
    {
        public string EndorsementId { get; set; } // for endorsements
        public string CommandId { get; set; } // for endorsements
        public DateTime Timestamp { get; set; } // for endorsements
        public string EntityId { get; set; }

        public string Id { get; set; }
        public bool IsIdChanged { get; set; }

        public string NameFormat { get; set; }
        public bool IsNameFormatChanged { get; set; }

        public string InternalCode { get; set; }
        public bool IsInternalCodeChanged { get; set; }

        public string PhotoPath { get; set; }
        public bool IsPhotoPathChanged { get; set; }

        public string Source { get; set; }
        public bool IsSourceChanged { get; set; }

        public string Fields { get; set; }
        public bool IsFieldsChanged { get; set; }
        public string FieldsPatch { get; set; }

        public IEnumerable<string> Tags { get; set; }
        public bool IsTagsChanged { get; set; }

        public string ModifiedById { get; set; }
    }
    public abstract class CreateEntityCommand
    {
        public string NameFormat { get; set; }
        public string CreatedById { get; set; }

        public string InternalCode { get; set; }
        public int? InternalCodeLength { get; set; }

        public string PhotoPath { get; set; }

        public string Source { get; set; }

        public string Fields { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    public class Link
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string TargetId { get; set; }
        public JToken Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public string LastModifiedById { get; set; }
    }

    public class RelationshipWhere : Where
    {
        public IEnumerable<RelationshipWhere> Or { get; set; }
        public IEnumerable<RelationshipWhere> And { get; set; }
        public IEnumerable<string> EntityId_in { get; set; } // checks both source and target
    }
}
