<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Elpris._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Elpris via API anrop</h1>
            <p class="lead">Här kan du anropa egna lokala API:er eller direkt mot www.elprisetjustnu.se API:er. www.elprisetjustnu.se erbjuder data för att få elpriser timvis via Severiges zoner, exempelvis SE01-SE04.<br /> Du kan även ange koordinater (latitude och longitude) för att hitta prisklass.</p>
        </section>

        <div class="row">
            <section class="col-md-6" aria-labelledby="librariesTitle">
                <h2 id="librariesTitle">Rest API</h2>
                <p>
                    REST API är ett sätt att låta olika system prata med varandra över internet genom att använda HTTP och JSON. Testa: <a href="/api/restapi">Hello world &raquo;</a>
                    <br />Här kan du anropa eller generera egna API anrop för att se el priserdagligen inom de olika pris klasserna.
                </p>
                <p>
                    <a href="RestAPI.aspx" class="btn btn-primary btn-md">Anropa API</a>
                </p>
            </section>
            <section class="col-md-6" aria-labelledby="gettingStartedTitle">
                <h2 id="gettingStartedTitle">API - SOAP 1.2</h2>
                <p>
                    SOAP API är ett protokoll för att kommunicera och överföra data mellan olika applikationer via internet, användande XML-baserade meddelanden. Testa: <a href="/Controllers/Api/Soap.asmx?op=HelloWorld">Hello world &raquo;</a>
                </p>
                <p>
                    <a href="Controllers/Api/Soap.asmx" target="_blank" class="btn btn-primary btn-md">Anropa API</a>
                </p>
            </section>
        </div>
    </main>
</asp:Content>
