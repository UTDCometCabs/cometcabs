using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;

namespace CometCabsAdmin.Web.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ViewCharts(string key)
        {
            byte[] myChart = null;

            switch (key)
            {
                case "rider":
                    myChart = new Chart(width: 800, height: 500)
                        .AddTitle("Weekly Cabs Usage By Driver")
                        .AddSeries(
                            name: "Monday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "3", "6", "4", "5", "3" }).AddLegend("Monday")
                        .AddSeries(
                            name: "Tuesday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "2", "4", "4", "6", "1" }).AddLegend("Tuesday")
                        .AddSeries(
                            name: "Wednesday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "6", "8", "5", "5", "6" }).AddLegend("Wednesday")
                        .AddSeries(
                            name: "Thursday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "5", "7", "4", "6", "7" }).AddLegend("Thursday")
                        .AddSeries(
                            name: "Friday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "2", "2", "3", "4", "1" }).AddLegend("Friday")
                        .AddSeries(
                            name: "Saturday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "1", "1", "1", "2", "3" }).AddLegend("Saturday")
                        .AddSeries(
                            name: "Sunday",
                            xValue: new[] { "Cab 1", "Cab 2", "Cab 3", "Cab 4", "Cab 5" },
                            yValues: new[] { "1", "0", "1", "2", "0" }).AddLegend("Sunday")
                        .GetBytes("png");

                    break;
                case "cab":
                    myChart = new Chart(width: 600, height: 400)
                        .AddTitle("Cabs")
                        .AddSeries(
                            chartType: "pie",
                            legend: "Rainfall",
                            xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                            yValues: new[] { "20", "20", "40", "10", "10" })
                        .GetBytes("png");

                    break;
                case "route":
                    myChart = new Chart(width: 600, height: 400)
                        .AddTitle("Route")
                        .AddSeries(
                            name: "Route 1",
                            chartType: "line",                            
                            xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                            yValues: new[] { "11", "20", "4", "5", "8" }).AddLegend("Route 1")
                        .AddSeries(
                            name: "Route 2",
                            chartType: "line",
                            xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                            yValues: new[] { "3", "20", "25", "16", "12" }).AddLegend("Route 2")
                        .GetBytes("png");
                    break;
            }

            return File(myChart, "image/png");
        }
    }
}