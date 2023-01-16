using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.Controllers
{
    public class AdminMembersController : Controller
    {
        private readonly ispanMsit145shibaContext _context;

        public AdminMembersController(ispanMsit145shibaContext context)
        {
            _context = context;
        }

        public IActionResult ALogin()
        {
            return View();
        }

        // GET: AdminMembers
        public async Task<IActionResult> Index()
        {
              return View(await _context.AdminMembers.ToListAsync());
        }

        // GET: AdminMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdminMembers == null)
            {
                return NotFound();
            }

            var adminMember = await _context.AdminMembers
                .FirstOrDefaultAsync(m => m.Fid == id);
            if (adminMember == null)
            {
                return NotFound();
            }

            return View(adminMember);
        }

        // GET: AdminMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fid,Account,Password,RoleLevel")] AdminMember adminMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminMember);
        }

        // GET: AdminMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdminMembers == null)
            {
                return NotFound();
            }

            var adminMember = await _context.AdminMembers.FindAsync(id);
            if (adminMember == null)
            {
                return NotFound();
            }
            return View(adminMember);
        }

        // POST: AdminMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Fid,Account,Password,RoleLevel")] AdminMember adminMember)
        {
            if (id != adminMember.Fid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminMemberExists(adminMember.Fid))
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
            return View(adminMember);
        }

        // GET: AdminMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdminMembers == null)
            {
                return NotFound();
            }

            var adminMember = await _context.AdminMembers
                .FirstOrDefaultAsync(m => m.Fid == id);
            if (adminMember == null)
            {
                return NotFound();
            }

            return View(adminMember);
        }

        // POST: AdminMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdminMembers == null)
            {
                return Problem("Entity set 'ispanMsit145shibaContext.AdminMembers'  is null.");
            }
            var adminMember = await _context.AdminMembers.FindAsync(id);
            if (adminMember != null)
            {
                _context.AdminMembers.Remove(adminMember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminMemberExists(int id)
        {
          return _context.AdminMembers.Any(e => e.Fid == id);
        }
    }
}
