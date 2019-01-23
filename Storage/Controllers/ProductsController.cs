using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storage.Models;

namespace Storage.Controllers
{
    public class ProductsController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StorageContext _context;
        

        public ProductsController(StorageContext context)
        {
            _context = context;
            
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
      
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Category = FirstCharToUpper(product.Category);
                product.Name = FirstCharToUpper(product.Name);
                product.Shelf = FirstCharToUpper(product.Shelf);
                product.Description = FirstCharToUpper(product.Description);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.Category = FirstCharToUpper(product.Category);
                    product.Name = FirstCharToUpper(product.Name);
                    product.Shelf = FirstCharToUpper(product.Shelf);
                    product.Description = FirstCharToUpper(product.Description);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Viev model
        // GET: MittRes 
       
        public async Task<IActionResult> myView()
        {
            var myL = await _context.Product.Select(i => new ProductViewModel() {   Name=i.Name, Price=i.Price, Count=i.Count, Sum = i.Count * i.Price }).ToArrayAsync();
            return View(myL);
        }
        public async Task<IActionResult> Electronics()
        {
            var model = await _context.Product.Where(i => i.Category == "Electronics").ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> EmptyInStock()
        {
            var model = await _context.Product.Where(i => i.Count <=0).ToListAsync();
            return View(model);
        }
        public IActionResult Seek()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Seek(string Name)
        {         
            var model = await _context.Product.Where(i => i.Name == Name).ToListAsync();
            return View("Index", model);
        }


        public async Task<IActionResult> XSeek(string CategoryId, string searchString)
        {
            // New Master
            IQueryable<string> catQuery = from m in _context.Product
                                            orderby m.Name
                                            select m.Category; //--

            var product = from m in _context.Product
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(CategoryId))
            {
                product = product.Where(x => x.Category == CategoryId);
            }

            var catProdVM = new ViewModel
            {
                Category = new SelectList(await catQuery.Distinct().ToListAsync()),
                Products = await product.ToListAsync()
            };

            return View(catProdVM);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }


        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
        
    }
}
