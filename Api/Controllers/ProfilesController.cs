using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models.Profile;
using Api.Utils;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;

namespace Api.Controllers
{

    public class ProfilesController : MyControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly SocialNetworkContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        public ProfilesController(SocialNetworkContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var profile = await _context.Profiles.Include(p => p.Avatar)
                                .Include(p => p.Skills)
                                .Include(p => p.Educations)
                                .Include(p => p.Experiences)
                                .FirstOrDefaultAsync(p => p.UserId == UserId);
            if (profile == null)
                return NotFound(new Utils.Error("User has no profile"));

            var profileDto = _mapper.Map<ProfileDto>(profile);

            return Ok(profileDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfile()
        {
            var profiles = await _context.Profiles.Include(p => p.Avatar)
                                .Include(p => p.Skills)
                                .ToListAsync();

            var profilesDto = _mapper.Map<IEnumerable<ProfileItemDto>>(profiles);
            return Ok(profilesDto);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileByUserId([FromRoute]string userId)
        {
            var profile = await _context.Profiles.Include(p => p.Avatar)
                                .Include(p => p.Skills)
                                .Include(p => p.Educations)
                                .Include(p => p.Experiences)
                                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return NotFound(new Utils.Error("Not found user or user do not have profile"));

            var profileDto = _mapper.Map<ProfileDto>(profile);

            return Ok(profileDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromForm]CreateProfileDto createProfileDto, [FromQuery]string edit)
        {
            Domain.Profile profile = null;

            if (!String.IsNullOrEmpty(edit) && edit == "true")
            {
                profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == UserId);
                _mapper.Map<CreateProfileDto, Domain.Profile>(createProfileDto, profile);

                if (createProfileDto.Avatar != null)
                {
                    var avatar = UploadImage(createProfileDto.Avatar);
                    profile.Avatar = avatar;
                }
            }
            else
            {
                if (createProfileDto.Avatar == null)
                    return BadRequest();
                profile = _mapper.Map<Domain.Profile>(createProfileDto);
                profile.UserId = UserId;
                var avatar = UploadImage(createProfileDto.Avatar);
                profile.Avatar = avatar;

                _context.Profiles.Add(profile);
            }

            await _context.SaveChangesAsync();

            var profileDto = _mapper.Map<ProfileDto>(profile);

            return Ok(profileDto);
        }

        private Photo UploadImage(IFormFile img)
        {

            var uploadResult = new ImageUploadResult();

            using (var stream = img.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(img.Name, stream)
                };
                uploadResult = _cloudinary.Upload(uploadParams);
            }

            var photo = new Photo()
            {
                Id = uploadResult.PublicId,
                Url = uploadResult.Uri.ToString(),
            };
            return photo;
        }
    }
}
