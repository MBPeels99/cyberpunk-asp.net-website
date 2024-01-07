<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteAccount.aspx.cs" Inherits="Night_City.Pages.DeleteAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delete Account</title>
    <link rel="stylesheet" href="../assets/css/main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1>Delete Account</h1>
        </header>
        <main>
            <h2>Delete Your Account</h2>
            <p>Are you sure you want to delete your account?</p>
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <input type="password" id="txtPassword" runat="server" class="form-control" />
            </div>
            <asp:Button ID="btnDelete" runat="server" Text="Delete Account" OnClick="btnDelete_Click" CssClass="button" />
            <asp:Button ID="btnReturn" runat="server" Text="Return" PostBackUrl="~/Pages/ExplorePage.aspx" CssClass="button" />
            <asp:Label ID="lblError" runat="server" Text="" BackColor="Red"></asp:Label>
        </main>
    </form>
</body>
</html>
