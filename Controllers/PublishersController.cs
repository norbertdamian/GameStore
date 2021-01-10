using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStore.Data;
using GameStore.Models;
using GameStore.Models.StoreViewModels;

namespace GameStore.Controllers
{
    public class PublishersController : Controller
    {
        private readonly StoreContext _context;

        public PublishersController(StoreContext context)
        {
            _context = context;
        }

        // GET: Publishers
        public async Task<IActionResult> Index(int? id, int? gameID)
        {
            var viewModel = new PublisherIndexData();
            viewModel.Publishers = await _context.Publishers
                .Include(i => i.PublishedGames)
                .ThenInclude(i => i.Game)
                .ThenInclude(i => i.Orders)
                .ThenInclude(i => i.Customer)
                .AsNoTracking()
                .OrderBy(i => i.PublisherName)
                .ToListAsync();
            if (id != null)
            {
                ViewData["PublisherID"] = id.Value;
                Publisher publisher = viewModel.Publishers.Where(
                i => i.ID == id.Value).Single();
                viewModel.Games = publisher.PublishedGames.Select(s => s.Game);
            }
            if (gameID != null)
            {
                ViewData["BoookID"] = gameID.Value;
                viewModel.Orders = viewModel.Games.Where(
                x => x.ID == gameID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PublisherName,Adress")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers
            .Include(i => i.PublishedGames).ThenInclude(i => i.Game)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            PopulatePublishedGameData(publisher);
            return View(publisher);
        }
        private void PopulatePublishedGameData(Publisher publisher)
        {
            var allGames = _context.Games;
            var publisherGames = new HashSet<int>(publisher.PublishedGames.Select(c => c.GameID));
            var viewModel = new List<PublishedGameData>();
            foreach (var game in allGames)
            {
                viewModel.Add(new PublishedGameData
                {
                    GameID = game.ID,
                    Title = game.Title,
                    IsPublished = publisherGames.Contains(game.ID)
                });
            }
            ViewData["Games"] = viewModel;
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedGames)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisherToUpdate = await _context.Publishers
            .Include(i => i.PublishedGames)
            .ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Publisher>(
            publisherToUpdate,
            "",
            i => i.PublisherName, i => i.Adress))
            {
                UpdatePublishedGames(selectedGames, publisherToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedGames(selectedGames, publisherToUpdate);
            PopulatePublishedGameData(publisherToUpdate);
            return View(publisherToUpdate);
        }
        private void UpdatePublishedGames(string[] selectedGames, Publisher publisherToUpdate)
        {
            if (selectedGames == null)
            {
                publisherToUpdate.PublishedGames = new List<PublishedGame>();
                return;
            }
            var selectedGamesHS = new HashSet<string>(selectedGames);
            var publishedGames = new HashSet<int>
            (publisherToUpdate.PublishedGames.Select(c => c.Game.ID));
            foreach (var game in _context.Games)
            {
                if (selectedGamesHS.Contains(game.ID.ToString()))
                {
                    if (!publishedGames.Contains(game.ID))
                    {
                        publisherToUpdate.PublishedGames.Add(new PublishedGame { PublisherID = publisherToUpdate.ID, GameID = game.ID });
                    }
                }
                else
                {
                    if (publishedGames.Contains(game.ID))
                    {
                        PublishedGame gameToRemove = publisherToUpdate.PublishedGames.FirstOrDefault(i => i.GameID == game.ID);
                        _context.Remove(gameToRemove);
                    }
                }
            }
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.ID == id);
        }
    }
}
