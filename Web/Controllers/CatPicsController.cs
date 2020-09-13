using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web;
using Web.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Web.Models;

namespace Web.Controllers
{
    public class CatPicsController : Controller
    {
        private readonly CatPicContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        // This constructor is called every time you make a request to any of these routes.
        // CatPicContext and IWebHostEnvironment are passed via dependency injection
        // to the constructor as needed. These are configured in Startup.cs.
        public CatPicsController(CatPicContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: CatPics
        public async Task<IActionResult> Index()
        {
            // We need to return the View and pass whatever model we have along.
            // This is good enough here, but we could convert all of these models
            // to our ViewModel if we choose. That is not required here, so we won't
            // for simplicity.
            return View(await _context.CatPics.ToListAsync());
        }

        // GET: CatPics/Create
        public IActionResult Create()
        {
            // We just need an empty form, so just return the View.
            return View();
        }

        // POST: CatPics/Create
        // Notice that we decorated with [HttpPost].
        // This allows us to say that if a POST request comes to the same endpoint, it routes here
        // rather than the GET route above.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatPicViewModel catPic)
        {
            // Use the built in model validation. This looks like magic, but it's built into the 
            // ASP.NET Controller class as part of the Controller class you inherit from.
            if (ModelState.IsValid)
            {
                string uniqueFileName = "";
                // If the uploaded file is not null
                if (catPic.Pic != null) 
                {
                    // Get our paths to save the file off to. We're just copying this picture to a directory
                    // and then saving the file path to the database for easy calling later.
                    // This could also be done by saving the file to the database as a 'blob' and retrieving
                    // it, but that would not allow you to render as neatly and efficiently on the web.
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "-" + catPic.Pic.FileName ;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Working with streams is just a part of C#, but because it implements IDisposable,
                    // it looks very similar to with open syntax of Python.
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) 
                    {
                        catPic.Pic.CopyTo(fileStream);
                    }
                }

                // This is the Entity Framework CatPic object.
                CatPic pic = new CatPic 
                {
                    // Notice that we are not adding an Id field.
                    // That will be handled entirely by the database and Entity Framework
                    // so that we aren't trying to generate unique identifiers outside of the database.
                    Pic = uniqueFileName,
                    UploadedDate = DateTime.UtcNow
                };

                // LINQ syntax for pushing the newly created picture to the database.
                _context.Add(pic);

                // Entity Framework requires that you "Save" or commit the changes to the database.
                await _context.SaveChangesAsync();

                // When we finish with our data work, redirect back to the CatPics Index route so we can see our list.
                // You can redirect or return any View you want here. You could even make it redirect you to another 
                // website if needed.
                return RedirectToAction(nameof(Index));
            }

            // If for some reason this has failed to work, just return us back to the form.
            return View(catPic);
        }
    }
}
