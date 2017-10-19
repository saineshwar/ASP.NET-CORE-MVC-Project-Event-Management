using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class VenueConcrete : IVenue
    {
        private DatabaseContext _context;

        public VenueConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void SaveVenue(Venue venue)
        {
            try
            {
                _context.Venue.Add(venue);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Venue> ShowVenue(string sortColumn , string sortColumnDir , string Search)
        {
            var IQueryableVenue = (from tempvenue in _context.Venue select tempvenue);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableVenue = IQueryableVenue.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableVenue = IQueryableVenue.Where(m => m.VenueName == Search);
            }

            return IQueryableVenue;
        }

        public IEnumerable<Venue> ShowAllVenue()
        {
            return _context.Venue.ToList();
        }

        public void UpdateVenue(Venue venue)
        {
            try
            {
                if (venue.VenueFilename != null)
                {
                    _context.Entry(venue).Property(x => x.VenueName).IsModified = true;
                    _context.Entry(venue).Property(x => x.VenueCost).IsModified = true;
                    _context.Entry(venue).Property(x => x.VenueFilename).IsModified = true;
                    _context.Entry(venue).Property(x => x.VenueFilePath).IsModified = true;
                    _context.Entry(venue).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Venue.Attach(venue);
                    _context.Entry(venue).Property(x => x.VenueName).IsModified = true;
                    _context.Entry(venue).Property(x => x.VenueCost).IsModified = true;
                    _context.Entry(venue).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Venue VenueByID(int id)
        {
            try
            {
                Venue venue = _context.Venue.Find(id);
                return venue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteVenue(int id)
        {
            try
            {
                Venue venue = _context.Venue.Find(id);
                _context.Venue.Remove(venue);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckVenueNameAlready(string venueName)
        {
            var VenueCount = (from venue in _context.Venue
                              where venue.VenueName == venueName
                              select venue).Count();

            if (VenueCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
