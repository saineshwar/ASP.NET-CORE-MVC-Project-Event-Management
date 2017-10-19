using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class BookingVenueConcrete : IBookingVenue
    {
        private DatabaseContext _context;
        public BookingVenueConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public bool checkBookingAvailability(BookingVenue objBV)
        {
            try
            {
                if (objBV != null)
                {
                    var booking = (from Bb in _context.BookingDetails
                                   join BV in _context.BookingVenue on Bb.BookingID equals BV.BookingID
                                   where Bb.BookingDate == objBV.BookingDate && BV.VenueID == objBV.VenueID
                                   select Bb).Count();

                    if (booking > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int BookEvent(BookingDetails BookingDetail)
        {
            try
            {
                if (BookingDetail != null)
                {
                    _context.BookingDetails.Add(BookingDetail);
                    _context.SaveChanges();

                    var currentBookingID = _context.BookingDetails.OrderByDescending(u => u.BookingID).FirstOrDefault();

                    var no = currentBookingID.BookingID.ToString() == "0" ? "1" : currentBookingID.BookingID.ToString();

                    var seq = "BK" + "-" + DateTime.Now.Year + "-" + no;

                    BookingDetail.BookingNo = seq;
                    _context.BookingDetails.Attach(BookingDetail);
                    _context.Entry(BookingDetail).Property(x => x.BookingNo).IsModified = true;
                    _context.SaveChanges();

                    return BookingDetail.BookingID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? BookVenue(BookingVenue objBV)
        {
            try
            {
                _context.BookingVenue.Add(objBV);
                _context.SaveChanges();
                return objBV.VenueID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int CancelBooking(int? BookingID)
        {
            try
            {
                if (BookingID != 0)
                {
                    BookingDetails objBD = _context.BookingDetails.Find(BookingID);
                    objBD.BookingApproval = "C";
                    _context.Entry(objBD).Property(x => x.BookingApprovalDate).IsModified = true;
                    return _context.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<BookingDetails> ShowBookingDetail(int UserID)
        {
            try
            {
                var ResultBookingDetail = (from tempBD in _context.BookingDetails
                                           where tempBD.Createdby == UserID
                                           select tempBD).ToList();

                return ResultBookingDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BookingDetails> ShowBookingDetail()
        {
            try
            {
                var ResultBookingDetail = (from tempBD in _context.BookingDetails
                                           select tempBD).ToList();

                return ResultBookingDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateBookingStatus(string BookingNo, string BookingStatus)
        {
            try
            {
                BookingDetails BD = (from BDs in _context.BookingDetails
                                     where BDs.BookingNo == BookingNo
                                     select BDs).Single();

                BD.BookingApproval = BookingStatus;
                BD.BookingApprovalDate = DateTime.Now;
                _context.BookingDetails.Attach(BD);
                _context.Entry(BD).Property(x => x.BookingApproval).IsModified = true;
                _context.Entry(BD).Property(x => x.BookingApprovalDate).IsModified = true;
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateBookingStatus(int BookingID)
        {
            try
            {
                BookingDetails bookingdetail = (from bookingdetails in _context.BookingDetails
                                     where bookingdetails.BookingID == BookingID
                                     select bookingdetails).Single();

                bookingdetail.BookingCompletedFlag = "C";
                _context.BookingDetails.Attach(bookingdetail);
                _context.Entry(bookingdetail).Property(x => x.BookingCompletedFlag).IsModified = true;
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<BookingDetailTemp> ShowAllBooking(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableBooking = (from tempbooking in _context.BookingDetails
                                     where tempbooking.BookingCompletedFlag == "C"
                                     select new BookingDetailTemp
                                     {
                                         BookingDate = tempbooking.BookingDate.Value.ToString("dd/MM/yyyy"),
                                         BookingApprovalDate = tempbooking.BookingApprovalDate == null ? "------" : tempbooking.BookingApprovalDate.Value.ToString("dd/MM/yyyy"),
                                         BookingNo = tempbooking.BookingNo,
                                         BookingID = tempbooking.BookingID,
                                         BookingApproval = tempbooking.BookingApproval == "P" ? "Pending" : tempbooking.BookingApproval == "R" ? "Rejected" : tempbooking.BookingApproval == "C" ? "Cancelled" : "Approved",
                                     });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableBooking = IQueryableBooking.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableBooking = IQueryableBooking.Where(m => m.BookingNo == Search);
            }

            return IQueryableBooking;
        }

        public IQueryable<BookingDetailTemp> ShowAllBookingUser(string sortColumn, string sortColumnDir, string Search, int Createdby)
        {
            var IQueryableBooking = (from tempbooking in _context.BookingDetails
                                     where tempbooking.BookingCompletedFlag == "C" && tempbooking.Createdby == Createdby
                                     select new BookingDetailTemp
                                     {
                                         BookingDate = tempbooking.BookingDate.Value.ToString("dd/MM/yyyy"),
                                         BookingApprovalDate = tempbooking.BookingApprovalDate == null ? "------" : tempbooking.BookingApprovalDate.Value.ToString("dd/MM/yyyy"),
                                         BookingNo = tempbooking.BookingNo,
                                         BookingID = tempbooking.BookingID,
                                         BookingApproval = tempbooking.BookingApproval == "P" ? "Pending" : tempbooking.BookingApproval == "R" ? "Rejected" : tempbooking.BookingApproval == "C" ? "Cancelled" : "Approved",
                                     });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableBooking = IQueryableBooking.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableBooking = IQueryableBooking.Where(m => m.BookingNo == Search);
            }

            return IQueryableBooking;
        }

    }
}
