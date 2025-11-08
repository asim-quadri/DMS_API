using ComplianceAPI.Models;
using ComplianceAPI.Repository;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowSpecificOrigin")]


    public class FileUploadController : ControllerBase
    {
        //private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");


        private readonly IFileUploadService _fileService;
        private readonly IConfiguration _configuration;
        private readonly string _fileUrl;
        private readonly IDmsService _dmsService;
        public FileUploadController(IFileUploadService fileService, IConfiguration configuration, IDmsService dmsService)
        {
            _dmsService = dmsService;
            _fileService = fileService;
            _configuration = configuration;
            // Check if the application is in Development or Production mode
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            _fileUrl = isDevelopment
                ? _configuration["FileUpload:DevelopmentUrl"]
                : _configuration["FileUpload:ProductionUrl"];
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            _fileService = fileService;

        }
        [HttpGet("GetComseq")]
        public async Task<ActionResult> GetComseq()
        {
            try
            {
                var comseq = await _dmsService.getcomseqdata();
                if (comseq != null)
                    return Ok(comseq);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("FileUpload")]
        public async Task<ActionResult> Upload(IFormFile file, int folderId, int userId)
        {
            try
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_folderPath, fileName);

                // Ensure the directory exists
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save file details to the database
                FileDetail fileDetail = new FileDetail
                {
                    FileName = fileName,
                    FileType = file.ContentType,
                    //FilePath = $"http://localhost:4200/UploadedFiles/{fileName}",
                    FilePath = $"{_fileUrl}{fileName}",
                    FolderId = folderId,
                    UserId = userId
                };

                var files = await _fileService.SaveFileDetails(fileDetail);
                if (files != null)
                {
                    return Ok(files);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getFiles")]
        public async Task<ActionResult> GetFilesbyFolder(int folderId)
        {
            try
            {
                var files = await _fileService.GetFilesbyFolder(folderId);
                if (files != null && files.Count > 0)
                    return Ok(files);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteFile")]
        public async Task<ActionResult> DeleteFile(int fileId)
        {
            try
            {
                var result = await _fileService.DeleteFile(fileId);
                if (result)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

