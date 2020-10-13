using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LDDC.DAL;
using LD.Common;
using System.Data;
using System.Configuration;
using System.Drawing;
using LD.QYH.Common;
using System.Text;
using Newtonsoft.Json.Linq;
using LDDC.Model;

namespace LD_DC.Controllers
{
    public class SupplierController : Controller
    {
        //
        // GET: /Supplier/
        public ActionResult Index()
        {
            //获取登录用户
            PublicDao pPublicDao = new PublicDao();
            //string accessToken = pPublicDao.GetCacheAccessToken();
            ApiHelper api = new ApiHelper();
            //if (Request.QueryString["token"] != null)
            //{
            //    string token = Request.QueryString["token"].ToString();
            //    //string token = "a2dffec4-9e70-4b17-86f8-982f0792bd85";
            //    JObject seal = api.SelectNeibu(token);

            //    EOPModel eOP = new EOPModel();
            //    eOP.code = seal.Value<string>("code");
            //    if (eOP.code == "0")
            //    {
            //        eOP.username = seal["data"]["username"].ToString();
            //        eOP.nickname = seal["data"]["nickname"].ToString();
            //        ViewData["GetToken"] = token;

            //        HttpContext.Session["userID"] = eOP.username;
            //        HttpContext.Session["userName"] = eOP.nickname;
            //    }
            //    else
            //    {
            //        eOP.username = "";
            //        eOP.nickname = "";

            //        HttpContext.Session["userID"] = eOP.username;
            //        HttpContext.Session["userName"] = eOP.nickname;
            //        return View("Error");
            //    }
            //}
            //else
            //{
            //    return View("Error");
            //}




            //if (Request.QueryString["code"] != null && Request.QueryString["code"] != String.Empty)
            //{
            //    string code = Request.QueryString["code"].ToString();
            //    ViewData["code"] = "code:" + Request.QueryString["code"].ToString();
            //    ViewData["code"] += "accessToken:" + accessToken + "</br>";

            //    string getjson = String.Empty;
            //    PublicDao.WXUser OAuthUser_Model = pPublicDao.Get_UserInfo(accessToken, code, ref getjson);
            //    ViewData["code"] += " GetJson:" + getjson;
            //    ViewData["code"] += " UserId:" + OAuthUser_Model.UserId + " DeviceId:" + OAuthUser_Model.DeviceId;

            //    PublicDao.UserGetJson pUserGetJson = pPublicDao.GetUser(accessToken, OAuthUser_Model.UserId);
            //    if (OAuthUser_Model.UserId != null && OAuthUser_Model.UserId != "")  //已获取得openid及其他信息
            //    {
            //        //CacheHelper.SetCache("userid", OAuthUser_Model.UserId);
            //        Cookies.SetUserCookie("", "", "", "", OAuthUser_Model.UserId, "", "");

            //        //  div1.InnerText += "b ";
            //        //在页面上输出用户信息
            //        ViewData["code"] += "成员UserID :" + OAuthUser_Model.UserId + "成员名称:" + pUserGetJson.name + "部门:" + pUserGetJson.department[0] + "</br>手机设备号:" + OAuthUser_Model.DeviceId + "</br>";

            //        HttpContext.Session["userID"] = OAuthUser_Model.UserId;
            //        HttpContext.Session["userName"] = pUserGetJson.name;
            //    }
            //    else  //未获得openid，回到wxProcess.aspx，访问弹出微信授权页面，提示用户授权
            //    {
            //        // div1.InnerText += "c ";
            //        //Response.Redirect("wx1.aspx?auth=1");
            //    }
            //}
            //else
            //{
            //    ViewData["code"] = "code:没有数据";
            //}

            HttpContext.Session["userID"] = "zhangwdc";
            HttpContext.Session["userName"] = "张伟东";


            string company = "集团总部";
            string restaurantGuid = String.Empty;
            DataSet dsRInfo = new DataSet();
            DataSet dsMenuInfo = new DataSet();
            DataSet dszd_VegetableType = new DataSet();
            LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
            if (HttpContext.Session["restaurantGuid"] == null)
            {
                //获取餐厅信息

                dsRInfo = pRestaurantInfo.GetList("company = '" + company + "'");
                if (dsRInfo.Tables[0].Rows.Count > 0)
                {
                    restaurantGuid = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                    ViewData["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                    HttpContext.Session["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                    HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                    ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                    ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                }
            }
            else
            {
                restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
                if (dsRInfo.Tables[0].Rows.Count > 0)
                {
                    ViewData["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                    ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                    ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                    ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                }
            }

            //获取所有菜品类型
            dszd_VegetableType = pPublicDao.GetMenuType(restaurantGuid);
            ViewData["zd_VegetableType"] = dszd_VegetableType.Tables[0];

            //获取菜品信息
            //dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "'", "a.vtId,a.createDate desc ");
            dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "' and a.createDate >='" + DateTime.Now.AddMonths(-2) + "'", "a.vtId,a.createDate desc ");
            ViewData["dsMenuInfo"] = dsMenuInfo.Tables[0];

            //配餐信息
            string dateBeg = String.Empty;
            string dateEnd = String.Empty;
            string weekBeg = String.Empty;
            string weekEnd = String.Empty;
            string arrsGuid = String.Empty;
            DataSet dsSetMeal = new DataSet();
            DataSet dsDailyMeal = new DataSet();
            string dateNow = String.Empty;

            //dateNow = DateTime.Now.ToShortDateString();
            dateNow = DateTime.Now.Year + "-" + "1-1";
            LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
            dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + dateNow + "'", "begDate desc");
            ViewData["dsSetMeal"] = dsSetMeal.Tables[0];

            if (dsSetMeal.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSetMeal.Tables[0].Rows.Count; i++)
                {
                    arrsGuid += "'" + dsSetMeal.Tables[0].Rows[i]["guid"].ToString() + "',";
                }
                arrsGuid = arrsGuid.Substring(0, arrsGuid.Length - 1);
            }
            LDDC.DAL.DailyMeal pDailyMeal = new LDDC.DAL.DailyMeal();
            if (arrsGuid != String.Empty)
            {
                dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in(" + arrsGuid + ")", "a.sGuid,b.vtID");
                ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];
            }

            //当日-选项卡
            //当月预订、实际
            DateTime nowday = DateTime.Now;
            ViewData["Today"] = nowday.ToShortDateString();
            ViewData["Tomorrow1"] = nowday.AddDays(1).ToShortDateString();
            Statistics pStatistics = new Statistics();
            DataSet dsThisMonth = new DataSet();
            string month = nowday.ToString("yyyy-MM");
            if (nowday.ToShortDateString() == DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1).ToShortDateString())
            {
                dsThisMonth = pStatistics.GetThisMonth(month, restaurantGuid);
            }
            else
            {
                dsThisMonth = pStatistics.GetThisMonth(DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.ToString("yyyy-MM-dd"), nowday.ToString("yyyy-MM-dd"), restaurantGuid);
            }
            ViewData["dsThisMonth"] = dsThisMonth.Tables[0];

            //当天午餐预订、实际
            DataSet dsTodayLunch = new DataSet();
            dsTodayLunch = pStatistics.GetTodayLunch(nowday.ToShortDateString(), restaurantGuid);
            ViewData["dsTodayLunch"] = dsTodayLunch.Tables[0];

            //当天晚餐预订、实际
            DataSet dsTodayDinner = new DataSet();
            dsTodayDinner = pStatistics.GetTodayDinner(nowday.ToShortDateString(), restaurantGuid);
            ViewData["dsTodayDinner"] = dsTodayDinner.Tables[0];

            //当天会议晚餐预定、实际
            DataSet dsTodayMeetingLunch = new DataSet();
            dsTodayMeetingLunch = pStatistics.GetDateMeetingLunch(nowday.ToShortDateString(), restaurantGuid);
            ViewData["dsTodayMeetingLunch"] = dsTodayMeetingLunch.Tables[0];

            //明天午餐预订、实际
            DataSet dsTomorrowLunch = new DataSet();
            dsTomorrowLunch = pStatistics.GetTodayLunch(nowday.AddDays(1).ToShortDateString(), restaurantGuid);
            ViewData["dsTomorrowLunch"] = dsTomorrowLunch.Tables[0];

            //明天晚餐预订、实际
            DataSet dsTomorrowDinner = new DataSet();
            dsTomorrowDinner = pStatistics.GetTodayDinner(nowday.AddDays(1).ToShortDateString(), restaurantGuid);
            ViewData["dsTomorrowDinner"] = dsTomorrowDinner.Tables[0];

            //明天会议中餐预定、实际
            DataSet dsTomorrowMeetingLunch = new DataSet();
            dsTomorrowMeetingLunch = pStatistics.GetDateMeetingLunch(nowday.AddDays(1).ToShortDateString(), restaurantGuid);
            ViewData["dsTomorrowMeetingLunch"] = dsTomorrowMeetingLunch.Tables[0];

            return View();
        }



        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult AddDailyMeal()
        {
            try
            {
                //获取登录用户
                PublicDao pPublicDao = new PublicDao();
                //string accessToken = pPublicDao.GetCacheAccessToken();

                string company = "集团总部";
                string restaurantGuid = String.Empty;
                DataSet dsRInfo = new DataSet();
                DataSet dsMenuInfo = new DataSet();
                DataSet dszd_VegetableType = new DataSet();
                DataSet dsMaxEndDate = new DataSet();

                //HttpContext.Session["userID"] = "zhangwdc";
                //HttpContext.Session["userName"] = "张伟东";

                //获取餐厅信息
                LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
                if (HttpContext.Session["restaurantGuid"] == null)
                {
                    dsRInfo = pRestaurantInfo.GetList("company = '" + company + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        restaurantGuid = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }
                else
                {
                    restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                    dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        ViewData["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }

                //初始化日期
                dsMaxEndDate = pPublicDao.GetMaxEndDate(restaurantGuid);
                if (dsMaxEndDate.Tables[0].Rows[0]["endDate"].ToString() != String.Empty)
                {
                    DateTime nowDate = Convert.ToDateTime(dsMaxEndDate.Tables[0].Rows[0]["endDate"]).AddDays(1);
                    ViewData["begDate"] = nowDate.ToString("M月d日");
                    ViewData["endDate"] = nowDate.ToString("M月d日");
                    ViewData["shortBegDate"] = nowDate.ToShortDateString();
                    ViewData["shortEndDate"] = nowDate.ToShortDateString();
                    ViewData["begDay"] = Week(nowDate);
                    ViewData["endDay"] = Week(nowDate);
                }
                else
                {
                    DateTime begDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;
                    ViewData["begDate"] = begDate.ToString("M月d日");
                    ViewData["endDate"] = endDate.ToString("M月d日");
                    ViewData["shortBegDate"] = begDate.ToShortDateString();
                    ViewData["shortEndDate"] = endDate.ToShortDateString();
                    ViewData["begDay"] = Week(DateTime.Now);
                    ViewData["endDay"] = Week(DateTime.Now);
                }

                //获取所有菜品类型
                dszd_VegetableType = pPublicDao.GetMenuType(restaurantGuid);
                ViewData["zd_VegetableType"] = dszd_VegetableType.Tables[0];

                //获取菜品信息
                //dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "'", "a.vtId,a.createDate desc ");
                dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "' and a.createDate >='" + DateTime.Now.AddMonths(-2) + "'", "a.vtId,a.createDate desc ");
                ViewData["dsMenuInfo"] = dsMenuInfo.Tables[0];



                //微信Config绑定
                WXConfig();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult UpdateDailyMeal()
        {
            try
            {
                //获取登录用户
                PublicDao pPublicDao = new PublicDao();
                string accessToken = pPublicDao.GetCacheAccessToken();

                //string company = "集团总部";
                string restaurantGuid = Request.QueryString["rGuid"].ToString();
                string sGuid = Request.QueryString["sGuid"].ToString();
                ViewData["sGuid"] = Request.QueryString["sGuid"].ToString();
                DataSet dsRInfo = new DataSet();
                DataSet dsMenuInfo = new DataSet();
                DataSet dszd_VegetableType = new DataSet();
                DataSet dsNowDate = new DataSet();

                //HttpContext.Session["userID"] = "zhangwdc";
                //HttpContext.Session["userName"] = "张伟东";

                //获取餐厅信息
                LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
                if (HttpContext.Session["restaurantGuid"] == null)
                {
                    dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        restaurantGuid = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }
                else
                {
                    restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                    dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        ViewData["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }

                //初始化日期
                dsNowDate = pPublicDao.GetNowDate(sGuid);
                if (dsNowDate.Tables[0].Rows.Count > 0)
                {
                    DateTime begDate = Convert.ToDateTime(dsNowDate.Tables[0].Rows[0]["begDate"]);
                    DateTime endDate = Convert.ToDateTime(dsNowDate.Tables[0].Rows[0]["endDate"]);
                    ViewData["begDate"] = begDate.ToString("M月d日");
                    ViewData["endDate"] = endDate.ToString("M月d日");
                    ViewData["shortBegDate"] = begDate.ToShortDateString();
                    ViewData["shortEndDate"] = endDate.ToShortDateString();
                    ViewData["begDay"] = Week(begDate);
                    ViewData["endDay"] = Week(endDate);
                }


                //获取所有菜品类型
                dszd_VegetableType = pPublicDao.GetMenuType(restaurantGuid);
                ViewData["zd_VegetableType"] = dszd_VegetableType.Tables[0];

                //获取菜品信息
                //dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "'", "a.vtId,a.createDate desc ");
                dsMenuInfo = pPublicDao.GetMenuInfoList("rGuid = '" + restaurantGuid + "' and a.createDate >='" + DateTime.Now.AddMonths(-2) + "'", "a.vtId,a.createDate desc ");
                ViewData["dsMenuInfo"] = dsMenuInfo.Tables[0];

                //获取当前已勾选的菜品
                DataSet dsDailyMeal = new DataSet();
                dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in('" + sGuid + "')", "a.sGuid,b.vtID");
                ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];

                //微信Config绑定
                WXConfig();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult AddFood()
        {
            try
            {
                //获取登录用户
                PublicDao pPublicDao = new PublicDao();
                //string accessToken = pPublicDao.GetCacheAccessToken();

                string company = "集团总部";
                string restaurantGuid = String.Empty;
                DataSet dsRInfo = new DataSet();

                //HttpContext.Session["userID"] = "zhangwdc";
                //HttpContext.Session["userName"] = "张伟东";

                //获取餐厅信息
                LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
                if (HttpContext.Session["restaurantGuid"] == null)
                {
                    dsRInfo = pRestaurantInfo.GetList("company = '" + company + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        restaurantGuid = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }
                else
                {
                    restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                    dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
                    if (dsRInfo.Tables[0].Rows.Count > 0)
                    {
                        ViewData["restaurantGuid"] = dsRInfo.Tables[0].Rows[0]["guid"].ToString();
                        ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                        ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                        ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                        ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                    }
                }

                //获取菜品类型
                //zd_VegetableType pzd_VegetableType = new zd_VegetableType();
                //dsVT = pzd_VegetableType.GetList(0, "isValid = 1","sortNo");
                //vtJson = pPublicDao.GetJsonByDataset(dsVT);

                //微信Config绑定
                WXConfig();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Test2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReloadPage(string rid)
        {
            HttpContext.Session["restaurantGuid"] = rid;
            string restaurantGuid = rid;
            StringBuilder sb = new StringBuilder();

            //return RedirectToAction("Index", "Supplier");
            //return Redirect("/Supplier/Index");
            //Response.Redirect("/Supplier/Index", true);

            return null;
            //return Content(sb.ToString());
        }

        [HttpPost]
        public ActionResult GetVTJson()
        {
            PublicDao pPublicDao = new PublicDao();

            string vtJson = String.Empty;
            DataSet dsVT = new DataSet();

            //获取菜品类型
            LDDC.DAL.zd_VegetableType pzd_VegetableType = new LDDC.DAL.zd_VegetableType();
            dsVT = pzd_VegetableType.GetList(0, "isValid = 1", "sortNo");
            vtJson = pPublicDao.GetVTJsonByDataset(dsVT);
            //vtJson = vtJson.Substring(vtJson.IndexOf("["), vtJson.LastIndexOf("]") - vtJson.IndexOf("[")+1);

            return Content(vtJson);
        }

        [HttpPost]
        public ActionResult GetRJson()
        {
            PublicDao pPublicDao = new PublicDao();

            string rJson = String.Empty;
            DataSet dsVT = new DataSet();

            //获取菜品类型
            LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
            dsVT = pRestaurantInfo.GetList(0, "isValid = 1", "sortNo");
            rJson = pPublicDao.GetVTJsonByDataset(dsVT, "guid", "name", "city", "county", "address");
            //vtJson = vtJson.Substring(vtJson.IndexOf("["), vtJson.LastIndexOf("]") - vtJson.IndexOf("[")+1);

            return Content(rJson);
        }

        [HttpPost]
        public ActionResult AddFood(int vtID, string vtName, string name, string describe, string imgPath)
        {
            try
            {
                PublicDao pPublicDao = new PublicDao();
                LDDC.Model.MenuInfo mMenuInfo = new LDDC.Model.MenuInfo();
                LDDC.Model.DishesImage mDishesImage = new LDDC.Model.DishesImage();

                Guid guid = Guid.NewGuid();
                string restaurantGuid = String.Empty;
                int rCount = 0;
                string errorInfo = String.Empty;
                restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

                //菜品信息
                mMenuInfo.guid = guid;
                mMenuInfo.rGuid = restaurantGuid;
                mMenuInfo.vtID = vtID;
                mMenuInfo.vtName = vtName;
                mMenuInfo.name = name;
                mMenuInfo.describe = describe;
                mMenuInfo.creatorguid = HttpContext.Session["userID"].ToString();
                mMenuInfo.creator = HttpContext.Session["userName"].ToString();
                //菜品图片
                mDishesImage.vGuid = guid.ToString();
                mDishesImage.imgPath = imgPath;
                mDishesImage.format = "";
                mDishesImage.imgName = "";
                mDishesImage.size = 0;
                mDishesImage.isCoverphoto = 1;

                pPublicDao.TransactionAddFood(mMenuInfo, mDishesImage, ref rCount, ref errorInfo);

                return Content("添加菜品成功！");
                //return View("Test");
                //return RedirectToAction("Test");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //return Content("添加菜品失败！");
            }
        }

        [HttpPost]
        public ActionResult DeleteDailyMeal(string rGuid,string sGuid)
        {
            try
            {
                //获取登录用户
                PublicDao pPublicDao = new PublicDao();
                //string accessToken = pPublicDao.GetCacheAccessToken();
                int rCount = 0;
                string errorInfo = String.Empty;

                //string company = "集团总部";
                string restaurantGuid = rGuid;

                pPublicDao.TransactionDeleteDMeal(sGuid, ref rCount, ref errorInfo);
                if (rCount > 0)
                {
                    return Content("删除成功");
                }
                else
                {
                    return Content("删除失败");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        //[HttpPost]
        //public ActionResult AddImage(string sId)
        //{
        //    PublicDao pPublicDao = new PublicDao();
        //    FileHelper pFileHelper = new FileHelper();
        //    //string accessToken = pPublicDao.GetCacheAccessToken();
        //    string filePath = String.Empty;

        //    List<string> rsFilePathList = new List<string>();
        //    try
        //    {
        //        string imgServerIds = sId; // 微信服务器图片Id
        //        List<string> imgServerIdList = imgServerIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //        foreach (string imgServerId in imgServerIdList)
        //        {
        //            // 1)获取图片
        //            Image img = pPublicDao.GetImage(accessToken, imgServerId);
        //            // 2)存放本地
        //            string imageFilePath = pFileHelper.SaveWeChatAttFileOfImage(img, "WeChat");
        //            rsFilePathList.Add(imageFilePath);
        //            filePath += imageFilePath;
        //        }
        //        //return Content(rsFilePathList);
        //        ViewData["filePath"] = filePath;
        //        return Content(filePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);
        //    }
        //}


        [HttpPost]
        public ActionResult UploadDailyMeal(int vtID, string vtName, DateTime begDate, DateTime endDate, string arrGuid)
        {
            try
            {
                PublicDao pPublicDao = new PublicDao();
                LDDC.Model.SetMeal mSetMeal = new LDDC.Model.SetMeal();
                LDDC.Model.DailyMeal mDailyMeal = new LDDC.Model.DailyMeal();

                //Guid guid = Guid.NewGuid();
                string restaurantGuid = String.Empty;
                int rCount = 0;
                string errorInfo = String.Empty;
                DataSet dsIsAddDailyMeal = new DataSet();
                restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

                dsIsAddDailyMeal = pPublicDao.GetIsAddDailyMeal(restaurantGuid, begDate.ToShortDateString(), endDate.ToShortDateString(), vtID);
                if (dsIsAddDailyMeal.Tables[0].Rows.Count > 0)
                {
                    return Content("您选择的时间段已有在配餐中出现过！");
                }

                //mSetMeal.guid = guid;
                mSetMeal.rGuid = restaurantGuid;
                mSetMeal.eGuid = vtID;
                mSetMeal.eType = vtName;
                //mSetMeal.begDate = begDate;
                //mSetMeal.endDate = endDate;
                mSetMeal.creatorguid = HttpContext.Session["userID"].ToString();
                mSetMeal.creator = HttpContext.Session["userName"].ToString();

                //bool t = pPublicDao.TransactionDailyMeal(mSetMeal, mDailyMeal, arrGuid, ref rCount, ref errorInfo);
                bool t = pPublicDao.TransactionDailyMeal(mSetMeal, mDailyMeal, begDate, endDate, arrGuid, ref rCount, ref errorInfo);
                if (t == true)
                {
                    return Content("配菜成功！");
                }
                else
                {
                    return Content("配菜失败！");
                }
                //return Content("配菜成功！");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        [HttpPost]
        public ActionResult UpdateDailyMeal(int vtID, string vtName, DateTime begDate, DateTime endDate, string arrGuid, string sGuid)
        {
            try
            {
                PublicDao pPublicDao = new PublicDao();
                LDDC.Model.SetMeal mSetMeal = new LDDC.Model.SetMeal();
                LDDC.Model.DailyMeal mDailyMeal = new LDDC.Model.DailyMeal();

                //Guid guid = Guid.NewGuid();
                string restaurantGuid = String.Empty;
                int rCount = 0;
                string errorInfo = String.Empty;
                //DataSet dsIsAddDailyMeal = new DataSet();
                //restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

                //dsIsAddDailyMeal = pPublicDao.GetIsAddDailyMeal(restaurantGuid, begDate.ToShortDateString(), endDate.ToShortDateString(), vtID);
                //if (dsIsAddDailyMeal.Tables[0].Rows.Count > 0)
                //{
                //    return Content("您选择的时间段已有在配餐中出现过！");
                //}

                //mSetMeal.guid = guid;
                mSetMeal.rGuid = restaurantGuid;
                mSetMeal.eGuid = vtID;
                mSetMeal.eType = vtName;
                //mSetMeal.begDate = begDate;
                //mSetMeal.endDate = endDate;
                mSetMeal.creatorguid = HttpContext.Session["userID"].ToString();
                mSetMeal.creator = HttpContext.Session["userName"].ToString();

                bool t = pPublicDao.TransactionUpdateDMeal(mSetMeal, mDailyMeal, begDate, endDate, arrGuid, sGuid, ref rCount, ref errorInfo);
                if (t == true)
                {
                    return Content("配菜成功！");
                }
                else
                {
                    return Content("配菜失败！");
                }
                //return Content("配菜成功！");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        [HttpPost]
        public ActionResult UpdateHtml(string defaultDate, string rGuid)
        {
            Statistics pStatistics = new Statistics();
            StringBuilder html = new StringBuilder();
            try
            {
                DateTime nowday = Convert.ToDateTime(defaultDate);
                //当月预订、实际
                DataSet dsThisMonth = new DataSet();
                string month = nowday.ToString("yyyy-MM");
                if (nowday.ToString("yyyy-MM") != DateTime.Now.ToString("yyyy-MM"))
                {
                    dsThisMonth = pStatistics.GetThisMonth(month, rGuid);
                }
                else
                {
                    dsThisMonth = pStatistics.GetThisMonth(DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.ToString("yyyy-MM-dd"), nowday.ToString("yyyy-MM-dd"), rGuid);
                }
                ViewData["dsThisMonth"] = dsThisMonth.Tables[0];

                //当天午餐预订、实际
                DataSet dsTodayLunch = new DataSet();
                dsTodayLunch = pStatistics.GetTodayLunch(nowday.ToShortDateString(), rGuid);

                //当天晚餐预订、实际
                DataSet dsTodayDinner = new DataSet();
                dsTodayDinner = pStatistics.GetTodayDinner(nowday.ToShortDateString(), rGuid);

                //当天会议中餐预订、实际
                DataSet dsTodayMeetingLunch = new DataSet();
                dsTodayMeetingLunch = pStatistics.GetDateMeetingLunch(nowday.ToShortDateString(), rGuid);

                html.Append("<div class=\"detailsTitle\">");
                html.Append("<div id=\"rectangle\">");
                html.Append("<p class=\"detailsP\">");
                html.Append("本月订餐情况");
                html.Append("</p>");
                html.Append("</div>");
                html.Append("</div>");
                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divZW\">全部</div>");
                if (dsThisMonth.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预计 <span style=\"font-size: 1.0rem;\">" + dsThisMonth.Tables[0].Rows[0]["yj"].ToString() + "</span> 人</div>");
                    html.Append("<div class=\"divSJ\">实际 <span style=\"font-size: 1.0rem;\">" + @dsThisMonth.Tables[0].Rows[0]["sj"].ToString() + "</span> 人</div>");
                }
                html.Append("</div>");
                html.Append("</div>");
                html.Append("<div class=\"detailsTitle\">");
                html.Append("<div id=\"rectangle\">");
                html.Append("<p class=\"detailsP\">");
                html.Append("<span>" + nowday.ToString("dd") + "日</span>订餐情况");
                html.Append("</p>");
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divZW\">中餐</div>");
                if (dsTodayLunch.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预计 <span style=\"font-size: 1.0rem;\">" + dsTodayLunch.Tables[0].Rows[0]["yj"].ToString() + "</span> 人</div>");
                    html.Append("<div class=\"divSJ\">实际 <span style=\"font-size: 1.0rem;\">" + dsTodayLunch.Tables[0].Rows[0]["sj"].ToString() + "</span> 人</div>");
                }
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divZW\">会议中餐</div>");
                if (dsTodayLunch.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预计 <span style=\"font-size: 1.0rem;\">" + dsTodayMeetingLunch.Tables[0].Rows[0]["yj"].ToString() + "</span> 人</div>");
                    html.Append("<div class=\"divSJ\">实际 <span style=\"font-size: 1.0rem;\">" + dsTodayMeetingLunch.Tables[0].Rows[0]["sj"].ToString() + "</span> 人</div>");
                }
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divWW\">晚餐</div>");
                if (dsTodayDinner.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预计 <span style=\"font-size: 1.0rem;\">" + dsTodayDinner.Tables[0].Rows[0]["yj"].ToString() + "</span> 人</div>");
                    html.Append("<div class=\"divSJ\">实际 <span style=\"font-size: 1.0rem;\">" + dsTodayDinner.Tables[0].Rows[0]["sj"].ToString() + "</span> 人</div>");
                }
                html.Append("</div>");
                html.Append("</div>");


                return Content(html.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        private string Week(DateTime dt)
        {
            string weekstr = dt.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": weekstr = "一"; break;
                case "Tuesday": weekstr = "二"; break;
                case "Wednesday": weekstr = "三"; break;
                case "Thursday": weekstr = "四"; break;
                case "Friday": weekstr = "五"; break;
                case "Saturday": weekstr = "六"; break;
                case "Sunday": weekstr = "日"; break;
            }
            return weekstr;
        }

        private void WXConfig()
        {
            //发消息
            PublicDao pPublicDao = new PublicDao();
            string appId = pPublicDao.GetCacheAccessToken();
            //string noncestr = new Random().Next(10000).ToString();
            string noncestr = pPublicDao.GetRandomString(16, true, true, true, false, "");
            //long timestamp = long.Parse(DateTime.Now.Ticks.ToString().Substring(0, 10));

            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            //long timestamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 时间戳
            string timestamp = (pPublicDao.GetTime() / 1000).ToString();

            string url = Request.Url.ToString().Replace("#", "");
            //string url = "http://app.lvdu-dc.com:81/Home/UserInfo?userid=lh&mobile=15188376695";
            url = url.Substring(0, url.LastIndexOf(":")) + ":" + ConfigurationManager.AppSettings["port"] + url.Substring(url.LastIndexOf(":") + 3);
            //string url = Request.Url.AbsoluteUri.ToString().Replace("#", "");
            string ticket = pPublicDao.GetCachewxTicket(appId);
            string jsapi_ticket = String.Empty;
            //string url = Request.Url.ToString().Trim();
            ViewData["ticket"] = ticket;
            ViewData["appId"] = ConfigurationManager.AppSettings["appid"];
            ViewData["timestamp"] = timestamp;
            ViewData["noncestr"] = noncestr;
            ViewData["url"] = url;
            ViewData["signature"] = pPublicDao.Signature(ticket, noncestr, timestamp.ToString(), url, ref jsapi_ticket);
            ViewData["jsapi_ticket"] = jsapi_ticket;
        }





    }
}