<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UnauthorizedPage.aspx.cs" Inherits="DynamicDMS.UI.UnauthorizedPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <div style="margin-top: 15px;"></div>
    <div class="panel panel-danger">
        <div class="panel-header">Unauthorized Area</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2 col-md-2"></div>
                <div class="col-sm-8 col-md-8" style="padding: 10px; text-align: center;">
                    <img src="../Images/road-barrier.png" style="border: 5px solid #A1DBB2; border-radius: 50%; width: 250px; height: 250px;" />
                    <br />
                    <br />
                    <h3>Oops, Sorry!</h3>
                    <p style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 14px;">
                        Sorry, your access is refused due to security reasons of our server and also our sensitive data.<br />
                        Please go back to the previous page to continue browsing.
                    </p>
                    <a class="btn btn-danger" href="javascript:history.back()">Go Back</a>
                </div>
                <div class="col-sm-2 col-md-2"></div>
            </div>
        </div>
    </div>

</asp:Content>
