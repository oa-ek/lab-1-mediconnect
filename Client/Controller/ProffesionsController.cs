// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Server.Data;
// using Shared.Models;
//
// namespace Client.Controllers
// {
//     public class ProffesionsController : Controller
//     {
//         private readonly Context _context;
//
//         public ProffesionsController(Context context)
//         {
//             _context = context;
//         }
//
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.Proffesions.ToListAsync());
//         }
//
//         public async Task<IActionResult> Details(string? name)
//         {
//             if (name == null)
//             {
//                 return NotFound();
//             }
//
//             var proffesion = await _context.Proffesions
//                 .FirstOrDefaultAsync(m => m.Name == name);
//             if (proffesion == null)
//             {
//                 return NotFound();
//             }
//
//             return View(proffesion);
//         }
//
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("ID,Name")] Proffesion proffesion)
//         {
//             if (ModelState.IsValid)
//             {
//                 _context.Add(proffesion);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(proffesion);
//         }
//
//         public async Task<IActionResult> Edit(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var proffesion = await _context.Proffesions.FindAsync(id);
//             if (proffesion == null)
//             {
//                 return NotFound();
//             }
//             return View(proffesion);
//         }
//
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Proffesion proffesion)
//         {
//             if (id !=proffesion.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(proffesion);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!ProffesionExists(proffesion.Id))
//                     {
//                         return NotFound();
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(proffesion);
//         }
//
//         public async Task<IActionResult> Delete(string? name)
//         {
//             if (name == null)
//             {
//                 return NotFound();
//             }
//
//             var proffesion = await _context.Proffesions
//                 .FirstOrDefaultAsync(m => m.Name == name);
//             if (proffesion == null)
//             {
//                 return NotFound();
//             }
//
//             return View(proffesion);
//         }
//
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(string name)
//         {
//             var proffesion = await _context.Proffesions.FindAsync(name);
//             _context.Proffesions.Remove(proffesion);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool ProffesionExists(int id)
//         {
//             return _context.Proffesions.Any(e => e.Id == id);
//         }
//     }
// }
