using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var list = await _SellerService.FindAllAysnc();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _DepartmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller Seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _DepartmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = Seller, Departments = departments };
                return View(viewModel);
            }
            await _SellerService.InsertAsync(Seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new {message = "ID not provided!"});
            }

            var obj = await _SellerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found!" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _SellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not provided!" }); ;
            }

            var obj = await _SellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found!" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not provided!" });
            }

            var obj = await _SellerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found!" });
            }

            List<Department> departments = await _DepartmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _DepartmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "ID mismatch!" });
            }

            try
            {
                await _SellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }


    }
}
