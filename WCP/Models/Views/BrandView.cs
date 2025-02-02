﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Entities;

namespace WCPShared.Models.Views
{
    public class BrandView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public OrganizationView Organization { get; set; }

        public BrandView(Brand obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            URL = obj.URL;
            OrganizationId = obj.OrganizationId;
        }
    }
}
