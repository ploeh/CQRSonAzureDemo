using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.AutoFixture.Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class CapacityFacts
    {
        [Theory, AutoDomainData]
        public void RemainingIsCorrect([Frozen]int capacity, Capacity sut)
        {
            Assert.Equal<int>(capacity, sut.Remaining);
        }

        [Theory, AutoDomainData]
        public void CanReserveReturnsFalseWhenQuantityIsGreaterThanRemaining(Capacity sut, Guid id)
        {
            var greaterQuantity = sut.Remaining + 1;
            bool result = sut.CanReserve(greaterQuantity, id);
            Assert.False(result);
        }

        [Theory, AutoDomainData]
        public void CanReserveReturnsTrueWhenQuantityIsEqualToRemaining(Capacity sut, Guid id)
        {
            var result = sut.CanReserve(sut.Remaining, id);
            Assert.True(result);
        }

        [Theory, AutoDomainData]
        public void CanReserveReturnsTrueWhenQuantityIsLessThanRemaining(Capacity sut, Guid id)
        {
            var lesserQuantity = sut.Remaining - 1;
            var result = sut.CanReserve(lesserQuantity, id);
            Assert.True(result);
        }

        [Theory, AutoDomainData]
        public void CanReserveIsConsistentAccrossReplays(Capacity sut, Guid id)
        {
            var remaining = sut.Remaining;
            sut.Reserve(remaining, id);
            var result = sut.CanReserve(remaining, id);
            Assert.True(result);
        }

        [Theory, AutoDomainData]
        public void ReserveThrowsWhenQuantityIsGreaterThanRemaining(Capacity sut, Guid id)
        {
            var greaterQuantity = sut.Remaining + 1;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.Reserve(greaterQuantity, id));
        }

        [Theory, AutoDomainData]
        public void ReserveDoesNotThrowWhenQuantityIsEqualToRemaining(Capacity sut, Guid id)
        {
            Assert.DoesNotThrow(() =>
                sut.Reserve(sut.Remaining, id));
        }

        [Theory, AutoDomainData]
        public void ReserveDoesNotThrowWhenQuantityIsLessThanRemaining(Capacity sut, Guid id)
        {
            var lesserQuantity = sut.Remaining - 1;
            Assert.DoesNotThrow(() =>
                sut.Reserve(lesserQuantity, id));
        }

        [Theory, AutoDomainData]
        public void ReserveReturnsInstanceWithCorrectlyDecrementedRemaining(Guid id, int quantity, Capacity sut)
        {
            var expected = sut.Remaining - quantity;
            var result = sut.Reserve(quantity, id);
            Assert.Equal(expected, result.Remaining);
        }

        [Theory, AutoDomainData]
        public void ReserveReturnsEquivalentInstanceWhenReplayed(Guid id, int quantity, Capacity sut)
        {
            var expected = sut.Reserve(quantity, id);
            var result = sut.Reserve(quantity, id);
            Assert.Equal(expected, result);
        }

        [Theory, AutoDomainData]
        public void ReserveDoesNotHaveSideEffects(Guid id, int quantity, Capacity sut)
        {
            var result = sut.Reserve(quantity, id);
            Assert.NotEqual(result, sut);
        }

        [Theory, AutoDomainData]
        public void ReserveReturnsInstanceWithWithoutDecrementingRemainingWhenIdAlreadyExists([Frozen]Guid[] ids, int quantity, Capacity sut)
        {
            var existingId = ids.First();
            var expectedRemaining = sut.Remaining;
            var result = sut.Reserve(quantity, existingId);
            Assert.Equal(expectedRemaining, result.Remaining);
        }

        [Theory, AutoDomainData]
        public void SutIsEquatable(Capacity sut)
        {
            Assert.IsAssignableFrom<IEquatable<Capacity>>(sut);
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualNullObject(Capacity sut)
        {
            Assert.False(sut.Equals((object)null));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualNullSut(Capacity sut)
        {
            Assert.False(sut.Equals((Capacity)null));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualArbitraryOtherObject(Capacity sut, object other)
        {
            Assert.False(sut.Equals(other));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualOtherObjectWhenRemainingDiffers(Capacity sut, Capacity other)
        {
            Assert.NotEqual(sut.Remaining, other.Remaining);
            Assert.False(sut.Equals((object)other));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualOtherSutWhenRemainingDiffers(Capacity sut, Capacity other)
        {
            Assert.NotEqual(sut.Remaining, other.Remaining);
            Assert.False(sut.Equals(other));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualOtherObjectWhenDifferentReservationsHaveBeenMade([Frozen]int remaining, Capacity sut, Capacity other)
        {
            Assert.False(sut.Equals((object)other));
        }

        [Theory, AutoDomainData]
        public void SutDoesNotEqualOtherSutWhenDifferentReservationsHaveBeenMade([Frozen]int remaining, Capacity sut, Capacity other)
        {
            Assert.False(sut.Equals(other));
        }

        [Theory, AutoDomainData]
        public void SutEqualsOtherObjectWhenBothReservationsAndRemainingAreEqual([Frozen]int remaining, [Frozen]Guid[] ids, Capacity sut, Capacity other)
        {
            Assert.True(sut.Equals((object)other));
        }

        [Theory, AutoDomainData]
        public void SutEqualsOtherSutWhenBothReservationsAndRemainingAreEqual([Frozen]int remaining, [Frozen]Guid[] ids, Capacity sut, Capacity other)
        {
            Assert.True(sut.Equals(other));
        }

        [Theory, AutoDomainData]
        public void GetHashCodeReturnsCorrectResult([Frozen]Guid[] ids, Capacity sut)
        {
            var expectedHashCode = ids.Select(g => g.GetHashCode()).Aggregate((x, y) => x ^ y) ^ sut.Remaining.GetHashCode();
            Assert.Equal(expectedHashCode, sut.GetHashCode());
        }
    }
}
