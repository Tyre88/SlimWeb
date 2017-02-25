using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SW.Core.DAL;

namespace SlimWeb.Models.Core
{
    public class ModuleLinkModel
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Sref { get; set; }
        public string Text { get; set; }
        public int AccessType { get; set; }
        public int AccessTypeRight { get; set; }
        public bool IsAdminLink { get; set; }

        public ModuleLinkModel() { }
        public ModuleLinkModel(ModuleLink link)
        {
            Id = link.Id;
            ModuleId = link.ModuleId;
            Sref = link.Sref;
            Text = link.Text;
            AccessType = link.AccessType;
            AccessTypeRight = link.AccessTypeRight;
            IsAdminLink = link.IsAdminLink;
        }

        internal static List<ModuleLinkModel> MapModuleLinks(List<ModuleLink> list)
        {
            List<ModuleLinkModel> links = new List<ModuleLinkModel>();
            list.ForEach(l => links.Add(new ModuleLinkModel(l)));
            return links;
        }
    }
}