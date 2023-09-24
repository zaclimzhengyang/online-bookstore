using OnlineBookShop.Data;
using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository<Category>
{
    private ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    

    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }
}