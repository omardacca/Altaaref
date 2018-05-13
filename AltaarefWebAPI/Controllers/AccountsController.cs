using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AltaarefWebAPI.Contexts;
using AltaarefWebAPI.Models;
using AutoMapper;
using AltaarefWebAPI.ViewModels;
using AltaarefWebAPI.Helpers;

namespace AltaarefWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly AltaarefDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, AltaarefDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var userIdentity = _mapper.Map<AppUser>(model);

            //var result = await _userManager.CreateAsync(userIdentity, model.Password);

            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _appDbContext.Student.AddAsync(
            //    new Student
            //    {
            //        IdentityId = userIdentity.Id,
            //        FullName = userIdentity.FullName,
            //        ProfilePicBlobUrl = userIdentity.ProfilePicBlobUrl,
            //        DOB = userIdentity.DOB,
            //        Id = userIdentity.StudentId
            //    });

            //await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}