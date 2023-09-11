<%@ Page Title="Flowpath | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Flowpath.aspx.cs" Inherits="DynamicDMS.UI.Flowpath" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">

    <style type="text/css">
        .CustomGridHeader th {
            padding: 4px;
            background-color: #d9edf7;
            border-color: #bce8f1;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-4">

                    <div class="panel panel-info">
                        <div class="panel-header">Search Document Category</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-2 col-md-2">Category <span style="color: red">*</span></div>
                                <div class="col-sm-10 col-md-10">
                                    <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                    <asp:Button runat="server" ID="btnDefault" Visible="true" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 15px">
                        <div class="panel-header">Create Flowpath</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-2">
                                    Flow <span style="color: red">*</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:HiddenField runat="server" ID="hfFlowId" />
                                    <asp:TextBox runat="server" ID="txtFlowName" placeholder="Flow Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtSerialNo" placeholder="Serial No" onkeypress="return CheckNumericOnly(this);" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2">Description</div>
                                <div class="col-sm-10">
                                    <asp:TextBox runat="server" ID="txtDiscription" placeholder="Document Flow Discription" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <asp:GridView ID="gvMetadata" runat="server"
                                        AutoGenerateColumns="false" Width="100%"
                                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:BoundField DataField="MetadataId" HeaderText="MetadataId" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                            <asp:BoundField DataField="Caption" HeaderText="Field Name" />

                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Is Enable?" HeaderStyle-Width="85px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="gcbIsEnable" OnCheckedChanged="gCheckBox_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("IsEnable")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Is Required?" HeaderStyle-Width="100px" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="gcbIsRequired" OnCheckedChanged="gCheckBox_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("IsRequired")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No metadata found for the selective document category.
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="CustomGridHeader" />
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2">
                                    Attachment
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="cbUpload" Text="Can Upload?" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="cbDownload" Text="Can Download?" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="cbDelete" Text="Can Delete?" CssClass="CheckBoxStyle" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2">
                                    Action
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="cbIsApprover" Text="Is Approver?" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="cbIsActive" Text="Is Active?" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-sm-3">
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-md-12">
                                    <div style="margin-top: 20px; text-align: center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="150px" CssClass="btn btn-md btn-success" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="150px" CssClass="btn btn-md btn-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-info">
                        <div class="panel-header">Document Flowpath List</div>
                        <div class="panel-body">

                            <asp:GridView ID="gvFlowpath" runat="server"
                                OnRowCommand="gvFlowpath_RowCommand"
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="FlowID" HeaderText="FlowID" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="FlowName" HeaderText="Flow Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Upload?">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbUpload" Checked='<%#Eval("IsCanUpload")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Download?">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbDownlaod" Checked='<%#Eval("IsCanDownload")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete?">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbDelete" Checked='<%#Eval("IsCanDelete")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approver?">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbIsApprover" Checked='<%#Eval("IsApprover")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active?">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbIsActive" Checked='<%#Eval("IsActive")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for edit" ImageUrl="~/assets/img/edit.png" Width="25px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnDelete" CommandName="DeleteRow" ToolTip="Click here to delete this document flow" ImageUrl="~/assets/img/delete.png" HeaderStyle-Width="25px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No document flow found for the search criteria.
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
