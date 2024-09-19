<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <h1 class="text-center" style="margin-top: 72pt">Upcoming  Events</h1>

    <% 

        if (Session["success"] != null)
        {
            String error_message = Session["success"].ToString();
            Session.Remove("success");
    %>
    <div class="alert alert-success" role="alert">
        <%=error_message%>
    </div>
    <% } %>
    <% 

        if (Session["error"] != null)
        {

            String error_message = Session["error"].ToString();
            Session.Remove("error");
    %>
    <div class="alert alert-danger" role="alert">
        <%=error_message%>
    </div>
    <% } %>


    <div class="row" style="margin-top: 15pt">
        <hr />

        <% foreach (System.Data.IDataRecord data in VenueListGenerator())
            { %>
        <a class="" href="<%=ResolveUrl("./Event") %>?id=<%=data["EventId"] %>" style="text-decoration: none !important">
            <div class="col-sm-3">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <b><%=data["Name"]%></b>
                    </div>
                    <img src="<%=ResolveUrl("~/Media/Images/" + data["image"])%>" class="img img-thumbnail" />
                    <div class="panel-body">
                        <h3 style="margin-top: 10px"><%=data["Name"]%></h3>
                        <% 
                            DateTime start_time = (DateTime)data["Start_time"];
                            DateTime end_time = (DateTime)data["End_time"];
                            %>
                        <p>Date: <%= start_time.Date == end_time.Date ? start_time.ToString("yyyy/MM/dd") : start_time.ToString("yyyy/MM/dd")+" - "+end_time.ToString("yyyy/MM/dd") %></p>
                        <p>Time: <%= start_time.ToString("hh:mm tt")+" - "+end_time.ToString("hh:mm tt") %></p>
                    </div>
                </div>
            </div>
        </a>
        <% } %>
    </div>
</asp:Content>
