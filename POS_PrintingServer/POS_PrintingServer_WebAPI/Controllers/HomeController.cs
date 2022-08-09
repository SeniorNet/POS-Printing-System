
using POS_PrintingServer_API.API.Datecs;
using POS_PrintingServer_API.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS_PrintingServer_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Login()
        {
            ClientDetailsViewModel _obj = ClientDetailsViewModel.GetClientDetails();
            if (_obj == null)
            {
                return View(new LoginViewModel());
            }
            else
            {
                return RedirectToAction("CurrentClientDetails");
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            string _errorMessage = string.Empty;
            if (string.IsNullOrEmpty(model.uuid))
            {
                _errorMessage += "UUID is required to login !";
            }
            else
            {
                ClientDetailsViewModel _obj = LoginViewModel.LoginCallBackURL(model.uuid);
                
                if (_obj == null)
                {
                    _errorMessage += "Invalid UUID !";
                }
                else
                {
                    return RedirectToAction("CurrentClientDetails");
                }
            }
            if (_errorMessage.Count() > 0)
            {
                ViewBag.ErrorMessage = _errorMessage;
                return View(model);
            }
            else
            {
                return RedirectToAction("CurrentClientDetails");
            }
        }

        public ActionResult CurrentClientDetails()
        {
            ClientDetailsViewModel model = ClientDetailsViewModel.GetClientDetails();
            if (model == null)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }
        public ActionResult DeleteCurrentClientDetails()
        {
            ClientDetailsViewModel.DeleteClientDetails();
            return RedirectToAction("Login");
        }
    }
}