using BloodSearch.Models.Common.DAL;
using System;
using System.Data.Entity;
using System.Linq;

namespace BloodSearch.Models.Common {

    public class BloodSearchContext : DbContext {

        public BloodSearchContext() : base("BloodSearchDB") {
            Database.SetInitializer<BloodSearchContext>(null);
        }

        public DbSet<Offer> Offers { get; set; }

        public Offer SaveOfferWithConcurencyCheck(Offer offer, RowProcessingStatusEnum? status = null) {
            if (status.HasValue) {
                offer.RowProcessingStatus = status.Value;
            }
            offer.RowChangedDate = DateTime.Now;
            this.SaveChanges();
            this.Entry(offer).State = EntityState.Detached;

            return this.Offers.Single(x => x.Id == offer.Id);
        }
    }
}