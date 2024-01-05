<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmationPage.aspx.cs" Inherits="Night_City.Pages.ConfirmationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirmation Page</title>
    <link rel="stylesheet" href="../Styles/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1 class="h1">Confirmation Page</h1>
            <h2>Notice</h2>
            <p>
                Night City is currently under lockdown. Our travel agents will contact you shortly,
                <asp:Label ID="lblFullName" runat="server" CssClass="yellow-text"></asp:Label>,
                with further details regarding your travel plans within
                <asp:Label ID="lblFromDate" runat="server" CssClass="yellow-text"></asp:Label> to
                <asp:Label ID="lblToDate" runat="server" CssClass="yellow-text"></asp:Label>.
            </p>
            <asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="btnReturn_Click" CssClass="button" />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" PostBackUrl="~/Pages/PlanTrip.aspx" CssClass="button" />
        </div>
    </form>
</body>
</html>
