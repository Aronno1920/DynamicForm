<%@ Page Title="Force Movement | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForceMovement.aspx.cs" Inherits="DynamicDMS.UI.ForceMovement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="panel panel-info">
                <div class="panel-header">Document Force Movement</div>
                <div class="panel-body">

                    <asp:HiddenField runat="server" ID="hfDocumentId" />
                    <asp:HiddenField runat="server" ID="hfServiceId" />

                    <div class="panel panel-info">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    Tracking No.
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:TextBox ID="txtBillRef" runat="server" placeholder="Search with tracking no." MaxLength="9" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 15px">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">Company</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">Expense Type</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblExpenseType" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">Deaprtment</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">Bill Ref No.</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblBillRefNo" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">Bill Amount</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblBillAmount2" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">Time Elapse</div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label ID="lblDuration" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">Status</div>
                                <div class="col-lg-11 col-md-11 col-sm-11">
                                    <asp:Label ID="lblCurrentStatus" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    Service Name
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label runat="server" ID="lblServiceName" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    Current Flow
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Label runat="server" ID="lblRoleName" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-info" style="margin-top: 15px">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    Move To
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtRemarks" placeholder="Write reason for force movement" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <table style="width: 100%; margin-top: 15px;">
                        <tr align="center">
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return confirm('Are you sure to Submit the Document?')" Text="Move" CssClass="btn btn-primary" />
                                <asp:Button ID="btnDecline" runat="server" OnClientClick="return confirm('Are you sure to Decline the Document?')" Text="Decline" CssClass="btn btn-danger" />
                            </td>
                        </tr>
                    </table>
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
