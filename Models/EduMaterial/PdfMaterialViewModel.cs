using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.EduMaterial
{
    public class PdfMaterialViewModel
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
    }
}
