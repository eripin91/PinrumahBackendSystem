using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PinBackendSystem.Areas.Identity;
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

        // GET: Listings
        public async Task<IActionResult> Index(string sortOrder, 
            string propertyType, string transactionType, string CurrentFilter, string searchString, 
            string minBed, string currentMinBed, string minPrice, string currentMinPrice,
            string maxPrice, string currentMaxPrice, string minLandSize, string currentMinLandSize,
            string maxLandSize, string currentMaxLandSize, string kota, string currentKota,
            string kecamatan, string currentKecamatan, int? pageNumber)
        {            

            int pageSize = 20;

            ViewBag.keywords = "Listing properti Indonesia";
            ViewBag.Description = "Listing rumah,apartemen,ruko,gedung dan tanah di Indonesia";


            ViewData["CurrentSort"] = sortOrder??string.Empty;
            ViewData["CurrentPropertyType"] = propertyType??string.Empty;
            ViewData["CurrentTransactionType"] = transactionType ?? string.Empty;

            // Use LINQ to get list of property type and others.
            IQueryable<string> propertyTypeQuery = from m in _context.Listings.OrderBy(s=>s.Id)
                                                   orderby m.PropertyType
                                                   select m.PropertyType;

            IQueryable<string> kotaQuery = from m in _context.Kotas.OrderBy(s => s.KotaName)
                                           select m.KotaName;

            IQueryable<string> kecamatanQuery = from m in _context.Kecamatans.OrderBy(s => s.Id)
                                                .Where(s => s.Kota.KotaName == kota)
                                                select m.KecamatanName;

            var listings = from m in _context.Listings
                           select m;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (minBed != null)
            {
                pageNumber = 1;
            }
            else
            {
                minBed = currentMinBed;
            }

            ViewData["CurrentMinBed"] = minBed;

            if (minPrice != null)
            {
                pageNumber = 1;
            }
            else
            {
                minPrice = currentMinPrice;
            }

            ViewData["CurrentMinPrice"] = minPrice;

            if (maxPrice != null)
            {
                pageNumber = 1;
            }
            else
            {
                maxPrice = currentMaxPrice;
            }

            ViewData["CurrentMaxPrice"] = maxPrice;

            if (minLandSize != null)
            {
                pageNumber = 1;
            }
            else
            {
                minLandSize = currentMinLandSize;
            }

            ViewData["CurrentMinLandSize"] = minLandSize;

            if (maxLandSize != null)
            {
                pageNumber = 1;
            }
            else
            {
                maxLandSize = currentMaxLandSize;
            }

            ViewData["CurrentMaxLandSize"] = maxLandSize;

            if (kota != null)
            {
                pageNumber = 1;
            }
            else
            {
                kota = currentKota;
            }

            ViewData["currentKota"] = kota;

            if (kecamatan != null)
            {
                pageNumber = 1;
            }
            else
            {
                kecamatan = currentKecamatan;
            }

            ViewData["currentKecamatan"] = kecamatan;

            if (!string.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(s => EF.Functions.Like(s.Title.ToUpper(), $"%{searchString.ToUpper()}%"));
            }

            if (!string.IsNullOrEmpty(minBed))
            {
                int noOfMinBed;
                int.TryParse(minBed, out noOfMinBed);
                listings = listings.Where(s => s.NoOfBed >= noOfMinBed);
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                decimal noOfMinPrice;
                Decimal.TryParse(minPrice, out noOfMinPrice);
                listings = listings.Where(s => s.Price >= noOfMinPrice);
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                decimal noOfMaxPrice;
                Decimal.TryParse(maxPrice, out noOfMaxPrice);
                listings = listings.Where(s => s.Price <= noOfMaxPrice);
            }

            if (!string.IsNullOrEmpty(minLandSize))
            {
                decimal noOfMinLandSize;
                Decimal.TryParse(minLandSize, out noOfMinLandSize);
                listings = listings.Where(s => s.LandSize >= noOfMinLandSize);
            }

            if (!string.IsNullOrEmpty(maxLandSize))
            {
                decimal noOfMaxLandSize;
                Decimal.TryParse(maxLandSize, out noOfMaxLandSize);
                listings = listings.Where(s => s.LandSize <= noOfMaxLandSize);
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                listings = listings.Where(x => x.PropertyType == propertyType);
            }

            if (!string.IsNullOrEmpty(transactionType))
            {
                listings = listings.Where(x => x.TransactionType == transactionType);
            }

            if (!string.IsNullOrEmpty(kota))
            {
                listings = listings.Where(x => x.Kota == kota);
            }

            if (!string.IsNullOrEmpty(kecamatan))
            {
                listings = listings.Where(x => x.Kecamatan == kecamatan);
            }

            //show only published listing
            listings = listings.Where(x => x.Status == PinrumahConstants.AdsStatus);

            

            switch (sortOrder)
            {
                case "date_desc":
                    listings = listings.OrderByDescending(s => s.CreatedOn);
                    break;
                case "price_asc":
                    listings = listings.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    listings = listings.OrderByDescending(s => s.Price);
                    break;
                case "rating_asc":
                    listings = listings.OrderBy(s => s.Rating);
                    break;
                case "rating_desc":
                    listings = listings.OrderByDescending(s => s.Rating);
                    break;
                default:
                    listings = listings.OrderByDescending(s => s.CreatedOn);
                    break;
            }

            var listingPropertyTypeVM = new ListingPropertyTypeViewModel
            {
                PropertyType = new SelectList(await propertyTypeQuery.Where(s=>!string.IsNullOrEmpty(s)).Distinct().ToListAsync()),
                Kota = new SelectList(await kotaQuery.Where(s => !string.IsNullOrEmpty(s)).ToListAsync()),
                Kecamatan = new SelectList(await kecamatanQuery.Where(s => !string.IsNullOrEmpty(s)).ToListAsync()),
                Listings = await PaginatedList<Listing>.CreateAsync(listings.AsNoTracking(), pageNumber ?? 1, pageSize)
            };
            return View(listingPropertyTypeVM);
            // return View(await _context.Listing.ToListAsync());
        }

        public async Task<IActionResult> Home(string sortOrder,
            string propertyType, string transactionType, string CurrentFilter, string searchString,
            string minBed, string currentMinBed, string minPrice, string currentMinPrice,
            string maxPrice, string currentMaxPrice, string minLandSize, string currentMinLandSize,
            string maxLandSize, string currentMaxLandSize, string kota, string currentKota,
            string kecamatan, string currentKecamatan,
            int? pageNumber)
        {

            int pageSize = 20;

            ViewBag.keywords = "Listing properti Indonesia";
            ViewBag.Description = "Listing rumah,apartemen,ruko,gedung dan tanah di Indonesia";


            ViewData["CurrentSort"] = sortOrder ?? string.Empty;
            ViewData["CurrentPropertyType"] = propertyType ?? string.Empty;
            ViewData["CurrentTransactionType"] = transactionType ?? string.Empty;

            // Use LINQ to get list of genres.
            IQueryable<string> propertyTypeQuery = from m in _context.Listings.OrderBy(s => s.Id)
                                                   orderby m.PropertyType
                                                   select m.PropertyType;

            IQueryable<string> kotaQuery = from m in _context.Kotas.OrderBy(s => s.KotaName)
                                           select m.KotaName;

            IQueryable<string> kecamatanQuery = from m in _context.Kecamatans.OrderBy(s => s.Id)
                                                .Where(s=>s.Kota.KotaName==kota)
                                                select m.KecamatanName;

            var listings = from m in _context.Listings
                           select m;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (minBed != null)
            {
                pageNumber = 1;
            }
            else
            {
                minBed = currentMinBed;
            }

            ViewData["CurrentMinBed"] = minBed;

            if (minPrice != null)
            {
                pageNumber = 1;
            }
            else
            {
                minPrice = currentMinPrice;
            }

            ViewData["CurrentMinPrice"] = minPrice;

            if (maxPrice != null)
            {
                pageNumber = 1;
            }
            else
            {
                maxPrice = currentMaxPrice;
            }

            ViewData["CurrentMaxPrice"] = maxPrice;

            if (minLandSize != null)
            {
                pageNumber = 1;
            }
            else
            {
                minLandSize = currentMinLandSize;
            }

            ViewData["CurrentMinLandSize"] = minLandSize;

            if (maxLandSize != null)
            {
                pageNumber = 1;
            }
            else
            {
                maxLandSize = currentMaxLandSize;
            }

            ViewData["CurrentMaxLandSize"] = maxLandSize;

            if (kota != null)
            {
                pageNumber = 1;
            }
            else
            {
                kota = currentKota;
            }

            ViewData["currentKota"] = kota;

            if (kecamatan != null)
            {
                pageNumber = 1;
            }
            else
            {
                kecamatan = currentKecamatan;
            }

            ViewData["currentKecamatan"] = kecamatan;


            if (!string.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(s => EF.Functions.Like(s.Title.ToUpper(), $"%{searchString.ToUpper()}%"));
            }

            if (!string.IsNullOrEmpty(minBed))
            {
                int noOfMinBed;
                int.TryParse(minBed, out noOfMinBed);
                listings = listings.Where(s => s.NoOfBed >= noOfMinBed);
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                decimal noOfMinPrice;
                Decimal.TryParse(minPrice, out noOfMinPrice);
                listings = listings.Where(s => s.Price >= noOfMinPrice);
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                decimal noOfMaxPrice;
                Decimal.TryParse(maxPrice, out noOfMaxPrice);
                listings = listings.Where(s => s.Price <= noOfMaxPrice);
            }

            if (!string.IsNullOrEmpty(minLandSize))
            {
                decimal noOfMinLandSize;
                Decimal.TryParse(minLandSize, out noOfMinLandSize);
                listings = listings.Where(s => s.LandSize >= noOfMinLandSize);
            }

            if (!string.IsNullOrEmpty(maxLandSize))
            {
                decimal noOfMaxLandSize;
                Decimal.TryParse(maxLandSize, out noOfMaxLandSize);
                listings = listings.Where(s => s.LandSize <= noOfMaxLandSize);
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                listings = listings.Where(x => x.PropertyType == propertyType);
            }

            if (!string.IsNullOrEmpty(transactionType))
            {
                listings = listings.Where(x => x.TransactionType == transactionType);
            }

            if (!string.IsNullOrEmpty(kota))
            {
                listings = listings.Where(x => x.Kota == kota);
            }

            if (!string.IsNullOrEmpty(kecamatan))
            {
                listings = listings.Where(x => x.Kecamatan == kecamatan);
            }

            //show only published listing
            listings = listings.Where(x => x.Status == PinrumahConstants.AdsStatus);

            switch (sortOrder)
            {
                case "date_desc":
                    listings = listings.OrderByDescending(s => s.CreatedOn);
                    break;
                case "price_asc":
                    listings = listings.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    listings = listings.OrderByDescending(s => s.Price);
                    break;
                case "rating_asc":
                    listings = listings.OrderBy(s => s.Rating);
                    break;
                case "rating_desc":
                    listings = listings.OrderByDescending(s => s.Rating);
                    break;
                default:
                    listings = listings.OrderByDescending(s => s.CreatedOn);
                    break;
            }
            
            var listingPropertyTypeVM = new ListingPropertyTypeViewModel
            {
                PropertyType = new SelectList(await propertyTypeQuery.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToListAsync()),
                Kota = new SelectList(await kotaQuery.Where(s => !string.IsNullOrEmpty(s)).ToListAsync()),
                Kecamatan = new SelectList(await kecamatanQuery.Where(s => !string.IsNullOrEmpty(s)).ToListAsync()),
                Listings = await PaginatedList<Listing>.CreateAsync(listings.AsNoTracking(), pageNumber ?? 1, pageSize)
            };
            return View(listingPropertyTypeVM);
            // return View(await _context.Listing.ToListAsync());
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

            listing.Listing_feature = _context.Listing_features.Include(s => s.Features).Where(s => s.Listings == listing).ToList();

            ViewBag.Keywords = listing.PropertyId + " " + listing.Title;
            ViewBag.Description = "Lihat listing " 
                + listing.TransactionType + " " + listing.PropertyType
                + " di " + listing.Address + " "
                + listing.PropertyId + " " + listing.Title;
            ViewBag.Image = listing.ImagePath1;

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

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

                    BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"),Configuration.GetValue<string>("Azure:BlobContainerName"));

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
                #endregion


                listing.AgentName = user.UserName;
                //replace 0 with 62
                string mobileNoCountryFormat = string.Empty;                

                if (user.PhoneNumber.StartsWith("0"))
                {
                    mobileNoCountryFormat = "62" + user.PhoneNumber.Substring(1);
                }
                else mobileNoCountryFormat = user.PhoneNumber;
                listing.AgentMobileNo = mobileNoCountryFormat;
                listing.AgentEmail = user.Email;
                listing.CreatedBy = user.Id;
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

            var listing = await _context.Listings.FindAsync(id);
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
                    #endregion

                    listing.ModifiedBy = user.Id;
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

            var listing = await _context.Listings
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
            var rumah = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(rumah);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RumahExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }

        public IActionResult GetKecamatan(string kotaName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items = _context.Kecamatans.Where(s => s.Status == PinrumahConstants.AdsStatus && s.Kota.KotaName == kotaName).Select(x => new SelectListItem() { Text = x.KecamatanName, Value = x.KecamatanName }).ToList();

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
