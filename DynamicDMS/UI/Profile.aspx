<%@ Page Title="Profile | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DynamicDMS.UI.Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="password" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-6 col-md-6">
                    <div class="panel panel-info">
                        <div class="panel-header">My Profile </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    <asp:Image runat="server" ID="imgPhoto" Height="150px" Width="125px" />
                                    <asp:HiddenField runat="server" ID="hfUserID" />
                                    <asp:HiddenField runat="server" ID="hfPassword" />
                                </div>
                                <div class="col-sm-9 col-md-9">
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Login ID</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtLoginID" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Name</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtName" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Designation</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtDesignation" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Department</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtDepartment" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Company</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtCompany" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Location</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtLocation" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Contact No</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtContactNo" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 col-md-4">Email Address</div>
                                        <div class="col-sm-8 col-md-8">
                                            <asp:TextBox runat="server" ID="txtEmailAddress" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 15px"></div>
                    <div class="panel panel-info">
                        <div class="panel-header">Change Photograph </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-10 col-md-10">
                                    <asp:FileUpload runat="server" ID="PhotoUpload" />
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    <asp:Button runat="server" OnClick="btnSave_Click" ID="btnSave" Text="Set Image" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6">
                    <div class="panel panel-info">
                        <div class="panel-header">Notification Settings </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-1 col-md-1">Mobile</div>
                                <div class="col-sm-5 col-md-5">
                                    <asp:TextBox ID="txtNewContact" placeholder="Mobile" runat="server" Text="" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 col-md-1">Email</div>
                                <div class="col-sm-5 col-md-5">
                                    <asp:TextBox ID="txtNewEmail" placeholder="Email" runat="server" Text="" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <center style="margin-top:10px;">
                                <asp:Button runat="server" ID="btnChangeEmail" Text="Save" OnClick="btnChangeEmail_Click" CssClass="btn btn-info" />
                            </center>
                        </div>
                    </div>

                    <div style="margin-top: 15px"></div>
                    <div class="panel panel-success">
                        <div class="panel-header">Change Password </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-3 col-md-4">Current Password</div>
                                <div class="col-sm-8 col-md-8">
                                    <asp:TextBox runat="server" ID="txtCurrentPassword" placeholder="Current Password" TextMode="Password" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-4">New Password</div>
                                <div class="col-sm-8 col-md-8">
                                    <asp:TextBox runat="server" ID="txtNewPassword" placeholder="New Password" TextMode="Password" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-4">Re-type New Password</div>
                                <div class="col-sm-8 col-md-8">
                                    <asp:TextBox runat="server" ID="txtConfirmNewPassword" placeholder="Re-type New Password" TextMode="Password" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <center style="margin-top:10px;">
                            <asp:Button runat="server" ID="btnPassword" Text="Change Password" OnClick="btnPassword_Click" CssClass="btn btn-info" />
                        </center>
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
