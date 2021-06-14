using ESchool.AppDbContext;
using ESchool.Models.EduMaterial;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Controllers
{
    public class PdfMaterialsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly ESchoolDbContext _context;

        public PdfMaterialsController(IWebHostEnvironment hostingEnv, ESchoolDbContext context)
        {
            _hostingEnv = hostingEnv;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.PdfMaterials.ToListAsync());
        }

        // GET: Questions/Create
        public IActionResult AddNewPdf()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewPdf(PdfMaterialViewModel pdfmaterialview)
        {
            if (pdfmaterialview.File != null)
            {
                //upload files to wwwroot
                var fileName = Path.GetFileName(pdfmaterialview.File.FileName);
                //judge if it is pdf file
                string ext = Path.GetExtension(pdfmaterialview.File.FileName);
                if (ext.ToLower() != ".pdf")
                {
                    return View();
                }
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "materials/pdfs", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await pdfmaterialview.File.CopyToAsync(fileSteam);
                }
                //your logic to save filePath to database, for example

                PdfMaterial pdfmaterial = new PdfMaterial();
                pdfmaterial.Title = pdfmaterialview.Title;
                pdfmaterial.FilePath = filePath;

                _context.PdfMaterials.Add(pdfmaterial);
                await _context.SaveChangesAsync();
            }
            else
            {

            }
            return View();
        }

        //download file
        public IActionResult DownloadEssay(string filePath)
        {

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "myfile.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //For preview pdf and the download it use below code
             //var stream = new FileStream(filePath, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
        }
    }
}
