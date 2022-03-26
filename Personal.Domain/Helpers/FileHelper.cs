using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Personal.Domain.Exceptions;

namespace Personal.Domain.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileHelper(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public bool IsImageValid(string file_name)
        {
            var allowedExtensions = new[] { ".jpeg", ".png", ".jpg",".gif" };
            var extension = Path.GetExtension(file_name).ToLower();
            if (!allowedExtensions.Contains(extension))
                return false;
            return true;
        }

        public bool IsExcelFileValid(string file_name)
        {
            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var extension = Path.GetExtension(file_name).ToLower();
            if (!allowedExtensions.Contains(extension))
                return false;
            return true;
        }

        public async Task<string> SaveImageAndGetFileName(IFormFile file, string destination_folder, string file_prefix = "")
        {
            if (!IsImageValid(file.FileName))
            {
                throw new CustomException("invalid Document format. Document must be an image.");
            }

            //if (!isImageSizeLessThan1Mb(file))
            //    throw new CustomException("Image size must be less than 1 Mb.");
            Random random = new Random();

            string file_name = "";
            if (string.IsNullOrWhiteSpace(file_prefix))
            {
                file_name = Path.GetFileNameWithoutExtension(file.FileName) + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }
            else
            {
                file_name = file_prefix + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }

            var filePath = Path.Combine(destination_folder, file_name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await  file.CopyToAsync(stream);
            }
            return file_name;
        }

        public void ImageResize(string input_image_path, string output_image_path, int new_width)
        {
            const long quality = 50L;
            Bitmap source_Bitmap = new Bitmap(input_image_path);
            double dblWidth_origial = source_Bitmap.Width;
            double dblHeigth_origial = source_Bitmap.Height;
            double relation_heigth_width = dblHeigth_origial / dblWidth_origial;

            int new_Height = (int)(new_width * relation_heigth_width);

            var new_DrawArea = new Bitmap(new_width, new_Height);
            using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))
            {
                graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;

                graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;
                //*imports the image into the drawarea

                graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, new_width, new_Height);
                //--< Output as .Jpg >--

                using (var output = System.IO.File.Open(output_image_path, FileMode.Create))
                {
                    //< setup jpg >
                    var qualityParamId = Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1);

                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    //< save Bitmap as Jpg >

                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    new_DrawArea.Save(output, codec, encoderParameters);
                    output.Close();
                }
                graphic_of_DrawArea.Dispose();
            }
            source_Bitmap.Dispose();
        }

        public bool IsImageSizeLessThan1Mb(IFormFile file)
        {
            if (file != null)
            {
                int maxFileSize = 1024 * 1024;
                if (file.Length <= maxFileSize)
                    return true;
            }
            return false;
        }

        public string GetFileName(IFormFile file, string file_prefix = "")
        {
            Random random = new Random();
            string file_name = "";
            if (string.IsNullOrWhiteSpace(file_prefix))
            {
                file_name = Path.GetFileNameWithoutExtension(file.FileName) + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }
            else
            {
                file_name = file_prefix + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }

            return file_name;
        }

        public string MoveImageFromTempPathToDestination(string image_name, string destination_folder)
        {
            var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, destination_folder);

            bool doDestinationDirectoryExists = System.IO.Directory.Exists(destinationPath);

            if (!doDestinationDirectoryExists)
                System.IO.Directory.CreateDirectory(destinationPath);

            var tempFolder = Path.GetTempPath();

            File.Move($@"{tempFolder}/{image_name}", $@"{destinationPath}/{image_name}");

            return $@"destinationPath/{image_name}";

        }

        public void DeleteFile(string destination_folder, string file_name)
        {
            var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, destination_folder);

            var filePath = Path.Combine(destinationPath, file_name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
