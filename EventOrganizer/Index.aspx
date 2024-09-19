<%@ Page Title="Event Organizer Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Admin_Index " Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>


    <div class="row">
        <a href="<%=ResolveUrl("./Venue/Index") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>Venues</h3>
                    </div>
                </div>
            </div>
        </a>
        <a href="<%=ResolveUrl("./Event/Index") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>Events</h3>
                    </div>
                </div>
            </div>
        </a>
        <a href="<%=ResolveUrl("./Venue/Reservation") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>Venue Reservations</h3>
                    </div>
                </div>
            </div>
        </a>
    </div>

</asp:Content>

