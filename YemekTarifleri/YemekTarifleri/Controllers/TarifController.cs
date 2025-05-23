using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekTarifleri.Data;
using YemekTarifleri.Models;
using System.Linq;

namespace YemekTarifleri.Controllers
{
    public class TarifController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TarifController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ÜYE ve ADMIN: Tarifleri listele
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var tarifler = _context.Tarifler
                .OrderByDescending(t => t.EklenmeTarihi)
                .ToList();

            return View(tarifler);
        }

        // ÜYE ve ADMIN: Tarif detayları
        public IActionResult Details(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var tarif = _context.Tarifler.FirstOrDefault(t => t.Id == id);
            if (tarif == null)
                return NotFound();

            return View(tarif);
        }

        // SADECE ADMIN: Yeni tarif oluşturma ekranı
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // SADECE ADMIN: Yeni tarif POST işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Tarif tarif)
        {
            if (ModelState.IsValid)
            {
                _context.Tarifler.Add(tarif);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(tarif);
        }

        // SADECE ADMIN: Tarif düzenleme ekranı
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var tarif = _context.Tarifler.Find(id);
            if (tarif == null)
                return NotFound();

            return View(tarif);
        }

        // SADECE ADMIN: Tarif düzenleme POST işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Tarif tarif)
        {
            if (id != tarif.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(tarif);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(tarif);
        }

        // SADECE ADMIN: Silme onay ekranı
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var tarif = _context.Tarifler.Find(id);
            if (tarif == null)
                return NotFound();

            return View(tarif);
        }

        // SADECE ADMIN: Silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tarif = _context.Tarifler.Find(id);
            if (tarif != null)
            {
                _context.Tarifler.Remove(tarif);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
