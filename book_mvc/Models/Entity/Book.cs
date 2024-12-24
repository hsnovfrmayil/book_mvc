using System;
namespace book_mvc.Models.Entity;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public bool IsFavorite { get; set; }
    public string ImageUrl { get; set; }

    // Relationships
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public ICollection<Comment> Comments { get; set; }
}