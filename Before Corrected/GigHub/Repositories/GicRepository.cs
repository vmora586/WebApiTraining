using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GigHub.Repositories
{
    public class GicRepository
    {
        private readonly ApplicationDbContext _context;

        public GicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGicById(int id)
        {
            /*
              var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id);
             */

            return _context.Gigs.Where(g => g.Id == id)
           .Include(g => g.Attendances.Select(a => a.Attendee)).FirstOrDefault();
        }

        public int AddGig(Gig gig)
        {
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return gig.Id;
        }
    }
}