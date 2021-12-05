using System.Collections.Generic;
using System.Diagnostics;
using AspectCore;
using Dapper.Contrib.Extensions;
using RongKang.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RongKang.Repository;
using RongKangBusiness.Models;
using Tool;
using RongKang.UnitOfWork;
using RongKang.IBase;
using Redis.Redis_Helper;
using Redis;

namespace RongKangBusiness.Controllers
{
    [Transactional]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //RedisHelper redisHelper = new RedisHelper();
        private IUnitOfWork _unitOfWork;
        IUserRepository _CustomerBll;
        public HomeController(IUnitOfWork unitOfWork, IUserRepository CustomerBll)
        {
           
            _unitOfWork = unitOfWork;
            _CustomerBll = CustomerBll;
        }

        [Transactional]
        public IActionResult Index()
        {
            if (1 == 1)
            {
                RedisSession _redisSession = new RedisSession(true, 20);

                if (_redisSession != null)
                    _redisSession["www"] = "df444d";
            }
            else
            {
                 
            }


            List<User1> list = new List<User1>();

            User1 user = new User1();
            user.Name = "wangg";
            user.PassWord = "123456";

            list.Add(user);


            ////_unitOfWork.BeginTransaction();
            _CustomerBll.Insert11(user);
            //_unitOfWork.Commit();

            //_unitOfWork.Commit();

            //CacheHelper.SetCache("test", "111166611", 10);

            //HttpContext.Response.Cookies.Append("getCookie", "setCookieValue");

            //redisHelper.StringSet("tt", "bussine");

            //new CookieHelper().AddCookie("3333", "4444");

            //var SS=new CookieHelper().GetValue("3333");

            //Response.WriteAsync(SS);

            //Response.WriteAsync(CacheHelper.GetCache<string>("test"));



            return View();
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
