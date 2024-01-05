<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="Night_City.Pages.AdminPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard - Night City</title>
    <link rel="stylesheet" href="../Styles/styles.css" />
    <link rel="stylesheet" href="../Styles/cyberpunk-css-main/cyberpunk.css" />
    <!-- Add any other stylesheets or scripts you need for the admin page -->
</head>
<body>
    <form id="form1" runat="server">
        <div class="cyber-header">
            <h1 class="cyber-h1">Admin Dashboard</h1>
        </div>
        <div class="nav-buttons-container">
            <asp:LinkButton ID="btnDistricts" runat="server" OnClick="btnDistricts_Click" CssClass="cyber-button bg-red fg-white ">
                Districts
                <span class="glitchtext">Districts</span>
            </asp:LinkButton>
            <asp:LinkButton ID="btnFigures" runat="server" OnClick="btnFigures_Click" CssClass="cyber-button bg-red fg-white ">
                Figures
                <span class="glitchtext">Figures</span>
            </asp:LinkButton>
            <asp:LinkButton ID="btnUsers" runat="server" OnClick="btnUsers_Click" CssClass="cyber-button bg-red fg-white ">
                Users
                <span class="glitchtext">Users</span>
            </asp:LinkButton>
            <asp:LinkButton ID="btnEmployees" runat="server" OnClick="btnEmployees_Click" CssClass="cyber-button bg-red fg-white ">
                Employees
                <span class="glitchtext">Employees</span>
            </asp:LinkButton>
            <asp:LinkButton ID="btnLogout" runat="server" PostBackUrl="~/Pages/SignIn.aspx" CssClass="cyber-button bg-red fg-white ">
                Log Out
                <span class="glitchtext">Log Out</span>
            </asp:LinkButton>
        </div>
        <div id="contentArea" runat="server">
            <asp:Panel ID="DistrictPanel" runat="server">
                <asp:GridView ID="gvDistricts" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="DistrictId" HeaderText="District ID" />
                        <asp:BoundField DataField="DistrictName" HeaderText="District Name" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDistrict" runat="server" CommandArgument='<%# Eval("DistrictId") %>' OnClick="ViewDistrict">View</asp:LinkButton>
                                <asp:LinkButton ID="lnkEditDistrict" runat="server" CommandArgument='<%# Eval("DistrictId") %>' OnClick="EditDistrict">Edit</asp:LinkButton>
                                <asp:LinkButton ID="lnkDeleteDistrict" runat="server" CommandArgument='<%# Eval("DistrictId") %>' OnClientClick="return confirm('Are you sure you want to delete this district?');" OnClick="DeleteDistrict">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="UserPanel" runat="server">
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="UserId" HeaderText="User ID" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewUser" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClick="ViewUser">View</asp:LinkButton>
                                <asp:LinkButton ID="lnkEditUser" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClick="EditUser">Edit</asp:LinkButton>
                                <asp:LinkButton ID="lnkDeleteUser" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClientClick="return confirm('Are you sure you want to delete this user?');" OnClick="DeleteUser">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="EmployeePanel" runat="server">
                <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="UserId" HeaderText="Employee ID" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewEmployee" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClick="ViewUser">View</asp:LinkButton>
                                <asp:LinkButton ID="lnkEditEmployee" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClick="EditUser">Edit</asp:LinkButton>
                                <asp:LinkButton ID="lnkDeleteEmployee" runat="server" CommandArgument='<%# Eval("UserId") %>' OnClientClick="return confirm('Are you sure you want to delete this employee?');" OnClick="DeleteUser">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        
    </form>
</body>
</html>
