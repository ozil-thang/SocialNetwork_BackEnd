using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Hubs;
using Api.Models.Post;
using Api.Models.Profile;
using Api.Utils;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;

namespace Api.Controllers
{
    [Authorize]
    public class PostsController : MyControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly SocialNetworkContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private readonly IHubContext<LikeHub> _likeHubContext;

        private readonly IHubContext<CommentHub> _commentHubContext;

        public PostsController(SocialNetworkContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig,
                                IHubContext<LikeHub> likeHubContext, IHubContext<CommentHub> commentHubContext)
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

            _likeHubContext = likeHubContext;
            _commentHubContext = commentHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts.Include(p => p.Photo).Include(p => p.Video)
                                        .Include(p => p.Likes).Include(p => p.Comments)
                                        .Include(p => p.UserProfile)
                                        .ThenInclude(pr => pr.Avatar)
                                        .ToListAsync();

            var postsDto = new List<PostItemDto>();

            foreach (var post in posts)
            {
                var postDto = _mapper.Map<PostItemDto>(post);
                if (post.Likes.Any(l => l.UserId == UserId))
                    postDto.IsLike = true;
                postsDto.Add(postDto);
            }
            return Ok(postsDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute]string id)
        {
            var post = await _context.Posts.Include(p => p.UserProfile).ThenInclude(pr => pr.Avatar)
                                        .Include(p => p.Likes)
                                        .Include(p => p.Comments).ThenInclude(p => p.UserProfile).ThenInclude(pr => pr.Avatar)
                                        .Include(p => p.Photo)
                                        .Include(p => p.Video)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound(new Utils.Error("Not found"));

            var postDetailDto = _mapper.Map<PostDetailDto>(post);

            if (post.Likes.Any(l => l.UserId == UserId))
                postDetailDto.isLike = true;

            return Ok(postDetailDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm]CreatePostDto createPostDto)
        {
            var post = new Post();
            post.UserId = UserId;

            if (!String.IsNullOrEmpty(createPostDto.Text))
                post.Text = createPostDto.Text;

            if (createPostDto.Photo != null)
            {
                var uploadResultImage = new ImageUploadResult();

                using (var stream = createPostDto.Photo.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(createPostDto.Photo.Name, stream)
                    };
                    uploadResultImage = _cloudinary.Upload(uploadParams);
                }

                var photo = new Photo
                {
                    Id = uploadResultImage.PublicId,
                    Url = uploadResultImage.Uri.ToString()
                };

                post.Photo = photo;
            }

            if (createPostDto.Video != null)
            {
                var uploadResultVideo = new VideoUploadResult();

                using (var stream = createPostDto.Video.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(createPostDto.Video.Name, stream)
                    };
                    uploadResultVideo = _cloudinary.Upload(uploadParams);
                }

                var video = new Domain.Video
                {
                    Id = uploadResultVideo.PublicId,
                    Url = uploadResultVideo.Uri.ToString()
                };

                post.Video = video;
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var postDto = _mapper.Map<PostItemDto>(post);

            var userProfile = await _context.Profiles.Include(p => p.Avatar).FirstOrDefaultAsync(p => p.UserId == UserId);

            postDto.Avatar = userProfile.Avatar.Url;

            return Ok(postDto);
        }

        [HttpPut("{id}/removelike")]
        public async Task<IActionResult> RemoveLike([FromRoute]string id)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == UserId && l.PostId == id);

            if (like == null)
                return NotFound();

            _context.Likes.Remove(like);

            await _context.SaveChangesAsync();

            var count = _context.Likes.Where(p => p.PostId == id).Count();
            var userId = UserId;
            var type = "UnLike";
            var postId = id;
            await _likeHubContext.Clients.All.SendAsync("updateLike", count, userId, type, postId);

            return NoContent();
        }

        [HttpPut("{id}/like")]
        public async Task<IActionResult> Like([FromRoute]string id)
        {
            if (await _context.Posts.FirstOrDefaultAsync(p => p.Id == id) == null)
                return NotFound();
            if (await _context.Likes.AnyAsync(l => l.UserId == UserId && l.PostId == id))
                return BadRequest();

            var like = new Like { UserId = UserId, PostId = id };

            _context.Likes.Add(like);

            await _context.SaveChangesAsync();

            var count = _context.Likes.Where(p => p.PostId == id).Count();
            var userId = UserId;
            var type = "Like";
            var postId = id;
            await _likeHubContext.Clients.All.SendAsync("updateLike", count, userId, type, postId);


            return NoContent();
        }

        [HttpPut("{id}/comment")]
        public async Task<IActionResult> Comment([FromRoute]string id, [FromBody] CreateCommentDto createCommentDto)
        {
            var comment = new Comment();
            comment.Text = createCommentDto.Text;
            comment.UserId = UserId;
            comment.PostId = id;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            comment = await _context.Comments.Include(c => c.UserProfile).ThenInclude(up => up.Avatar).FirstOrDefaultAsync(c => c.Id == comment.Id);

            var commentDto = _mapper.Map<CommentDto>(comment);
            var count = _context.Comments.Where(c => c.PostId == id).Count();
            var postId = id;
            await _commentHubContext.Clients.All.SendAsync("comment", commentDto, count, postId);

            return NoContent();
        }
    }
}
