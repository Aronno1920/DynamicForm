<%@ Page Title="Lookup | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Lookup.aspx.cs" Inherits="LogicDMS.UI.Lookup" %>

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
            <div class="row">
                <div class="col-5">
                    <div class="panel panel-info">
                        <div class="panel-header">Create Lookup</div>
                        <div class="panel-body">

                            <asp:HiddenField runat="server" ID="hfLookupId" Value="0" />
                            <asp:HiddenField runat="server" ID="hfLookupValue" Value="0" />

                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Lookup Type <span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:DropDownList runat="server" ID="ddlLookupType" OnSelectedIndexChanged="ddlLookupType_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Lookup Text <span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtName" placeholder="Lookup Text" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Description
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtDescription" placeholder="Lookup Description" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Serial
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:TextBox runat="server" ID="txtSerial" placeholder="Serial" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divIsRequired" class="row">
                                <div class="col-md-3 col-sm-3">
                                    Is Required?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="cbIsRequired" CssClass="CheckBoxStyle" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Is Active?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12" align="center">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="150px" CssClass="btn btn-md btn-success" />
                                    <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" Width="150px" CssClass="btn btn-md btn-danger" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-7">
                    <div class="panel panel-info">
                        <div class="panel-header">Lookup List</div>
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtSearch" placeholder="Search with Lookup text or discription" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-2" align="right">
                                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" />
                                    <asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" Text="Reload" />
                                </div>
                            </div>
                            <div style="margin: 10px"></div>
                            <asp:GridView ID="gvLookup" runat="server"
                                OnRowCommand="gvLookup_RowCommand"
                                OnPageIndexChanging="gvLookup_PageIndexChanging"
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="15" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="Serial" HeaderText="Serial" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                    <asp:BoundField DataField="LookupText" HeaderText="Lookup Text" />
                                    <asp:BoundField DataField="LookupDescription" HeaderText="Description" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for edit" ImageUrl="~/assets/img/edit.png" Width="25px" />

                                            <asp:HiddenField runat="server" ID="gHfLookupId" Value='<%#Eval("LookupId")%>' />
                                            <asp:HiddenField runat="server" ID="gHfLookupTypeId" Value='<%#Eval("LookupTypeId")%>' />
                                            <asp:HiddenField runat="server" ID="gHfLookupValue" Value='<%#Eval("LookupValue")%>' />
                                            <asp:HiddenField runat="server" ID="gHfIsRequired" Value='<%#Eval("IsRequired")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No data found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
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
