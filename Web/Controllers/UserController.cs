using BloodSearch.Core.Models;
using BloodSearch.Models.Api.Models.Auth;
using BloodSearch.Models.Api.Models.Auth.Request;
using BloodSearch.Models.Api.Models.Auth.Response;
using BloodSearch.Models.Common;
using BloodSearch.Models.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Web.Infrastructure;

namespace Web.Controllers {

    public class UserController : ApiController {

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        [Route("api/user/login")]
        [HttpPost]
        public LoginResult Login(LoginModel model) {

            if (model == null) {
                return new LoginResult() {
                    Success = false,
                    ErrMessages = new List<KeyMsg>() {
                        ResponseError.GetError(TypeError.RequestEmpty)
                    }
                };
            }

            var result = new LoginResult();

            using (var db = new BloodSearchContext()) {
                using (var dbContextTransaction = db.Database.BeginTransaction()) {
                    try {

                        var user = db.Users.FirstOrDefault(x => x.Email == model.Email.ToLower() && x.PasswordHash == model.PasswordHash);

                        result.IsAuth = user != null;

                        if (user == null) {
                            result.ErrMessages.Add(ResponseError.GetError(TypeError.UserNotFound));
                            dbContextTransaction.Rollback();
                            return result;
                        }

                        result.Token = Crypt.GetToken();
                        result.UserId = user.Id;
                        db.AuthTokens.Add(new AuthToken() {
                            Token = result.Token,
                            CreatedDate = DateTime.UtcNow,
                            ExpiryDate = DateTime.UtcNow.AddDays(1),
                            Ip = model.Ip,
                            UserId = user.Id
                        });
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    } catch (Exception ex) {
                        dbContextTransaction.Rollback();
                        return new LoginResult() {
                            Success = false,
                            ErrMessages = new List<KeyMsg> {
                                ResponseError.GetError(TypeError.DataSaveError)
                            }
                        };
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [Route("api/user/registration")]
        [HttpPost]
        public BaseResponse Registration(RegistrationModel model) {

            if (model == null) {
                return new BaseResponse() {
                    Success = false,
                    ErrMessages = new List<KeyMsg>() {
                        ResponseError.GetError(TypeError.RequestEmpty)
                    }
                };
            }

            var result = new BaseResponse();

            using (var db = new BloodSearchContext()) {
                using (var dbContextTransaction = db.Database.BeginTransaction()) {
                    try {
                        if (db.Users.All(x => x.Email != model.Email.ToLower())) {
                            db.Users.Add(new User {
                                PasswordHash = model.PasswordHash,
                                Email = model.Email.ToLower(),
                                RegisterFromIp = model.RegisterFromIp,
                                CreateDate = DateTime.UtcNow,
                                ChangedDate = DateTime.UtcNow

                            });

                            db.SaveChanges();
                            dbContextTransaction.Commit();
                        } else {
                            dbContextTransaction.Rollback();
                            result.Success = false;
                            result.ErrMessages.Add(ResponseError.GetError(TypeError.WhichUserIsAlready));
                        }
                    } catch (Exception ex) {
                        dbContextTransaction.Rollback();
                        return new BaseResponse() {
                            Success = false,
                            ErrMessages = new List<KeyMsg> {
                                ResponseError.GetError(TypeError.DataSaveError)
                            }
                        };
                    }
                }
            }
            return result;
        }
    }
}