<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DynamicDMS.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login | DMS</title>
    
    <link rel="shortcut icon" href="/Images/dms_logo.png"/>
    <link href="assets/css/app.css" rel="stylesheet" />
    <link href="assets/css/custom.css" rel="stylesheet" />
</head>
<body style="font-family: Tahoma">
    <form id="form2" runat="server">
        <section id="login">
            <div class="login-body">
                <div class="container">
                    <div class="row align-items-center">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-5 col-xl-5">
                            <div class="login-content text-center">
                                <img src="assets/img/login-logo.png" alt="Developer Company" />
                                <h1 style="opacity: 0.9; color: #061A39">Document<br />
                                    Management<br />
                                    System</h1>
                                <p style="color: #fff; background-color: #061A39; width: 60%; font-weight: bold; margin: 0 auto;">
                                    <span>One Stop Solution for DMS </span>
                                </p>
                            </div>
                        </div>
                        <div class="col-md-3 col-lg-3 col-xl-3"></div>
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 col-xl-4">
                            <div class="login-forms text-center">
                                <div class="login-signup text-center">
                                    <h3>Welcome to Dynamic DMS!</h3>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="txtEmployeeId" runat="server" class="form-control input-lg" placeholder="Employee Id"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="txtPassword" runat="server" type="password" class="form-control input-lg" placeholder="Password" TextMode="Password"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-primary btn-block" Style="margin-bottom: 20px;" OnClick="btnLogin_Click" Text="Login" />
                                <a href="#" class="forgot-pass">Forgot your Password ?</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="login-footer">
                <div class="copyright">
                    <span>&copy&nbsp;2023&nbsp;All Rights Reserved | Design & Developed by
                    <a target="_blank" href="http://www.logicsoftbd.com/">Logic Infotech Ltd.</a>
                    </span>
                </div>
            </div>
        </section>
    </form>
</body>
</html>

