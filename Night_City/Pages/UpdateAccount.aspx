<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateAccount.aspx.cs" Inherits="Night_City.Pages.UpdateAccount" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Account</title>
    <link rel="stylesheet" href="../Styles/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1>Update Account</h1>
        </header>
        <main>
            <h2>Update Your Account</h2>
            <div class="form-group">
                <label for="txtFullName">Full Name:</label>
                <input type="text" id="txtFullName" runat="server" class="form-control" />
            </div>
            <div class="form-group">
                <label for="txtEmail">Email:</label>
                <input type="email" id="txtEmail" runat="server" class="form-control" />
            </div>
            <div class="form-group">
                <label for="txtPhone">Phone Number:</label>
                <input type="text" id="txtPhone" runat="server" class="form-control" />
            </div>
            <div class="form-group">
                <label for="ddlCountry">Country:</label>
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">
                    <asp:ListItem Value="USA" Text="USA"></asp:ListItem>
                    <asp:ListItem Value="Canada" Text="Canada"></asp:ListItem>
                    <asp:ListItem Value="Netherlands" Text="Netherlands"></asp:ListItem>
                    <asp:ListItem Value="South Africa" Text="South Africa"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" CssClass="button" />
            <asp:Label ID="lblError" runat="server" Text="" BackColor="Red"></asp:Label>
        </main>
    </form>
</body>
</html>
