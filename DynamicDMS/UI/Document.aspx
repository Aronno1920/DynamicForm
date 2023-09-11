<%@ Page Title="Document | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" Async="true" CodeBehind="Document.aspx.cs" Inherits="DynamicDMS.UI.Document" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfCategoryId" />
            <asp:HiddenField runat="server" ID="hfDocumentId" Value="0" />
            <asp:HiddenField runat="server" ID="hfIsApproval" Value="0" />

            <%--General Information--%>
            <div class="panel panel-info">
                <div class="panel-header">General Information (<span runat="server" id="divPageHeader"></span>)</div>
                <div class="panel-body">
                    <asp:HiddenField runat="server" ID="hfRefDocumentID" />
                    <div class="row">
                        <div class="col-sm-1 col-md-1">Company <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1 col-md-1">Expense Type <span style="color: red">*</span></div>
                        <div class="col-sm-2 col-md-2">
                            <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="DropDownListStyle"></asp:DropDownList></td>
                        </div>
                        <div class="col-sm-1 col-md-1">Entry Date</div>
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
                            <asp:TextBox ID="txtRemarks" runat="server" placeholder="Remarks for this document (if any)" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 col-md-1">Ref# Tracking.</div>
                        <div class="col-sm-2 col-md-2">
                            <asp:TextBox ID="txtRefTracking" runat="server" placeholder="Ref. Tracking No" Enabled="false" Style="width: 85%"></asp:TextBox>
                            <asp:Button runat="server" ID="btnLoadRefNo" Text="..." CssClass="btn btn-outline-info" Style="padding: 0px; width: 20px" />

                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                TargetControlID="btnLoadRefNo"
                                PopupControlID="pnlpopup"
                                PopupDragHandleControlID="PopupHeader" Drag="true"
                                BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                        </div>
                    </div>
                    <%--</div>--%>
                </div>
            </div>
            <%--Dynamic Control Here--%>

            <asp:PlaceHolder runat="server" ID="placeholder"></asp:PlaceHolder>

            <%--Dynamic Control Here--%>


            <%--Documents Upload--%>
            <div runat="server" id="divUploader" class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Attachment Uploader</div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-1">Type</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtFileRemarks" placeholder="Remarks for the attachment" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1">Select File</div>
                        <div class="col-sm-9">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <div class="col-sm-2" align="right">
                            <asp:Button ID="btnUploadDocument" runat="server" OnClick="btnUploadDocument_Click" Text="Upload Attachment" />
                        </div>
                    </div>
                </div>
            </div>


            <%--Button Panel--%>
            <div style="margin-top: 15px"></div>
            <div runat="server" id="divButtonPrepare" class="panel panel-info">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-2" align="left">
                            <asp:Button runat="server" ID="btnAddNew" Text="Add New Document" OnClick="btnAddNew_Click" CssClass="btn btn-primary " />
                        </div>
                        <div class="col-md-8" align="center">
                            <asp:Button runat="server" ID="btnSaveDraft" OnClick="btnSaveDraft_Click" Text="Draft" CssClass="btn btn-warning" />
                            <asp:Button runat="server" ID="btnSendToNext" OnClick="btnSendToNext_Click" Text="Submit" CssClass="btn btn-success" />
                        </div>
                        <div class="col-md-2" align="right">
                            <asp:Button runat="server" ID="btnBackList" Text="Back to List" OnClick="btnBackList_Click" CssClass="btn btn-secondary" />
                            <asp:Button runat="server" ID="btnDefault" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div runat="server" id="divButtonApprove" class="panel panel-success">
                <div class="panel-header">Approval Process</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-1">Recommendation</div>
                        <div class="col-sm-11">
                            <asp:TextBox ID="txtRemarksBoss" runat="server" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6" align="right">
                            <asp:DropDownList runat="server" ID="ddlRevertTo" Style="width: 300px; height: 38px; font-size: 1rem; padding: 0.375rem 0.75rem;"></asp:DropDownList>
                        </div>
                        <div class="col-sm-6 col-md-6">
                            <asp:Button runat="server" ID="btnWorkflowDraft" OnClick="btnWorkflowDraft_Click" Text="Draft" CssClass="btn btn-primary" />
                            <asp:Button runat="server" ID="btnWorkflowForward" OnClick="btnWorkflowForward_Click" Text="Submit" CssClass="btn btn-success" OnClientClick="if (!confirm('Are you sure you want Submit/Approve?')) return false;" />
                            <asp:Button runat="server" ID="btnWorkflowReject" OnClick="btnWorkflowReject_Click" Text="Reject" CssClass="btn btn-warning" />
                            <asp:Button runat="server" ID="btnWorkflowDecline" OnClick="btnWorkflowDecline_Click" Text="Delete" CssClass="btn btn-danger" />
                            <asp:Button runat="server" ID="btnWorkflowBackToList" OnClick="btnWorkflowBackToList_Click" Text="Back to List" CssClass="btn btn-secondary" />
                        </div>
                    </div>
                </div>
            </div>

            <div style="margin-top: 15px"></div>
            <div class="row">
                <div class="col-sm-6 col-md-6">
                    <%--Comments Regarding The Documents--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Previous Comments</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvComment" runat="server" Width="100%" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-Width="95px" HeaderStyle-Width="95px" />
                                    <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                    <asp:BoundField DataField="User" HeaderText="User" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                                    <asp:BoundField DataField="Action" HeaderText="Action" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="Waiting" HeaderText="Waiting Hour" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
                                </Columns>
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
                        <div class="panel-header">Previous Attachments</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvAttachment" runat="server" Width="100%"
                                AutoGenerateColumns="false" BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="File" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="ContentType" HeaderText="ContentType" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="DocumentTypeName" HeaderText="Document Type" />
                                    <asp:BoundField DataField="Title" HeaderText="File Name" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Upload By" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="Upload Date" />
                                    <asp:TemplateField HeaderText="Preview" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnPreview" OnClick="gBtnPreview_Click" ImageUrl="~/assets/img/preview.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Download" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnDownload" OnClick="gBtnDownload_Click" ImageUrl="~/assets/img/download.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnDelete" OnClick="gBtnDelete_Click" ImageUrl="~/assets/img/delete.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
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


            <asp:Panel ID="pnlpopup" Style="display: none" runat="server">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="PopupHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 400px; display: table-cell;">
                                <span style="font-weight: bold; font-size: larger; padding-left: 10px;">Search - Document</span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <table width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Search By:
                                    <asp:TextBox runat="server" ID="txtSearchRefNo" placeholder="Ref. No." Width=""></asp:TextBox>
                                    <asp:Button runat="server" ID="btnSearchRefNo" Text="Search" OnClick="btnSearchRefNo_Click" CssClass="btn btn-primary btn-sm" />
                                    <asp:Button runat="server" ID="btnSearchClear" Text="Relolad" OnClick="btnSearchClear_Click" CssClass="btn btn-warning btn-sm" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvDraftDocuments" runat="server"
                                        OnPageIndexChanging="gvDraftDocuments_PageIndexChanging"
                                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnSelectAsset" OnClick="btnSelectAsset_Click" CommandName='<%#Eval("DocumentID") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillRefNo")%>' Text="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tracking No.">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lblTo" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' Target="_blank"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Company" HeaderText="Company" />
                                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Item Category" />
                                            <asp:BoundField DataField="CategoryName" HeaderText="Document Category" />
                                            <asp:BoundField DataField="FlowName" HeaderText="Flowpath" />
                                            <%--<asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                                            <asp:BoundField DataField="PONO" HeaderText="PO No." ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="LCNo" HeaderText="LC No." ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="MRRNo" HeaderText="MRR No." ItemStyle-HorizontalAlign="Right" />--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Own document found for the search criteria.
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="GridViewHeader" />
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadDocument" />
            <asp:PostBackTrigger ControlID="btnSaveDraft" />
            <asp:PostBackTrigger ControlID="gvAttachment" />
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
