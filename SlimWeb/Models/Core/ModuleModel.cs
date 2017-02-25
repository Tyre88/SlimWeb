using SW.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Core
{
    public class ModuleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AccessTypeId { get; set; }

        public ModuleModel() { }
        public ModuleModel(Module module)
        {
            Id = module.Id;
            Name = module.Name;
            Description = module.Description;
            AccessTypeId = module.AccessTypeId;
        }

        internal static List<ModuleModel> MapModules(List<Module> modules)
        {
            List<ModuleModel> models = new List<ModuleModel>();
            modules.ForEach(m => models.Add(new ModuleModel(m)));
            return models;
        }
    }
}