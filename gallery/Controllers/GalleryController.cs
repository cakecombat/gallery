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
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                item.ImageData = memoryStream.ToArray(); 
            }
        }

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetImage(int id)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item?.ImageData != null)
        {
            return File(item.ImageData, "image/jpeg");
        }
        return NotFound();
    }

}
