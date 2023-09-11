<%@ Page Title="Workflow Document | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FlowDocument.aspx.cs" Inherits="DynamicDMS.FlowDocument" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>

            <div class="panel panel-info">
                <div class="panel-header">Document - Search Criteria</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">Company</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Category</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Expense Type</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlExpense" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Status</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1">Tracking No</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtRefNo" runat="server" placeholder="Search with tracking number" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Search With</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtSearchWith" placeholder="Search with PO, LC, MRR and Challan" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            My Document
                        </div>
                        <div class="col-2" style="vertical-align: middle;">
                            <asp:CheckBox runat="server" ID="cbIsForwarded" CssClass="CheckBoxStyle" />
                        </div>
                        <div class="col-3">
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button runat="server" ID="btnReload" Text="Reload" OnClick="btnReload_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div style="margin-top: 20px"></div>
            <div class="panel panel-info">
                <div class="panel-header">Workflow Documents (<asp:Label ID="lblWorkflowCount" runat="server" Text="0"></asp:Label>)</div>
                <div class="panel-body">
                    <asp:GridView ID="gvWorkflowDocuments" runat="server"
                        OnRowCommand="gvWorkflowDocuments_RowCommand"
                        OnPageIndexChanging="gvWorkflowDocuments_PageIndexChanging"
                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="TranID" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranID" runat="server" Text='<%#Eval("DocumentID") %>'></asp:Label>
                                    <asp:Label ID="lblBillRefNo" runat="server" Text='<%#Eval("BillRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tracking No.">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblReturnPath" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ref Tracking">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblRefReturnPath" runat="server" NavigateUrl='<%#Eval("RefDocumentPath") %>' Text='<%#Eval("RefDocumentNo") %>' ToolTip='<%#Eval("RefDocumentNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Company" HeaderText="Company" />
                            <asp:BoundField DataField="ServiceName" HeaderText="Service" />
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Item Category" />

                            <asp:BoundField DataField="ReqNo" HeaderText="Req. No." />
                            <asp:BoundField DataField="PoNo" HeaderText="PO No." />
                            <asp:BoundField DataField="PiNo" HeaderText="PI No." />
                            <asp:BoundField DataField="LcNo" HeaderText="LC No." />
                            <asp:BoundField DataField="MrrNo" HeaderText="MRR No." />

                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="LastModified" HeaderText="Last Update" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="Waiting" HeaderText="Waiting" HeaderStyle-Width="80px" />
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="sbtnViewHistory" runat="server"  CommandName="ViewHistory" Width="40px" ImageUrl="~/assets/img/history.png" ToolTip="Click here to view docuemnt movement history" />
                                </ItemTemplate>
                            </asp:TemplateField>
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


            <asp:LinkButton Text="" ID="lnkFake" runat="server" />
            <asp:ModalPopupExtender runat="server" ID="modalExtenderHistory" PopupControlID="PanelDetails" TargetControlID="lnkFake"
                PopupDragHandleControlID="PopupDetailsHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="PanelDetails" Style="display: none;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="PopupDetailsHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 550px; display: table-cell;">
                                <span style="font-weight: bold; font-size: larger; padding-left: 10px;">Movement History</span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton2" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <div width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%; padding: 10px;" cellpadding="0" cellspacing="0">
                            <asp:GridView ID="gvHistory" runat="server" Width="100%"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvWorkflowDocuments" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
