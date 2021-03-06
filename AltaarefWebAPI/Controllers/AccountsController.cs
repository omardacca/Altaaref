﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AltaarefWebAPI.Contexts;
using AltaarefWebAPI.Models;
//using AutoMapper;
using AltaarefWebAPI.ViewModels;
using AltaarefWebAPI.Helpers;

namespace AltaarefWebAPI.Controllers
{
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly AltaarefDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager, AltaarefDbContext appDbContext)
        {
            _userManager = userManager;
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

            var userIdentity = new AppUser
            {
                StudentId = model.StudentId,
                FullName = model.FullName,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            if(model.ProfilePicBlobUrl != null)
            {
                await _appDbContext.Student.AddAsync(
                    new Student
                    {
                        IdentityId = userIdentity.Id,
                        FullName = model.FullName,
                        ProfilePicBlobUrl = model.ProfilePicBlobUrl,
                        DOB = model.DOB,
                        Id = model.StudentId
                    });
            }
            else
            {
                await _appDbContext.Student.AddAsync(
                    new Student
                    {
                        IdentityId = userIdentity.Id,
                        FullName = model.FullName,
                        DOB = model.DOB,
                        Id = model.StudentId
                    });
            }

            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}