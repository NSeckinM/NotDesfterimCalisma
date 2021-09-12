using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotDefterim.Data;
using NotDefterim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NotDefterim.Controllers
{
    [Authorize]
    public class NotlarController : Controller
    {
        private readonly ApplicationDbContext db;

        public NotlarController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Yeni()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Yeni(Note note)
        {
            if (ModelState.IsValid)
            {
                note.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                db.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Sil(int id)
        {
            return View(db.Notes.Find(id));
        }


        [HttpPost, ValidateAntiForgeryToken, ActionName("Sil")]
        public IActionResult Silinen(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Note note = db.Notes.Where(x => x.Id == id && x.AuthorId == userId).FirstOrDefault();
            if (note == null)
            {
                return NotFound();
            }
            db.Notes.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }


        public IActionResult Duzenle(int id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            NotViewModel nvm = new NotViewModel();

            nvm.Id = note.Id;
            nvm.Title = note.Title;
            nvm.Content = note.Content;

            return View(nvm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Duzenle(NotViewModel model)
        {
            if (ModelState.IsValid)
            {

                Note note = db.Notes.Where(x => x.Id == model.Id && x.AuthorId == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();

                if (note == null)
                {
                    return NotFound();
                }
                note.Title = model.Title;
                note.Content = model.Content;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


    }
}
