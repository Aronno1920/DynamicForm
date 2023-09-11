<%@ Page Title="User Management | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="DynamicDMS.UI.User" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <style type="text/css">
        .HideGridColumn {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="panel panel-info">
                <div class="panel-header">Create New User</div>
                <div class="panel-body">
                    <asp:HiddenField runat="server" ID="hfUserId" />
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Employee Id <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtEmployeeId" placeholder="Employee Id" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Name <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtUserName" placeholder="Employee Name" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Email Address</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtEmailAddress" placeholder="Email Address" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Password <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtPassword" placeholder="Password" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Company</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1 col-md-1">Department</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlDepartment" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>

                        <div class="col-sm-1 col-md-1">Location</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlLocation" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1 col-md-1">Designation <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlDesignation" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Contact No</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtContactNo" placeholder="Contact Number" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Is Notified?</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:CheckBox runat="server" ID="cbIsNotified" Checked="true" CssClass="CheckBoxStyle" />
                        </div>
                        <div class="col-sm-1 col-md-1">Is Admin</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:CheckBox runat="server" ID="cbIsAdmin" Checked="true" CssClass="CheckBoxStyle" />
                        </div>
                        <div class="col-sm-1 col-md-1">Is Active?</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:CheckBox runat="server" ID="cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-12" align="center">
                            <div style="margin-top: 20px;">
                                <asp:Button ID="btnCheck" runat="server" OnClick="btnCheck_Click" Text="Check" Width="150px" CssClass="btn btn-md btn-warning" />
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="150px" CssClass="btn btn-md btn-success" />
                                <asp:Button ID="btnResetPassword" runat="server" OnClick="btnResetPassword_Click" Text="Reset Password" Width="150px" CssClass="btn btn-md btn-success" />
                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" Width="150px" CssClass="btn btn-md btn-danger" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin: 20px"></div>
            <div class="panel panel-info">
                <div class="panel-header">User List</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-10 col-md-10">
                            <asp:TextBox runat="server" ID="txtSearch" placeholder="Search with user id, name, contact, email, company, department, designation or group" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 col-md-2" align="right">
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" />
                            <asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" Text="Reload" />
                        </div>
                    </div>

                    <div style="margin: 10px"></div>
                    <asp:GridView ID="gvUserList" runat="server"
                        OnRowCommand="gvUserList_RowCommand"
                        OnPageIndexChanging="gvUserList_PageIndexChanging"
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
                            <asp:BoundField DataField="UserID" HeaderText="UserID" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                            <asp:BoundField DataField="LoginID" HeaderText="ID" />
                            <asp:BoundField DataField="UserName" HeaderText="Name" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                            <asp:BoundField DataField="DesignationName" HeaderText="Designation" />
                            <asp:BoundField DataField="LocationName" HeaderText="Location" />
                            <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                            <asp:BoundField DataField="Email" HeaderText="Email Address" />
                            <asp:BoundField DataField="UserType" HeaderText="Is Admin?" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="IsSendNotification" HeaderText="IsSendNotification" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for edit" ImageUrl="~/assets/img/edit.png" Width="25px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No user found for the search criteria.
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="GridViewHeader" />
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
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
