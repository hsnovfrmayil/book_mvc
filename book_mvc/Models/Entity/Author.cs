using System;
namespace book_mvc.Models.Entity;

public class Author
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Bio { get; set; }

    // Relationships
    public ICollection<Book> Books { get; set; }
}