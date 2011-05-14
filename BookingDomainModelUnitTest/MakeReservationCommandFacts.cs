using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.AutoFixture.Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.SemanticComparison.Fluent;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class MakeReservationCommandFacts
    {
        [Theory, AutoDomainData]
        public void DateIsCorrect([Frozen]DateTime date, MakeReservationCommand sut)
        {
            Assert.Equal<DateTime>(date, sut.Date);
        }

        [Theory, AutoDomainData]
        public void EmailIsCorrect([Frozen]string email, MakeReservationCommand sut)
        {
            Assert.Equal<string>(email, sut.Email);
        }

        [Theory, AutoDomainData]
        public void NameIsCorrect([Frozen]string name, MakeReservationCommand sut)
        {
            Assert.Equal<string>(name, sut.Name);
        }

        [Theory, AutoDomainData]
        public void QuantityIsCorrect([Frozen]int quantity, MakeReservationCommand sut)
        {
            Assert.Equal<int>(quantity, sut.Quantity);
        }

        [Theory, AutoDomainData]
        public void IdIsUnique(MakeReservationCommand sut, MakeReservationCommand other)
        {
            Assert.NotEqual<Guid>(other.Id, sut.Id);
        }

        [Theory, AutoDomainData]
        public void IdIsStable(MakeReservationCommand sut)
        {
            Assert.Equal(sut.Id, sut.Id);
        }

        [Theory, AutoDomainData]
        public void AcceptReturnsCorrectResult(MakeReservationCommand sut)
        {
            var expected = sut.AsSource().OfLikeness<ReservationAcceptedEvent>();
            ReservationAcceptedEvent result = sut.Accept();
            expected.ShouldEqual(result);
        }

        [Theory, AutoDomainData]
        public void RejectReturnsCorrectResult(MakeReservationCommand sut)
        {
            var expected = sut.AsSource().OfLikeness<ReservationRejectedEvent>();
            ReservationRejectedEvent result = sut.Reject();
            expected.ShouldEqual(result);
        }
    }
}
