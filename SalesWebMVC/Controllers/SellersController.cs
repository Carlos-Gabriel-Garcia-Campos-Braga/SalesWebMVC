using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System.Threading;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _SellerService;
        private readonly DepartmentService _DepartmentService;
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _SellerService = sellerService;
            _DepartmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _SellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _DepartmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller Seller)
        {
            _SellerService.Insert(Seller);
            return RedirectToAction(nameof(Index));
        }
    }
}
