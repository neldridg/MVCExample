using System;
using Microsoft.AspNetCore.Http;
namespace Web.Models
{
    public class CatPicViewModel 
    {
        public IFormFile Pic { get; set; }
    }
}