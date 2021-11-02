using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjZodiac.Models;
using System.Web.Security;
using System.Security;
using PagedList;
namespace prjZodiac.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ZodiacEntities db = new ZodiacEntities();
        int pageSize = 4;

        [AllowAnonymous]
        // GET: Home
        public ActionResult Index(int page=1)
        {
            ViewBag.userName = User.Identity.Name;
            int currentPage = page < 1 ? 1 : page;
            var products = db.TabletZodiacs1081639.ToList();
            var result = products.ToPagedList(currentPage, pageSize);
            return View(result);
           
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userId,string userPwd)
        {
            var identity = db.TabletMembers1081639.Where(m => m.mName == userId && m.password == userPwd).FirstOrDefault();
            if (identity != null)
            {
                FormsAuthentication.RedirectFromLoginPage(userId, true);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "帳號或密碼錯誤!";
                return View("Login");
            }
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        //Post:Home/Register
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(TabletMembers1081639 theMember)
        {
            //若模型沒有通過驗證則顯示目前的View
            if (ModelState.IsValid == false)
            {
                return View();
            }
            // 依帳號取得會員並指定給member
            var member = db.TabletMembers1081639
                .Where(m => m.mName == theMember.mName)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                //將會員記錄新增到tMember資料表
                db.TabletMembers1081639.Add(theMember);
                db.SaveChanges();
                //執行Home控制器的Login動作方法
                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號己有人使用，註冊失敗";
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();   // 登出
            return RedirectToAction("Login", "Home");
        }
        public ActionResult EditMember()
        {
            return View(db.TabletMembers1081639.Where(m=>m.mName==User.Identity.Name).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditMember(TabletMembers1081639 member)
        {
            var temp = db.TabletMembers1081639.Where(m => m.mName == member.mName).FirstOrDefault();
            temp.password = member.password;
            temp.phone = member.phone;
            temp.zodiac = member.zodiac;
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult DeleteMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteMember(string userId, string userPwd)
        {
            FormsAuthentication.SignOut();
            TabletMembers1081639 data = db.TabletMembers1081639.Where(m => m.mName == userId && m.password == userPwd).FirstOrDefault();
            db.TabletMembers1081639.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Introduce(int zId)
        {
            ZodiacClass data = new ZodiacClass()
            {
                zodiacs = db.TabletZodiacs1081639.Where(m => m.zId == zId).ToList()
            };
            return View(data);
        }
        public ActionResult SelectByZodiac()
        {
            ZodiacClass data = new ZodiacClass()
            {
                zodiacs = db.TabletZodiacs1081639.ToList()
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult ShowSelectResult(string zodiacName)
        {
            ZodiacClass data = new ZodiacClass()
            {
                zodiacs = db.TabletZodiacs1081639.Where(m => m.zName == zodiacName).ToList()
            };
            return View(data);
        }
        public ActionResult DrawLots()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DrawLots(string name)
        {
            return View();
        }
    }
}