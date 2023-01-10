using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class HomeController : Controller
{
    private readonly IStoreRepository repository;

    public int PageSize = 4;

    public HomeController(IStoreRepository repository)
    {
        this.repository = repository;
    }

    public ViewResult Index(string category, int productPage = 1)
        => View(new ProductsListViewModel
        {
            Products = string.IsNullOrWhiteSpace(category) ?
            repository.Products
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize)
            :
            repository.Products
            .Where(p => p.Category == category)
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize),

            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = repository.Products.Count()
            },
            CurrentCategory = category
        });            
}
