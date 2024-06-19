<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Elpris._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle"><i class="fa fa-bolt"></i>Elpris via API anrop</h1>
            <p class="lead">
                Här kan du anropa egna lokala API:er eller direkt mot www.elprisetjustnu.se API:er. www.elprisetjustnu.se erbjuder data för att få elpriser timvis via Sveriges zoner, exempelvis SE01-SE04.
                <br />
                Du kan även ange koordinater (latitude och longitude) för att hitta prisklass.
            </p>
        </section>

        <div class="row">
            <section class="col-md-6" aria-labelledby="librariesTitle">
                <h2 id="librariesTitle"><i class="fa fa-cloud"></i>Rest API</h2>
                <p>
                    Ett REST API (Representational State Transfer Application Programming Interface) är ett sätt för olika system att kommunicera med varandra över internet med hjälp av HTTP och JSON. Testa det här: <a href="/api/restapi">Hello world »</a>
                    <br />
                </p>
                <h3><i class="fa fa-plug"></i>Anropa API för Elpriser</h3>
                <p>
                    Här kan du anropa eller skapa egna API-anrop för att se elpriser dagligen inom de olika prisklasserna. Vid varje API-anrop genereras automatiskt statistik och diagram baserat på den hämtade datan. <i class="fa fa-line-chart"></i>
                </p>
                <p>
                    <a href="RestAPI.aspx" class="btn btn-primary btn-md">Anropa API</a>
                </p>
            </section>
            <section class="col-md-6" aria-labelledby="gettingStartedTitle">
                <h2 id="gettingStartedTitle"><i class="fa fa-cogs"></i>API - SOAP 1.2</h2>
                <p>
                    SOAP API är ett protokoll för att kommunicera och överföra data mellan olika applikationer via internet, användande XML-baserade meddelanden.
                    <br />
                    Testa: <a href="/Controllers/Api/Soap.asmx?op=HelloWorld">Hello world &raquo;</a>
                </p>
                <h3><i class="fa fa-plug"></i>Anropa API för Elpriser</h3>
                <p>
                    Här kan du anropa funktionerna GetByDateAndCoordinates & GetByDateAndPriceClass och få resultatet i XML-format.
                </p>
                <p>
                    <a href="Controllers/Api/Soap.asmx" target="_blank" class="btn btn-primary btn-md">Anropa API</a>
                </p>
            </section>
        </div>

    </main>
</asp:Content>
