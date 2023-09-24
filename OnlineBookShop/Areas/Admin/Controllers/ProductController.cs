using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBookShop.Data;
using OnlineBookShop.Models;
using OnlineBookShop.Models.ViewModels;
using OnlineBookShop.Repository;
using OnlineBookShop.Utility;

namespace OnlineBookShop.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{
    // private readonly IProductRepository<Product> _productRepository;
    
    // public ProductController(IProductRepository<Product> productRepository)
    // {
    //     _productRepository = productRepository;
    // }
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public IActionResult Index()
    {
        List<Product> objCategoryList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        foreach (var item in objCategoryList)
        {
            Console.Write($"id: {item.Id}. title: {item.Title}. ISBN: {item.ISBN}");
            Console.WriteLine();
        }

        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            .GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
        return View(objCategoryList);
    }

    public IActionResult Upsert(int? id)
    {
        // ViewBag.CategoryList = CategoryList;
        // ViewData["CategoryList"] = CategoryList;
        
        ProductVM productVM = new ProductVM()
        {
            CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            Product = new Product()
        };
        if (id == null || id == 0)
        {
            // create
            return View(productVM);
        }
        else
        {
            // update
            productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(productVM);
        }
    }
    
    [HttpPost]
    public IActionResult Upsert(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    // delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.ImageUrl = @"/images/product/" + fileName;
            }

            if (productVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            
            _unitOfWork.Save();
            // to be more explicit, we can write this
            // return RedirectToAction("Index", "Category");
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
        };
        
        return View(productVM);
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
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = objProductList });
    }
    
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        Console.WriteLine("here");
        var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id==id);
        if (productToBeDeleted == null)
        {
            return Json(new
            {
                success = false,
                message = "Error while deleting"
            });
        }

        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
            productToBeDeleted.ImageUrl.TrimStart('/'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        
        _unitOfWork.Product.Remove(productToBeDeleted);
        _unitOfWork.Save();
        
        return Json(new
        {
            success = true,
            message = "Error successfully"
        });
    }
    
    // end region
}