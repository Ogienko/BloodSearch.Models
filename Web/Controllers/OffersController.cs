using BloodSearch.Models.Api.Models.Offers;
using BloodSearch.Models.Api.Models.Offers.Requests;
using BloodSearch.Models.Common;
using BloodSearch.Models.Common.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using static BloodSearch.Core.Utils.CommonUtils;

namespace Web.Controllers {

    public class OffersController : ApiController {

        [HttpPost]
        [Route("api/offers/add-offer")]
        public AddOfferResult AddOffer(AddOfferModel model) {
            var ret = new AddOfferResult { ErrMessages = ValidateOfferModel(model) };

            if (ret.ErrMessages.Any()) {
                ret.Success = false;
                return ret;
            }

            var offer = Retry.Do(() => SaveOffer(model));

            ret.Id = offer.Id;
            ret.Success = true;

            return ret;
        }

        [HttpGet]
        [Route("api/offers/get")]
        public GetOfferResult Get(long id) {
            var context = new BloodSearchContext();
            var offer = context.Offers.AsNoTracking().Single(x => x.Id == id);
            return ConvertOffer(offer);
        }

        [HttpGet]
        [Route("api/offers/get-by-user")]
        public List<GetOfferResult> GetByUser(int userId) {
            var context = new BloodSearchContext();
            var offers = context.Offers.Where(x => x.UserId.HasValue && x.UserId.Value == userId).ToList();
            var result = offers.Select(ConvertOffer).ToList();
            return result;
        }

        [HttpPost]
        [Route("api/offers/get-offers")]
        public GetOffersResult GetOffers(GetOffersByFiltersParameters parameters) {

            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            var filter = parameters.Filter;
            var pagingFilter = parameters.PagingFilter;

            var context = new BloodSearchContext();

            var query = context.Offers.Where(x => x.Type == filter.Type);

            var items = query.Where(x => filter.Statuses.Any(z => z == x.State)).ToList();

            var totalOffers = items.Select(ConvertOffer);

            if (filter.Categories.Any()) {
                totalOffers = totalOffers.Where(x => filter.Categories.Any(z => z == x.Offer.Category));
            }

            if (filter.Region != 1) {
                totalOffers = totalOffers.Where(x => filter.Region == x.Offer.GeoAddress.Id);
            }

            var totalCount = totalOffers.Count();

            var offers = totalOffers.OrderBy(x => x.Id)
                           .Skip((pagingFilter.PageNumber - 1) * pagingFilter.PageSize)
                           .Take(pagingFilter.PageSize)
                           .ToList();

            return new GetOffersResult {
                Offers = offers,
                TotalCount = totalCount
            };
        }

        private Offer SaveOffer(AddOfferModel model) {
            var context = new BloodSearchContext();

            Offer offer = null;
            if (model.Id.HasValue && model.Id > 0) {
                offer = context.Offers.FirstOrDefault(x => x.Id == model.Id);
            }

            if (offer == null) {
                offer = new Offer {
                    CreatedDate = DateTime.Now,
                    ChangedDate = DateTime.Now,
                    State = OfferStateEnum.Published,
                    Type = model.Type,
                    UserId = model.UserId
                };
            }

            if (offer.Id == 0) {
                context.Offers.Add(offer);
            }

            var jOfferModel = JsonConvert.SerializeObject(model.Offer);

            if (offer.JData != jOfferModel) {
                offer.JData = jOfferModel;
                offer.RowProcessingStatus = RowProcessingStatusEnum.New;
                offer.RowChangedDate = DateTime.Now;
                offer.ChangedDate = DateTime.Now;
            }

            if (offer.RowProcessingStatus != RowProcessingStatusEnum.New) {
                offer.RowProcessingStatus = RowProcessingStatusEnum.New;
                offer.RowChangedDate = DateTime.Now;
                offer.ChangedDate = DateTime.Now;
            }

            if (offer.UserId != model.UserId) {
                offer.UserId = model.UserId;
                offer.RowChangedDate = DateTime.Now;
                offer.ChangedDate = DateTime.Now;
            }

            context.SaveChanges();
            context.Entry(offer).State = System.Data.Entity.EntityState.Detached;

            return offer;
        }

        private List<KeyValuePair<string, string>> ValidateOfferModel(AddOfferModel model) {
            var context = new BloodSearchContext();

            var ret = new List<KeyValuePair<string, string>>();

            if (model.Id < 0) {
                ret.Add(new KeyValuePair<string, string>("offer-id-out-of-range", "неверное значение offerId"));
            }

            if (model.Offer == null) {
                ret.Add(new KeyValuePair<string, string>("offer-is-empty", "offer не может быть пустым"));
            }

            if (model.Id > 0) {
                var offer = context.Offers.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
                if (offer == null) {
                    ret.Add(new KeyValuePair<string, string>("can't-find-offer", $"не найдено объявления id = {model.Id}"));
                }
            }

            return ret;
        }

        private GetOfferResult ConvertOffer(Offer offer) {
            OfferModel offerModel = null;
            try {
                offerModel = string.IsNullOrWhiteSpace(offer.JData) ? null : JsonConvert.DeserializeObject<OfferModel>(offer.JData);
            } catch (Exception exc) {
                offerModel = new OfferModel();
            }

            return new GetOfferResult {
                Id = offer.Id,
                Type = offer.Type,
                State = offer.State,
                Offer = offerModel,
                UserId = offer.UserId,
                CreatedDate = offer.CreatedDate,
                UpdatedDate = offer.ChangedDate
            };
        }
    }
}