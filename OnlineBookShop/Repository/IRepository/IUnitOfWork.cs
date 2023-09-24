using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public interface IUnitOfWork
{
    ICategoryRepository<Category> Category { get; }
    IProductRepository<Product> Product { get; }
    ICompanyRepository<Company> Company { get; }

    void Save();
}