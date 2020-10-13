using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDDC.DAL;
using LD.Common;
using System.Data;
using System.Configuration;
using System.Text;

namespace LD_DC.Controllers
{
    public class RestaurantController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                //获取登录用户
                PublicDao pPublicDao = new PublicDao();
                string accessToken = pPublicDao.GetCacheAccessToken();

                if (Request.QueryString["code"] != null && Request.QueryString["code"] != String.Empty)
                {
                    string code = Request.QueryString["code"].ToString();
                    ViewData["code"] = "code:" + Request.QueryString["code"].ToString();
                    ViewData["code"] += "accessToken:" + accessToken + "</br>";

                    string getjson = String.Empty;
                    PublicDao.WXUser OAuthUser_Model = pPublicDao.Get_UserInfo(accessToken, code, ref getjson);
                    ViewData["code"] += " GetJson:" + getjson;
                    ViewData["code"] += " UserId:" + OAuthUser_Model.UserId + " DeviceId:" + OAuthUser_Model.DeviceId;

                    PublicDao.UserGetJson pUserGetJson = pPublicDao.GetUser(accessToken, OAuthUser_Model.UserId);
                    if (OAuthUser_Model.UserId != null && OAuthUser_Model.UserId != "")  //已获取得openid及其他信息
                    {
                        //CacheHelper.SetCache("userid", OAuthUser_Model.UserId);
                        Cookies.SetUserCookie("", "", "", "", OAuthUser_Model.UserId, "", "");

                        //  div1.InnerText += "b ";
                        //在页面上输出用户信息
                        ViewData["code"] += "成员UserID :" + OAuthUser_Model.UserId + "成员名称:" + pUserGetJson.name + "部门:" + pUserGetJson.department[0] + "</br>手机设备号:" + OAuthUser_Model.DeviceId + "</br>";

                        HttpContext.Session["userID"] = OAuthUser_Model.UserId;
                        HttpContext.Session["userName"] = pUserGetJson.name;
                    }
                    else  //未获得openid，回到wxProcess.aspx，访问弹出微信授权页面，提示用户授权
                    {
                        // div1.InnerText += "c ";
                        //Response.Redirect("wx1.aspx?auth=1");
                    }
                }
                else
                {
                    ViewData["code"] = "code:没有数据";
                }


                //HttpContext.Session["userID"] = OAuthUser_Model.UserId;
                //HttpContext.Session["userName"] = pUserGetJson.name;
                HttpContext.Session["userID"] = "zhangwdc";
                HttpContext.Session["userName"] = "张伟东";
                //HttpContext.Session["userID"] = "huangjjb";
                //HttpContext.Session["userName"] = "黄俊杰";
                ViewData["orderDate"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                //当前日期
                HttpContext.Session["currentDate"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

                //当前时间
                string nowTime = DateTime.Now.ToShortTimeString();
                //nowTime = "12:31";
                ViewData["nowTime"] = nowTime;
                //初始化-预定时间
                //string reserveTime = DateTime.Now.AddDays(1).ToShortTimeString();
                if (HttpContext.Session["reserveDate"] == null)
                {
                    ViewData["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                    ViewData["showReserveDate"] = "明日";
                }
                else
                {
                    if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"))
                    {
                        ViewData["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                        ViewData["showReserveDate"] = "明日";
                    }
                    else
                    {
                        ViewData["reserveDate"] = HttpContext.Session["reserveDate"].ToString();
                        //ViewData["showReserveDate"] = HttpContext.Session["reserveDate"].ToString();
                        ViewData["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()));
                    }
                }
                //ViewData["showReserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy年MM月dd日") + " " + Week(DateTime.Now.AddDays(1));

                string company = "集团总部";
                string companyGuid = String.Empty;
                string restaurantGuid = String.Empty;
                DataSet dsRInfo = new DataSet();
                DataSet dsCInfo = new DataSet();
                DataSet dsDInfo = new DataSet();
                DataSet dsOI = new DataSet();
                DataSet dsOITO = new DataSet();
                DataSet dsRI = new DataSet();
                string btnLunchState = String.Empty;
                string btnDinnerState = String.Empty;
                string btnReserveLunch = String.Empty;
                string btnReserveDinner = String.Empty;

                string userID = HttpContext.Session["userID"].ToString();//当前登录人
                string userName = HttpContext.Session["userName"].ToString();

                //获取公司Guid
                zd_Company pzd_Company = new zd_Company();
                dsCInfo = pzd_Company.GetList("name = '" + company + "'");
                if (dsCInfo.Tables[0].Rows.Count > 0)
                {
                    companyGuid = dsCInfo.Tables[0].Rows[0]["guid"].ToString();
                }

                //获取餐厅信息
                RestaurantInfo pRestaurantInfo = new RestaurantInfo();
                dsRInfo = pRestaurantInfo.GetList("companyGuid = '" + companyGuid + "'");
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

                //获取餐厅用餐时间
                Dinnertime pDinnertime = new Dinnertime();
                dsDInfo = pDinnertime.GetList("rGuid = '" + restaurantGuid + "'");
                if (dsDInfo.Tables[0].Rows.Count > 0)
                {
                    ViewData["dsDInfo"] = dsDInfo.Tables[0];
                }

                if (dsDInfo.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                    {
                        if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                        {
                            ViewData["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString();
                        }
                        else
                        {
                            ViewData["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString();
                        }
                    }
                }

                //获取今日订餐详情
                OrderInfo pOrderInfo = new OrderInfo();
                dsOI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "'", "eID");
                ViewData["dsOI"] = dsOI.Tables[0];

                //获取明日订餐详情
                dsRI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["reserveDate"] + "'", "eID");

                //获取转带详情
                dsOITO = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and tUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "'", "eID");
                ViewData["dsOITO"] = dsOITO.Tables[0];
                ViewData["toCount"] = dsOITO.Tables[0].Rows.Count;
                HttpContext.Session["dsOITO"] = dsOITO.Tables[0];

                //获取未来订餐详情
                DataSet dsOIFuture = new DataSet();
                dsOIFuture = pPublicDao.GetFutureMeal(restaurantGuid, userID, HttpContext.Session["currentDate"].ToString());
                ViewData["dsOIFuture"] = dsOIFuture.Tables[0];

                //当日控件状态
                DataRow[] drsZC = dsOI.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsWC = dsOI.Tables[0].Select("eType = '晚餐' ");

                //明日控件状态
                DataRow[] drsRZC = dsRI.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsRWC = dsRI.Tables[0].Select("eType = '晚餐' ");

                //明日中餐是否预定
                if (drsRZC.Length == 0)
                {
                    btnReserveLunch = "我要预订";
                }
                else
                {
                    btnReserveLunch = "订餐成功";
                }
                ViewData["btnReserveLunch"] = btnReserveLunch;
                //明日晚餐是否预定
                if (drsRWC.Length == 0)
                {
                    btnReserveDinner = "我要预订";
                }
                else
                {
                    btnReserveDinner = "订餐成功";
                }
                ViewData["btnReserveDinner"] = btnReserveDinner;


                for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                {
                    if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                    {
                        //if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()))
                        //{
                        //    //7:00 - 11:30
                        //    if (drsZC.Length == 0)
                        //    {
                        //        btnLunchState = "我要预订";
                        //    }
                        //    else
                        //    {
                        //        btnLunchState = "订餐成功";
                        //    }
                        //}
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                        {
                            //11:30 - 13:30
                            if (drsZC.Length == 0)
                            {
                                btnLunchState = "不可预订";
                            }
                            else
                            {
                                if (drsZC[0]["osState"].ToString() == "已预订")
                                {
                                    btnLunchState = "扫码就餐";
                                }
                                else if (drsZC[0]["osState"].ToString() == "已就餐")
                                {
                                    btnLunchState = "已用餐";
                                }
                                else if (drsZC[0]["osState"].ToString() == "已转带")
                                {
                                    btnLunchState = "已转带";
                                }
                            }
                        }
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                        {
                            //13:30 - 7:00
                            if (drsZC.Length == 0)
                            {
                                btnLunchState = "未预订";
                            }
                            else
                            {
                                if (drsZC[0]["osState"].ToString() == "已预订" || drsZC[0]["osState"].ToString() == "已转带")
                                {
                                    btnLunchState = "未用餐";
                                }
                                else if (drsZC[0]["osState"].ToString() == "已就餐")
                                {
                                    btnLunchState = "已用餐";
                                }
                            }
                        }
                        ViewData["btnLunchState"] = btnLunchState;
                        if (drsZC.Length > 0)
                        {
                            ViewData["orderLunchGuid"] = drsZC[0]["guid"].ToString();
                        }
                        else
                        {
                            ViewData["orderLunchGuid"] = String.Empty;
                        }
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                    {
                        //if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()))
                        //{
                        //    //7:00 - 11:30
                        //    if (drsWC.Length == 0)
                        //    {
                        //        btnDinnerState = "我要预订";
                        //    }
                        //    else
                        //    {
                        //        btnDinnerState = "订餐成功";
                        //    }
                        //}
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                        {
                            //11:30 - 13:30
                            if (drsWC.Length == 0)
                            {
                                btnDinnerState = "不可预订";
                            }
                            else
                            {
                                if (drsWC[0]["osState"].ToString() == "已预订")
                                {
                                    btnDinnerState = "扫码就餐";
                                }
                                else if (drsWC[0]["osState"].ToString() == "已就餐")
                                {
                                    btnDinnerState = "已用餐";
                                }
                                else if (drsWC[0]["osState"].ToString() == "已转带")
                                {
                                    btnDinnerState = "已转带";
                                }
                            }
                        }
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                        {
                            //13:30 - 7:00
                            if (drsWC.Length == 0)
                            {
                                btnDinnerState = "未预订";
                            }
                            else
                            {
                                if (drsWC[0]["osState"].ToString() == "已预订" || drsWC[0]["osState"].ToString() == "已转带")
                                {
                                    btnDinnerState = "未用餐";
                                }
                                else if (drsWC[0]["osState"].ToString() == "已就餐")
                                {
                                    btnDinnerState = "已用餐";
                                }
                            }
                        }
                        ViewData["btnDinnerState"] = btnDinnerState;
                        if (drsWC.Length > 0)
                        {
                            ViewData["orderDinnerGuid"] = drsWC[0]["guid"].ToString();
                        }
                        else
                        {
                            ViewData["orderDinnerGuid"] = String.Empty;
                        }
                    }


                }

                //配餐信息
                //string dateBeg = String.Empty;
                //string dateEnd = String.Empty;
                //string weekBeg = String.Empty;
                //string weekEnd = String.Empty;
                string arrsGuid = String.Empty;
                DataSet dsSetMeal = new DataSet();
                DataSet dsDailyMeal = new DataSet();
                string dateNow = String.Empty;

                //dateNow = DateTime.Now.ToShortDateString();
                dateNow = DateTime.Now.Year + "-" + "1-1";
                SetMeal pSetMeal = new SetMeal();
                dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate = '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate desc");
                ViewData["dsSetMeal"] = dsSetMeal.Tables[0];

                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSetMeal.Tables[0].Rows.Count; i++)
                    {
                        arrsGuid += "'" + dsSetMeal.Tables[0].Rows[i]["guid"].ToString() + "',";
                    }
                    arrsGuid = arrsGuid.Substring(0, arrsGuid.Length - 1);
                }
                DailyMeal pDailyMeal = new DailyMeal();
                if (arrsGuid != String.Empty)
                {
                    dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in(" + arrsGuid + ")", "a.sGuid,b.vtID");
                    ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];
                }

                //微信Config绑定
                WXConfig();

            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult History()
        {
            //获取登录用户
            PublicDao pPublicDao = new PublicDao();
            Statistics pStatistics = new Statistics();
            string accessToken = pPublicDao.GetCacheAccessToken();
            string userID = HttpContext.Session["userID"].ToString();
            string userName = HttpContext.Session["userName"].ToString();

            //string company = "集团总部";
            //string restaurantGuid = Request.QueryString["rGuid"].ToString();
            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            DataSet dsRInfo = new DataSet();
            DataSet dsMenuInfo = new DataSet();
            DataSet dszd_VegetableType = new DataSet();

            DateTime nowday = DateTime.Now;

            //获取餐厅信息
            RestaurantInfo pRestaurantInfo = new RestaurantInfo();
            dsRInfo = pRestaurantInfo.GetList("guid = '" + restaurantGuid + "'");
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

            //当月预订、实际
            DataSet dsThisMonth = new DataSet();
            string month = nowday.ToString("yyyy-MM");
            dsThisMonth = pStatistics.GetThisMonthByUser(month, restaurantGuid, userID);
            ViewData["dsThisMonth"] = dsThisMonth.Tables[0];

            //当天午餐预订、实际
            DataSet dsTodayLunch = new DataSet();
            dsTodayLunch = pStatistics.GetTodayLunchByUser(nowday.ToShortDateString(), restaurantGuid, userID);
            ViewData["dsTodayLunch"] = dsTodayLunch.Tables[0];

            //当天晚餐预订、实际
            DataSet dsTodayDinner = new DataSet();
            dsTodayDinner = pStatistics.GetTodayDinnerByUser(nowday.ToShortDateString(), restaurantGuid, userID);
            ViewData["dsTodayDinner"] = dsTodayDinner.Tables[0];


            return View();
        }

        public ActionResult DailyMeal()
        {
            try
            {
                PublicDao pPublicDao = new PublicDao();

                DataSet dsRInfo = new DataSet();
                DataSet dszd_VegetableType = new DataSet();
                DataSet dsMenuInfo = new DataSet();
                string company = "集团总部";
                string restaurantGuid = String.Empty;
                int vID = Convert.ToInt32(Request.QueryString["vID"]);

                //获取餐厅信息
                RestaurantInfo pRestaurantInfo = new RestaurantInfo();
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
                SetMeal pSetMeal = new SetMeal();
                dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate = '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "' and eGuid=" + vID, "begDate desc");

                ViewData["dsSetMeal"] = dsSetMeal.Tables[0];

                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSetMeal.Tables[0].Rows.Count; i++)
                    {
                        arrsGuid += "'" + dsSetMeal.Tables[0].Rows[i]["guid"].ToString() + "',";
                    }
                    arrsGuid = arrsGuid.Substring(0, arrsGuid.Length - 1);
                }
                DailyMeal pDailyMeal = new DailyMeal();
                if (arrsGuid != String.Empty)
                {
                    dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in(" + arrsGuid + ")", "a.sGuid,b.vtID");
                    ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];
                }

            }
            catch (Exception ex)
            {

            }

            return View();
        }



        [HttpPost]
        public ActionResult SetCollect(int eid, string etype, DateTime orderDate)
        {
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();
            OrderInfo pOrderInfo = new OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            int rCount = 0;
            string errorInfo = String.Empty;
            DataSet dsOI = new DataSet();

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();

            dsOI = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + orderDate + "'");
            if (dsOI.Tables[0].Rows.Count == 0)
            {
                mOrderInfo.rGuid = restaurantGuid;
                mOrderInfo.guid = Guid.NewGuid();
                mOrderInfo.orderDate = orderDate;
                mOrderInfo.eID = eid;
                mOrderInfo.eType = etype;
                mOrderInfo.rDate = DateTime.Now;
                //mOrderInfo.eDate = null;
                mOrderInfo.rUserID = userID;
                mOrderInfo.rUser = userName;
                mOrderInfo.isTakeout = 0;
                //mOrderInfo.tDate = null;
                //mOrderInfo.tUserID = null;
                //mOrderInfo.tUser = null; 
                mOrderInfo.osGuid = 2;
                mOrderInfo.osState = "已预订";
                mOrderInfo.creatorID = userID;
                mOrderInfo.creator = userName;

                LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                mOrderState.oGuid = mOrderInfo.guid.ToString();
                mOrderState.sID = 2;
                mOrderState.state = "已预订";
                mOrderState.creatorID = userID;
                mOrderState.creator = userName;

                pPublicDao.TransactionOrder(mOrderInfo, mOrderState, ref rCount, ref errorInfo);
                return Content("预订成功！");
            }
            else
            {
                pPublicDao.TransactionCancelOrder(dsOI.Tables[0].Rows[0]["guid"].ToString(), ref rCount, ref errorInfo);
                return Content("取消预订！");
            }

            //return RedirectToAction("Index", "Restaurant", new { name = "aaaaaaa" });
            //return View("/LDXX/BillingList");

            //return View();

        }

        [HttpPost]
        public ActionResult scanQRCode(int eid, string etype, string guid, string name)
        {
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();
            OrderInfo pOrderInfo = new OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            int rCount = 0;
            string errorInfo = String.Empty;

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();
            DataTable dsoito = (DataTable)HttpContext.Session["dsOITO"];

            //扫码成功后
            if (HttpContext.Session["restaurantName"].ToString() == name)
            {
                mOrderInfo.eDate = DateTime.Now;
                mOrderInfo.osGuid = 1;
                mOrderInfo.osState = "已就餐";
                mOrderInfo.guid = new Guid(guid);

                LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                mOrderState.oGuid = guid;
                mOrderState.sID = 1;
                mOrderState.state = "已就餐";
                mOrderState.creatorID = userID;
                mOrderState.creator = userName;

                bool tf = pPublicDao.TransactionScanQRCode(mOrderInfo, mOrderState, dsoito, etype, ref rCount, ref errorInfo);

                if (tf == true)
                {
                    return Content("扫描二维码成功！");
                }
                else
                {
                    return Content(errorInfo);
                    //return Content("扫描二维码失败！");
                }
            }
            else
            {
                return Content("无效的二维码！");
            }


        }


        [HttpPost]
        public ActionResult TakeOut(string tUserID, string tUserName, string guid, string eType, DateTime orderDate)
        {
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();
            OrderInfo pOrderInfo = new OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            DataSet dsOITO = new DataSet();
            int rCount = 0;
            string errorInfo = String.Empty;

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();

            //获取转带详情eType+ "' and eType = '" + eType + "'
            dsOITO = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + tUserID + "' and eType = '" + eType + "' and orderDate = '" + orderDate + "'");
            if (dsOITO.Tables[0].Rows.Count > 0)
            {
                mOrderInfo.isTakeout = 1;
                mOrderInfo.tDate = DateTime.Now;
                mOrderInfo.tUser = tUserName;
                mOrderInfo.tUserID = tUserID;
                mOrderInfo.osGuid = 3;
                mOrderInfo.osState = "已转带";
                mOrderInfo.guid = new Guid(guid);

                LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                mOrderState.oGuid = guid;
                mOrderState.sID = 3;
                mOrderState.state = "已转带";
                mOrderState.creatorID = userID;
                mOrderState.creator = userName;

                bool tf = pPublicDao.TransactionTakeOut(mOrderInfo, mOrderState, ref rCount, ref errorInfo);

                if (tf == true)
                {
                    return Content(tUserName + "帮您带饭！");
                }
                else
                {
                    return Content(errorInfo);
                    //return Content("扫描二维码失败！");
                }
            }
            else
            {
                return Content(tUserName + "还未订餐，无法外带！");
            }



        }

        [HttpPost]
        public ActionResult UpdateHtml(string defaultDate, string rGuid)
        {
            Statistics pStatistics = new Statistics();
            StringBuilder html = new StringBuilder();
            string UserID = HttpContext.Session["userID"].ToString();
            try
            {
                DateTime nowday = Convert.ToDateTime(defaultDate);
                //当月预订、实际
                DataSet dsThisMonth = new DataSet();
                string month = nowday.ToString("yyyy-MM");
                dsThisMonth = pStatistics.GetThisMonthByUser(month, rGuid, UserID);

                //当天午餐预订、实际
                DataSet dsTodayLunch = new DataSet();
                dsTodayLunch = pStatistics.GetTodayLunchByUser(nowday.ToShortDateString(), rGuid, UserID);

                //当天晚餐预订、实际
                DataSet dsTodayDinner = new DataSet();
                dsTodayDinner = pStatistics.GetTodayDinnerByUser(nowday.ToShortDateString(), rGuid, UserID);

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
                html.Append("今日订餐情况");
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

        [HttpPost]
        public ActionResult GetRDJson()
        {
            PublicDao pPublicDao = new PublicDao();
            DataSet dsSetMeal = new DataSet();
            string dateNow = String.Empty;
            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

            //获取已配餐日期
            string vtJson = String.Empty;
            dateNow = DateTime.Now.ToShortDateString();
            SetMeal pSetMeal = new SetMeal();
            dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate > '" + dateNow + "'", "begDate asc");
            vtJson = pPublicDao.GetVTJsonByDataset(dsSetMeal, "begDate", "endDate");

            return Content(vtJson);
        }

        [HttpPost]
        public ActionResult GetCurrentA(string type, string now)
        {
            PublicDao pPublicDao = new PublicDao();
            DataSet dsSetMeal = new DataSet();
            string dateNow = String.Empty;
            string returnDate = String.Empty;
            string tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

            //获取已配餐日期
            SetMeal pSetMeal = new SetMeal();

            if (now == "明日")
            {
                now = tomorrow;
            }
            dateNow = DateTime.Now.ToShortDateString();
            if (type == "pre")
            {
                dsSetMeal = pSetMeal.GetList(1, "rGuid = '" + restaurantGuid + "' and  begDate > '" + dateNow + "' and  begDate < '" + now + "'", "begDate desc");
                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == tomorrow)
                    {
                        returnDate = "明日";
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    if (now == tomorrow)
                    {
                        returnDate = "明日";
                        HttpContext.Session["reserveDate"] = now;
                    }
                    else
                    {
                        returnDate = now;
                        HttpContext.Session["reserveDate"] = now;
                    }
                }
            }
            else if (type == "next")
            {
                dsSetMeal = pSetMeal.GetList(1, "rGuid = '" + restaurantGuid + "' and  begDate > '" + now + "'", "begDate asc");
                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == tomorrow)
                    {
                        returnDate = "明日";
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    if (now == tomorrow)
                    {
                        returnDate = "明日";
                        HttpContext.Session["reserveDate"] = now;
                    }
                    else
                    {
                        returnDate = now;
                        HttpContext.Session["reserveDate"] = now;
                    }
                }
            }

            return Content(returnDate);
        }



        public ActionResult Test()
        {
            return View();
        }

        public ActionResult reserved()
        {
            return View();
        }

        public ActionResult TestAjax()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTime()
        {
            return Content(DateTime.Now.ToString());
        }

        private string Week(DateTime dt)
        {
            string weekstr = dt.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": weekstr = "星期一"; break;
                case "Tuesday": weekstr = "星期二"; break;
                case "Wednesday": weekstr = "星期三"; break;
                case "Thursday": weekstr = "星期四"; break;
                case "Friday": weekstr = "星期五"; break;
                case "Saturday": weekstr = "星期六"; break;
                case "Sunday": weekstr = "星期日"; break;
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

        private string PageLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                System.Data.DataTable dt = (System.Data.DataTable)ViewData["dsDInfo"];
            System.Data.DataTable dtOIWC = (System.Data.DataTable)ViewData["dsOI"];
            System.Data.DataTable dtOITO = (System.Data.DataTable)ViewData["dsoito"];//转带

            System.Data.DataRow[] drsZC = dtOIWC.Select("eType = '中餐' ");
            System.Data.DataRow[] drsWC = dtOIWC.Select("eType = '晚餐' ");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["eType"].ToString() == "中餐")
                    {
                        <div class="ldmDiv">
                            <div class="lunch">
                                <div class="imglunch"></div>
                                <p class="p1">
                                    @dt.Rows[i]["eType"].ToString()
                                </p>
                                <p class="p2">
                                    用餐时段：@Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm")~@Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm")
                                </p>
                                <p class="p3" id="btnZCInfo">
                                    想了解详细配餐情况
                                </p>
                            </div>
                            
                            @if (ViewData["btnReserveLunch"] == "我要预订")
                            {
                                <div class="lunchBtn" onclick="Collect_click(@dt.Rows[i]["eID"].ToString(),'@dt.Rows[i]["eType"].ToString()')">
                                    <div class="imgreserve">
                                        <div class="btndes">
                                            我要预订
                                        </div>
                                    </div>
                                </div>
                            }
                            else if (ViewData["btnReserveLunch"] == "订餐成功")
                            {
                                <div class="lunchBtn" onclick="Collect_click(@dt.Rows[i]["eID"].ToString(),'@dt.Rows[i]["eType"].ToString()')">
                                    <div class="imgrSuccess">
                                        <div class="btndes">
                                            订餐成功
                                        </div>
                                    </div>
                                </div>
                            }


                            </div>
                    }
                    else if (dt.Rows[i]["eType"].ToString() == "晚餐")
                    {
                        <div class="ldmDiv">
                            <div class="dinner">
                                <div class="imgdinner"></div>
                                <p class="p1">
                                    @dt.Rows[i]["eType"].ToString()
                                </p>
                                <p class="p2">
                                    用餐时段：@Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm")~@Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm")
                                </p>
                                <p class="p3" id="btnWCInfo">
                                    想了解详细配餐情况
                                </p>
                            </div>


                            @if (ViewData["btnReserveDinner"] == "我要预订")
                            {
                                <div class="lunchBtn" onclick="Collect_click(@dt.Rows[i]["eID"].ToString(),'@dt.Rows[i]["eType"].ToString()')">
                                    <div class="imgreserve">
                                        <div class="btndes">
                                            我要预订
                                        </div>
                                    </div>
                                </div>
                            }
                            else if (ViewData["btnReserveDinner"] == "订餐成功")
                            {
                                <div class="lunchBtn" onclick="Collect_click(@dt.Rows[i]["eID"].ToString(),'@dt.Rows[i]["eType"].ToString()')">
                                    <div class="imgrSuccess">
                                        <div class="btndes">
                                            订餐成功
                                        </div>
                                    </div>
                                </div>
                            }

                            </div>
                    }

                }
            }
        }

        @*详情区域*@
        <div class="details">
            <div class="detailsTitle">
                <div id="rectangle">
                    <p class="detailsP">
                        今日订餐详情
                    </p>
                </div>
            </div>
            @{
                System.Data.DataTable dtOI = (System.Data.DataTable)ViewData["dsOI"];


                if (dtOI.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOI.Rows.Count; i++)
                    {
                        <div class="detailsInfo">
                            <div id="divInfo1">
                                @if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                                {
                                    <div class="divZW">@dtOI.Rows[i]["eType"].ToString()券</div>
                                }
                                else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                                {
                                    <div class="divWW">@dtOI.Rows[i]["eType"].ToString()券</div>
                                }
                                <p>
                                    @dtOI.Rows[i]["rUser"].ToString()
                                </p>
                                @if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 1)
                                {
                                    <div class="divWDR">
                                        @dtOI.Rows[i]["tUser"].ToString()（带）
                                    </div>
                                }
                            </div>
                            <div id="divInfo2">
                                <p class="info2p1">预订时间：@Convert.ToDateTime(dtOI.Rows[i]["rDate"].ToString()).ToString("HH:mm")</p>
                                <p>
                                    就餐时间：@if (dtOI.Rows[i]["eDate"].ToString() != String.Empty)
                                    {
                                        @Convert.ToDateTime(dtOI.Rows[i]["eDate"].ToString()).ToString("HH:mm");
                                    }
                                </p>
                            </div>

                            @if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                            {
                                if (Convert.ToDateTime(ViewData["nowTime"].ToString()) < Convert.ToDateTime(ViewData["lunchrEndTime"].ToString()))
                                {
                                    if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                                    {
                                        <div id="divInfo3" onclick="takeOut_click('@dtOI.Rows[i]["guid"].ToString()',@Convert.ToInt32(ViewData["toCount"]), '@dtOI.Rows[i]["eType"].ToString()','@dtOI.Rows[i]["orderDate"].ToString()')">
                                            <div class="info3img">
                                            </div>
                                        </div>
                                    }
                                    else
                                    {
                                        <div id="divInfo3">
                                            <div class="infoOvertimeimg">
                                            </div>
                                        </div>
                                    }
                                }
                                else
                                {
                                    if (ViewData["btnLunchState"] == "扫码就餐")
                                    {
                                        <div id="divInfo3" onclick="scanQRCode_click(@dtOI.Rows[i]["eID"].ToString(),'@dtOI.Rows[i]["eType"].ToString()','@ViewData["orderLunchGuid"].ToString()')">
                                            <div class="infoQRcodeimg">

                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnLunchState"] == "已用餐")
                                    {
                                        <div id="divInfo3">
                                            <div class="infoFinish">

                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnLunchState"] == "已转带")
                                    {
                                        <div id="divInfo3">
                                            <div class="infoOvertimeimg">

                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnLunchState"] == "未用餐")
                                    {

                                        <div id="divInfo3">
                                            <div class="infoNonMeal">

                                            </div>
                                        </div>
                                    }

                                }

                            }
                            else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                            {
                                if (Convert.ToDateTime(ViewData["nowTime"].ToString()) < Convert.ToDateTime(ViewData["DinnerrEndTime"].ToString()))
                                {
                                    if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                                    {
                                        <div id="divInfo3" onclick="takeOut_click('@dtOI.Rows[i]["guid"].ToString()',@Convert.ToInt32(ViewData["toCount"]),  '@dtOI.Rows[i]["eType"].ToString()','@dtOI.Rows[i]["orderDate"].ToString()')">
                                            <div class="info3img">
                                            </div>
                                        </div>
                                    }
                                    else
                                    {
                                        <div id="divInfo3">
                                            <div class=" infoOvertimeimg">
                                            </div>
                                        </div>
                                    }
                                }
                                else
                                {
                                    if (ViewData["btnDinnerState"] == "扫码就餐")
                                    {
                                        <div id="divInfo3" onclick="scanQRCode_click(@dtOI.Rows[i]["eID"].ToString(),'@dtOI.Rows[i]["eType"].ToString()','@ViewData["orderDinnerGuid"].ToString()')">
                                            <div class="infoQRcodeimg">

                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnDinnerState"] == "已用餐")
                                    {
                                        <div id="divInfo3">
                                            <div class="infoFinish">
    
                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnDinnerState"] == "已转带")
                                    {
                                        <div id="divInfo3">
                                            <div class="infoOvertimeimg">

                                            </div>
                                        </div>
                                    }
                                    else if (ViewData["btnDinnerState"] == "未用餐")
                                    {
                                        <div id="divInfo3">
                                            <div class="infoNonMeal">

                                            </div>
                                        </div>
                                    }

                                }

                            }

                        </div>
                    }
                }
            }

            @{
                System.Data.DataTable dsOITO = (System.Data.DataTable)ViewData["dsOITO"];

                if (dsOITO.Rows.Count > 0)
                {
                    for (int i = 0; i < dsOITO.Rows.Count; i++)
                    {
                        <div class="detailsInfo">
                            <div id="divInfoWD">
                                <div class="divZW">@dsOITO.Rows[i]["eType"].ToString()券</div>
                                <p>
                                    @dsOITO.Rows[i]["rUser"].ToString()
                                </p>
                            </div>
                            <div id="divInfo2">
                                <p class="info2p1">预订时间：@Convert.ToDateTime(dsOITO.Rows[i]["rDate"].ToString()).ToString("HH:mm")</p>
                                <p>
                                    就餐时间：@if (dsOITO.Rows[i]["eDate"].ToString() != String.Empty)
                                    {
                                        @Convert.ToDateTime(dsOITO.Rows[i]["eDate"].ToString()).ToString("HH:mm");
                                    }
                                </p>
                            </div>
                            <div id="divInfo3">
                                <div class="info3imgWD">
                                </div>
                            </div>
                        </div>
                    }
                }
            }



    
        </div>
        <div class="details">
            <div class="detailsTitle">
                <div id="rectangle">
                    <p class="detailsP">
                        未来订餐详情
                    </p>
                </div>
            </div>
            @{
                System.Data.DataTable dtOIFuture = (System.Data.DataTable)ViewData["dsOIFuture"];
                if (dtOIFuture.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOIFuture.Rows.Count; i++)
                    {
                        <div class="detailsInfoW">
                            <div class="dwY">
                                已预订
                            </div>
                            <div class="dwD">
                                @Convert.ToDateTime(dtOIFuture.Rows[i]["orderDate"].ToString()).ToString("M月d日")
                            </div>
                            <div class="dwZ">
                                @dtOIFuture.Rows[i]["eType"].ToString()
                            </div>
                        </div>
                    }
                }
            }
            <div class="divhistorical">
                <a id="history" class="ahistorical">历史订餐统计</a>
            </div>
        </div>

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }


    }
}