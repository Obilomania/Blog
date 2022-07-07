using Blog.Data;
using Blog.Models;
using Blog.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Post> posts = _unitOfWork.Post.GetAll();
            return View(posts);
        }


        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (post.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, post.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    post.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (post.Id == 0)
                {
                    _unitOfWork.Post.Add(post);

                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _unitOfWork.Post.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    //if (post.ImageUrl != null)
                    //{
                    //    var oldImagePath = Path.Combine(wwwRootPath, post.ImageUrl.TrimStart('\\'));
                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    post.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if (post.Id != 0)
                {
                _unitOfWork.Post.Update(post);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(post);
        }


        ////UPSERT
        //public IActionResult Upsert(int? id)
        //{
        //    Post post = new Post();
        //    if (id == null || id == 0)
        //    {
        //        return View(post);
        //    }
        //    else
        //    {

        //    }
            
        //    return View(post);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Post post)
        //{
        //    if (ModelState.IsValid)
        //    {




        //        _unitOfWork.Post.Update(post);
        //        _unitOfWork.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View(post);
        //}



        //DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _unitOfWork.Post.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST (int? id)
        {
            var post = _unitOfWork.Post.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, post.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Post.Remove(post);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }



        //DETAIL
        public IActionResult Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _unitOfWork.Post.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}
