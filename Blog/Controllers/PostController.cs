using Blog.Data;
using Blog.Models;
using Blog.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _db;

        public PostController(IPostRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Post> posts = _db.GetAll();
            return View(posts);
        }


        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {

            if (ModelState.IsValid)
            {
                _db.Add(post);
                _db.Save();
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
            var post = _db.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                _db.Update(post);
                _db.Save();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _db.GetFirstOrDefault(p => p.Id == id);
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
            var post = _db.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            _db.Remove(post);
            _db.Save();
            return RedirectToAction("Index");
        }



        //EDIT
        public IActionResult Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _db.GetFirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }



    }
}
