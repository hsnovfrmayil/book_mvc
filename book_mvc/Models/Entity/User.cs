using System;
namespace book_mvc.Models.Entity;


public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    // Relationships
    public ICollection<Comment> Comments { get; set; }
}