<%@ Page Title="Venues" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Show.aspx.cs" Inherits="Venue_Show_Client" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <img src="<%=ResolveUrl("~/Media/Images/" + Image)%>" class="img img-thumbnail" />
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

    <div class="container" style="margin-top: 10pt">
        <div class="row">
            <div class="col-sm-7">
                <div class="margin-top-xs" >
                    <h3><b><%=Name %></b></h3>
                    <table class="table">
                        <tr>
                            <td >
                                <b>Capacity:</b>
                            </td>
                            <td>
                                <%=Capacity%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Location:</b>
                            </td>
                            <td>
                                <%=Location%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Price:</b>
                            </td>
                            <td>
                                NT$ <%=Price%> <%=(PricePer)Priceper %>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button runat="server" CssClass="btn btn-success" OnClick="OpenModal_OnClick" Text="Rent Now" Style="margin-top: 5pt; margin-bottom: 5pt" />
                <div class="margin-top-xs" style="margin-top: 15pt">
                    <p><b>Description:</b></p>
                    <p><%= Description%></p>
                </div>
            </div>
            <div class="col-md-5">

                <div class="text-center horizontal-center">

                    <div id='calendar'></div>
                </div>
            </div>
        </div>
    </div>


    <div style="margin-top: 15pt">
        <small>Last Update: <%=Updated_at%></small>
    </div>

    <div class="modal fade" id="createModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Add Record</h4>
                    <asp:Button ID="btncloseicon" aria-label="Close" class="close" OnClick="CloseModal_OnClick" runat="server" Text="&times;"></asp:Button>
                </div>
                <div class="modal-body">
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
                    <label>Description</label>
                    <asp:TextBox ID="RentDescription" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Description" runat="server" />

                    <label>Start Time</label>
                    <asp:TextBox type="datetime-local" ID="RentStartTime" CssClass="form-control" runat="server" />

                    <label>End Time</label>
                    <asp:TextBox type="datetime-local" ID="RentEndTime" CssClass="form-control" runat="server" />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btncreate" CssClass="btn btn-primary" OnClick="CreateBtn_OnClick" Text="Add" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@6.1.10/index.global.min.js'></script>
    <script>

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth'
            });
            calendar.render();
        });

    </script>
</asp:Content>

