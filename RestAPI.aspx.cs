using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elpris
{
    public partial class RestAPI : System.Web.UI.Page
    {

        bool hourlyData = false;//Default för chart
        Dictionary<string, double> prices = new Dictionary<string, double>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblAPIResult.Visible = false;
                inputStartDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                inputEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public string GetChartData()
        {
            StringBuilder sb = new StringBuilder();
            if (hourlyData)
                sb.Append("['Timme', 'Pris'], ");
            else
                sb.Append("['Dag', 'Avg pris'], ");

            foreach (var kvp in prices)
            {
                // Formatera priset med punkt som decimalavskiljare
                sb.Append($"['{kvp.Key}', {kvp.Value.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)}], ");
            }
            // Ta bort det sista kommatecknet
            return sb.ToString().TrimEnd(',', ' ');
        }
        protected void btnGenerateAPICalls_Click(object sender, EventArgs e)
        {
            ClearForm();
            string result = string.Empty;
            DateTime dstartDate;
            DateTime dendDate;
            string prisklass = ddlPriceClass.SelectedValue;
            if (!string.IsNullOrEmpty(prisklass))
            {
                if (DateTime.TryParse(inputStartDate.Value, out dstartDate) && DateTime.TryParse(inputEndDate.Value, out dendDate))
                {
                    for (DateTime date = (DateTime)dstartDate; date <= (DateTime)dendDate; date = date.AddDays(1))
                    {
                        //Generera länkar till Rest api
                        string linkRestAPi = string.Empty;
                        if (inputLongitude.Value != "" && inputLatitud.Value != "")
                            linkRestAPi = string.Format(@"<a href=""{0}{1}-{2}-{3}/{4}/{5}"" target=""_blank"">{0}{1}-{2}-{3}/{4}/{5}</a>", Common.RestAPIUrl, date.Year, date.Month.ToString("00"), date.Day.ToString("00"), Uri.EscapeDataString(inputLongitude.Value), Uri.EscapeDataString(inputLatitud.Value));
                        else
                            linkRestAPi = string.Format(@"<a href=""{0}{1}-{2}-{3}/{4}"" target=""_blank"">{0}{1}-{2}-{3}/{4}</a>", Common.RestAPIUrl, date.Year, date.Month.ToString("00"), date.Day.ToString("00"), prisklass);
                        string linkElprisjustnuRestAPi = string.Format(@"<a href=""{0}{1}/{2}-{3}_{4}.json"" target=""_blank"">{0}{1}/{2}-{3}_{4}.json</a>", Common.ElPrisetJustNuUrl, date.Year, date.Month.ToString("00"), date.Day.ToString("00"), prisklass);
                        result += linkRestAPi + " &raquo; " + linkElprisjustnuRestAPi + "<br/>";
                    }
                }
                else
                    result = "Vänligen ange giltigt datum";
            }
            else
                result = "Vänligen ange en prisklass";

            line_chart_div.Visible = false;
            lblStats.Visible = false;
            lblAPIResult.Visible = true;
            lblAPIResult.Text = result;
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            ClearForm();
            var service = new ElprisJson();
            string result = string.Empty;
            string stats = string.Empty;
            DateTime dstartDate;
            DateTime dendDate;
            string prisklass = ddlPriceClass.SelectedValue;
            // Beräkna snittpris, lägsta och högsta värden
            double totalSEK = 0;
            double lowestSEK = double.MaxValue;
            double highestSEK = double.MinValue;
            int totalCount = 0;

            if (!string.IsNullOrEmpty(prisklass))
            {
                if (DateTime.TryParse(inputStartDate.Value, out dstartDate) && DateTime.TryParse(inputEndDate.Value, out dendDate))
                {
                    if (dendDate <= DateTime.Now)
                    {
                        for (DateTime date = (DateTime)dstartDate; date <= (DateTime)dendDate; date = date.AddDays(1))
                        {
                            if (dstartDate == dendDate)
                                hourlyData = true;//Visar timvis i chart istället för dagvis
                            else
                                hourlyData = false;//Visar dagvis

                            result += $"<h3>{date.ToString("yyyy-MM-dd")}</h3>";
                            string url = string.Format(@"{0}{1}/{2}-{3}_{4}.json", Common.ElPrisetJustNuUrl, date.Year, date.Month.ToString("00"), date.Day.ToString("00"), prisklass);

                            Task.Run(async () =>
                            {
                                var elpris_list = await service.GetElprisAsync(url);
                                totalCount += elpris_list.Count;
                                double dailyTotalSEK = 0;
                                foreach (ElprisJson elpris in elpris_list)
                                {
                                    totalSEK += elpris.SEK_per_kWh;
                                    dailyTotalSEK += elpris.SEK_per_kWh;

                                    if (elpris.SEK_per_kWh < lowestSEK)
                                        lowestSEK = elpris.SEK_per_kWh;

                                    if (elpris.SEK_per_kWh > highestSEK)
                                        highestSEK = elpris.SEK_per_kWh;


                                    result += $"SEK per kWh: <b>{elpris.SEK_per_kWh}</b><br/>" +
                                                     $"EUR per kWh: <b>{elpris.EUR_per_kWh}</b><br/>" +
                                                     $"EXR: <b>{elpris.EXR}</b><br/>" +
                                                     $"Time start: <b>{elpris.time_start.ToString("yyyy-MM-dd HH:mm")}</b><br/>" +
                                                     $"Time end: <b>{elpris.time_end.ToString("yyyy-MM-dd HH:mm")}</b><hr/>";

                                    if (hourlyData)
                                        prices.Add($"{elpris.time_start.ToString("HH:mm")}", elpris.SEK_per_kWh);
                                }
                                double dailyAverageSEK = dailyTotalSEK / elpris_list.Count;
                                if (!hourlyData)
                                    prices.Add($"{date.ToString("MMM dd")}", dailyAverageSEK);

                            }).Wait(); // Vänta på att uppgiften ska slutföras


                        }
                        double averageSEK = totalSEK / totalCount;
                        // Lägg till statistik för dagen i början av resultatet
                        stats = $"<div class='stats'><h3>Statistics</h3>" +
                                         $"Average SEK per kWh: <b>{averageSEK.ToString("N5")}</b><br/>" +
                                         $"Lowest SEK per kWh: <b>{lowestSEK.ToString("N5")}</b><br/>" +
                                         $"Highest SEK per kWh: <b>{highestSEK.ToString("N5")}</b><br/>" +
                                         $"Total prices fetched: <b>{totalCount}</b><br/></div>";

                        line_chart_div.Visible = true;
                        lblStats.Visible = true;
                        lblStats.Text = stats;
                        lblAPIResult.Text = result;
                    }
                    else
                        lblAPIResult.Text = "Du kan inte ange datum som är större än dagens datum!";
                }
                else
                    lblAPIResult.Text = "Vänligen ange giltiga datum!";

            }
            else
                lblAPIResult.Text = "Vänligen ange en prisklass!";

            lblAPIResult.Visible = true;
        }

        protected void btnGetPriceClass_Click(object sender, EventArgs e)
        {
            ClearForm();
            if (inputLongitude.Value != "" && inputLatitud.Value != "")
            {
                GeoPolygonen geoPolygonen = new GeoPolygonen();
                string priceclass_coordinates = geoPolygonen.FindAreaByCoordinates(double.Parse(inputLongitude.Value.Replace(".", ",")), double.Parse(inputLatitud.Value.Replace(".", ",")));
                if (!string.IsNullOrEmpty(priceclass_coordinates))
                {
                    string prisklass = priceclass_coordinates;
                    ddlPriceClass.ClearSelection();
                    ddlPriceClass.Items.FindByValue(prisklass).Selected = true;
                }
                else
                {
                    ddlPriceClass.ClearSelection();
                    lblMessage.Text = @"<i class=""fa fa-warning""></i>Kunde inte hitta någon prisklass utifrån dina koordinater";
                }
            }
            else
            {
                ddlPriceClass.ClearSelection();
                lblMessage.Text = @"<i class=""fa fa-warning""></i>Kunde inte hitta någon prisklass utifrån dina koordinater";
            }
        }
        private void ClearForm()
        {
            lblStats.Visible = false;
            line_chart_div.Visible = false;
            lblMessage.Text = string.Empty;
        }
    }
}