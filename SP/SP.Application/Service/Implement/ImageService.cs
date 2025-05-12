using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.ImageDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;
using SP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = SP.Domain.Entity.Image;

namespace SP.Application.Service.Implement
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ImageFileDto> DownloadFile(int id)
        {
            var result = await _unitOfWork.ImageRepository.GetByIdAsync(id);
            if (result != null)
            {
                return new ImageFileDto
                {                
                    FileName = result.FileName,
                    FileData = result.FileData,
                    ProductVariantId = result.ProductVariantId,
                    ContentType = result.ContentType
                };
            }
            return null;         
        }
        public async Task<List<ImageFileDto>> GetAllFileAsync()
        {
            var files = await _unitOfWork.ImageRepository.GetAllFileAsync();
            return files.Select(f => new ImageFileDto
            {              
                FileName = f.FileName,
                FileData = f.FileData,
                ContentType = f.ContentType,
            }).ToList();

        }
        public async Task UploadFileAsync( IFormFile formFile, int productVariantId)
        {
         
            if (formFile == null || formFile.Length == 0)
            {
                throw new Exception("No file uploaded");
            }
     
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

       
            var imageData = new Image
            {
                ProductVariantId = productVariantId, 
                FileName = formFile.FileName,
                FileData = memoryStream.ToArray(),
                ContentType = formFile.ContentType
            };
           
            await _unitOfWork.ImageRepository.AddAsync(imageData);
            await _unitOfWork.SaveChangeAsync();
        }

    }
}
