using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI;
using System.IO;

namespace MvcAppTest.Controllers
{
    public class RatingController : BaseController
    {
        //
        // GET: /Rating/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(int id, float value)
        {
            float x = id + value;
            return Json(x);
        }
        public ActionResult RateChart()
        {
            Dictionary<string, float> d = new Dictionary<string, float>();
            d.Keys.ToArray();
            var them1 = "<Chart BackColor=\"#FADA5E\" BackGradientStyle=\"TopBottom\" BorderColor=\"#B8860B\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"EarthTones\">\r\n <ChartAreas>\r\n <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n <AxisY Enabled=\"False\">\r\n <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n </AxisY>\r\n <AxisX LineColor=\"64, 64, 64, 64\">\r\n<MajorGrid Enabled=\"false\" />\r\n<LabelStyle Font=\"Trebuchet MS, 6.25pt, style=Bold\" />\r\n </AxisX>\r\n </ChartArea>\r\n </ChartAreas>\r\n <Legends>\r\n <Legend _Template_=\"All\" BackColor=\"Transparent\" Docking=\"Bottom\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" LegendStyle=\"Row\">\r\n </Legend>\r\n </Legends>\r\n <BorderSkin SkinStyle=\"Emboss\" />\r\n</Chart>";
            var key = new System.Web.Helpers.Chart(width: 100, height: 120, theme: them1)
                .AddSeries(
                    chartType: "bar",
                    xValue: new[] { "1 star: ", "2 star: ", "3 star: ", "4 star: ", "5 star: " },
                    yValues: new[] { "20", "20", "40", "10", "10" }
                    ).Write();
            return null;
        }
        public ActionResult RateChart1()
        {
            Dictionary<string, int> chartList = new Dictionary<string, int>();
            chartList.Add("1 star:",23);
            chartList.Add("2 star:",43);
            chartList.Add("3 star:",34);
            chartList.Add("4 star:",52);
            chartList.Add("5 star:",48);

            int tong = 0;
            foreach (int i in chartList.Values)
                tong = tong + i;
            
            ViewData["Chart"] = chartList;

            System.Web.UI.DataVisualization.Charting.Chart Chart2 = new System.Web.UI.DataVisualization.Charting.Chart();
            Chart2.Width = 150;
            Chart2.Height = 120;
            Chart2.RenderType = RenderType.ImageTag;
            Chart2.BackColor = System.Drawing.Color.AliceBlue;
            //Chart2.Palette = ChartColorPalette.EarthTones;
            Chart2.Palette = ChartColorPalette.BrightPastel;
            Chart2.BackGradientStyle = GradientStyle.DiagonalRight;
            Chart2.BackSecondaryColor = System.Drawing.Color.Orange;
            
            
            Title t = new Title(tong + " reviews" , Docking.Top, new System.Drawing.Font("Trebuchet MS", 9, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
            t.Position.X = 50;
            t.Position.Y = 10;
            Chart2.Titles.Add(t);
            Chart2.ChartAreas.Add("Series 1");
            Chart2.ChartAreas["Series 1"].AxisX.MajorGrid.Enabled = false;
            Chart2.ChartAreas["Series 1"].AxisX.MajorTickMark.Enabled = true;
            Chart2.ChartAreas["Series 1"].AxisX.MajorTickMark.LineWidth = 0;
            Chart2.ChartAreas["Series 1"].AxisX.MajorTickMark.Interval = 0;
            Chart2.ChartAreas["Series 1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8, System.Drawing.FontStyle.Regular);
            Chart2.ChartAreas["Series 1"].AxisY.Enabled =AxisEnabled.False;
            //Chart2.ChartAreas["Series 1"].AxisY.LabelStyle.Enabled = false;
            Chart2.ChartAreas["Series 1"].BorderDashStyle = ChartDashStyle.Dot;
            Chart2.ChartAreas["Series 1"].BackColor = System.Drawing.Color.AliceBlue;
            Chart2.ChartAreas["Series 1"].BackGradientStyle = GradientStyle.DiagonalRight;
            Chart2.ChartAreas["Series 1"].BackSecondaryColor = System.Drawing.Color.Orange;
            // create a couple of series
            Chart2.Series.Add("Series 1");
            Chart2.Series["Series 1"].ChartType = SeriesChartType.Bar;
            Chart2.Series["Series 1"].IsValueShownAsLabel = true;
            //Chart2.Series["Series 1"].SmartLabelStyle.

            //Chart2.Series["Series 1"].EmptyPointStyle.Font = new System.Drawing.Font("Trebuchet MS", 6, System.Drawing.FontStyle.Bold);
            // add points to series 1
            foreach (KeyValuePair<string,int> value in ((Dictionary<string, int>)ViewData["Chart"]))
            {
                Chart2.Series["Series 1"].Points.AddXY(value.Key, value.Value);
            }
            Chart2.Series["Series 1"].Color = System.Drawing.Color.OrangeRed;
            Chart2.Series["Series 1"].Font = new System.Drawing.Font("Trebuchet MS", 7, System.Drawing.FontStyle.Bold);
            Chart2.BorderSkin.SkinStyle = BorderSkinStyle.Sunken;
            Chart2.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            Chart2.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart2.BorderWidth = 0;

            // Render chart control
            //Chart2.Page = this;
            //HtmlTextWriter writer = new HtmlTextWriter(ControllerContext.ParentActionViewContext.Writer);
            //Chart2.RenderControl(writer);
            var returnStream = new MemoryStream();
            Chart2.ImageType = ChartImageType.Png;
            Chart2.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }

        public ActionResult GetRainfallChartBar()
        {
            var key = new System.Web.Helpers.Chart(width: 600, height: 400)
                .AddSeries(
                    chartType: "bar",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" })
                .Write();

            return null;
        }
        public ActionResult GetRainfallChartPie()
        {
            var key = new System.Web.Helpers.Chart(width: 600, height: 400)
                .AddSeries(
                    chartType: "pie",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" })
                .Write();

            return null;
        }
        public ActionResult GetRainfallChart()
        {
            var key = new System.Web.Helpers.Chart(width: 600, height: 400)
                .AddSeries(
                    chartType: "area",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" })
                .Write();

            return null;
        }
        public ActionResult GetMonths()
        {
            var d = new DateTimeFormatInfo();
            var key = new System.Web.Helpers.Chart(width: 600, height: 400)
                .AddSeries(chartType: "column")
                .DataBindTable(d.MonthNames)
                .Write(format: "gif");
            return null;
        }
    }
}
