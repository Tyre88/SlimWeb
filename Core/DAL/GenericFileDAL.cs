using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Enums;

namespace SW.Core.DAL
{
    internal static class GenericFileDAL
    {
        internal static GenericFile GetGenericFile(GenericFileType fileType, GenericFileModuleType moduleType, int objectId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.GenericFile.FirstOrDefault(f => f.FileType == (int)fileType
                    && f.ModuleType == (int)moduleType && f.ObjectId == objectId);
        }

        internal static void UpdateGenericFile(GenericFile file)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                coreDAL.Entry(file).State = System.Data.Entity.EntityState.Modified;
                coreDAL.SaveChanges();
            }
        }

        internal static void SaveGenericFile(GenericFile file)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                coreDAL.GenericFile.Add(file);
                coreDAL.SaveChanges();
            }
        }
    }
}
