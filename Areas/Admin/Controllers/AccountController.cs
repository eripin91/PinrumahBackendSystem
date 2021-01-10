using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using PinBackendSystem.Data;
using PinBackendSystem.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PinBackendSystem.Areas.Identity;

namespace PinBackendSystem.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly PinContext _context;
        private IConfiguration Configuration;
        BlobStorageService _blobStorageService;
        private UserManager<PinrumahUser> _userManager;
        public AccountController(PinContext context, IConfiguration _configuration, UserManager<PinrumahUser> userManager)
        {
            Configuration = _configuration;
            _context = context;
            _userManager = userManager;
            _blobStorageService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"), Configuration.GetValue<string>("Azure:BlobContainerName"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PhotoUpload(IFormFile profilePicture)
        {
            if (ModelState.IsValid && User != null)
            {
                #region upload file  

                if (profilePicture != null)
                {
                    IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                        .Create(Configuration["AzureAdB2C:ClientId"])
                        .WithTenantId(Configuration["AzureAdB2C:Domain"])
                        .WithClientSecret(Configuration["AzureAdB2C:ClientSecret"])
                        .Build();

                    byte[] fileData;

                    await using (var target = new MemoryStream())
                    {
                        profilePicture.CopyTo(target);
                        fileData = target.ToArray();
                    }

                    //update azure b2c photoURL custom attribute                    

                    string profilePictureURL = _blobStorageService.UploadFileToBlob(profilePicture.FileName, fileData, profilePicture.ContentType);

                    //var user = new Microsoft.Graph.User
                    //{

                    //    AdditionalData = extensionInstance
                    //};
                    //await graphClient.Users[User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value]
                    //    .Request()
                    //    .UpdateAsync(user);

                    // Get the existing student from the db


                    var user = await _userManager.GetUserAsync(User);
                    user.avatar_link = profilePictureURL;
                    await _userManager.UpdateAsync(user);

                    //update httpcontext user
                    //var Identity = HttpContext.User.Identity as ClaimsIdentity;
                    //Identity.RemoveClaim(Identity.FindFirst("extension_ProfilePictureURL"));
                    //Identity.AddClaim(new Claim("extension_ProfilePictureURL", profilePictureURL));
                    ////var authenticationManager = Microsoft.AspNetCore.Http.HttpContext.GetOwinContext().Authentication;
                    ////authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                    ////    new ClaimsPrincipal(Identity), new AuthenticationProperties()
                    ///
                    ////{ IsPersistent = true });

                    //GET CURRENT USER
                    //var usr = await GetCurrentUserAsync();
                    ////OLD CLAIM
                    //var myClaims = await _userManager.GetClaimsAsync(usr);
                    //var oldClaim = myClaims.Where(o => o.Type.Equals("Club")).FirstOrDefault();
                    //if (oldClaim != null)
                    //{
                    //    await _userManager.RemoveClaimAsync(usr, oldClaim);
                    //}

                    ////CREATE CLUB CLAIM
                    //var clubClaim = new Claim("Club", "" + id);
                    //await _userManager.AddClaimAsync(usr, clubClaim);

                    ////RESET USER COOKIE
                    //await _signInManager.RefreshSignInAsync(usr);

                    //perlu investigasi lebih jauh
                    await HttpContext.SignOutAsync();


                    //await _userManager.UpdateSecurityStampAsync(user);



                }
                #endregion

            }
            return RedirectToAction(nameof(Index), nameof(ListingsController).Replace("Controller", ""));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> PhotoUpload(IFormFile profilePicture)
        //{
        //    if (ModelState.IsValid && User != null)
        //    {
        //        #region upload file  

        //        if (profilePicture != null)
        //        {
        //            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
        //                .Create(Configuration["AzureAdB2C:ClientId"])
        //                .WithTenantId(Configuration["AzureAdB2C:Domain"])
        //                .WithClientSecret(Configuration["AzureAdB2C:ClientSecret"])
        //                .Build();

        //            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);

        //            byte[] fileData;

        //            await using (var target = new MemoryStream())
        //            {
        //                profilePicture.CopyTo(target);
        //                fileData = target.ToArray();
        //            }

        //            BlobStorageService objBlobService = new BlobStorageService(Configuration.GetConnectionString("BlobStorageAccount"));

        //            //update azure b2c photoURL custom attribute                    

        //            string profilePictureURL = objBlobService.UploadFileToBlob(profilePicture.FileName, fileData, profilePicture.ContentType);


        //            GraphServiceClient graphClient = new GraphServiceClient(authProvider);
        //            IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
        //            extensionInstance.Add("extension_340e6ec3b4274b57ab9a83632f79681b_ProfilePictureURL", profilePictureURL);
        //            var user = new Microsoft.Graph.User
        //            {

        //                AdditionalData = extensionInstance
        //            };
        //            await graphClient.Users[User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value]
        //                .Request()
        //                .UpdateAsync(user);

        //            //update httpcontext user
        //            //var Identity = HttpContext.User.Identity as ClaimsIdentity;
        //            //Identity.RemoveClaim(Identity.FindFirst("extension_ProfilePictureURL"));
        //            //Identity.AddClaim(new Claim("extension_ProfilePictureURL", profilePictureURL));
        //            ////var authenticationManager = Microsoft.AspNetCore.Http.HttpContext.GetOwinContext().Authentication;
        //            ////authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
        //            ////    new ClaimsPrincipal(Identity), new AuthenticationProperties()
        //            ///
        //            ////{ IsPersistent = true });

        //            //GET CURRENT USER
        //            //var usr = await GetCurrentUserAsync();
        //            ////OLD CLAIM
        //            //var myClaims = await _userManager.GetClaimsAsync(usr);
        //            //var oldClaim = myClaims.Where(o => o.Type.Equals("Club")).FirstOrDefault();
        //            //if (oldClaim != null)
        //            //{
        //            //    await _userManager.RemoveClaimAsync(usr, oldClaim);
        //            //}

        //            ////CREATE CLUB CLAIM
        //            //var clubClaim = new Claim("Club", "" + id);
        //            //await _userManager.AddClaimAsync(usr, clubClaim);

        //            ////RESET USER COOKIE
        //            //await _signInManager.RefreshSignInAsync(usr);

        //            //perlu investigasi lebih jauh
        //            await HttpContext.SignOutAsync();
                    

        //            //await _userManager.UpdateSecurityStampAsync(user);



        //        }
        //        #endregion

        //    }
        //    return RedirectToAction(nameof(Index), nameof(ListingsController).Replace("Controller",""));
        //}

    }
}
