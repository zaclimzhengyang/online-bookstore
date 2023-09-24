using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public interface ICompanyRepository<T> : IRepository<Company>
{
    void Update(Company obj);

}