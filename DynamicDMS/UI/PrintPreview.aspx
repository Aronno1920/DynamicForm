<%@ Page Title="Print Preview | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintPreview.aspx.cs" Inherits="DynamicDMS.UI.PrintPreview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfServiceId" />
            <asp:HiddenField runat="server" ID="hfDocumentId" Value="0" />

            <%--General Information--%>
            <div class="panel panel-info">
                <div class="panel-header">General Information</div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-1 col-md-1">Company</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtCompany" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Item Category</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtItemType" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">
                            Entry Date
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtEntryDate" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Tracking No.</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtBillRefNo" runat="server" placeholder="Tracking No" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Remarks</div>
                        <div class="col-sm-8 col-md-8">
                            <asp:TextBox ID="txtRemarks" runat="server" placeholder="Remarks for this document (if any)" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">
                            Ref# Tracking.
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtRefTracking" runat="server" placeholder="Ref. Tracking No" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <%--Order Information--%>
            <div class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Order Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-1 col-md-1">PI No</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtPINumber" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">PO/Booking No.</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtPONo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">MRR No.</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtMRRNo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Challan No.</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtChallanNo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <%--Bill Information--%>
            <div class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Party Bill Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Bill No</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtBillNo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Bill Amount</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtBillAmount" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Discount Amonut</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtAuditAmount" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Final Amount</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox runat="server" ID="txtFinalAmount" placeholder="Final Amount" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 15px">
                <div class="col-sm-6 col-md-6">
                    <%--Comments Regarding The Documents--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Comments Regarding The Documents</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvComment" runat="server" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6">
                    <%--Documents Preview--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Documents Preview</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvAttachment" runat="server" Width="100%"
                                AutoGenerateColumns="false" BackColor="#FCFCFC"
                                BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px" CellPadding="8" CellSpacing="4"
                                ShowHeaderWhenEmpty="True" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="TransId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="Name" HeaderText="Document Type" />
                                    <asp:BoundField DataField="Title" HeaderText="File Name" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Upload By" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="Upload Date" HeaderStyle-Width="120px" />
                                    <asp:TemplateField HeaderText="Preview" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnPreview" OnClick="gBtnPreview_Click" ImageUrl="~/assets/img/preview.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No attachemnt file found for the search criteria.
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
            <asp:PostBackTrigger ControlID="gvAttachment" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
