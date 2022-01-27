#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryAPI.Data;
using CountryAPI.Models;
using System.IO;

namespace CountryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly DataContext _context;
        public static IWebHostEnvironment _webHostEnvironment;

        public FilesController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost("upload")]
        public async Task<ActionResult<Files>> UploadFile([FromForm] FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.files.Length > 0)
                {
                    Guid ref_file = Guid.NewGuid();

                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    Console.WriteLine(path);
                    var fileName = Path.GetFileName(fileUpload.files.FileName);
                    var fileExtension = Path.GetExtension(fileUpload.files.FileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + ref_file.ToString() + fileExtension))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    var objFiles = new Files()
                    {
                        Name = fileName,
                        Extention = fileExtension,
                        ref_assignto = fileUpload.ref_assignto,
                        ref_file = ref_file,
                        CreatedOn = DateTime.Now,
                        ContentType = fileUpload.files.ContentType,
                        FileName = ref_file.ToString() + fileExtension
                    };
                    _context.Files.Add(objFiles);
                    await _context.SaveChangesAsync();
                    return objFiles;
                }
                else
                {
                    return BadRequest("Failed upload");

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            /* if (files != null)
             {
                 if (files.Length > 0)
                 {
                     var fileName = Path.GetFileName(files.FileName);
                     var fileExtension = Path.GetExtension(fileName);
                     var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                     var objFiles = new Files()

                     {
                         Name = newFileName,
                         FileType = fileExtension,
                         ref_assignto = Guid.NewGuid(),
                         ref_file = Guid.NewGuid(),
                         CreatedOn = DateTime.Now,
                     };

                     _context.Files.Add(objFiles);
                     await _context.SaveChangesAsync();
                     return Ok(objFiles);
                 }
             }
             return BadRequest("File not found");*/
        }

        [HttpGet("uploads")]
        public async Task<ActionResult<string>> GetFile(string fileName)
        {
            try
            {
                var file = await _context.Files.Where(e => e.FileName == fileName).FirstAsync();
                if(file == null)
                    return NotFound("File Not exist");
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                var filePath = path + file.ref_file.ToString() + file.Extention;
                if (System.IO.File.Exists(filePath))
                {
                    byte[] b = System.IO.File.ReadAllBytes(filePath);
                    return File(b, file.ContentType);
                }
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("delete")]
        public async Task<ActionResult<string>> deleteFile(FileDbo fileDbo)
        {
            try
            {
                var file = await _context.Files.Where(e => e.ref_file == fileDbo.ref_file && e.ref_assignto == fileDbo.ref_assignto).FirstAsync();
                if (file == null)
                    return NotFound("File Not exist");
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                var filePath = path + file.ref_file.ToString() + file.Extention;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
                return Ok("Succesfully Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<Files>>> listFile(FileListDbo fileDbo)
        {
            try
            {
                return await _context.Files.Where(e => e.ref_assignto == fileDbo.ref_assignto).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
