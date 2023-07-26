using BMS.Core.DTO;
using BMS.Core.Models;
using BMS.Repo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private IWebHostEnvironment _webHostEnvironment;
        IImageRepository _imageRepository;
        public CommentsController(IPostRepository postRepository, ICommentRepository commentRepository, IWebHostEnvironment webHostEnvironment, IImageRepository imageRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("AllPosts")]
        public async Task<IActionResult> GetAllPost()
        {
            var finalPostDto = new List<PostCommentDTO>();
            var posts = await _postRepository.GetAll();
            foreach (var post in posts)
            {
                var postDto = new PostCommentDTO()
                {
                    Title = post.Title,
                    Content = post.Content,
                    Created = DateTime.Now,
                    Comments = _commentRepository.GetCommentsByPostId(post.Id)
                };
                finalPostDto.Add(postDto);
            }
            return Ok(finalPostDto);
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComent([FromBody] CreateCommentDTO createCommentDTO)
        {
            var comment = new Comment
            {
                AuthorName = createCommentDTO.AuthorName,
                Contet = createCommentDTO.Comment,
                Email = createCommentDTO.AuthorEmail,
                Created = DateTime.UtcNow,
                PostId = createCommentDTO.PostId
            };

            _commentRepository.Add(comment);
            _commentRepository.Save();
            return Ok(comment);
        }
        private string GetFilePath(string imageName)
        {
            return _webHostEnvironment.WebRootPath + "\\uploads\\" + imageName;
        }
    }
}
