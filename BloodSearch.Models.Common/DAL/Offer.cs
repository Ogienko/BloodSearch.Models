using BloodSearch.Models.Api.Models.Offers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodSearch.Models.Common.DAL {

    [Table("offers")]
    public class Offer {

        public long Id { get; set; }

        public OfferTypeEnum Type { get; set; }

        public int? UserId { get; set; }

        public string JData { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public OfferStateEnum State { get; set; }

        [ConcurrencyCheck]
        public string RowVersion { get; set; }

        public DateTime RowChangedDate { get; set; }

        public RowProcessingStatusEnum RowProcessingStatus { get; set; }
    }
}