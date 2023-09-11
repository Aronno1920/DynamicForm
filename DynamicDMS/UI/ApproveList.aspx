<%@ Page Title="Approve Document | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ApproveList.aspx.cs" Inherits="DynamicDMS.UI.ApproveList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>

            <div class="panel panel-info">
                <div class="panel-header">Document - Search Criteria</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-1 col-md-1">
                            Company
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1 col-md-1">Category</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1 col-md-1">
                            Expense Type
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList runat="server" ID="ddlExpenseType" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1 col-md-1">
                            Tracking No
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtRefNo" runat="server" placeholder="Search with tracking number" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">
                            Search With
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtPartyName" placeholder="Search with PO, LC, MRR and Challan" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-3 col-md-3">
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button runat="server" ID="btnReload" Text="Reload" OnClick="btnReload_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div style="margin-top: 20px"></div>
            <div class="panel panel-info">
                <div class="panel-header">Account Documents (<asp:Label ID="lblWorkflowCount" runat="server" Text="0"></asp:Label>)</div>
                <div class="panel-body">

                    <asp:GridView ID="gvWorkflowDocuments" runat="server"
                        OnPageIndexChanging="gvWorkflowDocuments_PageIndexChanging"
                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="TranID" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranID" runat="server" Text='<%#Eval("DocumentID") %>'></asp:Label>
                                    <asp:Label ID="lblTotalPage" runat="server" Text='<%#Eval("TotalPage") %>'></asp:Label>
                                    <asp:Label ID="lblBillRefNo" runat="server" Text='<%#Eval("BillRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tracking No.">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblTo" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ref Tracking">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblTo" runat="server" NavigateUrl='<%#Eval("RefDocumentPath") %>' Text='<%#Eval("RefDocumentNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Company" HeaderText="Company" />
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Item Category" />
                            <asp:BoundField DataField="ServiceName" HeaderText="Service" />
                            <%--<asp:BoundField DataField="RoleName" HeaderText="Role" />--%>
                            <asp:BoundField DataField="PINo" HeaderText="PI No." ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="PONO" HeaderText="PO No." ItemStyle-HorizontalAlign="Right" />
                            <%--<asp:BoundField DataField="LCNo" HeaderText="LC No." ItemStyle-HorizontalAlign="Right" />--%>
                            <asp:BoundField DataField="MRRNo" HeaderText="MRR No." ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ChallanNo" HeaderText="Challan No." ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DiscountAmount" HeaderText="Discount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="FinalAmount" HeaderText="Final Amount" ItemStyle-HorizontalAlign="Right" />
                            <%--<asp:BoundField DataField="Status" HeaderText="Status" />--%>
                            <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="LastModified" HeaderText="Approve Date" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <EmptyDataTemplate>
                            No workflow document found for the search criteria.
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
</asp:Content>
