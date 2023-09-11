<%@ Page Title="Metadata | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Metadata.aspx.cs" Inherits="DynamicDMS.UI.Metadata" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">

    <script type="text/javascript">
        function CheckNumericOnly(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {
                return true;
            }
            else if (keyCodeEntered == 46) {
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }
            return false;
        }
    </script>

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
                                    <asp:DropDownList runat="server" ID="ddlCategory" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                    <asp:Button runat="server" ID="btnDefault" Visible="true" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-info">
                        <div class="panel-header">Create Metadata</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-2 col-md-2">
                                    Input Caption <span style="color: red">*</span>
                                </div>
                                <div class="col-sm-4 col-md-4">
                                    <asp:HiddenField runat="server" ID="hfMetadataId" />
                                    <asp:TextBox runat="server" ID="txtFieldName" placeholder="Input Caption (don't use special charactor)" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    Input Type <span style="color: red">*</span>
                                </div>
                                <div class="col-sm-4 col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlFieldType" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-md-2">
                                    Panel Name
                                </div>
                                <div class="col-sm-4 col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlPanelNameSave" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    Serial
                                </div>
                                <div class="col-sm-3 col-md-3">
                                    <asp:TextBox runat="server" ID="txtSerialSave" placeholder="Serial" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 col-md-1">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Style="width: 100%" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 15px">
                <div class="col-sm-12 col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-header">Category Metadata List</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvMetadata" runat="server"
                                OnRowCommand="gvMetadata_RowCommand"
                                AutoGenerateColumns="false" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="MetadataId" HeaderText="MetadataId" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="CategoryId" HeaderText="CategoryId" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="DataSourceId" HeaderText="DataSourceId" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="Serial" HeaderText="Serial" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PanelName" HeaderText="Panel Name" />
                                    <asp:BoundField DataField="Caption" HeaderText="Input Caption" />
                                    <asp:BoundField DataField="InputType" HeaderText="Input Type" />
                                    <asp:BoundField DataField="MappingField" HeaderText="Mapping Field" />
                                    <asp:BoundField DataField="RequiredMessage" HeaderText="Required Message" />
                                    <asp:BoundField DataField="Placeholder" HeaderText="Placeholder" />
                                    <asp:BoundField DataField="DataSource" HeaderText="Data Source" />
                                    <asp:BoundField DataField="Orientation" HeaderText="Orientation" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active?" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="gcbIsActive" Checked='<%#Eval("IsActive")%>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Properties" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for other proeprties" ImageUrl="~/assets/img/properties.png" Width="25px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnDelete" CommandName="DeleteRow" ToolTip="Click here to delete this metadata" ImageUrl="~/assets/img/delete.png" HeaderStyle-Width="25px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No metadata found for the document category.
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

            <asp:LinkButton Text="" ID="lnkFake" runat="server" />
            <asp:ModalPopupExtender runat="server" ID="modalExtender" PopupControlID="pnlPopup" TargetControlID="lnkFake"
                PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>

            <asp:Panel runat="server" ID="pnlPopup" Style="display: none;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="PopupHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 550px; display: table-cell;">
                                <span style="font-weight: bold; font-size: larger; padding-left: 10px;">Field Properties</span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <div width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%; padding: 10px;" cellpadding="0" cellspacing="0">
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    Input Type
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:HiddenField runat="server" ID="pHfMetadataId" Value="" />
                                    <asp:HiddenField runat="server" ID="pHfDataSourceId" />
                                    <asp:TextBox runat="server" ID="ptxtFieldType" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divMappingField" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Mapping Field
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:TextBox runat="server" ID="txtMappingField" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divPopupHeader" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Panel Name
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:DropDownList runat="server" ID="ddlHeaderName" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    Caption <span style="color: red">*</span>
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:TextBox runat="server" ID="txtCaption" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    Serial
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:TextBox runat="server" ID="txtSerial" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divRequiredMessage" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Required Message
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:TextBox runat="server" ID="txtRequiredMessage" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divPopupPlaceholder" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Placeholder
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:TextBox runat="server" ID="txtPlaceholder" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div runat="server" id="divPopupLookup" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Data Source
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:DropDownList runat="server" ID="ddlDataSourceName" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div runat="server" id="divPopupOrientation" class="row">
                                <div class="col-sm-3 col-md-3">
                                    Orientation
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:DropDownList runat="server" ID="ddlOrientation" CssClass="DropDownListStyle">
                                        <asp:ListItem Value="Horizontal" Text="Horizontal"></asp:ListItem>
                                        <asp:ListItem Value="Vertical" Text="Vertical"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    Is Active?
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <asp:CheckBox runat="server" ID="cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-12" style="margin-top: 20px;" align="center">
                                    <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-sm btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvMetadata" />
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
