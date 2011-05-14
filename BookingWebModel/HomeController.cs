using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ploeh.Samples.Booking.DomainModel;

namespace Ploeh.Samples.Booking.WebModel
{
    public class HomeController : Controller
    {
        private readonly IChannel channel;
        private readonly IMonthViewReader monthReader;
        private readonly IDayViewReader dayReader;

        public HomeController(IChannel channel, IMonthViewReader monthReader, IDayViewReader dayReader)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }
            if (monthReader == null)
            {
                throw new ArgumentNullException("monthReader");
            }
            if (dayReader == null)
            {
                throw new ArgumentNullException("dayReader");
            }
            
            this.channel = channel;
            this.monthReader = monthReader;
            this.dayReader = dayReader;
        }

        public ViewResult Index()
        {
            var now = DateTime.Now;
            var model = this.monthReader.Read(now.Year, now.Month);
            return this.View(model);
        }

        [OutputCache(Duration = 0, VaryByParam = "none")]
        public JsonResult DisabledDays(int year, int month)
        {
            var data = this.monthReader.Read(year, month);
            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ViewResult NewBooking(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var remaining = this.dayReader.GetRemainingCapacity(date);
            var model = new BookingViewModel { Date = date, Remaining = remaining };
            return this.View(model);
        }

        [HttpPost]
        public ViewResult NewBooking(BookingViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            this.channel.Send(model.MakeNewReservation());
            return this.View("BookingReceipt", model);
        }
    }
}
