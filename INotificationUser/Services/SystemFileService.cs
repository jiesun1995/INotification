using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Guids;

namespace INotificationUser.Services
{
    public class SystemFileService:ApplicationService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IBlobContainer _blobContainer;

        public SystemFileService(IGuidGenerator guidGenerator, IBlobContainerFactory containerFactory)
        {
            _guidGenerator = guidGenerator;
            _blobContainer = containerFactory.Create("my-file-container");
        }

        [SwaggerOperation(Summary = "上传")]
        public async Task<string> Upload(IFormFile file)
        {
            try
            {
                string extension = Path.GetExtension(file.FileName!);
                var id = _guidGenerator.Create();
                var fileName = $"{id}{extension}";
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] content = memoryStream.ToArray();
                    await _blobContainer.SaveAsync(fileName, content, true);
                }
                return $"/api/app/system-file/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        [SwaggerOperation(Summary = "获取")]
        [Route("/api/app/system-file/{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            var id = Path.GetFileNameWithoutExtension(fileName);
            if (id == null) throw new  Exception($"请确认文件{fileName}");
            var content = await _blobContainer.GetAllBytesOrNullAsync(fileName);
            //var stream = await _blobContainer.GetAsync(fileName);
            return new FileContentResult(content!, GetContentType(fileName));
        }

        static string GetContentType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLower();

            switch (extension)
            {
                case ".txt":
                    return "text/plain";
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".html":
                case ".htm":
                    return "text/html";
                // 你可以继续添加更多的case来处理其他类型的文件  
                default:
                    return "application/octet-stream"; // 默认类型，表示二进制数据流  
            }
        }
    }
}
