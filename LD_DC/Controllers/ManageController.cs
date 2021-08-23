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
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LD_DC.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Supplier/
        public ActionResult Index()
        {
            //获取登录用户
            PublicDao pPublicDao = new PublicDao();
            Statistics pStatistics = new Statistics();
            string accessToken = pPublicDao.GetCacheAccessToken();

            string company = "集团总部";
            string restaurantGuid = String.Empty;
            DataSet dsRInfo = new DataSet();
            DataSet dsMenuInfo = new DataSet();
            DataSet dszd_VegetableType = new DataSet();

            DateTime nowday = DateTime.Now;
            //if (Request.QueryString["nowday"] != null && Request.QueryString["nowday"] != String.Empty)
            //{
            //    nowday = Convert.ToDateTime(Request.QueryString["nowday"]);
            //}

            //获取餐厅信息
            RestaurantInfo pRestaurantInfo = new RestaurantInfo();
            if (HttpContext.Session["restaurantGuid"] == null)
            {
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
                    HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                    ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                    ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                }
            }

            //当月预订、实际
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


            return View();
        }

        public ActionResult Statistics()
        {
            //获取登录用户
            PublicDao pPublicDao = new PublicDao();
            string accessToken = pPublicDao.GetCacheAccessToken();

            //string company = "集团总部";
            string restaurantGuid = Request.QueryString["rGuid"].ToString();
            DataSet dsRInfo = new DataSet();

            //获取餐厅信息
            RestaurantInfo pRestaurantInfo = new RestaurantInfo();
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
                    HttpContext.Session["restaurantName"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rname"] = dsRInfo.Tables[0].Rows[0]["name"].ToString();
                    ViewData["rcity"] = dsRInfo.Tables[0].Rows[0]["city"].ToString();
                    ViewData["rcounty"] = dsRInfo.Tables[0].Rows[0]["county"].ToString();
                    ViewData["raddress"] = dsRInfo.Tables[0].Rows[0]["address"].ToString();
                }
            }


            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRJson()
        {
            PublicDao pPublicDao = new PublicDao();

            string rJson = String.Empty;
            DataSet dsVT = new DataSet();

            //获取菜品类型
            RestaurantInfo pRestaurantInfo = new RestaurantInfo();
            dsVT = pRestaurantInfo.GetList(0, "isValid = 1", "sortNo");
            rJson = pPublicDao.GetVTJsonByDataset(dsVT, "guid", "name", "city", "county", "address");
            //vtJson = vtJson.Substring(vtJson.IndexOf("["), vtJson.LastIndexOf("]") - vtJson.IndexOf("[")+1);

            return Content(rJson);
        }

        [HttpPost]
        public ActionResult ReloadPage(string rid)
        {
            HttpContext.Session["restaurantGuid"] = rid;
            string restaurantGuid = rid;
            StringBuilder sb = new StringBuilder();

            return null;
            //return Content(sb.ToString());
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
                //dsThisMonth = pStatistics.GetThisMonth(month, rGuid);

                //当天午餐预订、实际
                DataSet dsTodayLunch = new DataSet();
                dsTodayLunch = pStatistics.GetTodayLunch(nowday.ToShortDateString(), rGuid);

                //当天晚餐预订、实际
                DataSet dsTodayDinner = new DataSet();
                dsTodayDinner = pStatistics.GetTodayDinner(nowday.ToShortDateString(), rGuid);

                //当天会议中餐预订、实际
                DataSet dsTodayMeetingLunch = new DataSet();
                dsTodayMeetingLunch = pStatistics.GetDateMeetingLunch(nowday.ToShortDateString(), rGuid);

                //当天未用餐名单
                DataSet dsNotMeal = new DataSet();
                dsNotMeal = pStatistics.GetOrderInfo(nowday.ToShortDateString(), rGuid, "2,3", 2);

                //top10中餐、晚餐
                DataSet dsTopInfo = new DataSet();
                dsTopInfo = pStatistics.GetTopInfo(FirstDayOfMonth(nowday).ToShortDateString(), DateTime.Now.ToShortDateString(), rGuid, "2,3", 2);

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


                html.Append("<div class=\"detailsTitle\">");
                html.Append("<div id=\"rectangle\">");
                html.Append("<p class=\"detailsP\">");
                html.Append("<span>" + nowday.ToString("dd") + "日</span>未用餐（" + dsNotMeal.Tables[0].Rows.Count + "人）");
                html.Append("</p>");
                html.Append("</div>");
                html.Append("</div>");

                if (dsNotMeal.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsNotMeal.Tables[0].Rows.Count; i++)
                    {
                        html.Append("<div class=\"detailsInfo\">");
                        html.Append("<div id=\"divInfo1\">");
                        html.Append("<div class=\"divZW\">" + dsNotMeal.Tables[0].Rows[i]["eType"].ToString() + "</div>");
                        html.Append("<div class=\"divYJ\">"+ dsNotMeal.Tables[0].Rows[i]["rUser"].ToString() + "</div>");
                        html.Append("<div class=\"divSJ\">" + dsNotMeal.Tables[0].Rows[i]["osState"].ToString() + " 未用餐</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                    }
                }

                    html.Append("<div class=\"detailsTitle\">");
                html.Append("<div id=\"rectangle\">");
                html.Append("<p class=\"detailsP\">");
                html.Append("<span>本月</span>未用餐排名");
                html.Append("</p>");
                html.Append("</div>");
                html.Append("</div>");
                if (dsTopInfo.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsTopInfo.Tables[0].Rows.Count; i++)
                    {
                        html.Append("<div class=\"detailsInfo\">");
                        html.Append("<div id=\"divInfo1\">");
                        html.Append("<div class=\"divZW\">" + dsTopInfo.Tables[0].Rows[i]["eType"].ToString() + "</div>");
                        html.Append("<div class=\"divYJ\">" + dsTopInfo.Tables[0].Rows[i]["rUser"].ToString() + "</div>");
                        html.Append("<div class=\"divSJ\">" + dsTopInfo.Tables[0].Rows[i]["次数"].ToString() + " 未用餐</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                    }
                }
                








                return Content(html.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        private DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        private DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
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


        public ActionResult Daochu()
        {


            return View();
        }

        //导出Excel
        public FileContentResult Excel(string time1, string time2, string user, string st)
        {
            string mag = "";
            DCExcelData dc = new DCExcelData();
            string where = "";
            if (time1 != "")
            {
                time1 = Convert.ToDateTime(time1).ToString("yyyy-MM-dd HH:ss:mm");
                where += "and a.orderDate >='" + time1 + "' ";
            }
            if (time2 != "")
            {
                time2 = Convert.ToDateTime(time2).ToString("yyyy-MM-dd 23:59:59");
                where += " and a.orderDate <= '" + time2 + "' ";
            }
            if (user != "")
            {
                where += "and (a.rUserID='" + user + "' or a.rUser='" + user + "' )";
            }

            if (st != "--全部--" && st != "")
            {
                where += "and a.rGuid in (" + st + ")";
            }
            DataTable dt = dc.Exists(where, mag);


            string path1 = Request.PhysicalApplicationPath;
            string newpath = Path.Combine(path1, "Excel\\订餐报表.xlsx");//相对路径

            //    string newpath = Server.MapPath("/") + "Excel\\订餐报表.xlsx";
            //string newpath = Path.Combine(path1, "\\印章管理台账.xls");//相对路径
            FileStream stream = new FileStream(newpath, FileMode.Open, FileAccess.Read);//文件流打开
            IWorkbook workbook = new XSSFWorkbook(stream);//NPOI打开excel

            ISheet sheet = workbook.GetSheetAt(0);//获取sheet1 下标为0


            int r = 2;
            int s = 0;

            for (int i = 2; i < dt.Rows.Count + 2; i++)
            {
                s++;
                r = r + 1;
                IRow row = sheet.CreateRow(i);

                #region 给格子赋值

                row.CreateCell(0).SetCellValue(s);//把获取到的样式放上去
                row.CreateCell(1).SetCellValue(dt.Rows[i - 2]["Mouth"].ToString());
                row.CreateCell(2).SetCellValue(dt.Rows[i - 2]["Types"].ToString());
                row.CreateCell(3).SetCellValue(dt.Rows[i - 2]["UserID"].ToString());
                row.CreateCell(4).SetCellValue(dt.Rows[i - 2]["UserName"].ToString());
                row.CreateCell(5).SetCellValue(dt.Rows[i - 2]["Readay"].ToString());
                row.CreateCell(6).SetCellValue(dt.Rows[i - 2]["NoReaday"].ToString());
                row.CreateCell(7).SetCellValue(dt.Rows[i - 2]["EatNot"].ToString());

            }
            #endregion
            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.ToArray();
                ms.Close();
            }
            return File(buffer, "application/ms-excel", DateTime.Now.ToLongDateString().ToString() + "员工订餐台账.xlsx");

        }



        public FileContentResult ExcelCT(string data, string start, string end)
        {
            string where = "";
            DCExcelData dc = new DCExcelData();
            //string where = "";
            if (start != "")
            {
                start = Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:ss:mm");
                where += "and a.orderDate >='" + start + "' ";
            }
            if (end != "")
            {
                end = Convert.ToDateTime(end).ToString("yyyy-MM-dd 23:59:59");
                where += " and a.orderDate <= '" + end + "' ";
            }
            if (data != "")
            {
                where += "and a.rGuid in (" + data + ")";
            }
            //if (user != "")
            //{
            //    where += "and (a.rUserID='" + user + "' or a.rUser='" + user + "' )";
            //}

            //if (st != "--全部--")
            //{
            //    mag = " and a.eType='" + st + "'";
            //}
            DataTable dt = dc.ExcelCT(where);


            string path1 = Request.PhysicalApplicationPath;
            string newpath = Path.Combine(path1, "Excel\\餐厅就餐明细.xlsx");//相对路径

            //    string newpath = Server.MapPath("/") + "Excel\\订餐报表.xlsx";
            //string newpath = Path.Combine(path1, "\\印章管理台账.xls");//相对路径
            FileStream stream = new FileStream(newpath, FileMode.Open, FileAccess.Read);//文件流打开
            IWorkbook workbook = new XSSFWorkbook(stream);//NPOI打开excel

            ISheet sheet = workbook.GetSheetAt(0);//获取sheet1 下标为0


            int r = 2;
            int s = 0;

            for (int i = 2; i < dt.Rows.Count + 2; i++)
            {
                s++;
                r = r + 1;
                IRow row = sheet.CreateRow(i);

                #region 给格子赋值

                row.CreateCell(0).SetCellValue(s);//把获取到的样式放上去
                row.CreateCell(1).SetCellValue(dt.Rows[i - 2]["rMonth"].ToString());
                row.CreateCell(2).SetCellValue(dt.Rows[i - 2]["name"].ToString());
                row.CreateCell(3).SetCellValue(dt.Rows[i - 2]["eType"].ToString());
                row.CreateCell(4).SetCellValue(dt.Rows[i - 2]["yd"].ToString());
                row.CreateCell(5).SetCellValue(dt.Rows[i - 2]["sj"].ToString());



            }
            #endregion
            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.ToArray();
                ms.Close();
            }
            return File(buffer, "application/ms-excel", DateTime.Now.ToLongDateString().ToString() + "餐厅就餐明细.xlsx");

        }



    }
}