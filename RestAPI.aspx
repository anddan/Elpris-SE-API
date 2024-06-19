<%@ Page Title="Rest API" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RestAPI.aspx.cs" Inherits="Elpris.RestAPI" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script>
        $(document).ready(function () {
            // Aktivera datepicker för startdatum
            $('#startDate').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true
            });

            // Aktivera datepicker för slutdatum
            $('#endDate').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true
            });
        });

        function openDialog(element) {
            $(element).css("display", "block");
        }
        function closeDialog(element) {
            $(element).css("display", "none");
        }

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
        function drawChart() {
            var data = google.visualization.arrayToDataTable([
                <%= GetChartData() %>
            ]);
            // Set chart options
            var options = {
                title: 'Elpris SEK',
                curveType: 'function',
                legend: {
                    position: 'right', // Placera legenden till höger om diagrammet
                    alignment: 'center',
                    textStyle: {
                        fontSize: 10,
                        rotation: 90
                    }
                }
            };
            var chart = new google.visualization.LineChart(document.getElementById('<%=line_chart_div.ClientID%>'));
            chart.draw(data, options);
        }
  </script>
    <style>
        .stats h3, .stats b {
            color: #4f4ff3;
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.5);
        }

        .modal-content {
            background-color: #fff;
            margin: 15% auto;
            padding: 20px;
            border: 1px solid #888;
            width: 80%;
            max-width: 500px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
        }

        .close {
            color: #aaa;
            font-size: 28px;
            font-weight: bold;
            position: absolute;
            right: 20px;
            top: 10px;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }
    </style>
    <h1>Rest API</h1>
    <div class="container mt-5">
        <div class="row m-1">
            <section class="col-md-3" aria-labelledby="librariesTitle">

                <h3>Utför/Generera API anrop</h3>
                <div class="form-group">
                    <label for="startDate">Från datum:</label>
                    <input type="date" runat="server" class="form-control" id="inputStartDate" name="startDate">
                </div>
                <div class="form-group">
                    <label for="endDate">Till datum:</label>
                    <input type="date" runat="server" class="form-control" id="inputEndDate" name="endDate">
                </div>
                <div class="form-group">
                    <label for="priceClass">Prisklass:</label><a style="margin-left: 10px;" href="#" onclick="openDialog('#myModal'); return false;">Hämta</a>
                    <asp:DropDownList ID="ddlPriceClass" runat="server" CssClass="form-control dropdown">
                        <asp:ListItem Value="">Ange prisklass</asp:ListItem>
                        <asp:ListItem Value="SE1">SE1 - Luleå / Norra Sverige</asp:ListItem>
                        <asp:ListItem Value="SE2">SE2 - Sundsvall / Norra Mellansverige</asp:ListItem>
                        <asp:ListItem Value="SE3">SE3 - Stockholm / Södra Mellansverige</asp:ListItem>
                        <asp:ListItem Value="SE4">SE4 - Malmö / Södra Sverige</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group mt-2">
                    <asp:Button ID="btnGenerateAPICalls" CssClass="btn btn-secondary" runat="server" OnClick="btnGenerateAPICalls_Click" Text="Generara API anrop" />
                    <asp:Button ID="btnExecute" CssClass="btn btn-primary" runat="server" OnClick="btnExecute_Click" Text="Anrop API" />
                </div>
            </section>
            <section class="col-md-3" aria-labelledby="librariesTitle">
                <asp:Label ID="lblStats" runat="server"></asp:Label>
            </section>
            <section class="col-md-6" aria-labelledby="librariesTitle">
                <asp:Panel id="line_chart_div" runat="server" style="width:100%;" Visible="false"></asp:Panel>
            </section>
            <div class="form-group mt-2">
                <asp:Label ID="lblAPIResult" CssClass="form-control" Visible="false" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div id="myModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeDialog('#myModal'); return false">&times;</span>
            <h2>Koordinater</h2>
            <div class="form-group">
                <label for="latitud">latitud:</label>
                <input type="text" runat="server" class="form-control" id="inputLatitud" value="">
            </div>
            <div class="form-group">
                <label for="longitude">longitude:</label>
                <input type="text" runat="server" class="form-control" id="inputLongitude" value="">
            </div>
            <div class="form-group mt-2">
                <asp:Button ID="btnGetPriceClass" CssClass="btn btn-primary" runat="server" Text="Hämta prisklass" OnClick="btnGetPriceClass_Click" />
            </div>
        </div>
    </div>

</asp:Content>
