using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBookShop.Data;
using OnlineBookShop.Models;
using OnlineBookShop.Models.ViewModels;
using OnlineBookShop.Repository;

namespace OnlineBookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IActionResult Index()
    {
        List<Company> objCategoryList = _unitOfWork.Company.GetAll().ToList();
        
        return View(objCategoryList);
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            // create
            return View(new Company());
        }
        else
        {
            // update
            Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
            return View(companyObj);
        }
    }
    
    [HttpPost]
    public IActionResult Upsert(Company companyObj)
    {
        if (ModelState.IsValid)
        {

            if (companyObj.Id == 0)
            {
                _unitOfWork.Company.Add(companyObj);
            }
            else
            {
                _unitOfWork.Company.Update(companyObj);
            }
            
            _unitOfWork.Save();
            // to be more explicit, we can write this
            // return RedirectToAction("Index", "Category");
            TempData["success"] = "Company created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(companyObj);
        };
    }

    // public IActionResult Edit(int id)
    // {
    //     if (id == null || id == 0)
    //     {
    //         return NotFound();
    //     }
    //
    //     // different ways to retrieve the Product object
    //     // Product? productFromDb2 = _db.Products.FirstOrDefault(u=>u.Id==id);
    //     // Product? productFromDb3 = _db.Products.Where(u=>u.Id==id).FirstOrDefault();
    //     Product? productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
    //     
    //     if (productFromDb == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(productFromDb);
    // }

    // [HttpPost]
    // public IActionResult Edit(Product obj)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         _unitOfWork.Product.Update(obj);
    //         _unitOfWork.Save();
    //         TempData["success"] = "Product updated successfully";
    //         return RedirectToAction("Index");
    //     }
    //
    //     return View();
    // }

    // public IActionResult Delete(int? id)
    // {
    //     if (id == null || id == 0)
    //     {
    //         return NotFound();
    //     }
    //
    //     Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
    //
    //     if (productFromDb == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(productFromDb);
    // }
    //
    // [HttpPost, ActionName("Delete")]
    // public IActionResult DeletePost(int? id)
    // {
    //     Product obj = _unitOfWork.Product.Get(u=>u.Id==id);
    //     if (obj == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     _unitOfWork.Product.Remove(obj);
    //     _unitOfWork.Save();
    //     TempData["success"] = "Category deleted successfully";
    //     return RedirectToAction("Index");
    // }
    
    // region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return Json(new { data = objCompanyList });
    }
    
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        Console.WriteLine("here");
        var productToBeDeleted = _unitOfWork.Company.Get(u=>u.Id==id);
        if (productToBeDeleted == null)
        {
            return Json(new
            {
                success = false,
                message = "Error while deleting"
            });
        }
        
        _unitOfWork.Company.Remove(productToBeDeleted);
        _unitOfWork.Save();
        
        return Json(new
        {
            success = true,
            message = "Error successfully"
        });
    }
    
    // end region
}