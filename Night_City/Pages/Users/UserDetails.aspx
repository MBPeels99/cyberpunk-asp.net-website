<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="Night_City.Pages.UserDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Details - Night City</title>
    <link rel="stylesheet" href="../../assets/css/main.css" />
    <link rel="stylesheet" href="../../css/cyberpunk-css-main/cyberpunk.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>User Details</h2>
            <asp:Label ID="lblUserId" runat="server" Text="User ID: "></asp:Label><br />
            <asp:Label ID="lblFullName" runat="server" Text="Full Name: "></asp:Label>
            <asp:TextBox ID="txtFullName" runat="server" ReadOnly="true"></asp:TextBox><br />
            <asp:Label ID="lblEmail" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true"></asp:TextBox><br />
            <asp:Label ID="lblPhoneNumber" runat="server" Text="Phone Number: "></asp:Label>
            <asp:TextBox ID="txtPhoneNumber" runat="server" ReadOnly="true"></asp:TextBox><br />
            <asp:Label ID="lblCountry" runat="server" Text="Country: "></asp:Label>
            <asp:TextBox ID="txtCountry" runat="server" ReadOnly="true"></asp:TextBox><br />
            <asp:Label ID="lblDateOfBirth" runat="server" Text="Date of Birth: "></asp:Label>
            <asp:TextBox ID="txtDateOfBirth" runat="server" ReadOnly="true"></asp:TextBox><br />

            <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" />
            <asp:Button ID="btnReturn" runat="server" Text="Return" PostBackUrl="~/Pages/AdminPage.aspx" />
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
        </div>
    </form>
</body>
</html>
