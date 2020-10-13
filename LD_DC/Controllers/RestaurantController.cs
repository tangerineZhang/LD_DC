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
using System.Drawing;
using Newtonsoft.Json.Linq;
using LDDC.Model;
using System.Web;
using zd_Company = LDDC.DAL.zd_Company;
using System.Web.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Page = System.Web.UI.Page;

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
                ApiHelper api = new ApiHelper();
                //string accessToken = pPublicDao.GetCacheAccessToken();

                //string Nameid = Request.Url.Query;
                //string token = Nameid.Replace("?token=", "");
           
            
                    if (Request.QueryString["token"] != null)
                    {
                        string token = Request.QueryString["token"].ToString();
                        //string token = "a2dffec4-9e70-4b17-86f8-982f0792bd85";
                        JObject seal = api.SelectNeibu(token);

                        EOPModel eOP = new EOPModel();
                        eOP.code = seal.Value<string>("code");
                        if (eOP.code == "0")
                        {
                            eOP.username = seal["data"]["username"].ToString();
                            eOP.nickname = seal["data"]["nickname"].ToString();
                            ViewData["GetToken"] = token;

                        HttpContext.Session["userID"] = eOP.username;
                        HttpContext.Session["userName"] = eOP.nickname;
                    }
                        else
                        {
                            eOP.username = "";
                            eOP.nickname = "";

                        HttpContext.Session["userID"] = eOP.username;
                        HttpContext.Session["userName"] = eOP.nickname;
                        return View("Error");
                        }
                    }
                else
                {
                    return View("Error");
                }


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


                //HttpContext.Session["userID"] = OAuthUser_Model.UserId;
                //HttpContext.Session["userName"] = pUserGetJson.name;


                //HttpContext.Session["userID"] = eOP.username;
                //HttpContext.Session["userName"] = eOP.nickname;

                //HttpContext.Session["userID"] = "zhangwdc";
                //HttpContext.Session["userName"] = "张伟东";

                //HttpContext.Session["userID"] = "huangjjb";
                //HttpContext.Session["userName"] = "黄俊杰";
                //HttpContext.Session["userID"] = "yangruia";
                //HttpContext.Session["userName"] = "杨锐";

                ViewData["orderDate"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                //当前日期
                HttpContext.Session["currentDate"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

                //当前时间
                string nowTime = DateTime.Now.ToShortTimeString();
                //nowTime = "18:31";
                ViewData["nowTime"] = nowTime;

                string company = "集团总部";
                //string company = "集团总部";
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
                string btnRMeetingLunch = String.Empty;

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
                LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();

                LDDC.DAL.OrderInfo pOInfo = new LDDC.DAL.OrderInfo();
                DataSet dsOInfo = new DataSet();
                dsOInfo = pOInfo.GetList(0, "rUserID = '" + userID + "' and orderDate >= '" + ViewData["orderDate"] + "'", "OrderDate");

                if (dsOInfo.Tables[0].Rows.Count > 0)
                {
                    dsRInfo = pRestaurantInfo.GetList("guid = '" + dsOInfo.Tables[0].Rows[0]["rGuid"].ToString() + "'");
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
                }

                //已配餐信息
                DataSet dsSetMeal = new DataSet();
                //string dateNow = String.Empty;
                //dateNow = DateTime.Now.Year + "-" + "1-1";
                LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
                //dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
                dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
                ViewData["dsSetMeal"] = dsSetMeal.Tables[0];

                //显示配餐详情用
                DataSet dsSetMealPC = new DataSet();
                dsSetMealPC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
                ViewData["dsSetMealPC"] = dsSetMealPC.Tables[0];

                //显示配餐详情用-晚餐
                DataSet dsSetMealWC = new DataSet();
                dsSetMealWC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "' and eType = '晚餐'", "begDate asc");



                //初始化-预定时间
                //if (dsSetMeal.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd") == Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"))
                //    {
                //        ViewData["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //        ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy年MM月dd日") + " " + "明日";
                //        HttpContext.Session["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //        HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy年MM月dd日") + " " + "明日";
                //    }
                //    else
                //    {
                //        ViewData["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                //        ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                //        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                //        HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                //    }
                //}
                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        ViewData["reserveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                        ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + "今日";
                        HttpContext.Session["reserveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                        HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + "今日";
                    }
                    else
                    {
                        ViewData["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                        ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                        HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                    }
                }


                //获取餐厅用餐时间
                LDDC.DAL.Dinnertime pDinnertime = new LDDC.DAL.Dinnertime();
                dsDInfo = pDinnertime.GetList("rGuid = '" + restaurantGuid + "'");
                if (dsDInfo.Tables[0].Rows.Count > 0)
                {
                    ViewData["dsDInfo"] = dsDInfo.Tables[0];
                    HttpContext.Session["dsDInfo"] = dsDInfo.Tables[0];
                }

                LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
                DataSet dsOIZ = new DataSet();
                DataSet dsOIW = new DataSet();
                if (dsDInfo.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                    {
                        if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                        {
                            ViewData["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            HttpContext.Session["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            //获取今日订餐详情-中餐
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                            {
                                dsOIZ = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "' and eid = 2", "eID");
                                ViewData["dsOIZ"] = dsOIZ.Tables[0];
                            }
                            else
                            {
                                ViewData["dsOIZ"] = null;
                            }
                        }
                        else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                        {
                            ViewData["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            HttpContext.Session["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            //获取今日订餐详情-晚餐
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                            {
                                dsOIW = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "' and eid = 3", "eID");
                                ViewData["dsOIW"] = dsOIW.Tables[0];
                            }
                            else
                            {
                                ViewData["dsOIW"] = null;
                            }
                        }
                    }
                }

                //获取页面权限——预定会议餐
                LDDC.DAL.PagePermission pPagePermission = new LDDC.DAL.PagePermission();
                DataSet dsPP = new DataSet();
                dsPP = pPagePermission.GetList("userID = '" + HttpContext.Session["userID"] + "' and pID = 1");
                ViewData["dsPP"] = dsPP.Tables[0];
                HttpContext.Session["dsPP"] = dsPP.Tables[0];

                //获取页面权限——预定晚餐
                DataSet dsPPWC = new DataSet();
                dsPPWC = pPagePermission.GetList("userID = '" + HttpContext.Session["userID"] + "' and pID = 2 and isValid = 1");
                ViewData["dsPPWC"] = dsPPWC.Tables[0];
                HttpContext.Session["dsPPWC"] = dsPPWC.Tables[0];
                //4月30号开放晚餐预定
                //string week = DateTime.Now.DayOfWeek.ToString();
                //if (week == "Friday")
                //{
                //    ViewData["isOpenWC"] = false;
                //}
                //else
                //{ 
                //    ViewData["isOpenWC"] = true;
                //}
                if (dsSetMealWC.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = dsSetMealWC.Tables[0].Select("begDate = '" + DateTime.Now.ToShortDateString() + " 00:00:00.000'");
                    if (drs.Count() > 0)
                    {
                        ViewData["isOpenWC"] = true;
                    }
                    else
                    {
                        ViewData["isOpenWC"] = false;
                    }
                }
                else
                {
                    ViewData["isOpenWC"] = false;
                }



                //ViewData["isOpenWC"] = System.DateTime.Now > Convert.ToDateTime("2019-4-30 14:00:00") ? true : false;

                ////获取今日订餐详情-中餐
                //OrderInfo pOrderInfo = new OrderInfo();
                //DataSet dsOIZ = new DataSet();
                //dsOIZ = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "' and eid = 2", "eID");
                //ViewData["dsOIZ"] = dsOIZ.Tables[0];

                ////获取今日订餐详情-晚餐
                //DataSet dsOIW = new DataSet();
                //dsOIW = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "' and eid = 3", "eID");
                //ViewData["dsOIW"] = dsOIW.Tables[0];


                //获取今日订餐详情
                pOrderInfo = new LDDC.DAL.OrderInfo();
                dsOI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + ViewData["orderDate"] + "'", "eID");
                ViewData["dsOI"] = dsOI.Tables[0];
                HttpContext.Session["dsOI"] = dsOI.Tables[0];

                //获取明日订餐详情
                dsRI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + HttpContext.Session["reserveDate"] + "'", "eID");

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

                DataRow[] drsZCTO = dsOITO.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsWCTO = dsOITO.Tables[0].Select("eType = '晚餐' ");

                ViewData["ZCtoCount"] = drsZCTO.Length;
                ViewData["WCtoCount"] = drsWCTO.Length;

                //明日控件状态
                DataRow[] drsRZC = dsRI.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsRWC = dsRI.Tables[0].Select("eType = '晚餐' ");
                DataRow[] drsMeetingWC = dsRI.Tables[0].Select("eType = '会议餐（中餐）' ");
                if (drsMeetingWC.Length > 0)
                {
                    ViewData["rAmount"] = drsMeetingWC[0]["rAmount"];
                }
                else
                {
                    ViewData["rAmount"] = 0;
                }

                for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                {
                    if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                    {
                        if (HttpContext.Session["reserveDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                        {//今日
                            if (drsRZC.Length == 0)
                            {
                                btnReserveLunch = "未预订";
                            }
                            else
                            {
                                btnReserveLunch = "已预订";
                            }
                        }
                        else if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToDouble(dsDInfo.Tables[0].Rows[i]["rWhichDay"].ToString()))).ToString("yyyy-MM-dd"))
                        {//明日中餐是否预定
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                            {
                                //预定时间之外
                                if (drsRZC.Length == 0)
                                {
                                    btnReserveLunch = "未预订";
                                }
                                else
                                {
                                    btnReserveLunch = "已预订";
                                }
                            }
                            else
                            {
                                if (drsRZC.Length == 0)
                                {
                                    btnReserveLunch = "我要预订";
                                }
                                else
                                {
                                    btnReserveLunch = "订餐成功";
                                }
                            }
                        }
                        else
                        {
                            if (drsRZC.Length == 0)
                            {
                                btnReserveLunch = "我要预订";
                            }
                            else
                            {
                                btnReserveLunch = "订餐成功";
                            }
                        }
                        ViewData["btnReserveLunch"] = btnReserveLunch;
                        HttpContext.Session["btnReserveLunch"] = btnReserveLunch;
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                    {
                        //明日晚餐是否预定
                        if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToDouble(dsDInfo.Tables[0].Rows[i]["rWhichDay"].ToString()))).ToString("yyyy-MM-dd"))
                        {
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                            {
                                if (drsRWC.Length == 0)
                                {
                                    btnReserveDinner = "未预订";
                                }
                                else
                                {
                                    btnReserveDinner = "已预订";
                                }
                            }
                            else
                            {
                                if (drsRWC.Length == 0)
                                {
                                    btnReserveDinner = "我要预订";
                                }
                                else
                                {
                                    btnReserveDinner = "订餐成功";
                                }
                            }
                        }
                        else
                        {
                            if (drsRWC.Length == 0)
                            {
                                btnReserveDinner = "我要预订";
                            }
                            else
                            {
                                btnReserveDinner = "订餐成功";
                            }
                        }
                        ViewData["btnReserveDinner"] = btnReserveDinner;
                        HttpContext.Session["btnReserveDinner"] = btnReserveDinner;
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "会议餐（中餐）")
                    {
                        if (HttpContext.Session["reserveDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                        {//今日
                            btnRMeetingLunch = "已就餐";
                        }
                        else
                        {
                            btnRMeetingLunch = "我要预订";
                        }
                        ViewData["btnRMeetingLunch"] = btnRMeetingLunch;
                        HttpContext.Session["btnRMeetingLunch"] = btnRMeetingLunch;
                    }
                }

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
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
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
                                else if (drsZC[0]["osState"].ToString() == "已就餐" || drsZC[0]["osState"].ToString() == "未预定就餐")
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
                                if (drsZC[0]["osState"].ToString() == "已预订")
                                {
                                    btnLunchState = "未用餐";
                                }
                                else if (drsZC[0]["osState"].ToString() == "已就餐" || drsZC[0]["osState"].ToString() == "未预定就餐")
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

                        //if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
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
                                else if (drsWC[0]["osState"].ToString() == "已就餐" || drsWC[0]["osState"].ToString() == "未预定就餐")
                                {
                                    btnDinnerState = "已用餐";
                                }
                                else if (drsWC[0]["osState"].ToString() == "已转带")
                                {
                                    btnDinnerState = "已转带";
                                }
                            }
                        }
                        //if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
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
                                else if (drsWC[0]["osState"].ToString() == "已就餐" || drsWC[0]["osState"].ToString() == "未预定就餐")
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
                DataSet dsDailyMeal = new DataSet();
                if (dsSetMealPC.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSetMealPC.Tables[0].Rows.Count; i++)
                    {
                        arrsGuid += "'" + dsSetMealPC.Tables[0].Rows[i]["guid"].ToString() + "',";
                    }
                    arrsGuid = arrsGuid.Substring(0, arrsGuid.Length - 1);
                }
                LDDC.DAL.DailyMeal pDailyMeal = new LDDC.DAL.DailyMeal();
                if (arrsGuid != String.Empty)
                {
                    dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in(" + arrsGuid + ")", "a.sGuid,b.vtID");
                    ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];
                }

                //就餐券背景色
                LDDC.DAL.TicketInfo pTicketInfo = new LDDC.DAL.TicketInfo();
                LDDC.Model.TicketInfo mTicketInfo = new LDDC.Model.TicketInfo();
                DataSet dsColorZ = new DataSet();
                DataSet dsColorW = new DataSet();
                dsColorZ = pTicketInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and orderDate = '" + ViewData["orderDate"] + "' and eID = 2", "eID");
                dsColorW = pTicketInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and orderDate = '" + ViewData["orderDate"] + "' and eID = 3", "eID");
                if (dsColorZ.Tables[0].Rows.Count > 0)
                {//中餐
                    ViewData["rgbZ1"] = dsColorZ.Tables[0].Rows[0]["rRgb1"].ToString();
                    ViewData["rgbZ2"] = dsColorZ.Tables[0].Rows[0]["rRgb2"].ToString();
                    ViewData["rgbZ3"] = dsColorZ.Tables[0].Rows[0]["rRgb3"].ToString();
                }
                else
                {
                    string rgb1 = GetRandomColor();
                    string rgb2 = GetRandomColor();
                    string rgb3 = GetRandomColor();
                    mTicketInfo.rGuid = restaurantGuid;
                    mTicketInfo.orderDate = Convert.ToDateTime(ViewData["orderDate"]);
                    mTicketInfo.eID = 2;
                    mTicketInfo.eType = "中餐";
                    mTicketInfo.rRgb1 = rgb1;
                    mTicketInfo.rRgb2 = rgb2;
                    mTicketInfo.rRgb3 = rgb3;
                    pTicketInfo.Add(mTicketInfo);

                    ViewData["rgbZ1"] = rgb1;
                    ViewData["rgbZ2"] = rgb2;
                    ViewData["rgbZ3"] = rgb3;
                }
                if (dsColorW.Tables[0].Rows.Count > 0)
                {//晚餐
                    ViewData["rgb1"] = GetRandomColor();
                    ViewData["rgb2"] = GetRandomColor();
                    ViewData["rgb3"] = GetRandomColor();
                }
                else
                {
                    string rgb1 = GetRandomColor();
                    string rgb2 = GetRandomColor();
                    string rgb3 = GetRandomColor();
                    mTicketInfo.rGuid = restaurantGuid;
                    mTicketInfo.orderDate = Convert.ToDateTime(ViewData["orderDate"]);
                    mTicketInfo.eID = 3;
                    mTicketInfo.eType = "晚餐";
                    mTicketInfo.rRgb1 = rgb1;
                    mTicketInfo.rRgb2 = rgb2;
                    mTicketInfo.rRgb3 = rgb3;
                    pTicketInfo.Add(mTicketInfo);

                    ViewData["rgb1"] = rgb1;
                    ViewData["rgb2"] = rgb2;
                    ViewData["rgb3"] = rgb3;
                }


                //微信Config绑定
                WXConfig();
            }
            catch (Exception ex)
            {

            }

            return View();
        }


        //[HttpGet]
        //public JavaScriptResult Tests()
        //{
        //    string js =@
        //}

        public ActionResult History()
        {
            //获取登录用户
            PublicDao pPublicDao = new PublicDao();
            Statistics pStatistics = new Statistics();
            //string accessToken = pPublicDao.GetCacheAccessToken();
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
            LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
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

            //当天会议午餐预订、实际
            DataSet dsMeetingLunch = new DataSet();
            dsMeetingLunch = pStatistics.GetMeetingLunchByUser(nowday.ToShortDateString(), restaurantGuid, userID);
            ViewData["dsMeetingLunch"] = dsMeetingLunch.Tables[0];

            //当天晚餐预订、实际
            DataSet dsTodayDinner = new DataSet();
            dsTodayDinner = pStatistics.GetTodayDinnerByUser(nowday.ToShortDateString(), restaurantGuid, userID);
            ViewData["dsTodayDinner"] = dsTodayDinner.Tables[0];

            return View();
        }

        public ActionResult Error()
        {
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
                LDDC.DAL.RestaurantInfo pRestaurantInfo = new LDDC.DAL.RestaurantInfo();
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
                LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
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
                LDDC.DAL.DailyMeal pDailyMeal = new LDDC.DAL.DailyMeal();
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
            LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
            int rCount = 0;
            string errorInfo = String.Empty;
            DataSet dsOI = new DataSet();
            DataSet dsIsOI = new DataSet();
            DataSet dsSetMeal = new DataSet();

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();

            dsSetMeal = pSetMeal.GetList("rGuid = '" + restaurantGuid + "' and eGuid = '" + eid + "' and (begDate <= '" + orderDate + "' and endDate >= '" + orderDate + "')");
            if (dsSetMeal.Tables[0].Rows.Count > 0)
            {
                dsIsOI = pOrderInfo.GetList("rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + orderDate + "'");
                dsOI = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + orderDate + "'");
                if (dsOI.Tables[0].Rows.Count == 0)
                {
                    if (dsIsOI.Tables[0].Rows.Count == 0)
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
                        mOrderInfo.rAmount = 1;
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
                        //return Content("预订成功！");          
                    }
                    else
                    {
                        return Content("已预订本日就餐，请取消后再预订！");
                    }
                }
                else
                {
                    pPublicDao.TransactionCancelOrder(dsOI.Tables[0].Rows[0]["guid"].ToString(), ref rCount, ref errorInfo);
                    //return Content("取消预订！");
                }
                //修改页面Html
                StringBuilder sb = new StringBuilder();
                //sb.Append(DateDivLoad());
                sb.Append(ReserveDivLoad());
                sb.Append(TodayDivLoad());
                sb.Append(TomorrowDivLoad());
                sb.Append(MuiJqueryLoad());
                return Content(sb.ToString());
            }
            else
            {
                return Content("还未配餐无法预定！");
            }



            //return RedirectToAction("Index", "Restaurant", new { name = "aaaaaaa" });
            //return View("/LDXX/BillingList");

            //return View();

        }


        [HttpPost]
        public ActionResult ReloadCollect(string rid)
        {
            HttpContext.Session["restaurantGuid"] = rid;
            string restaurantGuid = rid;

            //已配餐信息
            DataSet dsSetMeal = new DataSet();
            //string dateNow = String.Empty;
            //dateNow = DateTime.Now.Year + "-" + "1-1";
            LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
            //dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
            dsSetMeal = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
            ViewData["dsSetMeal"] = dsSetMeal.Tables[0];

            //显示配餐详情用
            DataSet dsSetMealPC = new DataSet();
            dsSetMealPC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
            ViewData["dsSetMealPC"] = dsSetMealPC.Tables[0];

            //显示配餐详情用-晚餐
            DataSet dsSetMealWC = new DataSet();
            dsSetMealWC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "' and eType = '晚餐'", "begDate asc");


            if (dsSetMeal.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    ViewData["reserveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                    ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + "今日";
                    HttpContext.Session["reserveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                    HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + "今日";
                }
                else
                {
                    ViewData["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                    ViewData["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                    HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy-MM-dd");
                    HttpContext.Session["showReserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]).ToString("yyyy/MM/dd") + " " + Week(Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"]));
                }
            }

            //修改页面Html
            StringBuilder sb = new StringBuilder();
            sb.Append(ReserveDivLoad());
            sb.Append(TodayDivLoad());
            sb.Append(TomorrowDivLoad());
            sb.Append(MealMenuLoad());
            sb.Append(MuiJqueryLoad());
            return Content(sb.ToString());
        }


        [HttpPost]
        public ActionResult BookingMeeting(int eid, string etype, DateTime orderDate, int amount)
        {
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();
            LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
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
                //添加会议预定信息
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
                mOrderInfo.rAmount = amount;
                mOrderInfo.osGuid = 4;
                mOrderInfo.osState = "会议餐";
                mOrderInfo.creatorID = userID;
                mOrderInfo.creator = userName;

                LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                mOrderState.oGuid = mOrderInfo.guid.ToString();
                mOrderState.sID = 4;
                mOrderState.state = "会议餐";
                mOrderState.creatorID = userID;
                mOrderState.creator = userName;

                pPublicDao.TransactionOrder(mOrderInfo, mOrderState, ref rCount, ref errorInfo);
            }
            else
            {
                pPublicDao.updateMeetingOrder(dsOI.Tables[0].Rows[0]["guid"].ToString(), amount, ref rCount, ref errorInfo);
            }

            //修改页面Html
            StringBuilder sb = new StringBuilder();
            //sb.Append(DateDivLoad());
            sb.Append(ReserveDivLoad());
            sb.Append(TodayDivLoad());
            sb.Append(TomorrowDivLoad());
            sb.Append(MuiJqueryLoad());
            return Content(sb.ToString());
        }

        [HttpPost]
        public ActionResult scanQRCode(int eid, string etype, string guid, string name)
        {
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();
            LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            LDDC.DAL.QRCode pQRCode = new LDDC.DAL.QRCode();
            int rCount = 0;
            string errorInfo = String.Empty;
            string code = String.Empty;
            DataSet dsQRCode = new DataSet();

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();
            DataTable dsoito = (DataTable)HttpContext.Session["dsOITO"];
            dsQRCode = pQRCode.GetList("rGuid='" + restaurantGuid + "' and isValid =1");
            if (dsQRCode.Tables[0].Rows.Count > 0)
            {
                code = dsQRCode.Tables[0].Rows[0]["code"].ToString();
            }

            //扫码成功后
            //if (HttpContext.Session["restaurantName"].ToString() == name)
            if (code == name)
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
        public ActionResult MealNotReserved(int eid, string etype, string name)
        {
            DateTime orderDate = DateTime.Now;
            PublicDao pPublicDao = new PublicDao();
            LDDC.DAL.QRCode pQRCode = new LDDC.DAL.QRCode();
            LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
            LDDC.Model.OrderInfo mOrderInfo = new LDDC.Model.OrderInfo();

            DataSet dsOI = new DataSet();
            DataSet dsIsOI = new DataSet();
            int rCount = 0;
            string errorInfo = String.Empty;
            string code = String.Empty;
            DataSet dsQRCode = new DataSet();

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();
            DataTable dsoito = (DataTable)HttpContext.Session["dsOITO"];
            dsQRCode = pQRCode.GetList("rGuid='" + restaurantGuid + "' and isValid =1");
            if (dsQRCode.Tables[0].Rows.Count > 0)
            {
                code = dsQRCode.Tables[0].Rows[0]["code"].ToString();
            }

            //扫码成功后
            //if (HttpContext.Session["restaurantName"].ToString() == name)
            if (code == name)
            {
                //判断是否有扫码
                dsIsOI = pOrderInfo.GetList("rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + orderDate + "'");
                dsOI = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + orderDate + "'");
                if (dsOI.Tables[0].Rows.Count == 0)
                {
                    if (dsIsOI.Tables[0].Rows.Count == 0)
                    {
                        mOrderInfo.rGuid = restaurantGuid;
                        mOrderInfo.guid = Guid.NewGuid();
                        mOrderInfo.orderDate = orderDate;
                        mOrderInfo.eID = eid;
                        mOrderInfo.eType = etype;
                        mOrderInfo.rDate = DateTime.Now;
                        mOrderInfo.eDate = DateTime.Now;
                        mOrderInfo.rUserID = userID;
                        mOrderInfo.rUser = userName;
                        mOrderInfo.isTakeout = 0;
                        //mOrderInfo.tDate = null;
                        //mOrderInfo.tUserID = null;
                        //mOrderInfo.tUser = null; s
                        mOrderInfo.rAmount = 1;
                        mOrderInfo.osGuid = 5;
                        mOrderInfo.osState = "未预定就餐";
                        mOrderInfo.creatorID = userID;
                        mOrderInfo.creator = userName;

                        LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                        mOrderState.oGuid = mOrderInfo.guid.ToString();
                        mOrderState.sID = 5;
                        mOrderState.state = "未预定就餐";
                        mOrderState.creatorID = userID;
                        mOrderState.creator = userName;

                        bool tf = pPublicDao.MealNotReserved(mOrderInfo, mOrderState, dsoito, etype, ref rCount, ref errorInfo);
                        if (tf == true)
                        {
                            DataSet dsWYDJC = new DataSet();
                            dsWYDJC = pOrderInfo.GetList("rUserID = '" + userID + "' and osGuid = 5 and orderDate >= convert(varchar,dateadd(day,-day(getdate())+1,getdate()),111)+' 00:00:01' and orderDate <= convert(varchar,dateadd(day,-day(getdate()),dateadd(month,1,getdate())),111)+' 23:59:59'");
                            return Content("扫描二维码成功！本月未预定扫码就餐 " + dsWYDJC.Tables[0].Rows.Count + " 次。");
                        }
                        else
                        {
                            return Content(errorInfo);
                            //return Content("扫描二维码失败！");
                        }
                        //return Content("测试！");
                    }
                    else
                    {
                        return Content("已预订本日就餐，请取消后再预订！");
                    }
                }
                else
                {
                    return Content("系统问题，请联系管理员！");
                }
            }
            else
            {
                return Content("无效的二维码！");
            }


        }


        [HttpPost]
        public ActionResult scanQRCodeTO(int eid, string etype, string name)
        {
            PublicDao pPublicDao = new PublicDao();
            LDDC.DAL.QRCode pQRCode = new LDDC.DAL.QRCode();
            int rCount = 0;
            string errorInfo = String.Empty;
            string code = String.Empty;
            DataSet dsQRCode = new DataSet();

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();
            DataTable dsoito = (DataTable)HttpContext.Session["dsOITO"];
            dsQRCode = pQRCode.GetList("rGuid='" + restaurantGuid + "' and isValid =1");
            if (dsQRCode.Tables[0].Rows.Count > 0)
            {
                code = dsQRCode.Tables[0].Rows[0]["code"].ToString();
            }

            //扫码成功后
            //if (HttpContext.Session["restaurantName"].ToString() == name)
            if (code == name)
            {
                LDDC.Model.OrderState mOrderState = new LDDC.Model.OrderState();
                //mOrderState.oGuid = guid;
                mOrderState.sID = 1;
                mOrderState.state = "已就餐";
                mOrderState.creatorID = userID;
                mOrderState.creator = userName;

                bool tf = pPublicDao.TransactionScanQRCodeTO(mOrderState, dsoito, etype, ref rCount, ref errorInfo);

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
            LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
            PublicDao pPublicDao = new PublicDao();
            DataSet dsOITO = new DataSet();
            int rCount = 0;
            string errorInfo = String.Empty;

            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
            string userID = HttpContext.Session["userID"].ToString();//当前登录人
            string userName = HttpContext.Session["userName"].ToString();

            //当转带人和预定人是同一个时
            if (userID == tUserID)
            {
                return Content("不能选择自己转带！");
            }

            //获取转带详情eType+ "' and eType = '" + eType + "'
            //dsOITO = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + tUserID + "' and eType = '" + eType + "' and orderDate = '" + orderDate + "'");
            //if (dsOITO.Tables[0].Rows.Count > 0)
            //{
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
            //}
            //else
            //{
            //    return Content(tUserName + "还未订餐，无法外带！");
            //}



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

                //当天会议午餐预订、实际
                DataSet dsMeetingLunch = new DataSet();
                dsMeetingLunch = pStatistics.GetMeetingLunchByUser(nowday.ToShortDateString(), rGuid, UserID);

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
                    html.Append("<div class=\"divYJ\">预定 <span style=\"font-size: 1.0rem;\">" + dsThisMonth.Tables[0].Rows[0]["yj"].ToString() + "</span> 次</div>");
                    html.Append("<div class=\"divSJ\">就餐 <span style=\"font-size: 1.0rem;\">" + @dsThisMonth.Tables[0].Rows[0]["sj"].ToString() + "</span> 次</div>");
                }
                html.Append("</div>");
                html.Append("</div>");
                html.Append("<div class=\"detailsTitle\">");
                html.Append("<div id=\"rectangle\">");
                html.Append("<p class=\"detailsP\">");

                html.Append("<span>" + nowday.ToString("dd") + "日</span>订餐情况");
                //html.Append("今日订餐情况");
                html.Append("</p>");
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divZW\">中餐</div>");
                if (dsTodayLunch.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预定 <span style=\"font-size: 1.0rem;\">" + dsTodayLunch.Tables[0].Rows[0]["yj"].ToString() + "</span> 次</div>");
                    html.Append("<div class=\"divSJ\">就餐 <span style=\"font-size: 1.0rem;\">" + dsTodayLunch.Tables[0].Rows[0]["sj"].ToString() + "</span> 次</div>");
                }
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divZW\">会议中餐</div>");
                if (dsMeetingLunch.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预定 <span style=\"font-size: 1.0rem;\">" + dsMeetingLunch.Tables[0].Rows[0]["yj"].ToString() + "</span> 次</div>");
                    html.Append("<div class=\"divSJ\">就餐 <span style=\"font-size: 1.0rem;\">" + dsMeetingLunch.Tables[0].Rows[0]["sj"].ToString() + "</span> 次</div>");
                }
                html.Append("</div>");
                html.Append("</div>");

                html.Append("<div class=\"detailsInfo\">");
                html.Append("<div id=\"divInfo1\">");
                html.Append("<div class=\"divWW\">晚餐</div>");
                if (dsTodayDinner.Tables[0].Rows.Count > 0)
                {
                    html.Append("<div class=\"divYJ\">预定 <span style=\"font-size: 1.0rem;\">" + dsTodayDinner.Tables[0].Rows[0]["yj"].ToString() + "</span> 次</div>");
                    html.Append("<div class=\"divSJ\">就餐 <span style=\"font-size: 1.0rem;\">" + dsTodayDinner.Tables[0].Rows[0]["sj"].ToString() + "</span> 次</div>");
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
        public ActionResult GetRDJson()
        {
            PublicDao pPublicDao = new PublicDao();
            DataSet dsSetMeal = new DataSet();
            string dateNow = String.Empty;
            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

            //获取已配餐日期
            string vtJson = String.Empty;
            dateNow = DateTime.Now.ToShortDateString();
            LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
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
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();

            //获取已配餐日期
            LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();

            //dateNow = DateTime.Now.ToShortDateString();
            //if (type == "pre")
            //{
            //    dsSetMeal = pSetMeal.GetList(1, "rGuid = '" + restaurantGuid + "' and  begDate > '" + dateNow + "' and  begDate < '" + now + "'", "begDate desc");
            //    if (dsSetMeal.Tables[0].Rows.Count > 0)
            //    {
            //        if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == tomorrow)
            //        {
            //            returnDate = "明日";
            //            HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //        }
            //        else
            //        {
            //            returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //            HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //        }
            //    }
            //    else
            //    {
            //        if (now == tomorrow)
            //        {
            //            returnDate = "明日";
            //            HttpContext.Session["reserveDate"] = now;
            //        }
            //        else
            //        {
            //            returnDate = now;
            //            HttpContext.Session["reserveDate"] = now;
            //        }
            //    }
            //}
            //else if (type == "next")
            //{
            //    dsSetMeal = pSetMeal.GetList(1, "rGuid = '" + restaurantGuid + "' and  begDate > '" + now + "'", "begDate asc");
            //    if (dsSetMeal.Tables[0].Rows.Count > 0)
            //    {
            //        if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == tomorrow)
            //        {
            //            returnDate = "明日";
            //            HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //        }
            //        else
            //        {
            //            returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //            HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
            //        }
            //    }
            //    else
            //    {
            //        if (now == tomorrow)
            //        {
            //            returnDate = "明日";
            //            HttpContext.Session["reserveDate"] = now;
            //        }
            //        else
            //        {
            //            returnDate = now;
            //            HttpContext.Session["reserveDate"] = now;
            //        }
            //    }
            //}

            //if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"))
            //{
            //    HttpContext.Session["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
            //    HttpContext.Session["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy年MM月dd日") + " " + "明日";
            //}
            //else
            //{
            //    HttpContext.Session["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()));
            //}
            dateNow = DateTime.Now.ToShortDateString();
            if (type == "pre")
            {
                dsSetMeal = pSetMeal.GetList(1, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + dateNow + "' and  begDate < '" + now + "'", "begDate desc");
                if (dsSetMeal.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == today)
                    {
                        returnDate = "今日";
                        HttpContext.Session["reserveDate"] = today;
                    }
                    else
                    {
                        returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    if (now == today)
                    {
                        returnDate = "今日";
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
                    if (Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd") == today)
                    {
                        returnDate = "今日";
                        HttpContext.Session["reserveDate"] = today;
                    }
                    else
                    {
                        returnDate = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                        HttpContext.Session["reserveDate"] = Convert.ToDateTime(dsSetMeal.Tables[0].Rows[0]["begDate"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    if (now == today)
                    {
                        returnDate = "今日";
                        HttpContext.Session["reserveDate"] = now;
                    }
                    else
                    {
                        returnDate = now;
                        HttpContext.Session["reserveDate"] = now;
                    }
                }
            }

            if (HttpContext.Session["reserveDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                HttpContext.Session["reserveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                HttpContext.Session["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy/MM/dd") + " " + "今日";
            }
            else
            {
                HttpContext.Session["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy/MM/dd") + " " + Week(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()));
            }


            //修改页面Html
            StringBuilder sb = new StringBuilder();
            sb.Append(ReserveDivLoad());
            sb.Append(TodayDivLoad());
            sb.Append(TomorrowDivLoad());
            sb.Append(MealMenuLoad());
            sb.Append(MuiJqueryLoad());
            return Content(sb.ToString());
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

        public string Week(DateTime dt)
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

        public string GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            Random RandomNum_Thired = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;

            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            Color color = Color.FromArgb(int_Red, int_Green, int_Blue);
            string strColor = "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2, 6);
            return strColor;
        }

        //private string JCQLoad()
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        string rgb1 = String.Empty;
        //        string rgb2 = String.Empty;
        //        rgb1 = GetRandomColor();
        //        rgb2 = GetRandomColor();

        //        sb.Append("<style>");

        //        sb.Append(".jcq_t {");
        //        sb.Append("width: 100%;");
        //        sb.Append("height: 70px;");
        //        sb.Append("border-radius: 7px 7px 0px 0px;");
        //        sb.Append("background-image: -o-linear-gradient(180deg, " + rgb1 + " 0%," + rgb2 + " 100%); /* Opera 11.1 - 12.0 */");
        //        sb.Append("background-image: -moz-linear-gradient(180deg, " + rgb1 + " 0%," + rgb2 + " 100%); /* Firefox 3.6 - 15 */");
        //        sb.Append("background-image: -webkit-linear-gradient(180deg, " + rgb1 + " 0%," + rgb2 + " 100%); /* Safari 5.1 - 6.0 */");
        //        sb.Append("background-image: -ms-linear-gradient(180deg, " + rgb1 + " 0%," + rgb2 + " 100%);");
        //        sb.Append("background-image: linear-gradient(180deg, " + rgb1 + " 0%," + rgb2 + " 100%); /* 标准的语法（必须放在最后）*/");
        //        sb.Append("}");

        //        sb.Append(".jcq_c {");
        //        sb.Append("width: 100%;");
        //        sb.Append("height: 130px;");
        //        sb.Append("border-radius: 0px 0px 7px 7px;");
        //        sb.Append("background-image: -o-linear-gradient(360deg, rgba(219, 54, 164,1) 0%,rgba(254,220,27,0.5) 100%); /* Opera 11.1 - 12.0 */");
        //        sb.Append("background-image: -moz-linear-gradient(360deg, rgba(219, 54, 164,1) 0%,rgba(254,220,27,0.5) 100%); /* Firefox 3.6 - 15 */");
        //        sb.Append("background-image: -webkit-linear-gradient(360deg, rgba(219, 54, 164,1) 0%,rgba(254,220,27,0.5) 100%); /* Safari 5.1 - 6.0 */");
        //        sb.Append("background-image: -ms-linear-gradient(360deg, rgba(219, 54, 164,1) 0%,rgba(254,220,27,0.5) 100%);");
        //        sb.Append("background-image: linear-gradient(360deg, rgba(219, 54, 164,1) 0%,rgba(254,220,27,0.5) 100%); /* 标准的语法（必须放在最后）*/");
        //        sb.Append("}");

        //        sb.Append("</style>");

        //        return sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }

        //}

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
            //url = url.Substring(0, url.LastIndexOf(":")) + ":" + ConfigurationManager.AppSettings["port"] + url.Substring(url.LastIndexOf(":") + 3);
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

        //private string DateDivLoad()
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append("<div class=\"datePicker\">");
        //        sb.Append("<a id=\"preA\" class=\"preA\" onclick=\"preA_onclick()\"> &lt;上个工作日</a><a class=\"currentA\" id=\"showReserveDate\">" + HttpContext.Session["showReserveDate"] + "</a><a id=\"nextA\" class=\"nextA\" onclick=\"nextA_onclick()\">下个工作日&gt;</a>");
        //        sb.Append(" <a id=\"reserveDate\" style=\"display:none\">" + HttpContext.Session["reserveDate"] + "</a>");
        //        sb.Append("</div>");

        //        return sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }

        //}

        private string ReserveDivLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string userID = HttpContext.Session["userID"].ToString();
                string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                string nowTime = DateTime.Now.ToShortTimeString();
                //nowTime = "11:31";

                //获取页面权限-预定会议餐
                DataTable dtPP = (DataTable)HttpContext.Session["dsPP"];
                DataTable dtPPWC = (DataTable)HttpContext.Session["dsPPWC"];

                //获取餐厅用餐时间
                DataSet dsDInfo = new DataSet();
                LDDC.DAL.Dinnertime pDinnertime = new LDDC.DAL.Dinnertime();
                dsDInfo = pDinnertime.GetList("rGuid = '" + restaurantGuid + "'");

                System.Data.DataTable dt = dsDInfo.Tables[0];
                //System.Data.DataTable dtOIWC = (System.Data.DataTable)HttpContext.Session["dsOI"];
                //System.Data.DataRow[] drsZC = dtOIWC.Select("eType = '中餐' ");
                //System.Data.DataRow[] drsWC = dtOIWC.Select("eType = '晚餐' ");

                //获取某日订餐详情
                DataSet dsRI = new DataSet();
                LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
                dsRI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + HttpContext.Session["reserveDate"] + "'", "eID");
                //获取会议订餐详情
                //dsMeetingOI = pOrderInfo.GetList("rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and eID = '" + eid + "' and orderDate = '" + HttpContext.Session["reserveDate"]  + "'");

                //显示配餐详情用-晚餐
                DataSet dsSetMealWC = new DataSet();
                LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();
                dsSetMealWC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate = '" + Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToShortDateString() + " 00:00:00.000" + "' and eType = '晚餐'", "begDate asc");
                bool isSetMealWC = true;
                if (dsSetMealWC.Tables[0].Rows.Count == 0)
                {
                    isSetMealWC = false;
                }

                //明日控件状态
                DataRow[] drsRZC = dsRI.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsRWC = dsRI.Tables[0].Select("eType = '晚餐' ");
                DataRow[] drsMeetingWC = dsRI.Tables[0].Select("eType = '会议餐（中餐）' ");

                //某日中餐是否预定
                string btnReserveLunch = String.Empty;
                string btnReserveDinner = String.Empty;
                string btnRMeetingLunch = String.Empty;
                for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                {
                    if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                    {
                        if (HttpContext.Session["reserveDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                        {//今日
                            if (drsRZC.Length == 0)
                            {
                                btnReserveLunch = "未预订";
                            }
                            else
                            {
                                btnReserveLunch = "已就餐";
                            }
                        }
                        else if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToDouble(dsDInfo.Tables[0].Rows[i]["rWhichDay"].ToString()))).ToString("yyyy-MM-dd"))
                        {        //明日中餐是否预定
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                            {
                                //13:30 - 7:00
                                if (drsRZC.Length == 0)
                                {
                                    btnReserveLunch = "未预订";
                                }
                                else
                                {
                                    btnReserveLunch = "已预订";
                                }
                            }
                            else
                            {
                                if (drsRZC.Length == 0)
                                {
                                    btnReserveLunch = "我要预订";
                                }
                                else
                                {
                                    btnReserveLunch = "订餐成功";
                                }
                            }
                        }
                        else
                        {
                            if (drsRZC.Length == 0)
                            {
                                btnReserveLunch = "我要预订";
                            }
                            else
                            {
                                btnReserveLunch = "订餐成功";
                            }
                        }
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                    {
                        //明日晚餐是否预定
                        if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToDouble(dsDInfo.Tables[0].Rows[i]["rWhichDay"].ToString()))).ToString("yyyy-MM-dd"))
                        {
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["rEndTime"].ToString()) < Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) < Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + dsDInfo.Tables[0].Rows[i]["rBegTime"].ToString()))
                            {
                                if (drsRWC.Length == 0)
                                {
                                    btnReserveDinner = "未预订";
                                }
                                else
                                {
                                    btnReserveDinner = "已预订";
                                }
                            }
                            else
                            {
                                if (drsRWC.Length == 0)
                                {
                                    btnReserveDinner = "我要预订";
                                }
                                else
                                {
                                    btnReserveDinner = "订餐成功";
                                }
                            }
                        }
                        else
                        {
                            if (drsRWC.Length == 0)
                            {
                                btnReserveDinner = "我要预订";
                            }
                            else
                            {
                                btnReserveDinner = "订餐成功";
                            }
                        }
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "会议餐（中餐）")
                    {
                        if (HttpContext.Session["reserveDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                        {//今日
                            btnRMeetingLunch = "已就餐";
                        }
                        else
                        {
                            btnRMeetingLunch = "我要预订";
                        }
                    }

                }
                //if (drsRZC.Length == 0)
                //{
                //    btnReserveLunch = "我要预订";
                //}
                //else
                //{
                //    btnReserveLunch = "订餐成功";
                //}
                ////某日晚餐是否预定
                //string btnReserveDinner = String.Empty;
                //if (drsRWC.Length == 0)
                //{
                //    btnReserveDinner = "我要预订";
                //}
                //else
                //{
                //    btnReserveDinner = "订餐成功";
                //}

                //if (HttpContext.Session["reserveDate"] == null)
                //{
                //    ViewData["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //    ViewData["showReserveDate"] = "明日";
                //    HttpContext.Session["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //    HttpContext.Session["showReserveDate"] = "明日";
                //}
                //else
                //{
                //    if (HttpContext.Session["reserveDate"].ToString() == Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"))
                //    {
                //        ViewData["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //        ViewData["showReserveDate"] = "明日";
                //        HttpContext.Session["reserveDate"] = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd");
                //        HttpContext.Session["showReserveDate"] = "明日";
                //    }
                //    else
                //    {
                //        ViewData["reserveDate"] = HttpContext.Session["reserveDate"].ToString();
                //        //ViewData["showReserveDate"] = HttpContext.Session["reserveDate"].ToString();
                //        ViewData["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()));

                //        HttpContext.Session["showReserveDate"] = Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).ToString("yyyy年MM月dd日") + " " + Week(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()));
                //    }
                //}

                //
                sb.Append("<div class=\"details\">");
                sb.Append("<div class=\"detailsTitle\">");
                sb.Append("<div id=\"rectangle\">");
                sb.Append("<p class=\"detailsP\">");
                sb.Append("就餐预定");
                sb.Append("</p>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<div class=\"datePicker\">");
                //sb.Append("<a id=\"preA\" class=\"preA\" onclick=\"preA_onclick()\"> &lt;上个工作日</a><a class=\"currentA\" id=\"showReserveDate\">" + HttpContext.Session["showReserveDate"] + "</a><a id=\"nextA\" class=\"nextA\" onclick=\"nextA_onclick()\">下个工作日&gt;</a>");
                sb.Append("<a id=\"preA\" class=\"preA\" onclick=\"preA_onclick()\"> <img class=\"jtImg\" src=\"/Images/Index/上一步.png\" /></a><a class=\"currentA\" id=\"showReserveDate\">" + HttpContext.Session["showReserveDate"] + "</a><a id=\"nextA\" class=\"nextA\" onclick=\"nextA_onclick()\"><img class=\"jtImg\" src=\"/Images/Index/下一步.png\" /></a>");
                sb.Append(" <a id=\"reserveDate\" style=\"display:none\">" + HttpContext.Session["reserveDate"] + "</a>");
                sb.Append("</div>");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["eType"].ToString() == "中餐")
                        {
                            sb.Append("<div class=\"ldmDiv\">");
                            sb.Append("<div class=\"lunch\">");
                            sb.Append("<div class=\"imglunch\"></div>");
                            sb.Append("<p class=\"p1\">");
                            sb.Append(dt.Rows[i]["eType"].ToString());
                            sb.Append("</p>");
                            sb.Append("<p class=\"p2\">");
                            //sb.Append("餐时：" + Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm") + "~" + Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm"));
                            sb.Append("餐时：12:00~13:30");
                            sb.Append("</p>");
                            sb.Append("<p class=\"p3\" id=\"btnZCInfo\">");
                            sb.Append("想了解详细配餐情况");
                            sb.Append("</p>");
                            sb.Append("</div>");

                            if (btnReserveLunch == "我要预订")
                            {
                                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                                sb.Append("<div class=\"imgreserve\">");
                                sb.Append("<div class=\"btndes\">");
                                sb.Append("我要预订");
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                            }
                            else if (btnReserveLunch == "订餐成功")
                            {
                                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                                sb.Append("<div class=\"imgrSuccess\">");
                                sb.Append("<div class=\"btndes\">");
                                sb.Append("订餐成功");
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                            }
                            else if (btnReserveLunch == "不可预订" || btnReserveLunch == "未预订")
                            {
                                sb.Append("<div class=\"lunchBtn\">");
                                sb.Append("<div class=\"imgrUnscheduled\">");
                                sb.Append("<div class=\"btndes\">");
                                sb.Append(btnReserveLunch);
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                            }
                            else if (btnReserveLunch == "已预订")
                            {
                                sb.Append("<div class=\"lunchBtn\">");
                                sb.Append("<div class=\"imgrSuccess\">");
                                sb.Append("<div class=\"btndes\">");
                                sb.Append("已预订");
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                            }
                            else if (btnReserveLunch == "已就餐")
                            {
                                sb.Append("<div class=\"lunchBtn\">");
                                sb.Append("<div class=\"imgrSuccess\">");
                                sb.Append("<div class=\"btndes\">");
                                sb.Append("已就餐");
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                            }
                            sb.Append("</div>");
                        }
                        else if (dt.Rows[i]["eType"].ToString() == "晚餐")
                        {
                            //if (dtPPWC.Rows.Count > 0)
                            //{


                            //if(Convert.ToDateTime(HttpContext.Session["reserveDate"].ToString()).DayOfWeek.ToString() != "Friday")
                            //{
                            if (isSetMealWC == true)
                            {
                                sb.Append("<div class=\"ldmDiv\">");
                                sb.Append("<div class=\"dinner\">");
                                sb.Append("<div class=\"imgdinner\"></div>");
                                sb.Append("<p class=\"p1\">");
                                sb.Append(dt.Rows[i]["eType"].ToString());
                                sb.Append("</p>");
                                sb.Append("<p class=\"p2\">");
                                sb.Append("餐时：" + Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm") + "~" + Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm"));
                                sb.Append("</p>");
                                sb.Append("<p class=\"p3\" id=\"btnWCInfo\">");
                                sb.Append("想了解详细配餐情况");
                                sb.Append("</p>");
                                sb.Append("</div>");


                                if (btnReserveDinner == "我要预订")
                                {
                                    sb.Append("<div class=\"dinnerBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                                    sb.Append("<div class=\"imgreserve\">");
                                    sb.Append("<div class=\"btndes\">");
                                    sb.Append("我要预订");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnReserveDinner == "订餐成功")
                                {
                                    sb.Append("<div class=\"dinnerBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                                    sb.Append("<div class=\"imgrSuccess\">");
                                    sb.Append("<div class=\"btndes\">");
                                    sb.Append("订餐成功");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnReserveDinner == "不可预订" || btnReserveDinner == "未预订")
                                {
                                    sb.Append("<div class=\"dinnerBtn\">");
                                    sb.Append("<div class=\"imgrUnscheduled\">");
                                    sb.Append("<div class=\"btndes\">");
                                    sb.Append(btnReserveDinner);
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnReserveDinner == "已预订")
                                {
                                    sb.Append("<div class=\"dinnerBtn\">");
                                    sb.Append("<div class=\"imgrSuccess\">");
                                    sb.Append("<div class=\"btndes\">");
                                    sb.Append("已预订");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                sb.Append("</div>");
                            }
                        }
                        else if (dt.Rows[i]["eType"].ToString() == "会议餐（中餐）")
                        {
                            if (dtPP.Rows.Count > 0)
                            {
                                sb.Append("<div class=\"ldmDiv2\">");
                                sb.Append("<div class=\"lunch\">");
                                sb.Append("<div class=\"imgMettinglunch\"></div>");
                                sb.Append("<p class=\"p1\">");
                                sb.Append(dt.Rows[i]["eType"].ToString());
                                sb.Append("</p>");
                                sb.Append("<div class=\"meetinglunchNum\">");
                                sb.Append("<div class=\"mui-numbox\" data-numbox-min='0' data-numbox-max='100'>");
                                sb.Append("<button class=\"mui-btn mui-btn-numbox-minus\" type=\"button\">-</button>");
                                if (drsMeetingWC.Length > 0)
                                {
                                    sb.Append("<input id=\"amountBox\" class=\"mui-input-numbox\" type=\"number\" value=\"" + drsMeetingWC[0]["rAmount"].ToString() + "\" />");
                                }
                                else
                                {
                                    sb.Append("<input id=\"amountBox\" class=\"mui-input-numbox\" type=\"number\" value=\"0\" />");
                                }
                                sb.Append("<button class=\"mui-btn mui-btn-numbox-plus\" type=\"button\">+</button>");
                                sb.Append("</div>");
                                sb.Append("</div>");
                                sb.Append("</div>");

                                if (btnRMeetingLunch == "我要预订")
                                {
                                    sb.Append("<div class=\"lunchBtn\" onclick=\"BookingMeeting_click(4,'会议餐（中餐）')\">");
                                    sb.Append("<div class=\"imgreserve\">");
                                    sb.Append("<div class=\"btndes\">");
                                    sb.Append("确定人数");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else
                                {
                                    sb.Append("<div class=\"lunchBtn\">");
                                    sb.Append("<div class=\"imgrSuccess\">");
                                    sb.Append("<div class=\"btndes\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }

                                sb.Append("</div>");

                                sb.Append("<script type=\"text/javascript\">");
                                sb.Append("mui('.mui-numbox').numbox();");
                                sb.Append("</script>");
                            }
                        }





                    }

                }
                sb.Append("</div>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        private string TodayDivLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                DataSet dsOI = new DataSet();
                DataSet dsOITO = new DataSet();

                //当前时间
                string nowTime = DateTime.Now.ToShortTimeString();
                //nowTime = "18:31";
                string userID = HttpContext.Session["userID"].ToString();
                string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                string orderDate = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

                //获取今日订餐详情
                LDDC.DAL.OrderInfo pOrderInfo = new LDDC.DAL.OrderInfo();
                dsOI = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + orderDate + "'", "eID");
                //ViewData["dsOI"] = dsOI.Tables[0];
                //HttpContext.Session["dsOI"] = dsOI.Tables[0];

                //当日控件状态
                DataRow[] drsZC = dsOI.Tables[0].Select("eType = '中餐' ");
                DataRow[] drsWC = dsOI.Tables[0].Select("eType = '晚餐' ");

                //获取转带详情
                dsOITO = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and tUserID = '" + userID + "' and orderDate = '" + orderDate + "'", "eID");
                //ViewData["dsOITO"] = dsOITO.Tables[0];
                int toCount = dsOITO.Tables[0].Rows.Count;
                //HttpContext.Session["dsOITO"] = dsOITO.Tables[0];

                //获取餐厅用餐时间
                LDDC.DAL.Dinnertime pDinnertime = new LDDC.DAL.Dinnertime();
                DataSet dsDInfo = new DataSet();
                dsDInfo = pDinnertime.GetList("rGuid = '" + restaurantGuid + "'");
                DataSet dsOIZ = new DataSet();
                DataSet dsOIW = new DataSet();


                //if (dsDInfo.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                //    {
                //        if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                //        {
                //            ViewData["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                //            HttpContext.Session["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                //        }
                //        else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                //        {
                //            ViewData["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                //            HttpContext.Session["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                //        }
                //    }
                //}

                DataTable dtOIZ = new DataTable();
                DataTable dtOIW = new DataTable();
                if (dsDInfo.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                    {
                        if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                        {
                            ViewData["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            HttpContext.Session["lunchrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            //获取今日订餐详情-中餐
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                            {
                                dsOIZ = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + Convert.ToDateTime(nowTime) + "' and eid = 2", "eID");
                                dtOIZ = dsOIZ.Tables[0];
                            }
                            else
                            {
                                dtOIZ = null;
                            }
                        }
                        else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                        {
                            ViewData["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            HttpContext.Session["DinnerrEndTime"] = dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString();
                            //获取今日订餐详情-晚餐
                            if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
                            {
                                dsOIW = pOrderInfo.GetList(0, "rGuid = '" + restaurantGuid + "' and rUserID = '" + userID + "' and orderDate = '" + Convert.ToDateTime(nowTime) + "' and eid = 3", "eID");
                                dtOIW = dsOIW.Tables[0];
                            }
                            else
                            {
                                dtOIW = null;
                            }
                        }
                    }
                }


                string btnLunchState = String.Empty;
                string btnDinnerState = String.Empty;
                string orderLunchGuid = String.Empty;
                string orderDinnerGuid = String.Empty;
                for (int i = 0; i < dsDInfo.Tables[0].Rows.Count; i++)
                {
                    if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "中餐")
                    {
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
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
                                else if (drsZC[0]["osState"].ToString() == "已就餐" || drsZC[0]["osState"].ToString() == "未预定就餐")
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
                                else if (drsZC[0]["osState"].ToString() == "已就餐" || drsZC[0]["osState"].ToString() == "未预定就餐")
                                {
                                    btnLunchState = "已用餐";
                                }
                            }
                        }
                        //ViewData["btnLunchState"] = btnLunchState;
                        if (drsZC.Length > 0)
                        {
                            orderLunchGuid = drsZC[0]["guid"].ToString();
                        }
                        else
                        {
                            orderLunchGuid = String.Empty;
                        }
                    }
                    else if (dsDInfo.Tables[0].Rows[i]["eType"].ToString() == "晚餐")
                    {
                        if (Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eBegTime"].ToString()) <= Convert.ToDateTime(nowTime) && Convert.ToDateTime(nowTime) <= Convert.ToDateTime(dsDInfo.Tables[0].Rows[i]["eEndTime"].ToString()))
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
                                else if (drsWC[0]["osState"].ToString() == "已就餐" || drsWC[0]["osState"].ToString() == "未预定就餐")
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
                                else if (drsWC[0]["osState"].ToString() == "已就餐" || drsWC[0]["osState"].ToString() == "未预定就餐")
                                {
                                    btnDinnerState = "已用餐";
                                }
                            }
                        }
                        //ViewData["btnDinnerState"] = btnDinnerState;
                        if (drsWC.Length > 0)
                        {
                            orderDinnerGuid = drsWC[0]["guid"].ToString();
                        }
                        else
                        {
                            orderDinnerGuid = String.Empty;
                        }
                    }
                }



                sb.Append("<div class=\"details\">");
                sb.Append("<div class=\"detailsTitle\">");
                sb.Append("<div id=\"rectangle\">");
                sb.Append("<p class=\"detailsP\">");
                sb.Append("扫码领餐");
                sb.Append("</p>");
                sb.Append("</div>");
                sb.Append("</div>");

                System.Data.DataTable dtOI = dsOI.Tables[0];

                //未预定中餐
                if (dtOIZ != null)
                {
                    if (dtOIZ.Rows.Count == 0)
                    {
                        sb.Append("<div class=\"detailsInfoNoReserved\">");
                        sb.Append("<div id = \"divInfo1\" >");
                        sb.Append("<div class=\"divZW\">中</div>");
                        sb.Append("</div>");
                        sb.Append("<div id = \"divInfo2\" >");
                        sb.Append("<p class=\"info2p1\">预订时间：无预定</p>");
                        sb.Append("<p>");
                        sb.Append("就餐人数：1人");
                        sb.Append("</p>");
                        sb.Append("</div>");
                        sb.Append("<div id = \"divInfo3\" onclick=\"MealNotReserved_click(2,'中餐','')\">");
                        sb.Append("<div class=\"infoQRcodeimg\">");
                        //sb.Append("@*扫码就餐-未预定，但就餐*@");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }
                //未预定晚餐
                if (dtOIW != null)
                {
                    if (dtOIW.Rows.Count == 0)
                    {
                        sb.Append("<div class=\"detailsInfoNoReserved\">");
                        sb.Append("<div id = \"divInfo1\" >");
                        sb.Append("<div class=\"divWW\">晚</div>");
                        sb.Append("</div>");
                        sb.Append("<div id = \"divInfo2\" >");
                        sb.Append("<p class=\"info2p1\">预订时间：无预定</p>");
                        sb.Append("<p>");
                        sb.Append("就餐人数：1人");
                        sb.Append("</p>");
                        sb.Append("</div>");
                        sb.Append("<div id = \"divInfo3\" onclick=\"MealNotReserved_click(3,'晚餐','')\">");
                        sb.Append("<div class=\"infoQRcodeimg\">");
                        //sb.Append("@* 扫码就餐-未预定，但就餐*@");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }


                if (dtOI.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOI.Rows.Count; i++)
                    {
                        //if (Convert.ToInt32(dtOI.Rows[i]["eID"].ToString()) == 1 || Convert.ToInt32(dtOI.Rows[i]["eID"].ToString()) == 2 || Convert.ToInt32(dtOI.Rows[i]["eID"].ToString()) == 3)
                        //{
                        sb.Append("<div class=\"detailsInfo\">");
                        sb.Append("<div id=\"divInfo1\">");
                        if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                        {
                            sb.Append("<div class=\"divZW\">中</div>");
                        }
                        else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                        {
                            sb.Append("<div class=\"divWW\">晚</div>");
                        }
                        else if (dtOI.Rows[i]["eType"].ToString() == "会议餐（中餐）")
                        {
                            sb.Append("<div class=\"divZW\">会</div>");
                        }
                        sb.Append("<p>");
                        sb.Append(dtOI.Rows[i]["rUser"].ToString());
                        sb.Append("</p>");
                        if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 1)
                        {
                            sb.Append("<div class=\"divWDR\">");
                            sb.Append(dtOI.Rows[i]["tUser"].ToString() + "（带）");
                            sb.Append("</div>");
                        }
                        sb.Append("</div>");

                        if (dtOI.Rows[i]["eType"].ToString() == "会议餐（中餐）")
                        {
                            sb.Append("<div id=\"divInfo2\">");
                            sb.Append("<p class=\"info2p1\">预订时间：" + Convert.ToDateTime(dtOI.Rows[i]["rDate"].ToString()).ToString("HH:mm") + "</p>");
                            sb.Append("<p>");
                            sb.Append("就餐人数：" + Convert.ToInt32(dtOI.Rows[i]["rAmount"].ToString()) + "人");
                            sb.Append("</p>");
                            sb.Append("</div>");
                        }
                        else
                        {
                            sb.Append("<div id=\"divInfo2\">");
                            sb.Append("<p class=\"info2p1\">预订时间：" + Convert.ToDateTime(dtOI.Rows[i]["rDate"].ToString()).ToString("HH:mm") + "</p>");
                            sb.Append("<p>");
                            sb.Append("就餐时间："); if (dtOI.Rows[i]["eDate"].ToString() != String.Empty)
                            {
                                sb.Append(Convert.ToDateTime(dtOI.Rows[i]["eDate"].ToString()).ToString("HH:mm"));
                            }
                            sb.Append("</p>");
                            sb.Append("</div>");
                        }
                        if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                        {
                            if (Convert.ToDateTime(nowTime) < Convert.ToDateTime(HttpContext.Session["lunchrEndTime"].ToString()))
                            {
                                if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                                {
                                    sb.Append("<div id=\"divInfo3\" onclick=\"takeOut_click('" + dtOI.Rows[i]["guid"].ToString() + "'," + toCount + ", '" + dtOI.Rows[i]["eType"].ToString() + "','" + dtOI.Rows[i]["orderDate"].ToString() + "')\">");
                                    sb.Append("<div class=\"info3img\">");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else
                                {
                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\"infoOvertimeimg\">");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                            }
                            else
                            {
                                if (btnLunchState == "扫码就餐")
                                {
                                    sb.Append("<div id=\"divInfo3\" onclick=\"scanQRCode_click(" + dtOI.Rows[i]["eID"].ToString() + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + orderLunchGuid.ToString() + "')\">");
                                    sb.Append("<div class=\"infoQRcodeimg\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnLunchState == "已用餐")
                                {
                                    sb.Append("<div id=\"divInfo3\" onclick=\"btnJCInfo_onclick()\">");
                                    sb.Append("<div class=\"infoFinish\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnLunchState == "已转带")
                                {
                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\"infoOvertimeimg\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnLunchState == "未用餐")
                                {

                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\"infoNonMeal\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }

                            }

                        }
                        else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                        {
                            if (Convert.ToDateTime(nowTime) < Convert.ToDateTime(HttpContext.Session["DinnerrEndTime"].ToString()))
                            {
                                if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                                {
                                    sb.Append("<div id=\"divInfo3\" onclick=\"takeOut_click('" + dtOI.Rows[i]["guid"].ToString() + "'," + toCount + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + dtOI.Rows[i]["orderDate"].ToString() + "')\">");
                                    sb.Append("<div class=\"info3img\">");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else
                                {
                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\" infoOvertimeimg\">");
                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                            }
                            else
                            {
                                if (btnDinnerState == "扫码就餐")
                                {
                                    sb.Append("<div id=\"divInfo3\" onclick=\"scanQRCode_click(" + dtOI.Rows[i]["eID"].ToString() + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + orderDinnerGuid.ToString() + "')\">");
                                    sb.Append("<div class=\"infoQRcodeimg\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnDinnerState == "已用餐")
                                {
                                    sb.Append("<div id=\"divInfo3\"  onclick=\"btnNewJCInfo_onclick()\">");
                                    sb.Append("<div class=\"infoFinish\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnDinnerState == "已转带")
                                {
                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\"infoOvertimeimg\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                else if (btnDinnerState == "未用餐")
                                {
                                    sb.Append("<div id=\"divInfo3\">");
                                    sb.Append("<div class=\"infoNonMeal\">");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }

                            }

                        }

                        sb.Append("</div>");
                        //}
                    }
                }


                System.Data.DataTable dtOITO = dsOITO.Tables[0];

                if (dtOITO.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOITO.Rows.Count; i++)
                    {
                        sb.Append("<div class=\"detailsInfo\">");
                        sb.Append("<div id=\"divInfoWD\">");
                        if (dtOITO.Rows[i]["eType"].ToString() == "中餐")
                        {
                            sb.Append("<div class=\"divZW\">中</div>");
                        }
                        else if (dtOITO.Rows[i]["eType"].ToString() == "晚餐")
                        {
                            sb.Append("<div class=\"divZW\">晚</div>");
                        }

                        sb.Append("<p>");
                        sb.Append(dtOITO.Rows[i]["rUser"].ToString());
                        sb.Append("</p>");
                        sb.Append("</div>");
                        sb.Append("<div id=\"divInfo2\">");
                        sb.Append("<p class=\"info2p1\">预订时间：" + Convert.ToDateTime(dtOITO.Rows[i]["rDate"].ToString()).ToString("HH:mm") + "</p>");
                        sb.Append("<p>");
                        sb.Append("就餐时间："); if (dtOITO.Rows[i]["eDate"].ToString() != String.Empty)
                        {
                            sb.Append(Convert.ToDateTime(dtOITO.Rows[i]["eDate"].ToString()).ToString("HH:mm"));
                        }
                        sb.Append("</p>");
                        sb.Append("</div>");
                        sb.Append("<div id=\"divInfo3\">");



                        //sb.Append("<div class=\"info3imgWD\">");

                        if (dtOITO.Rows[i]["osState"].ToString() == "已转带")
                        {
                            sb.Append("<div id=\"divInfo3\" onclick=\"scanQRCodeTO_click(" + dtOITO.Rows[i]["eID"].ToString() + ",'" + dtOITO.Rows[i]["eType"].ToString() + "')\">");
                            sb.Append("<div class=\"infoQRcodeimg\">");
                            //@*扫码就餐*@
                            sb.Append("</div>");
                            sb.Append("</div>");
                        }
                        else if (dtOITO.Rows[i]["osState"].ToString() == "已就餐" && dtOITO.Rows[i]["eType"].ToString() == "中餐")
                        {
                            sb.Append("<div id=\"divInfo3\" onclick=\btnJCInfo_onclick()\">");
                            sb.Append("<div class=\"infoFinish\">");
                            //@*中餐已用餐*@
                            sb.Append("</div>");
                            sb.Append("</div>");
                        }
                        else if (dtOITO.Rows[i]["osState"].ToString() == "已就餐" && dtOITO.Rows[i]["eType"].ToString() == "晚餐")
                        {
                            sb.Append("<div id=\"divInfo3\" onclick=\"btnNewJCInfo_onclick()\">");
                            sb.Append("<div class=\"infoFinish\">");
                            //@*晚餐已用餐*@
                            sb.Append("</div>");
                            sb.Append("</div>");
                        }


                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        private string TomorrowDivLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                PublicDao pPublicDao = new PublicDao();
                string userID = HttpContext.Session["userID"].ToString();
                string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                string currentDate = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

                //获取未来订餐详情
                DataSet dsOIFuture = new DataSet();
                dsOIFuture = pPublicDao.GetFutureMeal(restaurantGuid, userID, currentDate);

                sb.Append("<div class=\"details\">");
                sb.Append("<div class=\"detailsTitle\">");
                sb.Append("<div id=\"rectangle\">");
                sb.Append("<p class=\"detailsP\">");
                sb.Append("订餐详情");
                sb.Append("</p>");
                sb.Append("</div>");
                sb.Append("</div>");

                System.Data.DataTable dtOIFuture = dsOIFuture.Tables[0];
                if (dtOIFuture.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOIFuture.Rows.Count; i++)
                    {
                        sb.Append("<div class=\"detailsInfoW\">");
                        sb.Append("<div class=\"dwY\">");
                        sb.Append(dtOIFuture.Rows[i]["osState"].ToString());
                        sb.Append("</div>");
                        sb.Append("<div class=\"dwD\">");
                        sb.Append(Convert.ToDateTime(dtOIFuture.Rows[i]["orderDate"].ToString()).ToString("M/d") + " " + dtOIFuture.Rows[i]["week"].ToString());
                        sb.Append("</div>");
                        sb.Append("<div class=\"dwZ\">");
                        sb.Append(dtOIFuture.Rows[i]["eType"].ToString() + " " + dtOIFuture.Rows[i]["name"].ToString());
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }

                sb.Append("<div class=\"divhistorical\">");
                sb.Append("<a id=\"history\" class=\"ahistorical\" onclick=\"history_onclick()\">历史订餐统计</a>");
                sb.Append("</div>");
                sb.Append("</div>");


                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        private string MealMenuLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string restaurantGuid = HttpContext.Session["restaurantGuid"].ToString();
                PublicDao pPublicDao = new PublicDao();
                LDDC.DAL.SetMeal pSetMeal = new LDDC.DAL.SetMeal();

                //显示配餐详情用
                DataSet dsSetMealPC = new DataSet();
                dsSetMealPC = pSetMeal.GetList(0, "rGuid = '" + restaurantGuid + "' and  begDate >= '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "'", "begDate asc");
                ViewData["dsSetMealPC"] = dsSetMealPC.Tables[0];

                //配餐信息
                string arrsGuid = String.Empty;
                DataSet dsDailyMeal = new DataSet();
                if (dsSetMealPC.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSetMealPC.Tables[0].Rows.Count; i++)
                    {
                        arrsGuid += "'" + dsSetMealPC.Tables[0].Rows[i]["guid"].ToString() + "',";
                    }
                    arrsGuid = arrsGuid.Substring(0, arrsGuid.Length - 1);
                }
                LDDC.DAL.DailyMeal pDailyMeal = new LDDC.DAL.DailyMeal();
                if (arrsGuid != String.Empty)
                {
                    dsDailyMeal = pPublicDao.GetDailyMealList("a.sGuid in(" + arrsGuid + ")", "a.sGuid,b.vtID");
                    ViewData["dsDailyMeal"] = dsDailyMeal.Tables[0];
                }





                //绑定中餐菜单html
                sb.Append("<div id=\"popoverDiv\" class=\"mui-popover\" style=\"width: 90%; max-height: 520px; min-height: 520px; \">");
                sb.Append("<div class=\"mui-scroll-wrapper\">");
                sb.Append("<div class=\"mui-scroll\">");
                sb.Append("<div>");
                sb.Append("<div class=\"mui-card\">");
                sb.Append("<ul class=\"mui-table-view\">");

                System.Data.DataTable dtSetMeal = (System.Data.DataTable)ViewData["dsSetMealPC"];
                System.Data.DataTable dtDailyMeal = (System.Data.DataTable)ViewData["dsDailyMeal"];
                System.Data.DataRow[] drsDM = null;
                System.Data.DataRow[] drsSetMeal = null;
                drsSetMeal = dtSetMeal.Select("eGuid = 2 ");
                bool isNow = false;

                if (drsSetMeal.Length > 0)
                {
                    for (int i = 0; i < drsSetMeal.Length; i++)
                    {
                        drsDM = dtDailyMeal.Select("sGuid = '" + @drsSetMeal[i]["guid"].ToString() + "' ");
                        var weekdays = new string[] { "日", "一", "二", "三", "四", "五", "六" };
                        var begtime = Convert.ToDateTime(drsSetMeal[i]["begDate"].ToString());
                        string begW = weekdays[(int)begtime.DayOfWeek];
                        var endtime = Convert.ToDateTime(drsSetMeal[i]["endDate"].ToString());
                        string endW = weekdays[(int)endtime.DayOfWeek];
                        DateTime nowdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        if (i == 0)
                        {
                            sb.Append("<li class=\"mui-table-view-cell mui-collapse mui-active\">");
                            sb.Append("<a class=\"mui-navigate-right\" href=\"#\">");
                            sb.Append("<b>" + Convert.ToDateTime(drsSetMeal[i]["begDate"].ToString()).ToString("M月d日") + "</b> 周" + (begW) + "- <b>" + Convert.ToDateTime(drsSetMeal[i]["endDate"].ToString()).ToString("M月d日") + "</b> 周" + (endW) + " <b>" + drsSetMeal[i]["eType"].ToString() + "</b><span class=\"mui-badge mui-badge-primary\">" + drsDM.Length + "</span>");
                            sb.Append("</a>");
                            sb.Append("<div class=\"mui-collapse-content\">");

                            for (int j = 0; j < drsDM.Length; j++)
                            {
                                sb.Append("<div class=\"mui-input-row\">");
                                sb.Append("<label style=\"width: 60px; color: #007aff;\">" + drsDM[j]["vtName"].ToString() + "</label>");
                                sb.Append("<label style=\"width:160px;\">" + drsDM[j]["name"].ToString() + "</label>");
                                sb.Append("<img style=\"width: 48px; height: 35px; float: right; margin-top: 2px; margin-right: 2px\" src=\"" + drsDM[j]["imgPath"].ToString() + "\">");

                                sb.Append("</div>");

                            }
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li class=\"mui-table-view-cell mui-collapse\">");
                            sb.Append("<a class=\"mui-navigate-right\" href=\"#\">");
                            sb.Append("<b>" + Convert.ToDateTime(drsSetMeal[i]["begDate"].ToString()).ToString("M月d日") + "</b> 周" + (begW) + "- <b>" + Convert.ToDateTime(drsSetMeal[i]["endDate"].ToString()).ToString("M月d日") + "</b> 周" + (endW) + "<b>" + drsSetMeal[i]["eType"].ToString() + "</b><span class=\"mui-badge mui-badge-primary\">" + drsDM.Length + "</span>");
                            sb.Append("</a>");
                            sb.Append("<div class=\"mui-collapse-content\">");

                            for (int j = 0; j < drsDM.Length; j++)
                            {
                                sb.Append("<div class=\"mui-input-row\">");
                                sb.Append("<label style=\"width: 60px; color: #007aff;\">" + drsDM[j]["vtName"].ToString() + "</label>");
                                sb.Append("<label style=\"width:160px;\">" + drsDM[j]["name"].ToString() + "</label>");
                                sb.Append("<img style=\"width: 48px; height: 35px; float: right; margin-top: 2px; margin-right: 2px\" src=\"" + drsDM[j]["imgPath"].ToString() + "\">");

                                sb.Append("</div>");

                            }
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }

                    }
                }
                else
                {
                    sb.Append("<div style=\"margin: 0px 0px 10px 0px;padding-top:10px;\">");
                    sb.Append("<span style=\"padding: 0px 0px 0px 10px; color: #8a8a8a; font-weight: bold;\">今日还未配餐</span>");
                    sb.Append("</div>");
                }

                sb.Append("</ul>");

                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");


                //绑定晚餐菜单html
                sb.Append("<div id=\"popoverWCDiv\" class=\"mui-popover\" style=\"width: 90%; max-height: 520px; min-height: 520px; \">");
                sb.Append("<div class=\"mui-scroll-wrapper\">");
                sb.Append("<div class=\"mui-scroll\">");
                sb.Append("<div>");
                sb.Append("<div class=\"mui-card\">");
                sb.Append("<ul class=\"mui-table-view\">");

                dtSetMeal = (System.Data.DataTable)ViewData["dsSetMealPC"];
                dtDailyMeal = (System.Data.DataTable)ViewData["dsDailyMeal"];
                drsDM = null;
                drsSetMeal = null;
                drsSetMeal = dtSetMeal.Select("eGuid = 3 ");
                isNow = false;

                if (drsSetMeal.Length > 0)
                {
                    for (int i = 0; i < drsSetMeal.Length; i++)
                    {
                        drsDM = dtDailyMeal.Select("sGuid = '" + drsSetMeal[i]["guid"].ToString() + "' ");
                        var weekdays = new string[] { "日", "一", "二", "三", "四", "五", "六" };
                        var begtime = Convert.ToDateTime(drsSetMeal[i]["begDate"].ToString());
                        string begW = weekdays[(int)begtime.DayOfWeek];
                        var endtime = Convert.ToDateTime(drsSetMeal[i]["endDate"].ToString());
                        string endW = weekdays[(int)endtime.DayOfWeek];
                        DateTime nowdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());



                        sb.Append("<li class=\"mui-table-view-cell mui-collapse mui-active\">");
                        sb.Append("<a class=\"mui-navigate-right\" href=\"#\">");
                        sb.Append("<b>" + Convert.ToDateTime(drsSetMeal[i]["begDate"].ToString()).ToString("M月d日") + "</b> 周" + (begW) + " - <b>" + Convert.ToDateTime(drsSetMeal[i]["endDate"].ToString()).ToString("M月d日") + "</b> 周" + (endW) + " <b>" + drsSetMeal[i]["eType"].ToString() + "</b><span class=\"mui-badge mui-badge-primary\">" + drsDM.Length + "</span>");
                        sb.Append("</a>");
                        sb.Append("<div class=\"mui-collapse-content\">");


                        for (int j = 0; j < drsDM.Length; j++)
                        {
                            sb.Append("<div class=\"mui-input-row\">");
                            sb.Append("<label style=\"width: 60px; color: #007aff;\">" + drsDM[j]["vtName"].ToString() + "</label>");
                            sb.Append("<label style=\"width:160px;\">" + drsDM[j]["name"].ToString() + "</label>");
                            sb.Append("<img style=\"width: 48px; height: 35px; float: right; margin-top: 2px; margin-right: 2px\" src=\"" + drsDM[j]["imgPath"].ToString() + "\">");
                            sb.Append("</div>");

                        }




                        sb.Append("</div>");
                        sb.Append("</li>");

                    }
                }
                else
                {
                    sb.Append("<div style=\"margin: 0px 0px 10px 0px;padding-top:10px;\">");
                    sb.Append("<span style=\"padding: 0px 0px 0px 10px; color: #8a8a8a; font-weight: bold;\">今日还未配餐</span>");
                    sb.Append("</div>");
                }


                sb.Append("</ul>");

                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");


                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private string MuiJqueryLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("mui.init({");
                sb.Append("});");
                sb.Append("mui('.mui-scroll-wrapper').scroll();");

                sb.Append("document.getElementById(\"btnZCInfo\").addEventListener(\"tap\", function() {");
                sb.Append("mui(\"#popoverDiv\").popover('toggle', document.getElementById(\"div\"));");
                sb.Append("});");

                sb.Append("document.getElementById(\"btnWCInfo\").addEventListener(\"tap\", function() {");
                sb.Append("mui(\"#popoverWCDiv\").popover('toggle', document.getElementById(\"div\"));");
                sb.Append("});");

                sb.Append("document.getElementById(\"btnJCInfo\").addEventListener(\"tap\", function() {");
                sb.Append("alert('ok');");
                sb.Append("mui(\"#popoverJCDiv\").popover('toggle', document.getElementById(\"div1\"));");
                sb.Append("});");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        private string DivLoad()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                ////sb.Append("<div class=\"datePicker\">");
                ////sb.Append("<a id=\"preA\" class=\"preA\"> &lt;上个工作日</a><a class=\"currentA\" id=\"showReserveDate\">" + HttpContext.Session["showReserveDate"] + "</a><a id=\"nextA\" class=\"nextA\">下个工作日&gt;</a>");
                ////sb.Append(" <a id=\"reserveDate\" style=\"display:none\">" + HttpContext.Session["reserveDate"] + "</a>");
                ////sb.Append("</div>");

                //System.Data.DataTable dt = (System.Data.DataTable)HttpContext.Session["dsDInfo"];
                //System.Data.DataTable dtOIWC = (System.Data.DataTable)HttpContext.Session["dsOI"];
                ////System.Data.DataTable dtOITO = (System.Data.DataTable)ViewData["dsoito"];//转带

                //System.Data.DataRow[] drsZC = dtOIWC.Select("eType = '中餐' ");
                //System.Data.DataRow[] drsWC = dtOIWC.Select("eType = '晚餐' ");

                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        if (dt.Rows[i]["eType"].ToString() == "中餐")
                //        {
                //            sb.Append("<div class=\"ldmDiv\">");
                //            sb.Append("<div class=\"lunch\">");
                //            sb.Append("<div class=\"imglunch\"></div>");
                //            sb.Append("<p class=\"p1\">");
                //            sb.Append(dt.Rows[i]["eType"].ToString());
                //            sb.Append("</p>");
                //            sb.Append("<p class=\"p2\">");
                //            sb.Append("用餐时段：" + Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm") + "~" + Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm"));
                //            sb.Append("</p>");
                //            sb.Append("<p class=\"p3\" id=\"btnZCInfo\" onclick=\"btnZCInfo_onclick()\">");
                //            sb.Append("想了解详细配餐情况");
                //            sb.Append("</p>");
                //            sb.Append("</div>");

                //            if (HttpContext.Session["btnReserveLunch"].ToString() == "我要预订")
                //            {
                //                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                //                sb.Append("<div class=\"imgreserve\">");
                //                sb.Append("<div class=\"btndes\">");
                //                sb.Append("我要预订");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //            }
                //            else if (HttpContext.Session["btnReserveLunch"].ToString() == "订餐成功")
                //            {
                //                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                //                sb.Append("<div class=\"imgrSuccess\">");
                //                sb.Append("<div class=\"btndes\">");
                //                sb.Append("订餐成功");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //            }
                //            sb.Append("</div>");
                //        }
                //        else if (dt.Rows[i]["eType"].ToString() == "晚餐")
                //        {
                //            sb.Append("<div class=\"ldmDiv\">");
                //            sb.Append("<div class=\"dinner\">");
                //            sb.Append("<div class=\"imgdinner\"></div>");
                //            sb.Append("<p class=\"p1\">");
                //            sb.Append(dt.Rows[i]["eType"].ToString());
                //            sb.Append("</p>");
                //            sb.Append("<p class=\"p2\">");
                //            sb.Append("用餐时段：" + Convert.ToDateTime(dt.Rows[i]["eBegTime"].ToString()).ToString("HH:mm") + "~" + Convert.ToDateTime(dt.Rows[i]["eEndTime"].ToString()).ToString("HH:mm"));
                //            sb.Append("</p>");
                //            sb.Append("<p class=\"p3\" id=\"btnWCInfo\" onclick=\"btnWCInfo_onclick()\">");
                //            sb.Append("想了解详细配餐情况");
                //            sb.Append("</p>");
                //            sb.Append("</div>");


                //            if (HttpContext.Session["btnReserveDinner"].ToString() == "我要预订")
                //            {
                //                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                //                sb.Append("<div class=\"imgreserve\">");
                //                sb.Append("<div class=\"btndes\">");
                //                sb.Append("我要预订");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //            }
                //            else if (HttpContext.Session["btnReserveDinner"].ToString() == "订餐成功")
                //            {
                //                sb.Append("<div class=\"lunchBtn\" onclick=\"Collect_click(" + dt.Rows[i]["eID"].ToString() + ",'" + dt.Rows[i]["eType"].ToString() + "')\">");
                //                sb.Append("<div class=\"imgrSuccess\">");
                //                sb.Append("<div class=\"btndes\">");
                //                sb.Append("订餐成功");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //                sb.Append("</div>");
                //            }

                //            sb.Append("</div>");
                //        }

                //    }
                //}


                //sb.Append("@*详情区域*@");
                //sb.Append("<div class=\"details\">");
                //sb.Append("<div class=\"detailsTitle\">");
                //sb.Append("<div id=\"rectangle\">");
                //sb.Append("<p class=\"detailsP\">");
                //sb.Append("今日订餐详情");
                //sb.Append("</p>");
                //sb.Append("</div>");
                //sb.Append("</div>");

                //System.Data.DataTable dtOI = (System.Data.DataTable)ViewData["dsOI"];


                //if (dtOI.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtOI.Rows.Count; i++)
                //    {
                //        sb.Append("<div class=\"detailsInfo\">");
                //        sb.Append("<div id=\"divInfo1\">");
                //        if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                //        {
                //            sb.Append("<div class=\"divZW\">" + dtOI.Rows[i]["eType"].ToString() + "券</div>");
                //        }
                //        else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                //        {
                //            sb.Append("<div class=\"divWW\">" + dtOI.Rows[i]["eType"].ToString() + "券</div>");
                //        }
                //        sb.Append("<p>");
                //        sb.Append(dtOI.Rows[i]["rUser"].ToString());
                //        sb.Append("</p>");
                //        if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 1)
                //        {
                //            sb.Append("<div class=\"divWDR\">");
                //            sb.Append(dtOI.Rows[i]["tUser"].ToString() + "（带）");
                //            sb.Append("</div>");
                //        }
                //        sb.Append("</div>");
                //        sb.Append("<div id=\"divInfo2\">");
                //        sb.Append("<p class=\"info2p1\">预订时间：" + Convert.ToDateTime(dtOI.Rows[i]["rDate"].ToString()).ToString("HH:mm") + "</p>");
                //        sb.Append("<p>");
                //        sb.Append("就餐时间："); if (dtOI.Rows[i]["eDate"].ToString() != String.Empty)
                //        {
                //            sb.Append(Convert.ToDateTime(dtOI.Rows[i]["eDate"].ToString()).ToString("HH:mm"));
                //        }
                //        sb.Append("</p>");
                //        sb.Append("</div>");

                //        if (dtOI.Rows[i]["eType"].ToString() == "中餐")
                //        {
                //            if (Convert.ToDateTime(ViewData["nowTime"].ToString()) < Convert.ToDateTime(ViewData["lunchrEndTime"].ToString()))
                //            {
                //                if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                //                {
                //                    sb.Append("<div id=\"divInfo3\" onclick=\"takeOut_click('" + dtOI.Rows[i]["guid"].ToString() + "'," + Convert.ToInt32(ViewData["toCount"]) + ", '" + dtOI.Rows[i]["eType"].ToString() + "','" + dtOI.Rows[i]["orderDate"].ToString() + "')\">");
                //                    sb.Append("<div class=\"info3img\">");
                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoOvertimeimg\">");
                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //            }
                //            else
                //            {
                //                if (ViewData["btnLunchState"] == "扫码就餐")
                //                {
                //                    sb.Append("<div id=\"divInfo3\" onclick=\"scanQRCode_click(" + dtOI.Rows[i]["eID"].ToString() + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + ViewData["orderLunchGuid"].ToString() + "')\">");
                //                    sb.Append("<div class=\"infoQRcodeimg\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnLunchState"] == "已用餐")
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoFinish\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnLunchState"] == "已转带")
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoOvertimeimg\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnLunchState"] == "未用餐")
                //                {

                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoNonMeal\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }

                //            }

                //        }
                //        else if (dtOI.Rows[i]["eType"].ToString() == "晚餐")
                //        {
                //            if (Convert.ToDateTime(ViewData["nowTime"].ToString()) < Convert.ToDateTime(ViewData["DinnerrEndTime"].ToString()))
                //            {
                //                if (Convert.ToInt32(dtOI.Rows[i]["isTakeout"]) == 0)
                //                {
                //                    sb.Append("<div id=\"divInfo3\" onclick=\"takeOut_click('" + dtOI.Rows[i]["guid"].ToString() + "'," + Convert.ToInt32(ViewData["toCount"]) + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + dtOI.Rows[i]["orderDate"].ToString() + "')\">");
                //                    sb.Append("<div class=\"info3img\">");
                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\" infoOvertimeimg\">");
                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //            }
                //            else
                //            {
                //                if (ViewData["btnDinnerState"] == "扫码就餐")
                //                {
                //                    sb.Append("<div id=\"divInfo3\" onclick=\"scanQRCode_click(" + dtOI.Rows[i]["eID"].ToString() + ",'" + dtOI.Rows[i]["eType"].ToString() + "','" + ViewData["orderDinnerGuid"].ToString() + "')\">");
                //                    sb.Append("<div class=\"infoQRcodeimg\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnDinnerState"] == "已用餐")
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoFinish\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnDinnerState"] == "已转带")
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoOvertimeimg\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }
                //                else if (ViewData["btnDinnerState"] == "未用餐")
                //                {
                //                    sb.Append("<div id=\"divInfo3\">");
                //                    sb.Append("<div class=\"infoNonMeal\">");

                //                    sb.Append("</div>");
                //                    sb.Append("</div>");
                //                }

                //            }

                //        }

                //        sb.Append("</div>");
                //    }
                //}



                //System.Data.DataTable dsOITO = (System.Data.DataTable)ViewData["dsOITO"];

                //if (dsOITO.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dsOITO.Rows.Count; i++)
                //    {
                //        sb.Append("<div class=\"detailsInfo\">");
                //        sb.Append("<div id=\"divInfoWD\">");
                //        sb.Append("<div class=\"divZW\">" + dsOITO.Rows[i]["eType"].ToString() + "券</div>");
                //        sb.Append("<p>");
                //        sb.Append(dsOITO.Rows[i]["rUser"].ToString());
                //        sb.Append("</p>");
                //        sb.Append("</div>");
                //        sb.Append("<div id=\"divInfo2\">");
                //        sb.Append("<p class=\"info2p1\">预订时间：" + Convert.ToDateTime(dsOITO.Rows[i]["rDate"].ToString()).ToString("HH:mm") + "</p>");
                //        sb.Append("<p>");
                //        sb.Append("就餐时间："); if (dsOITO.Rows[i]["eDate"].ToString() != String.Empty)
                //        {
                //            sb.Append(Convert.ToDateTime(dsOITO.Rows[i]["eDate"].ToString()).ToString("HH:mm"));
                //        }
                //        sb.Append("</p>");
                //        sb.Append("</div>");
                //        sb.Append("<div id=\"divInfo3\">");
                //        sb.Append("<div class=\"info3imgWD\">");
                //        sb.Append("</div>");
                //        sb.Append("</div>");
                //        sb.Append("</div>");
                //    }
                //}
                //sb.Append("</div>");



                //sb.Append("<div class=\"details\">");
                //sb.Append("<div class=\"detailsTitle\">");
                //sb.Append("<div id=\"rectangle\">");
                //sb.Append("<p class=\"detailsP\">");
                //sb.Append("未来订餐详情");
                //sb.Append("</p>");
                //sb.Append("</div>");
                //sb.Append("</div>");

                //System.Data.DataTable dtOIFuture = (System.Data.DataTable)ViewData["dsOIFuture"];
                //if (dtOIFuture.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtOIFuture.Rows.Count; i++)
                //    {
                //        sb.Append("<div class=\"detailsInfoW\">");
                //        sb.Append("<div class=\"dwY\">");
                //        sb.Append("已预订");
                //        sb.Append("</div>");
                //        sb.Append("<div class=\"dwD\">");
                //        sb.Append(Convert.ToDateTime(dtOIFuture.Rows[i]["orderDate"].ToString()).ToString("M月d日"));
                //        sb.Append("</div>");
                //        sb.Append("<div class=\"dwZ\">");
                //        sb.Append(dtOIFuture.Rows[i]["eType"].ToString());
                //        sb.Append("</div>");
                //        sb.Append("</div>");
                //    }
                //}

                //sb.Append("<div class=\"divhistorical\">");
                //sb.Append("<a id=\"history\" class=\"ahistorical\">历史订餐统计</a>");
                //sb.Append("</div>");
                //sb.Append("</div>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }


    }
}