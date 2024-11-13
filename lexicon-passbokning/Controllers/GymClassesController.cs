using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lexicon_passbokning.Data;
using lexicon_passbokning.Models;
using lexicon_passbokning.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace lexicon_passbokning.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymClasses.ToListAsync());
        }

        [Authorize]
        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .Include(g => g.AttendingClasses)
                    .ThenInclude(ug => ug.ApplicationUser)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymClassEditCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gymClass = new GymClass
                {
                    Name = model.Name,
                    StartTime = model.StartTime,
                    Duration = model.Duration,
                    Description = model.Description
                };

                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            var model = new GymClassEditCreateViewModel
            {
                Id = gymClass.Id,
                Name = gymClass.Name,
                StartTime = gymClass.StartTime,
                Duration = gymClass.Duration,
                Description = gymClass.Description
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GymClassEditCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var gymClass = await _context.GymClasses.FindAsync(id);
                if (gymClass == null)
                {
                    return NotFound();
                }

                gymClass.Name = model.Name;
                gymClass.StartTime = model.StartTime;
                gymClass.Duration = model.Duration;
                gymClass.Description = model.Description;

                try
                {
                    _context.Entry(gymClass).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
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

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClasses.Remove(gymClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }

        [Authorize]
        // POST book the class
        // GymClasses/BookingToggle/5
        public async Task<IActionResult> BookingToggle(int id)
        {
            // find the class
            var gymClass = await _context.GymClasses
                .Include(user => user.AttendingClasses)
                .ThenInclude(u => u.ApplicationUser)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gymClass == null)
            {
                TempData["ErrorMessage"] = "Gym class not found.";
                return NotFound();
            }

            // define the userId
            var userId = _userManager.GetUserId(User);

            // check if user  has booked the class
            var existingBooking = gymClass.AttendingClasses
                .FirstOrDefault(user => user.ApplicationUserId == userId);

            if (existingBooking != null)
            {
                // user has already booked the class => delete booking
                gymClass.AttendingClasses.Remove(existingBooking);
                TempData["WarningMessage"] = "You have successfully unbooked the class.";
            }
            else
            {
                // book the class
                var booking = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = id
                };
                gymClass.AttendingClasses.Add(booking);
                TempData["SuccessMessage"] = "You have successfully booked the class.";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while saving your changes. Please try again later.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
