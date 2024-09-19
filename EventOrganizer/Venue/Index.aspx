<%@ Page Title="Venues" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Venue_Index_Client" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="container">
        <div class="margin-top-xs" style="margin-top: 5pt">
            <a class="btn btn-primary" href="<%=ResolveUrl("./Reservation") %>">My Reservation Status
            </a>
        </div>
        <div class="row" style="margin-top: 5pt">

            <% foreach (System.Data.IDataRecord data in VenueListGenerator())
                { %>
            <a class="" href="<%=ResolveUrl("./Show") %>?id=<%=data["VenueId"] %>" style="text-decoration: none !important">
                <div class="col-sm-3">

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <b><%=data["name"]%></b>
                        </div>
                        <img src="<%=ResolveUrl("~/Media/Images/" + data["image"])%>" class="img img-thumbnail" />

                        <div class="panel-body">
                            <h3><%=data["name"]%></h3>
                            <p>Location: <%=data["location"]%></p>
                        </div>
                    </div>
                </div>
            </a>
            <% } %>
        </div>
    </div>


</asp:Content>

