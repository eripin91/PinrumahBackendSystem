using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PinBackendSystem.Data;
using PinBackendSystem.Models;
using PinBackendSystem.Services;
using PinBackendSystem.Util;


namespace PinBackendSystem.User.Controllers
{
    public class ListingsController : Controller
    {
        private readonly PinContext _context;
        private IConfiguration Configuration;
        private GeneralFunction _generalFunction;

        public ListingsController(PinContext context, IConfiguration _configuration)
        {
            Configuration = _configuration;
            _context = context;
            _generalFunction = new GeneralFunction();
        }

        // GET: Listings
        public async Task<IActionResult> Index(string propertyType, string CurrentFilter, string searchString, int? pageNumber)
        {            

            int pageSize = 3;
            // Use LINQ to get list of genres.
            IQueryable<string> propertyTypeQuery = from m in _context.Listing
                                                   orderby m.PropertyType
                                                   select m.PropertyType;

            var listings = from m in _context.Listing
                           select m;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(s => s.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                listings = listings.Where(x => x.PropertyType == propertyType);
            }
            
            ViewData["CurrentFilter"] = searchString;

            var listingPropertyTypeVM = new ListingPropertyTypeViewModel
            {
                PropertyType = new SelectList(await propertyTypeQuery.Distinct().ToListAsync()),
                Listings = await PaginatedList<Listing>.CreateAsync(listings.AsNoTracking(), pageNumber ?? 1, pageSize)
            };

            return View(listingPropertyTypeVM);
            //return View(await _context.Listing.ToListAsync());
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing)
        {
            if (ModelState.IsValid && User!=null)
            {
                #region upload file  

                if (listing.File1 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File1.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                    //Resize image
                    //var resizedImage = _generalFunction.ResizeImage(fileData);

                    listing.ImagePath1 = objBlobService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
                }
                if (listing.File2 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File2.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                    //Resize image
                    //var resizedImage = _generalFunction.ResizeImage(fileData);

                    listing.ImagePath2 = objBlobService.UploadFileToBlob(listing.File2.FileName, fileData, listing.File2.ContentType);
                }
                if (listing.File3 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File3.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                    //Resize image
                    //var resizedImage = _generalFunction.ResizeImage(fileData);

                    listing.ImagePath3 = objBlobService.UploadFileToBlob(listing.File3.FileName, fileData, listing.File3.ContentType);
                }
                if (listing.File4 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File4.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                    //Resize image
                    //var resizedImage = _generalFunction.ResizeImage(fileData);

                    listing.ImagePath4 = objBlobService.UploadFileToBlob(listing.File4.FileName, fileData, listing.File4.ContentType);
                }
                if (listing.File5 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File5.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                    //Resize image
                    //var resizedImage = _generalFunction.ResizeImage(fileData);

                    listing.ImagePath5 = objBlobService.UploadFileToBlob(listing.File5.FileName, fileData, listing.File5.ContentType);
                }
                #endregion


                listing.AgentName = User.Identity.Name;
                //replace 0 with 62
                string mobileNoCountryFormat = string.Empty;
                if (User.FindFirst(s => s.Type == "extension_Mobile_No").Value.StartsWith("0"))
                {
                    mobileNoCountryFormat = "62" + User.FindFirst(s => s.Type == "extension_Mobile_No").Value.Substring(1);
                }
                else mobileNoCountryFormat = User.FindFirst(s => s.Type == "extension_Mobile_No").Value;
                listing.AgentMobileNo = mobileNoCountryFormat;
                listing.AgentEmail = User.FindFirst(s => s.Type == "emails").Value;
                listing.CreatedBy = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                listing.CreatedOn = DateTime.UtcNow;
                _context.Add(listing);
                 await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Listing listing)
        {
            if (id != listing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && User!=null)
            {
                try
                {
                    #region upload file  
                    if (listing.File1 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File1.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath1 = objBlobService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
                    }
                    if (listing.File2 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File2.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath2 = objBlobService.UploadFileToBlob(listing.File2.FileName, fileData, listing.File2.ContentType);
                    }
                    if (listing.File3 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File3.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath3 = objBlobService.UploadFileToBlob(listing.File3.FileName, fileData, listing.File3.ContentType);
                    }
                    if (listing.File4 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File4.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath4 = objBlobService.UploadFileToBlob(listing.File4.FileName, fileData, listing.File4.ContentType);
                    }
                    if (listing.File5 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File5.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath5 = objBlobService.UploadFileToBlob(listing.File5.FileName, fileData, listing.File5.ContentType);
                    }
                    #endregion

                    listing.ModifiedBy = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    listing.ModifiedOn = DateTime.UtcNow;
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RumahExists(listing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rumah = await _context.Listing.FindAsync(id);
            _context.Listing.Remove(rumah);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RumahExists(int id)
        {
            return _context.Listing.Any(e => e.Id == id);
        }
    }
}
