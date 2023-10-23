using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Text.Json;
using FluentValidation;
using FormHelper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using Newtonsoft.Json;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.ViewModels;
using TestShop.Utility;
using TestShop.Models.Models;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IValidator<ProductVM> validator)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
		}
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAll(includeProperties:"Categories");
            
            return View(objProductList);
        }
        //GET
        // Update/Insert
        public async Task<IActionResult> Upsert(int? id)
        {
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;


            ProductVM productVm = new ProductVM()
            {
	            CategoryList = (await _unitOfWork.Category
		            .GetAll())
		            .Select(x => new SelectListItem
		            {
			            Text = x.Name,
			            Value = x.Id.ToString()
		            }),
	            Product = new Product()
            };
            if (id is null || id == 0)
            {
                //Create case
	            return View(productVm);
			}
            else
            {
                //Update case
                productVm.Product = await _unitOfWork.Product.Get(x => x.Id==id, includeProperties:"ProductImages,Categories");
                productVm.CategoryIds = productVm.Product.Categories.Select(x => x.Id);
				return View(productVm);
			}
			
        }
		//POST
		[HttpPost, FormValidator]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(ProductVM productVm, List<IFormFile>? files)
		{
			if (ModelState.IsValid)
			{
				productVm.Product.Categories = (await _unitOfWork.Category.GetAll(x => productVm.CategoryIds.Contains(x.Id))).ToList();

				//First - upsert the product
				if (productVm.Product.Id == 0)
				{
					await _unitOfWork.Product.Add(productVm.Product);
				}
				else
				{
					await _unitOfWork.Product.Update(productVm.Product);
				}
				await _unitOfWork.Save();

				//get the path to the wwwroot
				string wwwRootPath = _webHostEnvironment.WebRootPath;

				//Then - add images
				if (files != null && files.Count!=0)
				{
					//Thread safe
					ConcurrentBag<ProductImage> productImages = new ConcurrentBag<ProductImage>();
					await Parallel.ForEachAsync(files, async (file, CancellationToken) =>
					{
						//new file name
						string fileName = Guid.NewGuid().ToString() + Environment.CurrentManagedThreadId + Path.GetExtension(file.FileName);
						//path to product folder
						string productPath = @"images\product\product-" + productVm.Product.Id;
						//Complete path to right folder
						string finalPath = Path.Combine(wwwRootPath, productPath);

						if (!Directory.Exists(finalPath))
						{
							//create directory if needed
							Directory.CreateDirectory(finalPath);
						}

						await using (FileStream fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							//copy file to directory
							await file.CopyToAsync(fileStream);
						}

						ProductImage productImage = new()
						{
							ImageUrl = @"\" + productPath + @"\" + fileName,
							ProductId = productVm.Product.Id,
						};

						productImages.Add(productImage);
					});

					productVm.Product.ProductImages = productImages.ToList();

					await _unitOfWork.Product.Update(productVm.Product);
					await _unitOfWork.Save();
				}
				TempData["success"] = (productVm.Product.Id != 0) ? "Product updated successfully" : "Product created successfully";
				return FormResult.CreateSuccessResult("Success",Url.Action(nameof(Index)));
			}
			else
			{
				//provide the category list again
				productVm.CategoryList = (await _unitOfWork.Category
					.GetAll())
					.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					});
				return FormResult.CreateErrorResultWithObject(productVm, "bad request", Url.Action(nameof(Upsert)));
			}
		}

		public async Task<IActionResult> DeleteImage(int? imageId)
        {
	        var imageToBeDeleted = await _unitOfWork.ProductImage.Get(x => x.Id == imageId);
	        int productId = imageToBeDeleted.ProductId;
	        if (imageToBeDeleted != null)
	        {
		        if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
		        {
			        var oldImagePath =
				        Path.Combine(_webHostEnvironment.WebRootPath,
					        imageToBeDeleted.ImageUrl.TrimStart('\\'));
			        if (System.IO.File.Exists(oldImagePath))
			        {
				        System.IO.File.Delete(oldImagePath);
			        }
		        }

		        await _unitOfWork.ProductImage.Remove(imageToBeDeleted);
		        await _unitOfWork.Save();

		        TempData["success"] = "Image Deleted Successfully";

	        }

	        return RedirectToAction(nameof(Upsert), new { id = productId });
        }

		#region API CALLS

		[HttpGet]
        public async Task<IActionResult> GetAll()
        {
	        IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAll(includeProperties: "Categories");
	        return Json(objProductList) ;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            Product productToBeDeleted = await _unitOfWork.Product.Get(x => x.Id == id);
	        if (productToBeDeleted == null)
	        {
		        return Json(new { success = false, message = "Error while deleting" });
	        }

			//path to product folder
			string productPath = @"images\product\product-" + id;
			//Complete path to right folder
			string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

			if (Directory.Exists(finalPath))
			{
				string[] filePaths = Directory.GetFiles(finalPath);
				await Parallel.ForEachAsync(filePaths, async (filePath, CancellationToken) =>
				{
					System.IO.File.Delete(filePath);
				});
				Directory.Delete(finalPath);
			}

			await _unitOfWork.Product.Remove(productToBeDeleted);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
		}

        
		#endregion
	}
}
