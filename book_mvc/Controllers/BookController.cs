using book_mvc.Contexts;
using book_mvc.Models.Entity;
using book_mvc.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace book_mvc.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var books = _context.Books.Select(b => new BookViewModel
        {
            Id = b.Id,
            Title = b.Title,
            ShortDescription = b.ShortDescription,
            Price = b.Price,
            ImageUrl = b.ImageUrl
        }).ToList();

        return View(books);
    }

    [HttpGet("book/details/{id}")]
    public IActionResult Details(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound("Kitab tapılmadı.");
        }
        return View(book);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Authors = _context.Authors.ToList();  // Assuming _context is your DbContext
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            // Save book to the database
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Pass authors back to the view if validation fails
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }


    // Kitabı yenilə
    [HttpGet("{id}")]
    public IActionResult Edit(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound("Kitab tapılmadı.");
        }
        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [FromForm] Book updatedBook)
    {
        if (id != updatedBook.Id)
        {
            return BadRequest("ID uyğun gəlmir.");
        }

        if (ModelState.IsValid)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound("Kitab tapılmadı.");
            }

            book.Title = updatedBook.Title;
            book.ShortDescription = updatedBook.ShortDescription;
            book.Price = updatedBook.Price;
            book.IsFavorite = updatedBook.IsFavorite;
            book.AuthorId = updatedBook.AuthorId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(updatedBook);
    }

    
    [HttpPost]
    public IActionResult AddToCart(int bookId)
    {
        // Sepet bilgisi oturumda (session) saklanıyor
        var cart = HttpContext.Session.GetString("Cart");
        List<int> cartItems = cart == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(cart);

        if (!cartItems.Contains(bookId))
        {
            cartItems.Add(bookId);
        }

        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

        return RedirectToAction("Index");
    }

    // Sepet Sayfası
    [HttpGet]
    public IActionResult Cart()
    {
        var cart = HttpContext.Session.GetString("Cart");
        if (cart == null)
        {
            return View(new List<Book>());
        }

        var cartItems = JsonConvert.DeserializeObject<List<int>>(cart);
        var booksInCart = _context.Books.Where(b => cartItems.Contains(b.Id)).ToList();

        return View(booksInCart);
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound(); // If the product is not found, return 404
        }

        _context.Books.Remove(book);
        _context.SaveChanges();

        return Ok(); // Return success status
    }
}





