<%@ Page Title="Events" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="Event_Show_Customer" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <div class="" style="margin-top: 10pt">

        <img src="<%=ResolveUrl("~/Media/Images/" + String.Format("{0}", Image))%>" class="img img-thumbnail" />
    </div>

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

    <h1><b><%=Name %></b></h1>
    <div class="container" style="margin-top: 10pt">
        <div class="row">
            <div class="col-sm-8">
                <div class="margin-top-xs">
                    <table class="table">
                        <tr>
                            <td>
                                <b>Event Start Time:</b>
                            </td>
                            <td>
                                <%=Start_Time%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Event End Time:</b>
                            </td>
                            <td>
                                <%=End_Time%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Duration:</b>
                            </td>
                            <td>
                                <%=End_Time - Start_Time%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Venue:</b>
                            </td>
                            <td><b><%=VenueName%></b>
                            </td>
                        </tr>
                    </table>
                </div>

            </div>

            <div>
                <div class="col-sm-4">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <h3 style="margin-top: 10px">NT$ <%=Price %></h3>
                            <small>Registered: <%=Count%>/<%=Capacity %></small>
                            <%
                                if (Session["id"]!=null)
                                {
                            %>
                            <asp:Button runat="server" CssClass="btn btn-success btn-block" OnClick="OpenModal_OnClick" Text="Sign Up" Style="margin-top: 5pt; margin-bottom: 5pt; margin-right: 0pt" />
                            <%
                                }
                                else
                                {
                            %>
                            <a href="<%=ResolveUrl("Account/Login") %>" class="btn btn-primary btn-block">Login to Signup</a>
                            <%
                                }
                            %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div class="margin-top-xs">
                <p><b>Details:</b></p>
                <p><%= Details%></p>
            </div>

        </div>
    </div>



    <div class="modal fade" id="createModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Sign Up Event</h4>
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
                    <asp:HiddenField runat="server" ID="eventid" />
                    <p>Are you sure want to sign up for this event?</p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="No" runat="server" />
                    <asp:Button ID="btncreate" CssClass="btn btn-primary" OnClick="CreateBtn_OnClick" Text="Yes" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@6.1.10/index.global.min.js'></script>

</asp:Content>

