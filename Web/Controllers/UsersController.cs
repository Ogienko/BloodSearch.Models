using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using BloodSearch.Core.Models;
//using Auth.Api.Models;
//using Auth.Api.Models.Request;
//using Auth.Api.Models.Response;
//using Auth.Common;
//using Auth.Common.Models;
//using Web.Infrastructure;
//using Web.Infrastructure.Extension;
//using Web.Managers;

namespace Web.Controllers {

    public class UsersController : ApiController {

        ///// <summary>
        ///// Авторизация пользователя
        ///// </summary>
        //[Route("api/user/login")]
        //[HttpPost]
        //public LoginResult Login(LoginModel model) {
        //    if (model == null)
        //        return new LoginResult() { Success = false, ErrMessages = new List<KeyMsg>() { ResponseError.GetError(TypeError.RequestEmpty) } };

        //    var result = new LoginResult();
        //    using (var db = new DataContext()) {
        //        using (var dbContextTransaction = db.Database.BeginTransaction()) {
        //            try {
        //                var user = db.Users.FirstOrDefault(u => u.Email == model.Email.ToLower() && u.PasswordHash == model.Password);
        //                result.IsAuth = user != null;
        //                if (user == null) {
        //                    result.ErrMessages.Add(ResponseError.GetError(TypeError.UserNotFound));
        //                    dbContextTransaction.Rollback();
        //                    return result;
        //                }

        //                result.Token = Crypt.GetToken();
        //                result.UserId = user.Id;
        //                db.AuthTokens.Add(new AuthToken() {
        //                    CreatDate = DateTime.UtcNow,
        //                    ExpiryDate = DateTime.UtcNow.AddDays(1),
        //                    Ip = model.Ip.ToIntIp(),
        //                    Token = result.Token,
        //                    UserId = user.Id
        //                });
        //                db.SaveChanges();
        //                dbContextTransaction.Commit();

        //            } catch (Exception ex) {
        //                dbContextTransaction.Rollback();
        //                return new LoginResult() { Success = false, ErrMessages = new List<KeyMsg> { ResponseError.GetError(TypeError.DataSaveError) } };
        //            }
        //        }

        //    }
        //    return result;
        //}
        ///// <summary>
        ///// Регистрация нового пользователя
        ///// </summary>
        //[Route("api/user/Registration")]
        //[HttpPost]
        //public BaseResponse Registration(RegistrationModel model) {
        //    if (model == null)
        //        return new BaseResponse() { Success = false, ErrMessages = new List<KeyMsg>() { ResponseError.GetError(TypeError.RequestEmpty) } };
        //    var result = new BaseResponse();

        //    using (var db = new DataContext()) {
        //        using (var dbContextTransaction = db.Database.BeginTransaction()) {
        //            try {
        //                if (db.Users.All(_ => _.Email != model.Email.ToLower())) {
        //                    db.Users.Add(new User {
        //                        PasswordHash = Crypt.CreatePasswordHash(model.Password),
        //                        Email = model.Email.ToLower(),
        //                        RegisterFromIp = model.RegisterFromIp,
        //                        CreateDate = DateTime.UtcNow,
        //                        EmailConfirmHash = "",
        //                        PasswordRestoreHash = "",
        //                        EmailConfirmed = false
        //                    });
        //                    db.SaveChanges();
        //                    dbContextTransaction.Commit();
        //                } else {
        //                    dbContextTransaction.Rollback();
        //                    result.Success = false;
        //                    result.ErrMessages.Add(ResponseError.GetError(TypeError.WhichUserIsAlready));
        //                }
        //            } catch (Exception ex) {
        //                dbContextTransaction.Rollback();
        //                return new LoginResult() { Success = false, ErrMessages = new List<KeyMsg> { ResponseError.GetError(TypeError.DataSaveError) } };
        //            }
        //        }

        //    }

        //    return result;
        //}
        ///// <summary>
        ///// Получаем основные параметры пользователя
        ///// </summary>
        //[Route("api/user/GetUserByContext")]
        //[HttpPost]
        //[AuthFilter]
        //public UserResult GetUserByContext(AuthRequest model) {
        //    var result = new UserResult();
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.AuthTokens.Any(t => t.Token == model.Token));
        //        if (user == null) {
        //            return new UserResult() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        result.Id = user.Id;
        //        result.Email = user.Email;
        //        result.EmailConfirmed = user.EmailConfirmed;
        //        result.Name = user.Name;
        //        result.Phone = user.Phone;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Получаем основные параметры пользователя по его id
        ///// </summary>
        //[Route("api/user/GetUserById")]
        //[HttpPost]
        //[AuthFilter]
        //public UserResult GetUserById(int userId) {
        //    var result = new UserResult();
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.Id == userId);
        //        if (user == null) {
        //            return new UserResult() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        result.Id = user.Id;
        //        result.Email = user.Email;
        //        result.Name = user.Name;
        //        result.Phone = user.Phone;
        //        result.EmailConfirmed = user.EmailConfirmed;
        //    }
        //    return result;
        //}

        //[Route("api/user/EditUser")]
        //[HttpPost]
        //[AuthFilter]
        //public BaseResponse EditUser(EditUserRequest model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.Id == model.UserId);
        //        if (user == null) {
        //            return new UserResult() {
        //                Success = false,
        //                ErrMessages = new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        user.Name = model.Name?.Trim();
        //        user.Phone = model.Phone?.Trim();
        //        db.SaveChanges();
        //        return new BaseResponse() { Success = true };
        //    }
        //}

        ///// <summary> 
        ///// Отправка запроса на подтверждения эмайла
        ///// </summary>
        //[Route("api/user/RequestEmailConfirm")]
        //[HttpPost]
        //public BaseResponse RequestEmailConfirm(EmailConfirmedModel model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.Email == model.Email.ToLower());
        //        if (user == null) {
        //            return new BaseResponse() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        user.EmailConfirmHash = Crypt.GetToken();
        //        db.SaveChanges();
        //        ManagerEmail.Send("Подтверждения почты", $"код:{user.EmailConfirmHash}", model.Email);
        //    }

        //    return new BaseResponse();
        //}

        ///// <summary>
        ///// Ответ на подтверждения эмайла
        ///// </summary>
        //[Route("api/user/ResponseEmailConfirm")]
        //[HttpPost]
        //public BaseResponse ResponseEmailConfirm(EmailConfirmedModel model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.EmailConfirmHash == model.EmailConfirmHash);
        //        if (user == null) {
        //            return new BaseResponse() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        user.EmailConfirmed = true;
        //        db.SaveChanges();
        //    }
        //    return new BaseResponse();
        //}
        ///// <summary>
        ///// Запрос на восстановления пароля
        ///// </summary>
        //[Route("api/user/RequestRecoveryPassword")]
        //[HttpPost]
        //public BaseResponse RequestRecoveryPassword(RequestRecoveryPasswordModel model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.Email == model.Email.ToLower());
        //        if (user == null) {
        //            return new BaseResponse() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.EmailNotFound) }
        //            };
        //        }
        //        user.PasswordRestoreHash = Crypt.GetToken();
        //        db.SaveChanges();
        //        ManagerEmail.Send("Для восстановления пароля.", $"код:{user.PasswordRestoreHash}", model.Email);
        //    }
        //    return new BaseResponse();
        //}
        ///// <summary>
        ///// Ответ на восстановления пароля
        ///// </summary>
        //[Route("api/user/ResponseRecoveryPassword")]
        //[HttpPost]
        //public BaseResponse ResponseRecoveryPassword(ResponseRecoveryPasswordModel model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.PasswordRestoreHash == model.PasswordRestoreHash);
        //        if (user == null) {
        //            return new BaseResponse() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.RequestEmpty) }
        //            };
        //        }
        //        user.PasswordRestoreHash = "";
        //        user.PasswordHash = Crypt.CreatePasswordHash(model.Password);
        //        db.SaveChanges();

        //    }
        //    return new BaseResponse();
        //}
        ///// <summary>
        ///// Сменить пароль
        ///// </summary>
        //[Route("api/user/ChangePassword")]
        //[HttpPost]
        //[AuthFilter]
        //public BaseResponse ChangePassword(ChangePasswordModel model) {
        //    using (var db = new DataContext()) {
        //        var user = db.Users.FirstOrDefault(_ => _.AuthTokens.Any(t => t.Token == model.Token));
        //        if (user == null) {
        //            return new BaseResponse() {
        //                Success = false,
        //                ErrMessages =
        //                    new List<KeyMsg>() { ResponseError.GetError(TypeError.UserNotFound) }
        //            };
        //        }
        //        user.PasswordHash = Crypt.CreatePasswordHash(model.NewPassword);
        //        db.SaveChanges();
        //    }
        //    return new BaseResponse();
        //}
    }
}