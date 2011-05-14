using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    [Serializable]
    public class Capacity : IEquatable<Capacity>
    {
        private readonly int remaining;
        private readonly IEnumerable<Guid> acceptedReservations;

        public Capacity(int remaining, params Guid[] acceptedReservations)
        {
            if (acceptedReservations == null)
            {
                throw new ArgumentNullException("acceptedReservations");
            }

            this.remaining = remaining;
            this.acceptedReservations = acceptedReservations;
        }

        public int Remaining
        {
            get { return this.remaining; }
        }

        public bool CanReserve(int quantity, Guid id)
        {
            if (this.IsReplay(id))
            {
                return true;
            }
            return this.remaining >= quantity;
        }

        private bool IsReplay(Guid id)
        {
            return this.acceptedReservations.Contains(id);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Capacity;
            if (other != null)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.acceptedReservations
                .Select(g => g.GetHashCode())
                .Aggregate((x, y) => x ^ y) ^ this.Remaining;
        }

        public Capacity Reserve(int quantity, Guid id)
        {
            if (!this.CanReserve(quantity, id))
            {
                throw new ArgumentOutOfRangeException("quantity", "The quantity must be less than or equal to the remaining quantity.");
            }

            if (this.IsReplay(id))
            {
                return this;
            }

            return new Capacity(this.Remaining - quantity, 
                this.acceptedReservations
                    .Concat(new[] { id }).ToArray());
        }

        #region IEquatable<Capacity> Members

        public bool Equals(Capacity other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Remaining == other.Remaining)
                && !this.acceptedReservations.Except(other.acceptedReservations).Any();
        }

        #endregion
    }
}
