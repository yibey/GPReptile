using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GPReptile.Data;
using GPReptile.Models;
using GPReptile.Service;

namespace GPReptile.Controllers
{
    public class DayTransactsController : Controller
    {
        private readonly GPContext _context;
        private readonly NeteaseDTCrawler _service;
        public DayTransactsController(GPContext context, NeteaseDTCrawler service)
        {
            _context = context;
            _service = service;
        }

        // GET: DayTransacts
        public async Task<IActionResult> Index()
        {





            var list = await _context.DayTransact.ToListAsync();

            var re = new GPViewModel()
            {
                code = "",
                list = list
            };

            return View(re);
        }




        [HttpPost]
        // GET: DayTransacts
        public async Task<IActionResult> Index(string code)
        {
            //_service.download(code, "测试", "20200526", "20200526");

            int i = 0;
            foreach (var share in _context.Share.ToArray())
            {


                try
                {
                    _service.download(share.code, share.name, "20200301", "20200601");
                }
                catch { }
                i++;
                if (i % 500 == 0)
                {
                    _context.DayTransact.AddRange(_service.getDtList());
                    await _context.SaveChangesAsync();

                    _service.clearList();
                }
              
            }




            var list = _service.getDtList();

            _context.DayTransact.AddRange(list);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [HttpPost]
        // GET: DayTransacts
        public async Task<IActionResult> InitShare()
        {

            var list = new List<Share>();
            foreach (var dic in _service.downloadGP())
            {

                var share = _context.Share.Where(r => r.code == dic.Key).FirstOrDefault();

                if (share == null)
                {
                    share = new Share()
                    {

                        name = dic.Value,
                        code = dic.Key

                    };
                    list.Add(share);
                }
            }

            _context.Share.AddRange(list.ToArray());
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: DayTransacts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayTransact = await _context.DayTransact
                .FirstOrDefaultAsync(m => m.id == id);
            if (dayTransact == null)
            {
                return NotFound();
            }

            return View(dayTransact);
        }

        // GET: DayTransacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DayTransacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,day,code,name,tclose,high,low,topen,lclose,chg,pchg,turnover,voturnover,vaturnover,tcap,mcap")] DayTransact dayTransact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dayTransact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dayTransact);
        }

        // GET: DayTransacts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayTransact = await _context.DayTransact.FindAsync(id);
            if (dayTransact == null)
            {
                return NotFound();
            }
            return View(dayTransact);
        }

        // POST: DayTransacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("id,day,code,name,tclose,high,low,topen,lclose,chg,pchg,turnover,voturnover,vaturnover,tcap,mcap")] DayTransact dayTransact)
        {
            if (id != dayTransact.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dayTransact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayTransactExists(dayTransact.id))
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
            return View(dayTransact);
        }

        // GET: DayTransacts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayTransact = await _context.DayTransact
                .FirstOrDefaultAsync(m => m.id == id);
            if (dayTransact == null)
            {
                return NotFound();
            }

            return View(dayTransact);
        }

        // POST: DayTransacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dayTransact = await _context.DayTransact.FindAsync(id);
            _context.DayTransact.Remove(dayTransact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayTransactExists(long id)
        {
            return _context.DayTransact.Any(e => e.id == id);
        }
    }
}
