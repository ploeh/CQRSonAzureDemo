using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.WebModel;
using System.Web.Mvc;
using Ploeh.AutoFixture.Xunit;
using Moq;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.SemanticComparison.Fluent;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.WebModel.UnitTest
{
    public class HomeControllerFacts
    {
        [Theory, AutoWebData]
        public void SutIsController(HomeController sut)
        {
            Assert.IsAssignableFrom<IController>(sut);
        }

        [Theory, AutoWebData]
        public void IndexReturnsCorrectModel([Frozen]Mock<IMonthViewReader>readerStub, string[] dates, HomeController sut)
        {
            var start = DateTime.Now;
            readerStub.Setup(r => r.Read(It.Is<int>(y => start.Year <= y && y <= DateTime.Now.Year), It.Is<int>(m => start.Month <= m && m <= DateTime.Now.Month))).Returns(dates);
            ViewResult result = sut.Index();
            Assert.Equal(dates, result.ViewData.Model);
        }

        [Theory, AutoWebData]
        public void NewBookingReturnsInstance(HomeController sut, int year, int month, int day)
        {
            ViewResult result = sut.NewBooking(year, month, day);
            Assert.NotNull(result);
        }

        [Theory, AutoWebData]
        public void NewBookingReturnsCorrectTypeOfModel(HomeController sut, int year, int month, int day)
        {
            var result = sut.NewBooking(year, month, day);
            Assert.IsAssignableFrom<BookingViewModel>(result.ViewData.Model);
        }

        [Theory, AutoWebData]
        public void NewBookingReturnsModelWithCorrectDate(HomeController sut, int year, int month, int day)
        {
            var result = sut.NewBooking(year, month, day);
            var vm = Assert.IsAssignableFrom<BookingViewModel>(result.ViewData.Model);
            Assert.Equal(new DateTime(year, month, day), vm.Date);
        }

        [Theory, AutoWebData]
        public void NewBookingReturnsModelWithCorrectRemaining([Frozen]Mock<IDayViewReader> readerStub, int remaining, HomeController sut, int year, int month, int day)
        {
            readerStub.Setup(r => r.GetRemainingCapacity(new DateTime(year, month, day))).Returns(remaining);
            var result = sut.NewBooking(year, month, day);
            var vm = Assert.IsAssignableFrom<BookingViewModel>(result.ViewData.Model);
            Assert.Equal(remaining, vm.Remaining);
        }

        [Theory, AutoWebData]
        public void NewBookingPostReturnsInstance(HomeController sut, BookingViewModel model)
        {
            ViewResult result = sut.NewBooking(model);
            Assert.NotNull(result);
        }

        [Theory, AutoWebData]
        public void NewBookingPostReturnsCorrectViewName(HomeController sut, BookingViewModel model)
        {
            var result = sut.NewBooking(model);
            Assert.Equal("BookingReceipt", result.ViewName);
        }

        [Theory, AutoWebData]
        public void NewBookingPostReturnsCorrectTypeOfModel(HomeController sut, BookingViewModel model)
        {
            var result = sut.NewBooking(model);
            Assert.IsAssignableFrom<BookingViewModel>(result.ViewData.Model);
        }

        [Theory, AutoWebData]
        public void NewBookingPostReturnsCorrectModel(HomeController sut, BookingViewModel model)
        {
            var result = sut.NewBooking(model);
            Assert.Equal(model, result.ViewData.Model);
        }

        [Theory, AutoWebData]
        public void NewBookingPostCorrectlySendsOnChannel([Frozen]Mock<IChannel> channelMock, HomeController sut, BookingViewModel model)
        {
            sut.NewBooking(model);
            var expected = model.AsSource().OfLikeness<MakeReservationCommand>().Without(d => d.Id);
            channelMock.Verify(c => c.Send(expected));
        }

        [Theory, AutoWebData]
        public void DisabledDaysReturnsCorrectResult([Frozen]Mock<IMonthViewReader> readerStub, string[] disableDays, HomeController sut, int year, int month)
        {
            readerStub.Setup(r => r.Read(year, month)).Returns(disableDays);
            JsonResult result = sut.DisabledDays(year, month);
            Assert.Equal(disableDays, result.Data);
        }

        [Theory, AutoWebData]
        public void DisabledDaysReturnsCorrectJsonBehavior(HomeController sut, int year, int month)
        {
            var result = sut.DisabledDays(year, month);
            Assert.Equal(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
        }
    }
}
