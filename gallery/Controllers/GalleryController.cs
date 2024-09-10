using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gallery.Data; 
using gallery.Models;

public class GalleryController : Controller
{
    private readonly ApplicationDbContext _context;

    public GalleryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _context.Items.ToListAsync();
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(Item item, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            item.ImagePath = "/images/" + file.FileName;
        }

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}