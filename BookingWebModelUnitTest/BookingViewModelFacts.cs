using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.AutoFixture.Xunit;
using Ploeh.Samples.Booking.WebModel;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.SemanticComparison.Fluent;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.WebModel.UnitTest
{
    public class BookingViewModelFacts
    {
        [Theory, AutoWebData]
        public void DateIsCorrect([Frozen]DateTime date, BookingViewModel sut)
        {
            Assert.Equal(date, sut.Date);
        }

        [Theory, AutoWebData]
        public void NameIsCorrect([Frozen]string name, BookingViewModel sut)
        {
            Assert.Equal(name, sut.Name);
        }

        [Theory, AutoWebData]
        public void EmailIsCorrect([Frozen]string email, BookingViewModel sut)
        {
            Assert.Equal<string>(email, sut.Email);
        }

        [Theory, AutoWebData]
        public void QuantityIsCorrect([Frozen]int quantity, BookingViewModel sut)
        {
            Assert.Equal(quantity, sut.Quantity);
        }

        [Theory, AutoWebData]
        public void RemainingEstimateIsProperWritableProper(BookingViewModel sut, int remaining)
        {
            sut.Remaining = remaining;
            var result = sut.Remaining;
            Assert.Equal(remaining, result);
        }

        [Theory, AutoWebData]
        public void MakeNewReservationReturnsCorrectResult(BookingViewModel sut)
        {
            MakeReservationCommand result = sut.MakeNewReservation();
            var expected = sut.AsSource().OfLikeness<MakeReservationCommand>().Without(d => d.Id);
            expected.ShouldEqual(result);
        }
    }
}
