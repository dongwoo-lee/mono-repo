using DevExpress.Data.Extensions;
using log4net.Repository.Hierarchy;
using M30.AudioFile.DAL.Dao;
using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.ParamEntity;
using M30_ManagementControlDAO.WebService;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class ProgramInfomationBll
    {
        private readonly IProgramInfomationDAO _dao;
        private readonly IStudioWebService _studioService;
        private readonly APIDao _api_dao;
        private readonly ILogger<ProgramInfomationBll> _logger;

        public ProgramInfomationBll(IProgramInfomationDAO dao, StudioWebService studioService, APIDao api_dao,ILogger<ProgramInfomationBll> logger)
        {
            _dao = dao;
            //_studioService = new StudioSystemMockup(studioService);
            _studioService = studioService;
            _api_dao = api_dao;
            _logger= logger;
        }
        public ProgramInfomationDTO GetProgramInfomationList(ProgramInfoParamDTO dto)
        {
            var result = new ProgramInfomationDTO();
            result.productIdDetails = new List<ProductIdDetail>();
            var param = new ProgramInfomationParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetMedia(dto.media)
                .SetPgmCode(dto.pgmcode)
                .Build();
            var entity = _dao.GetProgramInfomationList(param);
            var options = _api_dao.GetOptions("S01G06C001");
            var index = options.FindIndex(op => op.Name == "PGM_IMAGE_PATH");

            if (entity.Count > 0 && DateTime.ParseExact(entity[0].STARTDATE, "yyyyMMdd", null)<= DateTime.ParseExact(dto.brddate, "yyyyMMdd", null))
            {
                var studioAssign = _studioService.GetStudioAssign(dto.brddate, dto.brddate, "", "");
                _logger.LogInformation($"SelectAssignedStudio_program : {studioAssign}");
                result = _dao.GetProgramInfomationList(param).Converting(studioAssign, index > 0 ? options[index].Value:"");
            }
            _logger.LogInformation($"SelectAssignedStudio_program_result : {result}");
            return result;
        }
        public void UpdatePgmImgFile(IFormFile file,string pgmcode)
        {
            var options = _api_dao.GetOptions("S01G06C001");
            var index = options.FindIndex(op => op.Name == "PGM_IMAGE_PATH");
            var path = index > 0 ? options[index].Value : "";
            var imgPath = ExtentionsControl.GetpgmImgPath(pgmcode, path);
            if (File.Exists(imgPath))
            {
                File.Delete(imgPath);
            }
            if(!String.IsNullOrEmpty(path)&&file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string newFilePath = Path.Combine(path, pgmcode + fileExtension);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                if(!ResizeImageFile(newFilePath, 400, 226))
                {
                    File.Delete(newFilePath);
                    throw new Exception("ResizeImageFile Error");
                }
            }
        }
        private static bool ResizeImageFile(string imageFilePath, in int newWidth, in int newHeight, in bool keepSizeRatio = true)
        {
            try
            {
                byte[] byteArr = File.ReadAllBytes(imageFilePath);

                using (var stream = new System.IO.MemoryStream(byteArr))
                {
                    Bitmap bitmap = new Bitmap(stream);

                    int applyWidth = newWidth;
                    int applyHeight = newHeight;

                    if (keepSizeRatio)
                    {
                        double percentW = 0;
                        double percentH = 0;
                        double targetPercent = 0;

                        percentW = (double)newWidth / bitmap.Width;
                        percentH = (double)newHeight / bitmap.Height;

                        if (percentW < percentH) targetPercent = percentW;
                        else targetPercent = percentH;

                        applyWidth = (int)(bitmap.Width * targetPercent);
                        applyHeight = (int)(bitmap.Height * targetPercent);

                        if (applyWidth > newWidth) applyWidth = newWidth;
                        if (applyHeight > newHeight) applyHeight = newHeight;
                    }

                    bitmap = ResizeImage(bitmap, applyWidth, applyHeight);
                    bitmap.Save(imageFilePath);
                    bitmap.Dispose();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
