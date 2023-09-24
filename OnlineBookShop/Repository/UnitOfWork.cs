using OnlineBookShop.Data;
using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _db;
    public ICategoryRepository<Category> Category { get; private set; }
    
    public IProductRepository<Product> Product { get; private set; }
    
    public ICompanyRepository<Company> Company { get; private set; }
    
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        Company = new CompanyRepository(_db);
    }
    
    public void Save()
    {
        _db.SaveChanges();
    }
}