using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
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
        public IActionResult checkPwd(string account, string pwd)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd))
                return Content("請輸入完整的帳號和密碼");
            else
            {
                string result = "0";
                AdminMember member = _context.AdminMembers.FirstOrDefault(u => u.Account == account && u.Password == pwd);
                if (member != null)
                    result = "1";

                return Content(result);

            }
        }
        public IActionResult ALogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ALogin(AdminMember admin)
        {
            AdminMember member = _context.AdminMembers.FirstOrDefault(u => u.Account == admin.Account && u.Password == admin.Password);
            if (member == null)
                return RedirectToAction("ALogin");

            string json = JsonSerializer.Serialize(member);
            HttpContext.Session.SetString(CDictionary.SK_LOGINED_ADMIN, json);
            //return RedirectToAction("ANormalMemberList");
            return RedirectToAction("Index");
        }

        public IActionResult ANormalMemberList()
        {
            return View();
        }


        // GET: AdminMembers
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_ADMIN))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_ADMIN);
                AdminMember admin = JsonSerializer.Deserialize<AdminMember>(json);
                ViewBag.AdminAcc = admin.Account;
            }
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
