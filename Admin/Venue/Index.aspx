<%@ Page Title="Venue Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Venue_Index" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="col-md-12">
            <h4>Venues</h4>
            <asp:Button CssClass="btn btn-primary" runat="server" OnClick="CreateModal_OnClick" Text="Add Venue" />
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
                        <asp:BoundField DataField="VenueId" HeaderText="Venue ID" />
                        <asp:TemplateField HeaderText="Pricing">
                            <ItemTemplate>
                                <img src="<%#ResolveUrl("~/Media/Images/" + String.Format("{0}", Eval("image")))%>" width="150" class="img img-thumbnail" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Capacity" HeaderText="Capacity" />
                        <asp:BoundField DataField="Price" HeaderText="Price" />
                        <asp:TemplateField HeaderText="Pricing">
                            <ItemTemplate>
                                <%#(PricePer)Eval("Priceper")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="usersname" HeaderText="Created By" />
                        <asp:BoundField DataField="Created_at" HeaderText="Created At" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditModal"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandName="EditModal"
                                    Text="Edit"
                                    CommandArgument='<%# Eval("VenueId") %>' />
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
                    <label>Name</label>
                    <asp:TextBox ID="Name" CssClass="form-control" placeholder="Name" runat="server" />

                    <label>Description</label>
                    <asp:TextBox ID="Description" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Description" runat="server" />

                    <label>Location</label>
                    <asp:TextBox ID="Location" CssClass="form-control" placeholder="Location" runat="server" />

                    <label>Capacity</label>
                    <asp:TextBox type="number" ID="Capacity" CssClass="form-control" placeholder="Capacity" runat="server" />

                    <label>Price</label>
                    <asp:TextBox type="number" ID="Price" CssClass="form-control" placeholder="Price" runat="server" />

                    <label>Pricing</label>
                    <asp:DropDownList ID="PricePerSelect" CssClass="form-control" runat="server">
                        <asp:ListItem Value="1" Text="Hourly" />
                        <asp:ListItem Value="2" Text="Daily" />
                    </asp:DropDownList>

                    <label>Image</label>
                    <asp:FileUpload ID="Image" CssClass="form-control" runat="server" />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclose2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btncreate" CssClass="btn btn-primary" OnClick="CreateBtn_OnClick" Text="Add" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <div class="modal fade" id="editModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Record</h4>
                    <asp:Button ID="btncloseiconedit" aria-label="Close" class="close" OnClick="CloseModal_OnClick" runat="server" Text="&times;"></asp:Button>
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
                    <asp:HiddenField runat="server" ID="EditVenueId" />
                    <asp:HiddenField runat="server" ID="EditNameHidden" />

                    <label>Name</label>
                    <asp:TextBox ID="EditName" CssClass="form-control" placeholder="Name" runat="server" />

                    <label>Description</label>
                    <asp:TextBox ID="EditDescription" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Description" runat="server" />

                    <label>Location</label>
                    <asp:TextBox ID="EditLocation" CssClass="form-control" placeholder="Location" runat="server" />

                    <label>Capacity</label>
                    <asp:TextBox type="number" ID="EditCapacity" CssClass="form-control" placeholder="Capacity" runat="server" />

                    <label>Price</label>
                    <asp:TextBox type="number" ID="EditPrice" CssClass="form-control" placeholder="Price" runat="server" />

                    <label>Pricing</label>
                    <asp:DropDownList ID="EditPriceper" CssClass="form-control" runat="server">
                        <asp:ListItem Value="1" Text="Hourly" />
                        <asp:ListItem Value="2" Text="Daily" />
                    </asp:DropDownList>

                    <label>Image</label>
                    <asp:Image CssClass="img img-thumbnail" ID="EditImageShow" runat="server" />
                    <asp:FileUpload ID="EditImage" CssClass="form-control" runat="server" />
                    <small>Leave empty for no update.</small>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btncloseedit2" CssClass="btn btn-secondary" OnClick="CloseModal_OnClick" Text="Close" runat="server" />
                    <asp:Button ID="btndelete" CssClass="btn btn-danger" OnClick="DeleteModal_OnClick" Text="Delete" runat="server" />
                    <asp:Button ID="btnsave" CssClass="btn btn-primary" OnClick="SaveBtn_OnClick" Text="Save" runat="server" />
                </div>
            </div>

        </div>

    </div>

    <div class="modal fade" id="deleteModal" data-backdrop="false" role="dialog">
        <div class=" modal-dialog modal-dialog-centered">
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

