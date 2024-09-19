﻿<%@ Page Title="Venue Rental Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VenueRent.aspx.cs" Inherits="Venue_Rent" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="col-md-12">
            <h4>Venue Rental</h4>
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
            <div>

                <asp:GridView Style="width: 100%"
                    ID="gridService"
                    runat="server"
                    AllowPaging="true"
                    AutoGenerateColumns="false"
                    OnRowCommand="gridService_OnRowCommand"
                    GridLines="Horizontal"
                    CssClass="table  table-sm table-bordered table-condensed table-responsive table-hover table-striped">
                    <Columns>
                        <asp:BoundField DataField="RentId" HeaderText="ID" />
                        <asp:BoundField DataField="description" HeaderText="Description" />
                        <asp:BoundField DataField="venuename" HeaderText="Venue Name" />
                        <asp:BoundField DataField="start_time" HeaderText="From" />
                        <asp:BoundField DataField="end_time" HeaderText="To" />
                        <asp:BoundField DataField="creator" HeaderText="Created By" />
                        <asp:BoundField DataField="created_at" HeaderText="Created At" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%#(Status)Eval("status")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="pic" HeaderText="Person In Charge" />
                        <asp:BoundField DataField="approved_at" HeaderText="Status Update" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditModal"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandName="Approve"
                                    Text="Approve"
                                    CommandArgument='<%# Eval("RentId") %>' />
                                <asp:LinkButton ID="LinkButton1"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandName="Decline"
                                    Text="Decline"
                                    CommandArgument='<%# Eval("RentId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

   
</asp:Content>

