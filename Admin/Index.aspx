<%@ Page Title="Admin Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Admin_Index " Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>


    <div class="row">
        <a href="<%=ResolveUrl("./User/Index") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>User</h3>
                    </div>
                </div>
            </div>
        </a>
        <a href="<%=ResolveUrl("./Venue/Index") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>Venue</h3>
                    </div>
                </div>
            </div>
        </a>
        <a href="<%=ResolveUrl("./VenueRent/Index") %>" style="text-decoration: none !important">

            <div class="col-md-3">

                <div class="panel panel-default">

                    <div class="panel-body">
                        <h3>Venue Rental</h3>
                    </div>
                </div>
            </div>
        </a>
    </div>

</asp:Content>

