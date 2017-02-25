using SW.Mediabank.Interfaces;
using System.Collections.Generic;
using SW.Entities.Mediabank;
using SW.Mediabank.DAL;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using SW.Core.Helpers;

namespace SW.Mediabank.BLL
{
    public class MediabankBLL : IMediabank
    {
        private readonly List<string> IMAGE_EXTENSTIONS = new List<string>() { ".jpg", ".png", ".gif", ".jpeg" };
        private readonly List<string> VIDEO_EXTENSTIONS = new List<string>() { ".mp4" };
        private readonly List<string> EXCEL_EXTENSTIONS = new List<string>() { ".xls", ".xlsx" };
        private readonly List<string> WORD_EXTENSTIONS = new List<string>() { ".docx ", ".doc", ".odt" };
        private readonly List<string> CSV_EXTENSTIONS = new List<string>() { ".csv" };

        private const int MIN_THUMBNAIL_SIZE = 100;

        public IList<MediabankEntity> GetAllFiles(int clubId)
        {
            return MapMediabankEnties(MediabankDAL.GetAllFiles(clubId));
        }

        public IList<MediabankEntity> GetAllFilesWithType(int clubId, string fileType)
        {
            return MapMediabankEnties(MediabankDAL.GetAllFilesWithType(clubId, fileType));
        }

        public MediabankEntity GetFile(int clubId, int fileId)
        {
            return MapMediabankEntity(MediabankDAL.GetFile(clubId, fileId));
        }

        public MediabankEntity SaveFile(byte[] fileData, string originalFileName, MediabankEntity file)
        {
            file.FileExtension = GetFileExtension(originalFileName);
            file.FileType = GetFileType(file.FileExtension);

            string fileName = $"{Guid.NewGuid().ToString()}{file.FileExtension}";
            string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}Uploads\Mediabank\{file.ClubId}";
            string fullFileName = $@"{directory}\{fileName}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (FileStream fs = new FileStream(fullFileName, FileMode.OpenOrCreate))
            {
                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
            }

            file.FileUrl = $@"/Uploads/Mediabank/{file.ClubId}/{fileName}";

            GenerateThumbnail(file, fullFileName, fileName);

            MediabankFile mbFile = MapMediabankFile(file);

            return MapMediabankEntity(MediabankDAL.SaveFile(mbFile));
        }

        private void GenerateThumbnail(MediabankEntity file, string fullFileName, string fileName)
        {
            if (file.FileType == "IMAGE")
            {
                GenerageImageThumbnail(file, fullFileName, fileName);
            }
            else if (file.FileType == "VIDEO")
            {
                GenerageVideoThumbnail(file, fullFileName, fileName);
            }
        }

        private void GenerageVideoThumbnail(MediabankEntity file, string fullFileName, string fileName, int percentIntoVideo = 50)
        {
            try
            {
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                float thumbnailTimeStart = 0;

                var ffp = new NReco.VideoInfo.FFProbe();
                var videoInfo = ffp.GetMediaInfo(fullFileName);

                thumbnailTimeStart = (float)((videoInfo.Duration.TotalSeconds * percentIntoVideo) / 100);

                string newFullFileName = $@"{fullFileName.Replace(file.FileExtension, string.Empty)}_Thumbnail.jpg";

                ffMpeg.GetVideoThumbnail(fullFileName, newFullFileName, thumbnailTimeStart);

                file.Thumbnail = $@"/Uploads/Mediabank/{file.ClubId}/{fileName.Replace(file.FileExtension, string.Empty)}_Thumbnail.jpg";
            }
            catch (Exception ex)
            {
                LogHelper.LogError($"Error generating video thumbnail.", ex, file.ClubId);
            }
        }

        private void GenerageImageThumbnail(MediabankEntity file, string fullFileName, string fileName)
        {
            Image img = Image.FromFile(fullFileName);

            int newWidth = 0;
            int newHeight = 0;
            double divider = 0;

            if (img.Width > img.Height)
            {
                divider = (img.Height / (double)MIN_THUMBNAIL_SIZE);
            }
            else
            {
                divider = (img.Width / (double)MIN_THUMBNAIL_SIZE);
            }

            newWidth = (int)(img.Width / divider);
            newHeight = (int)(img.Height / divider);

            Image newImage = Resize(img, newWidth, newHeight, RotateFlipType.RotateNoneFlipNone);

            string newFullFileName = $@"{fullFileName.Replace(file.FileExtension, string.Empty)}_Thumbnail{file.FileExtension}";
            newImage.Save(newFullFileName);
            file.Thumbnail = $@"/Uploads/Mediabank/{file.ClubId}/{fileName.Replace(file.FileExtension, string.Empty)}_Thumbnail{file.FileExtension}";
        }

        private MediabankFile MapMediabankFile(MediabankEntity file)
        {
            MediabankFile mbFile = null;

            if(file != null)
            {
                mbFile = new MediabankFile()
                {
                    ClubId = file.ClubId,
                    CreatedBy = file.CreatedById,
                    CreatedDate = DateTime.Now,
                    Description = file.Description,
                    FileExtension = file.FileExtension,
                    FileType = file.FileType,
                    FileUrl = file.FileUrl,
                    Name = file.Name,
                    Thumbnail = file.Thumbnail,
                    Id = file.Id,
                    LimitFileAccess = file.LimitFileAccess
                };
            }

            return mbFile;
        }

        private string GetFileType(string extension)
        {
            if (IMAGE_EXTENSTIONS.Contains(extension.ToLower()))
                return "IMAGE";
            else if (VIDEO_EXTENSTIONS.Contains(extension.ToLower()))
                return "VIDEO";
            else if (EXCEL_EXTENSTIONS.Contains(extension.ToLower()))
                return "EXCEL";
            else if (WORD_EXTENSTIONS.Contains(extension.ToLower()))
                return "WORD";
            else if (CSV_EXTENSTIONS.Contains(extension.ToLower()))
                return "CSV";
            else
                return "UNKNOWN";
        }

        private string GetFileExtension(string originalFileName)
        {
            return System.IO.Path.GetExtension(originalFileName);
        }

        private IList<MediabankEntity> MapMediabankEnties(IList<MediabankFile> list)
        {
            IList<MediabankEntity> entities = new List<MediabankEntity>();
            if(list != null)
            {
                foreach (var entity in list)
                {
                    entities.Add(MapMediabankEntity(entity));
                }
            }
            return entities;
        }

        private MediabankEntity MapMediabankEntity(MediabankFile entity)
        {
            MediabankEntity mediabankEntity = null;
            if (entity != null)
            {
                mediabankEntity = new MediabankEntity()
                {
                    ClubId = entity.ClubId,
                    CreatedById = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate,
                    Description = entity.Description,
                    FileExtension = entity.FileExtension,
                    FileType = entity.FileType,
                    FileUrl = entity.FileUrl,
                    Id = entity.Id,
                    Name = entity.Name,
                    Thumbnail = entity.Thumbnail,
                    LimitFileAccess = entity.LimitFileAccess
                };
            }

            return mediabankEntity;
        }

        private Image Resize(Image image, int width, int height, RotateFlipType rotateFlipType)
        {
            // clone the Image instance, since we don't want to resize the original Image instance
            var rotatedImage = image.Clone() as Image;
            rotatedImage.RotateFlip(rotateFlipType);
            var newSize = CalculateResizedDimensions(rotatedImage, width, height);

            var resizedImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppArgb);
            resizedImage.SetResolution(72, 72);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                // set parameters to create a high-quality thumbnail
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);

                    // draws the resized image to the bitmap
                    graphics.DrawImage(rotatedImage, new Rectangle(new Point(0, 0), newSize), 0, 0, rotatedImage.Width, rotatedImage.Height, GraphicsUnit.Pixel, attribute);
                }
            }

            return resizedImage;
        }
        private Size CalculateResizedDimensions(Image image, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / image.Width;
            var heightScale = (double)desiredHeight / image.Height;

            // scale to whichever ratio is smaller, this works for both scaling up and scaling down
            var scale = widthScale < heightScale ? widthScale : heightScale;

            return new Size
            {
                Width = (int)(scale * image.Width),
                Height = (int)(scale * image.Height)
            };
        }

        public bool UpdateMediabankFile(MediabankEntity file)
        {
            MediabankFile mediabankFile = MapMediabankFile(file);
            return MediabankDAL.UpdateMediabankFile(mediabankFile);
        }

        public bool DeleteMediabankFile(int fileId, int clubId)
        {
            bool success = false;
            MediabankEntity file = GetFile(clubId, fileId);

            if (file != null)
            {
                string physicalPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{file.FileUrl.Replace("/", @"\")}";
                string physicalThumbnailPath = string.Empty;

                if(!string.IsNullOrEmpty(file.Thumbnail))
                {
                    physicalThumbnailPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{file.Thumbnail.Replace("/", @"\")}";
                }

                if (File.Exists(physicalPath))
                    File.Delete(physicalPath);

                if (!string.IsNullOrEmpty(physicalThumbnailPath) && File.Exists(physicalThumbnailPath))
                    File.Delete(physicalThumbnailPath);

                success = MediabankDAL.DeleteMediabankFile(MapMediabankFile(file));
            }

            return success;
        }
    }
}
