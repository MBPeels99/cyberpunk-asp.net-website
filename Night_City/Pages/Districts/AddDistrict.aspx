<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDistrict.aspx.cs" Inherits="Night_City.Pages.AddDistrict" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add New District</title>
    <link rel="stylesheet" href="../../assets/css/admin_page.css" />
    <link rel="stylesheet" href="../../css/cyberpunk-css-main/cyberpunk.css" />
</head>
<body>
    <h1 class="cyber-h1">Add a New Ditrict</h1>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="main">
            <label for="DistrictName">District Name:</label>
            <asp:TextBox ID="txtDistrictName" runat="server" MaxLength="50" /><br />

            <label for="Description">Description:</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox><br />

            <label for="ImageMap">Image Map:</label>
            <asp:FileUpload ID="fuImageMap" runat="server" /><br />

            <label for="BackImage">Background Image:</label>
            <asp:FileUpload ID="fuBackImage" runat="server" /><br />

            <label for="ImageOne">First Image:</label>
            <asp:FileUpload ID="fuImageOne" runat="server" /><br />

            <label for="ImageTwo">Second Image:</label>
            <asp:FileUpload ID="fuImageTwo" runat="server" /><br />

            <asp:Button ID="btnAddDistrict" runat="server" Text="Add District" OnClick="btnAddDistrict_Click" />
            <asp:Button ID="btnReturn" runat="server" Text="Return" PostBackUrl="~/Pages/AdminPage.aspx" />
        </div>
    </form>
</body>
</html>

