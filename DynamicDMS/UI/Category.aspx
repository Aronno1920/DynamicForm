<%@ Page Title="Category | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="LogicDMS.UI.Category" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <style type="text/css">
        .HideGridColumn {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-5">
                    <div class="panel panel-info">
                        <div class="panel-header">Create Category</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-2">Name <span style="color: red">*</span></div>
                                <div class="col-10">
                                    <asp:HiddenField runat="server" ID="hfCategoryId" />
                                    <asp:TextBox runat="server" ID="txtCategoryName" placeholder="Category Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Desciption</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtDescription" placeholder="Category Description" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Is Active?</div>
                                <div class="col-10">
                                    <asp:CheckBox runat="server" ID="cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12" align="center">
                                    <div style="margin-top: 20px; text-align: center">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="150px" CssClass="btn btn-md btn-success" />
                                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" Width="150px" CssClass="btn btn-md btn-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 15px">
                        <div class="panel-header">Category List</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-9">
                                    <asp:TextBox runat="server" ID="txtSearch" placeholder="Search with Category name or description" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-3" align="right">
                                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" />
                                    <asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" Text="Reload" />
                                </div>
                            </div>
                            <div style="margin: 10px"></div>
                            <asp:GridView ID="gvCategorys" runat="server"
                                OnRowCommand="gvCategorys_RowCommand"
                                OnPageIndexChanging="gvCategorys_PageIndexChanging"
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="15" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL." ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CategoryID" HeaderText="Id" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnView" CommandName="ViewRow" ToolTip="Click here to view Category wise user" ImageUrl="~/assets/img/preview.png" Width="20px" />
                                            <asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for edit" ImageUrl="~/assets/img/edit.png" Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Category found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-7">
                    <div class="panel panel-info">
                        <div class="panel-header">Category User<asp:Label runat="server" ID="lblCategoryName"></asp:Label></div>
                        <div class="panel-body">
                            <asp:GridView ID="gvCategoryUser" runat="server"
                                AutoGenerateColumns="false" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <Columns>
                                    <asp:BoundField DataField="RoleCompany" HeaderText="Flow Company" />
                                    <asp:BoundField DataField="FlowName" HeaderText="Flow Name" />
                                    <asp:BoundField DataField="UserName" HeaderText="User" />
                                    <asp:BoundField DataField="Company" HeaderText="Company" />
                                    <asp:BoundField DataField="Department" HeaderText="Department" />
                                    <asp:BoundField DataField="Designation" HeaderText="Designation" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No Category user found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvCategorys" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div class="loading-panel">
                <div class="loading-container">
                    <center>
                        <div style="background-color: white; height: 120px; width: 400px; padding-top: 50px;" class="border border-info rounded-5">
                            <span>Processing, Please wait a moment...</span>
                            <br />
                            <img src="<%= this.ResolveUrl("~/Images/loading-logo.gif")%>" width="350px" alt="Please wait..." />
                        </div>
                    </center>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
