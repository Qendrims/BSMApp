using BMS.Core.DTO;
using BMS.Core.Models;
using BMS.Repo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BMS.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _postRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;

        public PostController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository,
            IImageRepository imageRepository, ITagRepository tagRepository, IUserRepository userRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _postRepository = postRepository;
            _imageRepository = imageRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }


        [Route("CreatePost")]
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromForm] CreatePostDTO createPostDTO)
        {
            if (ModelState.IsValid)
            {
                var post = new Post();
                post.Title = createPostDTO.Title;
                post.Content = createPostDTO.Content;
                post.UserId = LoggedUser();
                post.Created = DateTime.Now;
                _postRepository.Add(post);
                _postRepository.Save();
                foreach (var file in createPostDTO.Images)
                {
                    var image = new Image();
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = System.IO.File.Create(path + file.FileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    image.PostId = post.Id;
                    image.ImagePath = path;
                    image.ImageType = file.FileName;
                    image.Id = Guid.NewGuid().ToString();
                    _imageRepository.Add(image);
                }
                foreach (var tags in createPostDTO.Tags)
                {
                    var tag = new Tag()
                    {
                        TagName = tags,
                        Post = post
                    };
                    _tagRepository.Add(tag);
                }
                _imageRepository.Save();
                _tagRepository.Save();
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost([FromBody] int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var post = _postRepository.GetPostById(Id);
            if (post == null || post.UserId != LoggedUser())
            {
                return BadRequest();
            }
            _postRepository.Remove(post);
            _postRepository.Save();
            return Ok("Post Deleted");
        }

        [HttpPatch]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDTO updatePostDTO)
        {
            if (updatePostDTO.PostId == 0)
            {
                return BadRequest();
            }
            var post = _postRepository.GetPostById(updatePostDTO.PostId);
            post.Title = updatePostDTO.Title;
            post.Content = updatePostDTO.Content;
            _postRepository.UpdatePost(post);
            _tagRepository.TagToRemove(updatePostDTO.PostId);
            _imageRepository.RemoveImage(updatePostDTO.PostId);
            foreach (var file in updatePostDTO.Images)
            {
                var image = new Image();
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = System.IO.File.Create(path + file.FileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
                image.PostId = post.Id;
                image.ImagePath = path;
                image.ImageType = file.FileName;
                image.Id = Guid.NewGuid().ToString();
                _imageRepository.Add(image);
            }
            foreach (var tags in updatePostDTO.Tags)
            {
                var tag = new Tag()
                {
                    TagName = tags,
                    Post = post
                };
                _tagRepository.Add(tag);
            }
            _imageRepository.Save();
            _tagRepository.Save();
            return Ok();
        }


        [HttpGet]
        [Route("SearchFilter")]
        public async Task<IActionResult> GetAll(string search = null)
        {
            var posts = _postRepository.GetAll().Result;

            if (!string.IsNullOrEmpty(search))
            {
                posts = await _postRepository.GetSearchPost(search);
                return Ok(posts);
            }
            return Ok(posts);
        }

        [HttpGet]
        [Route("Pagination")]
        public async Task<IActionResult> Pagination(int page = 1, int pageSize = 5)
        {
            var posts = _postRepository.GetAll().Result;

            var totalPost = posts.Count();
            var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
            posts = posts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            Response.Headers.Add("X-Total-Posts", totalPost.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            Response.Headers.Add("X-Current-Page", page.ToString());
            return Ok(posts);
        }


        private string LoggedUser()
        {
            string user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _userRepository.GetUserIdByEmail(user);
            return userId;
        }
        
    }
}
