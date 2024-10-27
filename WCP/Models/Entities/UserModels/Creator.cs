﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using WCPShared.Models.Entities;
using WCPShared.Interfaces;

namespace WCPShared.Models.Entities.UserModels
{
    public class Creator : IEquatable<Creator?>, IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
        public string SubType { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public List<Language>? Languages { get; set; } = new List<Language>();
        public List<CreatorParticipation> Participations { get; set; } = new List<CreatorParticipation>();
        public string? Gender { get; set; } = string.Empty;

        public bool Validate()
        {
            if (SubType.IsNullOrEmpty())
                return false;

            if (SubType == "UGC" && (string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Gender)))
                return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Creator);
        }

        public bool Equals(Creator? other)
        {
            return other is not null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
