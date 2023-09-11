<%@ Page Title="User Wise Flowpath | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserFlowpath.aspx.cs" Inherits="DynamicDMS.UI.UserFlowpath" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-3 col-md-4">
                    <div class="panel panel-info">
                        <div class="panel-header">Search User</div>
                        <div class="panel-body">
                            <asp:HiddenField ID="hfUserID" runat="server" />

                            <div class="row">
                                <div class="col-sm-2 col-md-2">Login ID</div>
                                <div class="col-sm-7 col-md-7">
                                    <asp:TextBox ID="txtSearchString" runat="server" placeholder="Search with user login id" CssClass="TextBoxStyle"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoComplete_User" runat="server"
                                        ServiceMethod="GetEmployeeInfo"
                                        ServicePath="~/AutoComplete.asmx"
                                        TargetControlID="txtSearchString"
                                        EnableCaching="false"
                                        MinimumPrefixLength="2"
                                        CompletionInterval="100"
                                        CompletionSetCount="10"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="AutoExtender"
                                        CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-sm-3 col-md-3" align="right">
                                    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Search" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-2 col-md-2">Name</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtName" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Designation</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtDesignation" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Company</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtCompany" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Department</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtDepartment" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 20px"></div>
                    <div class="panel panel-success">
                        <div class="panel-header">Search - Copy User</div>
                        <div class="panel-body">
                            <asp:HiddenField ID="hfCopyUserId" runat="server" />

                            <div class="row">
                                <div class="col-sm-2 col-md-2">Login ID</div>
                                <div class="col-sm-7 col-md-7">
                                    <asp:TextBox ID="txtCopyLoginId" runat="server" placeholder="Search with user login id" CssClass="TextBoxStyle"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoComplete_CopyUser" runat="server"
                                        ServiceMethod="GetEmployeeInfo"
                                        ServicePath="~/AutoComplete.asmx"
                                        TargetControlID="txtCopyLoginId"
                                        EnableCaching="false"
                                        MinimumPrefixLength="2"
                                        CompletionInterval="100"
                                        CompletionSetCount="10"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="AutoExtender"
                                        CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-sm-3 col-md-3" align="right">
                                    <asp:Button runat="server" ID="btnCopyRefresh" OnClick="btnCopyRefresh_Click" Text="Search" />
                                    <asp:Button runat="server" ID="btnCopyClear" OnClick="btnCopyClear_Click" Text="Clear" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-2 col-md-2">Name</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtCopyName" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Designation</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtCopyDesignation" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Company</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtCopyCompany" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Department</div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:TextBox runat="server" ID="txtCopyDepartment" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12" style="margin-top: 15px; text-align: center">
                                    <asp:Button runat="server" ID="btnCopyPermission" OnClick="btnCopyPermission_Click" Text="Copy Permission" CssClass="btn btn-md btn-warning" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-8 col-md-8">
                    <div class="panel panel-info">
                        <div class="panel-header">User Wise Flowpath Mapping</div>
                        <div class="panel-body">

                            <asp:GridView ID="gvCompanyWisePermission" runat="server"
                                OnRowCancelingEdit="gvCompanyWisePermission_RowCancelingEdit"
                                OnRowCommand="gvCompanyWisePermission_RowCommand"
                                OnRowDeleting="gvCompanyWisePermission_RowDeleting"
                                OnRowEditing="gvCompanyWisePermission_RowEditing"
                                OnRowDataBound="gvCompanyWisePermission_RowDataBound"
                                AutoGenerateColumns="false" ShowFooter="True" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL." ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hTranID" runat="server" Value='<%#Eval("TranID") %>'></asp:HiddenField>
                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Document Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategoryName" runat="server" Text='<%#Eval("CategoryName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlCategory" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Flow Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFlowName" runat="server" Text='<%#Eval("FlowName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlFlowpath" CssClass="DropDownListStyle"></asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Can Create?">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrepare" runat="server" Text='<%#Eval("CanPrepare") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox runat="server" ID="cbCanPrepare" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <span onclick="return confirm('Are you sure want to delete?')">
                                                <asp:LinkButton ID="btnDelete" Text="Remove" runat="server" CommandName="Delete" />
                                            </span>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnInsertRecord" runat="server" Text="Add" ValidationGroup="ValgrpCust" CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Flowpath found for the Search Criteria.
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvCompanyWisePermission" />
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
