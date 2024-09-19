<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="User_Index" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="col-md-12">
            <h4>Users</h4>
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
            <div>

                <asp:GridView Style="width: 100%"
                    ID="gridService"
                    runat="server"
                    AllowPaging="true"
                    AutoGenerateColumns="false"
                    GridLines="Horizontal"
                    OnRowCommand="gridService_OnRowCommand"
                    CssClass="table  table-sm table-bordered table-condensed table-responsive table-hover table-striped">
                    <Columns>
                        <asp:BoundField DataField="UserId" HeaderText="User ID" />
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate>
                                <%#(Role)Eval("Role")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Created_at" HeaderText="Created At" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditModal"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandName="EditModal"
                                    Text="Edit"
                                    CommandArgument='<%# Eval("UserId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="modal fade" id="editModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dailog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Record</h4>
                    <asp:Button ID="btncloseicon" aria-label="Close" class="close" OnClick="CloseModal_OnClick" runat="server" Text="&times;">

                    </asp:Button>
                </div>
                <div class="modal-body">
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
                    <asp:HiddenField ID="UserId" runat="server" />
                    <asp:HiddenField ID="UsernameHidden" runat="server" />
                    <label>Username</label>
                    <asp:TextBox ID="Username" CssClass="form-control" placeholder="Username" runat="server" />

                    <label>Name</label>
                    <asp:TextBox ID="Name" CssClass="form-control" placeholder="Name" runat="server" />

                    <label>Email</label>
                    <asp:TextBox ID="Email" CssClass="form-control" placeholder="Email" runat="server" />

                    <label>Password</label>
                    <asp:TextBox ID="Password" CssClass="form-control" placeholder="Leave it blank for no update" runat="server" />

                    <label>Role</label>
                    <asp:DropDownList ID="RoleSelect" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Customer" />
                        <asp:ListItem Text="Event Manager" />
                        <asp:ListItem Text="Venue Manager" />
                    </asp:DropDownList>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btndelete" CssClass="btn btn-danger" OnClick="DeleteModal_OnClick" Text="Delete" runat="server"/>
                    <asp:Button ID="btnsave" CssClass="btn btn-primary" OnClick="SaveBtn_OnClick" Text="Save" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <div class="modal fade" id="deleteModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dailog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Delete Record</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
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

                    <asp:HiddenField ID="UserIdDelete" runat="server" />
                    Are you sure want to delete "<%=ViewState["username"]%>" ?

                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button3" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="Button1" CssClass="btn btn-danger" OnClick="DeleteBtn_OnClick" Text="Delete" runat="server" />
                </div>
            </div>

        </div>

    </div>

</asp:Content>

