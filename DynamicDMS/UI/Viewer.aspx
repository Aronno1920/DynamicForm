<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="DynamicDMS.UI.Viewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hfName" />
        <asp:HiddenField runat="server" ID="hfContentType" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
    </form>
</body>
</html>
