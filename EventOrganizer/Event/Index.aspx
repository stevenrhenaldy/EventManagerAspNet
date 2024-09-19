<%@ Page Title="Event Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Event_Index_Client" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="col-md-12">
            <h4>Events</h4>
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
            <asp:Button ID="CreateButton" OnClick="CreateModal_OnClick" CssClass="btn btn-primary" Style="margin-top: 5pt" Text="Create Event" runat="server" />
            <div>

                <asp:GridView Style="width: 100%;margin-top: 5pt"
                    ID="gridService"
                    runat="server"
                    AllowPaging="true"
                    AutoGenerateColumns="false"
                    GridLines="Horizontal"
                    CssClass="table  table-sm table-bordered table-condensed table-responsive table-hover table-striped">
                    <Columns>
                        <asp:BoundField DataField="EventId" HeaderText="Event ID" ItemStyle-CssClass="col-md-1" />
                        <asp:TemplateField HeaderText="Poster" ItemStyle-CssClass="col-md-3">
                            <ItemTemplate>
                                <asp:Image runat="server"
                                    ImageUrl='<%# ResolveUrl("~/Media/Images/" + Eval("image")) %>'
                                    Width="150"
                                    CssClass="img img-thumbnail" 
                                    Visible='<%# !string.IsNullOrEmpty(Eval("image").ToString()) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="col-md-2"/>
                        <asp:BoundField DataField="Start_Time" HeaderText="Start" ItemStyle-CssClass="col-md-1"/>
                        <asp:BoundField DataField="End_Time" HeaderText="Finish" ItemStyle-CssClass="col-md-1"/>
                        <asp:BoundField DataField="Capacity" HeaderText="Capacity" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>

                                <a href="<%# ResolveUrl("./Show") + "?id=" + Eval("EventId") as String %>">View</a>

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="modal fade" id="createModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Create event</h4>
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
                    <label>Name</label>
                    <asp:TextBox ID="Name" CssClass="form-control" placeholder="Name" runat="server" />

                    <label>Venue</label>
                    <asp:DropDownList ID="DropDownReservation" CssClass="form-control" runat="server"></asp:DropDownList>

                    <label>Start</label>
                    <asp:TextBox type="datetime-local" ID="Time_start" CssClass="form-control" placeholder="Capacity" runat="server" />

                    <label>End</label>
                    <asp:TextBox type="datetime-local" ID="Time_end" CssClass="form-control" placeholder="Price" runat="server" />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btncreate" CssClass="btn btn-primary" OnClick="CreateBtn_OnClick" Text="Create" runat="server" />
                </div>
            </div>

        </div>

    </div>



</asp:Content>

