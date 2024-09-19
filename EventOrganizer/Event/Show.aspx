<%@ Page Title="Venues" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Show.aspx.cs" Inherits="Venue_Show_Client" Async="true" %>

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

    <h3><b><%=Name %></b></h3>
    <asp:Button runat="server" CssClass="btn btn-success" OnClick="OpenModal_OnClick" Text="Edit" Style="margin-top: 5pt; margin-bottom: 5pt" />
    <div class="container" style="margin-top: 10pt">
        <div class="row">
            <div class="col-sm-7">
                <div class="margin-top-xs">
                    <table class="table">
                        <tr>
                            <td>
                                <b>Status:</b>
                            </td>
                            <td>
                                <%=(Status)status%>
                            </td>
                        </tr>
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
                                <b>Venue Reservation ID:</b>
                            </td>
                            <td>#<%=VenueRentId%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Venue:</b>
                            </td>
                            <td><b><%=VenueName%></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Capacity:</b>
                            </td>
                            <td><%=Capacity%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Price:</b>
                            </td>
                            <td>NT$ <%=Price%>
                            </td>
                        </tr>
                    </table>
                </div>

            </div>
            <div>
                <div class="margin-top-xs">
                    <p><b>Details:</b></p>
                    <p><%= Details%></p>
                </div>

            </div>
        </div>
    </div>

    <div>

        <div>

            <asp:GridView Style="width: 100%"
                ID="gridService"
                runat="server"
                AllowPaging="true"
                AutoGenerateColumns="false"
                GridLines="Horizontal"
                CssClass="table  table-sm table-bordered table-condensed table-responsive table-hover table-striped">
                <Columns>
                    <asp:BoundField DataField="SignupId" HeaderText="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="Created_at" HeaderText="Time" />

                </Columns>
            </asp:GridView>
        </div>
    </div>


    <div style="margin-top: 15pt">
        <small>Created at: <%=Created_at%></small>
    </div>

    <div class="modal fade" id="createModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Event</h4>
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
                    <label>Name</label>
                    <asp:TextBox ID="EventName" CssClass="form-control" placeholder="Event Name" runat="server" />

                    <label>Status</label>
                    <asp:DropDownList ID="EventStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0" Text="Draft" />
                        <asp:ListItem Value="1" Text="Publish" />
                    </asp:DropDownList>

                    <label>Start Time</label>
                    <asp:TextBox type="datetime-local" ID="EventStartTime" CssClass="form-control" runat="server" />

                    <label>Finish Time</label>
                    <asp:TextBox type="datetime-local" ID="EventEndTime" CssClass="form-control" runat="server" />

                    <label>Details</label>
                    <asp:TextBox ID="EventDetails" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Details" runat="server" />

                    <label>Venue Rental ID</label>
                    <input value="#<%=VenueRentId%>" class="form-control" disabled />

                    <label>Capacity</label>
                    <asp:TextBox type="number" min="0" ID="EventCapacity" CssClass="form-control" placeholder="Capacity" runat="server" />

                    <label>Price</label>
                    <asp:TextBox type="number" min="0" ID="EventPrice" CssClass="form-control" placeholder="Price" runat="server" />

                    <label>Image</label>
                    <asp:FileUpload ID="EventImage" CssClass="form-control" runat="server" />
                    <small>Leave empty for no update.</small>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btncreate" CssClass="btn btn-primary" OnClick="CreateBtn_OnClick" Text="Save" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@6.1.10/index.global.min.js'></script>

</asp:Content>

