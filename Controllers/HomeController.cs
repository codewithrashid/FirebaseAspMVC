using Firebase.Auth;
using FirebaseAspMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FirebaseAspMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FirebaseAuthProvider _provider;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _provider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCPbYh6NPMLx-2U8PGQDXGWA6niMQPZpBU"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUser(LoginModel loginModel)
        {
            try
            {
                await _provider.CreateUserWithEmailAndPasswordAsync(loginModel.EmailAddress, loginModel.Password);
                var firelink = await _provider.SignInWithEmailAndPasswordAsync(loginModel.EmailAddress, loginModel.Password);
                string accesstoken = firelink.FirebaseToken;
                if (accesstoken != null)
                {
                    HttpContext.Session.SetString("AccessToken", accesstoken);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(loginModel);

                }
            }
            catch (FirebaseAuthException ex)
            {

                var firebaseex = JsonConvert.DeserializeObject<ErrorModel>(ex.RequestData);
                ModelState.AddModelError(string.Empty, firebaseex.Message);
                return View(loginModel);
            }

            return View(loginModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
