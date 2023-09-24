using OnlineBookShop.Data;
using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository<Company>
{
    private ApplicationDbContext _db;

    public CompanyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    

    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }
}