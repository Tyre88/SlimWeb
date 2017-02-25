using SW.Entities.Mediabank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mediabank.Interfaces
{
    public interface IMediabank
    {
        IList<MediabankEntity> GetAllFiles(int clubId);
        IList<MediabankEntity> GetAllFilesWithType(int clubId, string fileType);
        MediabankEntity GetFile(int clubId, int fileId);
        MediabankEntity SaveFile(byte[] fileData, string originalFileName, MediabankEntity file);
        bool UpdateMediabankFile(MediabankEntity file);
        bool DeleteMediabankFile(int fileId, int clubId);
    }
}
