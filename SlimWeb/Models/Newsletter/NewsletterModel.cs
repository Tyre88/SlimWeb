using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Newsletter
{
    public class NewsletterModel
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public static NewsletterModel MapNewsletterModel(SW.Newsletter.DAL.Newsletters newsletter)
        {
            return new NewsletterModel()
            {
                ClubId = newsletter.ClubId,
                Content = newsletter.Content,
                CreatedBy = newsletter.CreatedBy,
                CreatedDate = newsletter.CreatedDate,
                Id = newsletter.Id, 
                IsDeleted = newsletter.IsDeleted,
                Name = newsletter.Name,
                CreatedByName = string.Format("{0} {1}", newsletter.Account.FirstName, newsletter.Account.LastName)
            };
        }

        public static List<NewsletterModel> MapNewsletterModels(List<SW.Newsletter.DAL.Newsletters> newsletters)
        {
            List<NewsletterModel> models = new List<NewsletterModel>();
            newsletters.ForEach(n => models.Add(MapNewsletterModel(n)));
            return models;
        }

        public static SW.Newsletter.DAL.Newsletters MapDal(NewsletterModel model)
        {
            SW.Newsletter.DAL.Newsletters newsletter = new SW.Newsletter.DAL.Newsletters()
            {
                ClubId = model.ClubId,
                Content = model.Content,
                CreatedBy = model.CreatedBy,
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                Name = model.Name,
                CreatedDate = model.CreatedDate
            };

            if (model.Id <= 0)
                model.CreatedDate = DateTime.Now;

            return newsletter;
        }
    }
}