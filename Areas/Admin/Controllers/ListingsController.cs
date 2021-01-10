using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using PinBackendSystem.Areas.Identity;
using PinBackendSystem.Data;
using PinBackendSystem.Models;
using PinBackendSystem.Services;
using PinBackendSystem.Util;

namespace PinBackendSystem.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    
    public class ListingsController : Controller
    {
        private readonly PinContext _context;
        private IConfiguration Configuration;
        private GeneralFunction _generalFunction;
        private UserManager<PinrumahUser> _userManager;
        BlobStorageService _blobStorageService;
        public ListingsController(PinContext context, IConfiguration _configuration, UserManager<PinrumahUser> userManager)
        {
            Configuration = _configuration;
            _context = context;
            _generalFunction = new GeneralFunction();
            _userManager = userManager;
            _blobStorageService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"), Configuration.GetValue<string>("Azure:BlobContainerName"));
        }

        [Authorize(Policy = "RequireSurveyorTeleAdsRole")]
        // GET: Listings
        public async Task<IActionResult> Index(string propertyType, string CurrentFilter, string searchString, int? pageNumber, int status, string searchIdString, string CurrentIdFilter)
        {
            int pageSize = 10;
            // Use LINQ to get list of genres.
            IQueryable<string> propertyTypeQuery = from m in _context.Listings
                                                   orderby m.PropertyType
                                                   select m.PropertyType;

            var listings = from m in _context.Listings.OrderByDescending(s=>s.CreatedOn).ThenBy(s=>!s.ModifiedOn.HasValue).ThenByDescending(s => s.ModifiedOn ?? DateTime.MaxValue).ThenByDescending(s => s.Id)
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
                listings = listings.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
            }

            if (searchIdString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchIdString = CurrentIdFilter;
            }

            if (!string.IsNullOrEmpty(searchIdString))
            {
                listings = listings.Where(s => s.PropertyId.ToUpper().Contains(searchIdString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                listings = listings.Where(x => x.PropertyType == propertyType);
            }

            listings = listings.Where(x => x.Status == status);

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentIdFilter"] = searchIdString;

            var listingPropertyTypeVM = new ListingPropertyTypeViewModel
            {
                PropertyType = new SelectList(await propertyTypeQuery.Distinct().ToListAsync()),
                Listings = await PaginatedList<Listing>.CreateAsync(listings.AsNoTracking(), pageNumber ?? 1, pageSize)
            };

            return View(listingPropertyTypeVM);
            //return View(await _context.Listing.ToListAsync());
        }

        // GET: Listings/Personal
        public async Task<IActionResult> Personal(string propertyType, string CurrentFilter, string searchString, int? pageNumber, string searchIdString, string CurrentIdFilter, int status=0)
        {_userManager.GetUserId(User);
            int pageSize = 10;
            // Use LINQ to get list of genres.
            IQueryable<string> propertyTypeQuery = from m in _context.Listings.Where(s=>s.CreatedBy== _userManager.GetUserId(User))
                                                   orderby m.PropertyType
                                                   select m.PropertyType;

            var listings = from m in _context.Listings.OrderByDescending(s => s.CreatedOn).OrderBy(s => !s.ModifiedOn.HasValue).ThenByDescending(s=>s.ModifiedOn?? DateTime.MaxValue).ThenByDescending(s=>s.Id)
                           .Where(s => s.CreatedBy == _userManager.GetUserId(User))                           
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
                listings = listings.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
            }

            if (searchIdString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchIdString = CurrentIdFilter;
            }

            if (!string.IsNullOrEmpty(searchIdString))
            {
                listings = listings.Where(s => s.PropertyId.ToUpper().Contains(searchIdString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                listings = listings.Where(x => x.PropertyType == propertyType);
            }

            listings = listings.Where(x => x.Status == status);

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentIdFilter"] = searchIdString;

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

            var listing = await _context.Listings
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (listing == null)
            {
                return NotFound();
            }

            listing.Listing_feature = _context.Listing_features.Include(s=>s.Features).Where(s => s.Listings == listing).ToList();

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            var model = new Listing();
            model.TransactionType = "Jual";
            model.PropertyType = "Rumah";
            model.Features = _context.Features.Where(s => s.Status == PinrumahConstants.AdsStatus).ToList();
            
            model.Kotas = _context.Kotas.Where(s => s.Status == PinrumahConstants.AdsStatus).Select(x=>new SelectListItem() { Text=x.KotaName,Value=x.KotaName}).ToList();
            

            return View(model);
        }

        // GET: Listings/Createa
        public IActionResult Createa()
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
            if (ModelState.IsValid && User != null)
            {
                //get user detail
                var user = await _userManager.GetUserAsync(User);

                #region upload file  

                if (listing.File1 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File1.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    listing.ImagePath1 = _blobStorageService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
                }
                if (listing.File6 != null)
                {
                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        listing.File6.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    listing.ImagePath6 = _blobStorageService.UploadFileToBlob(listing.File6.FileName, fileData, listing.File6.ContentType);
                }
                if (listing.FileList != null) { 
                    if (listing.FileList.Count()>0)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.FileList[0].CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath2 = _blobStorageService.UploadFileToBlob(listing.FileList[0].FileName, fileData, listing.FileList[0].ContentType);
                    }
                    if (listing.FileList.Count() > 1)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.FileList[1].CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath3 = _blobStorageService.UploadFileToBlob(listing.FileList[1].FileName, fileData, listing.FileList[1].ContentType);
                    }
                    if (listing.FileList.Count() > 2)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.FileList[2].CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath4 = _blobStorageService.UploadFileToBlob(listing.FileList[2].FileName, fileData, listing.FileList[2].ContentType);
                    }
                    if (listing.FileList.Count() > 3)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.FileList[3].CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath5 = _blobStorageService.UploadFileToBlob(listing.FileList[3].FileName, fileData, listing.FileList[3].ContentType);
                    }
                }
                #endregion


                listing.AgentEmail = user.UserName;

                //replace 0 with 62
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    string mobileNoCountryFormat = string.Empty;

                    if (user.PhoneNumber.StartsWith("0"))
                    {
                        mobileNoCountryFormat = "62" + user.PhoneNumber.Substring(1);
                    }
                    else mobileNoCountryFormat = user.PhoneNumber;

                    listing.AgentMobileNo = mobileNoCountryFormat;
                }
                //property ID substring increment one
                int currentPropertyId = 0;
                var latestListing = GetLatestListing();

                string propertyId = latestListing?.PropertyId?.Substring(2);
                int.TryParse(propertyId, out currentPropertyId);

                //For now only PR so use hard code
                string newPropertyId = "PR" + (currentPropertyId + 1);

                listing.PropertyId = newPropertyId;
                listing.AgentEmail = user.Email;
                listing.CreatedBy = user.Id;
                listing.CreatedOn = DateTime.UtcNow;
                

                foreach (var item in listing.Features.Where(s=>s.Selected))
                {                    
                    Listing_feature listing_feature = new Listing_feature()
                    {
                        FeaturesId = item.Id
                    };
                    listing.Listing_feature.Add(listing_feature);
                }
                
                _context.Add(listing);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Personal));
            }
            return View(listing);
        }

        // POST: Listings/Publish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id,int _status,int _returnStatus)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            listing.Status = _status;

            await Edit(id, listing);

            return RedirectToAction(nameof(Index), new { status= _returnStatus });
        }

        // POST: Listings/Promote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Promote(int id, bool _isFeatured, int _returnStatus)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            listing.IsFeatured = _isFeatured;

            await Edit(id, listing);

            return RedirectToAction(nameof(Index), new { status = _returnStatus });
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            var featureList = _context.Features.Where(s => s.Status == PinrumahConstants.AdsStatus).ToList();
            List<Feature> newFeatureList = new List<Feature>();
            foreach (var item in featureList)
            {
                var listing_feature = _context.Listing_features.FirstOrDefault(s => s.Listings == listing && s.Features==item);
                if (listing_feature != null) item.Selected = true;
                else item.Selected = false;

                newFeatureList.Add(item);
            }

            listing.Features = newFeatureList;
            listing.Kotas = _context.Kotas.Where(s => s.Status == PinrumahConstants.AdsStatus).Select(x => new SelectListItem() { Text = x.KotaName, Value = x.KotaName }).ToList();
            listing.Kecamatans = _context.Kecamatans.Where(s => s.Status == PinrumahConstants.AdsStatus && s.Kota.KotaName==listing.Kota).Select(x => new SelectListItem() { Text = x.KecamatanName, Value = x.KecamatanName }).ToList();
            
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

            if (ModelState.IsValid && User != null)
            {
                try
                {
                    //get user detail
                    var user = await _userManager.GetUserAsync(User);

                    #region upload file  

                    if (listing.File1 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File1.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath1 = _blobStorageService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
                    }

                    if (listing.File2 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File2.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath2 = _blobStorageService.UploadFileToBlob(listing.File2.FileName, fileData, listing.File2.ContentType);
                    }
                    if (listing.File3 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File3.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath3 = _blobStorageService.UploadFileToBlob(listing.File3.FileName, fileData, listing.File3.ContentType);
                    }
                    if (listing.File4 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File4.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath4 = _blobStorageService.UploadFileToBlob(listing.File4.FileName, fileData, listing.File4.ContentType);
                    }
                    if (listing.File5 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File5.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath5 = _blobStorageService.UploadFileToBlob(listing.File5.FileName, fileData, listing.File5.ContentType);
                    }
                    if (listing.File6 != null)
                    {
                        byte[] fileData;

                        await using (var target = new MemoryStream())
                        {
                            listing.File6.CopyTo(target);
                            fileData = target.ToArray();
                        }

                        //Resize image
                        //var resizedImage = _generalFunction.ResizeImage(fileData);

                        listing.ImagePath5 = _blobStorageService.UploadFileToBlob(listing.File6.FileName, fileData, listing.File6.ContentType);
                    }
                    #endregion

                    listing.ModifiedBy = user.Id;
                    listing.ModifiedOn = DateTime.UtcNow;

                    if (listing.Features!=null)
                    {
                        var current_listing_feature = _context.Listing_features.Where(s => s.Listings == listing);
                        foreach (var item in listing.Features.Where(s => s.Selected))
                        {
                            Listing_feature listing_feature = new Listing_feature()
                            {
                                FeaturesId = item.Id
                            };
                            listing.Listing_feature.Add(listing_feature);
                        }

                        _context.RemoveRange(current_listing_feature);
                    }
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

                var returnUrl = HttpContext.Request.Query["returnurl"];
                return RedirectToAction(returnUrl);
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

            var listing = await _context.Listings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }
            listing.Listing_feature = _context.Listing_features.Include(s => s.Features).Where(s => s.Listings == listing).ToList();

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rumah = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(rumah);
            await _context.SaveChangesAsync();
            var returnUrl = HttpContext.Request.Query["returnurl"];
            return RedirectToAction(returnUrl);
        }

        private bool RumahExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }

        public Listing GetLatestListing()
        {
            return _context.Listings.OrderByDescending(s => s.Id).FirstOrDefault();
        }

        public IActionResult GetKecamatan(string kotaName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items = _context.Kecamatans.Where(s => s.Status == PinrumahConstants.AdsStatus && s.Kota.KotaName==kotaName).Select(x => new SelectListItem() { Text = x.KecamatanName, Value = x.KecamatanName }).ToList();

            return Ok(items);
        }
        #region azure ad b2c
        //// POST: Listings/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Listing listing)
        //{
        //    if (ModelState.IsValid && User!=null)
        //    {
        //        #region upload file  

        //        if (listing.File1 != null)
        //        {
        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                listing.File1.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //Resize image
        //            //var resizedImage = _generalFunction.ResizeImage(fileData);

        //            listing.ImagePath1 = objBlobService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
        //        }
        //        if (listing.File2 != null)
        //        {
        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                listing.File2.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //Resize image
        //            //var resizedImage = _generalFunction.ResizeImage(fileData);

        //            listing.ImagePath2 = objBlobService.UploadFileToBlob(listing.File2.FileName, fileData, listing.File2.ContentType);
        //        }
        //        if (listing.File3 != null)
        //        {
        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                listing.File3.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //Resize image
        //            //var resizedImage = _generalFunction.ResizeImage(fileData);

        //            listing.ImagePath3 = objBlobService.UploadFileToBlob(listing.File3.FileName, fileData, listing.File3.ContentType);
        //        }
        //        if (listing.File4 != null)
        //        {
        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                listing.File4.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //Resize image
        //            //var resizedImage = _generalFunction.ResizeImage(fileData);

        //            listing.ImagePath4 = objBlobService.UploadFileToBlob(listing.File4.FileName, fileData, listing.File4.ContentType);
        //        }
        //        if (listing.File5 != null)
        //        {
        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                listing.File5.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //Resize image
        //            //var resizedImage = _generalFunction.ResizeImage(fileData);

        //            listing.ImagePath5 = objBlobService.UploadFileToBlob(listing.File5.FileName, fileData, listing.File5.ContentType);
        //        }
        //        #endregion


        //        listing.AgentName = User.Identity.Name;
        //        //replace 0 with 62
        //        string mobileNoCountryFormat = string.Empty;

        //        if (User.FindFirst(s => s.Type == "extension_Mobile_No").Value.StartsWith("0"))
        //        {
        //            mobileNoCountryFormat = "62" + User.FindFirst(s => s.Type == "extension_Mobile_No").Value.Substring(1);
        //        }
        //        else mobileNoCountryFormat = User.FindFirst(s => s.Type == "extension_Mobile_No").Value;
        //        listing.AgentMobileNo = mobileNoCountryFormat;
        //        listing.AgentEmail = User.FindFirst(s => s.Type == "emails").Value;
        //        listing.CreatedBy = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        //        listing.CreatedOn = DateTime.UtcNow;
        //        _context.Add(listing);
        //         await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(listing);
        //}

        //// POST: Listings/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Listing listing)
        //{
        //    if (id != listing.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid && User != null)
        //    {
        //        try
        //        {
        //            #region upload file  
        //            if (listing.File1 != null)
        //            {
        //                byte[] fileData;

        //                await using (var target = new MemoryStream())
        //                {
        //                    listing.File1.CopyTo(target);
        //                    fileData = target.ToArray();
        //                }

        //                BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //                //Resize image
        //                //var resizedImage = _generalFunction.ResizeImage(fileData);

        //                listing.ImagePath1 = objBlobService.UploadFileToBlob(listing.File1.FileName, fileData, listing.File1.ContentType);
        //            }
        //            if (listing.File2 != null)
        //            {
        //                byte[] fileData;

        //                await using (var target = new MemoryStream())
        //                {
        //                    listing.File2.CopyTo(target);
        //                    fileData = target.ToArray();
        //                }

        //                BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //                //Resize image
        //                //var resizedImage = _generalFunction.ResizeImage(fileData);

        //                listing.ImagePath2 = objBlobService.UploadFileToBlob(listing.File2.FileName, fileData, listing.File2.ContentType);
        //            }
        //            if (listing.File3 != null)
        //            {
        //                byte[] fileData;

        //                await using (var target = new MemoryStream())
        //                {
        //                    listing.File3.CopyTo(target);
        //                    fileData = target.ToArray();
        //                }

        //                BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //                //Resize image
        //                //var resizedImage = _generalFunction.ResizeImage(fileData);

        //                listing.ImagePath3 = objBlobService.UploadFileToBlob(listing.File3.FileName, fileData, listing.File3.ContentType);
        //            }
        //            if (listing.File4 != null)
        //            {
        //                byte[] fileData;

        //                await using (var target = new MemoryStream())
        //                {
        //                    listing.File4.CopyTo(target);
        //                    fileData = target.ToArray();
        //                }

        //                BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //                //Resize image
        //                //var resizedImage = _generalFunction.ResizeImage(fileData);

        //                listing.ImagePath4 = objBlobService.UploadFileToBlob(listing.File4.FileName, fileData, listing.File4.ContentType);
        //            }
        //            if (listing.File5 != null)
        //            {
        //                byte[] fileData;

        //                await using (var target = new MemoryStream())
        //                {
        //                    listing.File5.CopyTo(target);
        //                    fileData = target.ToArray();
        //                }

        //                BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //                //Resize image
        //                //var resizedImage = _generalFunction.ResizeImage(fileData);

        //                listing.ImagePath5 = objBlobService.UploadFileToBlob(listing.File5.FileName, fileData, listing.File5.ContentType);
        //            }
        //            #endregion

        //            listing.ModifiedBy = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        //            listing.ModifiedOn = DateTime.UtcNow;
        //            _context.Update(listing);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RumahExists(listing.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(listing);
        //}
        #endregion
    }
}
