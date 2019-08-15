using System;
using System.Data.Entity;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using GigHub.ViewModels;
using GigHub.Repositories;

namespace GigHub.Controllers.Api
{
    [RoutePrefix("api/gigs")]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly GicRepository _gigRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _gigRepository = new GicRepository(_context);

        }

        [Route("{id:int}", Name = "GetGicById")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var gig = _gigRepository.GetGicById(id);

            if (gig.IsCanceled)
                return NotFound();

            return Ok(gig);
        }


        [HttpPost]
        public IHttpActionResult Create([FromBody]GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //viewModel.Genres = _context.Genres.ToList();
                return BadRequest();//the model sent is not valid
            }

            var gig = new Gig
            {
                ArtistId = "f4c311ce-31f1-4459-ab9d-edc647dd37fc",
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            var id = _gigRepository.AddGig(gig);

            return Ok(id);
            // return RedirectToAction("Mine", "Gigs");
        }



        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id && g.ArtistId == userId);
            
            if (gig.IsCanceled)
                return NotFound();

            gig.Cancel();

            _context.SaveChanges();

            return Ok();
        }

       
        

        /*
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
        */

    }
}
