﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.EduMaterial
{
    public class PdfMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string FilePath { get; set; }

    }
}
