using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mediabank.DAL
{
    internal class MediabankDAL
    {
        internal static IList<MediabankFile> GetAllFiles(int clubId)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                return db.MediabankFile.Where(m => m.ClubId == clubId).ToList();
            }
        }

        internal static IList<MediabankFile> GetAllFilesWithType(int clubId, string fileType)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                return db.MediabankFile.Where(m => m.ClubId == clubId && m.FileType == fileType).ToList();
            }
        }

        internal static MediabankFile GetFile(int clubId, int fileId)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                return db.MediabankFile.Where(m => m.ClubId == clubId && m.Id == fileId).FirstOrDefault();
            }
        }

        internal static MediabankFile SaveFile(MediabankFile mbFile)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                try
                {
                    db.MediabankFile.Add(mbFile);
                    db.SaveChanges();
                    return mbFile;
                }
                catch (Exception ex)
                {
                    LogHelper.LogError($"", ex, mbFile.ClubId);
                }

                return null;
            }
        }

        internal static bool UpdateMediabankFile(MediabankFile mediabankFile)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                try
                {
                    db.Entry(mediabankFile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.LogError($"", ex, mediabankFile.ClubId);
                }

                return false;
            }
        }

        internal static bool DeleteMediabankFile(MediabankFile mediabankFile)
        {
            using (MediabankEntities db = new MediabankEntities())
            {
                try
                {
                    db.Entry(mediabankFile).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.LogError($"", ex, mediabankFile.ClubId);
                }

                return false;
            }
        }
    }
}
