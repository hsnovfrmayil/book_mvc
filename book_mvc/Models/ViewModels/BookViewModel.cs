using System;
namespace book_mvc.Models.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}