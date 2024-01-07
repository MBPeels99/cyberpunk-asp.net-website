<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFigures.aspx.cs" Inherits="Night_City.Pages.Figures.AddFigures" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Figures</title>

    <!-- JQuery Scripts -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

</head>
<body>
    
    <h1>Add New Figure</h1>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

        <div class="form-group">
            <label for="txtFullName">Full Name:</label>
            <input type="text" id="txtFullName" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtStageName">Stage Name:</label>
            <input type="text" id="txtStageName" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtDateOfBirth">Date of Birth:</label>
            <input type="Date" id="txtDateOfBirth" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtPlaceOfBirth">place of Birth:</label>
            <input type="text" id="txtPlaceOfBirth" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtDateOfDeath">Date of Death:</label>
            <input type="Date" id="txtDateOfDeath" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtPlaceOfDeath">Place of Death:</label>
            <input type="text" id="txtPlaceOfDeath" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtStatus">Status:</label>
            <input type="text" id="txtStatus" runat="server" class="form-control" maxlength="50"/>
        </div>

        <div class="form-group">
            <label for="ddlGender">Gender:</label>
            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                <asp:ListItem Value="">Select Gender</asp:ListItem>
                <asp:ListItem Value="Male">Male</asp:ListItem>
                <asp:ListItem Value="Female">Female</asp:ListItem>
                <asp:ListItem Value="Other">Other</asp:ListItem>
            </asp:DropDownList>
        </div>


        <div class="form-group">
            <label for="txtHairColor">Hair Color:</label>
            <input type="text" id="txtHairColor" runat="server" class="form-control" maxlength="50"/>
        </div>

        <div class="form-group">
            <label for="txtEyeColor">Eye Color:</label>
            <input type="text" id="txtEyeColor" runat="server" class="form-control" maxlength="50"/>
        </div>

        <label for="Occupation">Occupation:</label>
        <asp:ListBox ID="Occupation" runat="server" SelectionMode="Multiple"></asp:ListBox>

        <button type="button" onclick="promptForNewOccupation();">Add New Occupation</button>

        <label for="Affiliation">Affiliation:</label>
        <asp:ListBox ID="Affiliation" runat="server" SelectionMode="Multiple"></asp:ListBox>

        <button type="button" onclick="promptForNewAffiliation();">Add New Affiliation</button>

        <div class="form-group">
            <label for="txtKnownFor">Known For:</label>
            <asp:TextBox id="txtKnownFor" runat="server" class="form-control" textmode="MultiLine" rows="10"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtBackground">Background:</label>
            <asp:TextBox id="txtBackground" runat="server" class="form-control" textmode="MultiLine" rows="10"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtPartner">Partner:</label>
            <input type="text" id="txtPartner" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="txtVoicedBy">Voiced By:</label>
            <input type="text" id="txtVoicedBy" runat="server" class="form-control" maxlength="100"/>
        </div>

        <div class="form-group">
            <label for="fuFigureImage">Figure Image:</label>
            <asp:FileUpload ID="fuFigureImage" runat="server" /><br />
        </div>

        <asp:Button ID="btnAddFigure" runat="server" Text="Add Figure" OnClientClick="return validateForm();" OnClick="btnAddFigure_Click" />


    </form>

    <!-- JavaScripts-->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src ="../../assets/js/Figures.js"></script>

</body>
</html>
