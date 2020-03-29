using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Rooms;
using WanderlustRealms.Models.ViewModel;

namespace WanderlustRealms.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddItem(int RoomID)
        {
            ViewBag.RoomID = RoomID;
            ViewBag.RoomName = _context.Rooms.Where(x => x.RoomID == RoomID).Select(x => x.Title).FirstOrDefault();
            ViewData["ItemList"] = new SelectList(_context.Items.Where(x => x.IsActive).OrderBy(x => x.Name), "ItemID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(RoomItemViewModel item)
        {
            var room = _context.Rooms.Find(item.RoomID);

            if (!string.IsNullOrEmpty(room.ItemIDList))
            {
                room.ItemIDList = room.ItemIDList + "," + item.ItemID.ToString();
            }
            else
            {
                room.ItemIDList = item.ItemID.ToString();
            }

            _context.Update(room);
            _context.SaveChanges();

            return RedirectToAction("Index", "Rooms", new { RoomAreaID = room.RoomAreaID });
        }

        public async Task<IActionResult> AddNPC(int RoomID)
        {
            ViewBag.RoomID = RoomID;
            ViewBag.RoomName = _context.Rooms.Where(x => x.RoomID == RoomID).Select(x => x.Title).FirstOrDefault();

            ViewData["NPCList"] = new SelectList(_context.NPCs.Where(x => x.IsActive).OrderBy(x => x.Name), "LivingID", "Name");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNPC(RoomNPCViewModel model)
        {
            var room = _context.Rooms.Find(model.RoomID);

            if (!string.IsNullOrEmpty(room.NPCIDList))
            {
                room.NPCIDList = room.NPCIDList + "," + model.LivingID.ToString();
            }
            else
            {
                room.NPCIDList = model.LivingID.ToString();
            }

            _context.Update(room);
            _context.SaveChanges();

            return RedirectToAction("Index", "Rooms", new { RoomAreaID = room.RoomAreaID });
        }

        public async Task<IActionResult> Index(int RoomAreaID)
        {
            ViewBag.RoomAreaID = RoomAreaID;
            ViewBag.RoomAreaName = _context.RoomAreas.Where(x => x.RoomAreaID == RoomAreaID).Select(x => x.Name).FirstOrDefault();

            var applicationDbContext = _context.Rooms.Where(x => x.RoomAreaID == RoomAreaID).Include(r => r.RoomArea);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomArea)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        public IActionResult Create(int RoomAreaID)
        {
            Room r = new Room();
            r.RoomAreaID = RoomAreaID;

            ViewData["RoomAreaID"] = new SelectList(_context.RoomAreas, "RoomAreaID", "RoomAreaID");
            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { RoomAreaID = room.RoomAreaID });
            }
            ViewData["RoomAreaID"] = new SelectList(_context.RoomAreas, "RoomAreaID", "RoomAreaID", room.RoomAreaID);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomAreaID"] = new SelectList(_context.RoomAreas, "RoomAreaID", "RoomAreaID", room.RoomAreaID);
            return View(room);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { RoomAreaID = room.RoomAreaID });
            }
            ViewData["RoomAreaID"] = new SelectList(_context.RoomAreas, "RoomAreaID", "RoomAreaID", room.RoomAreaID);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomArea)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomID == id);
        }
    }
}
