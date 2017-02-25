using SW.Core.DAL;
using SW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models
{
    public class AccessrightModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AccessrightRightModel> Accessright_Rights { get; set; }

        public AccessrightModel()
        {
            Accessright_Rights = new List<AccessrightRightModel>();
        }

        public static List<AccessrightModel> MapAccessrights(List<Accessright> accessrights, bool deepLoad)
        {
            List<AccessrightModel> accessrightModels = new List<AccessrightModel>();
            accessrights.ForEach(a => accessrightModels.Add(MapAccessright(a, deepLoad)));
            return accessrightModels;
        }

        public static List<AccessrightModel> MapAccessrights(List<Accessright> accessrights)
        {
            List<AccessrightModel> accessrightModels = new List<AccessrightModel>();
            accessrights.ForEach(a => accessrightModels.Add(MapAccessright(a)));
            return accessrightModels;
        }

        public static AccessrightModel MapAccessright(Accessright accessright)
        {
            return new AccessrightModel()
            {
                Id = accessright.ID,
                Description = accessright.Description,
                Name = accessright.Name
            };
        }

        public static AccessrightModel MapAccessright(Accessright accessright, bool deepLoad)
        {
            AccessrightModel model = new AccessrightModel()
            {
                Id = accessright.ID,
                Description = accessright.Description,
                Name = accessright.Name
            };

            foreach (var right in accessright.Accessright_Right)
            {
                model.Accessright_Rights.Add(new AccessrightRightModel()
                {
                    AccessType = (AccessType)right.AccessType,
                    AccessTypeRight = (AccessTypeRight)right.AccessTypeRight,
                    Id = right.Id
                });
            }

            return model;
        }

        public static Accessright MapModelToAccessright(AccessrightModel model)
        {
            Accessright accessright = new Accessright()
            {
                ID = model.Id,
                Description = model.Description,
                Name = model.Name,
                Accessright_Right = new List<Accessright_Right>()
            };

            model.Accessright_Rights.ForEach(a => accessright.Accessright_Right.Add(new Accessright_Right()
            {
                Id = a.Id,
                AccessType = (int)a.AccessType,
                AccessTypeRight = (int)a.AccessTypeRight,
                AccessrightId = model.Id
            }));

            return accessright;
        }
    }
}