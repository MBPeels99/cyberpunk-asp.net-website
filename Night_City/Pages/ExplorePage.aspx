<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExplorePage.aspx.cs" Inherits="Night_City.Pages.Profile" EnableEventValidation="false" %>
<!DOCTYPE html>
<html>
<head>
    <title>Welcome to Night City</title>
    <link rel="stylesheet" href="../assets/css/main.css" />
    <link rel="stylesheet" href="../assets/css/cyberpunk-css-main/cyberpunk.css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <header>
            <h1 class="glow" id="welcomeMessage" runat="server"></h1>
        </header>

        <div class="nav-buttons-container" runat="server" id="navButtonsContainer">
            <asp:LinkButton runat="server" CssClass="cyber-button bg-red fg-white cyber-button-inline" ID="updateAccount" OnClick="updateAccount_Click">
                Update Account
                <span class="glitchtext">Some edgy txt</span>
                <span class="tag">R25</span>
            </asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="cyber-button bg-red fg-white cyber-button-inline" ID="deleteAccount" OnClick="deleteAccount_Click">
                Delete Account
                <span class="glitchtext">Some edgy txt</span>
                <span class="tag">R25</span>
            </asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="cyber-button bg-red fg-white cyber-button-inline" ID="btnLogout" OnClick="btnLogOut_Click">
                Log Out
                <span class="glitchtext">Some edgy txt</span>
                <span class="tag">R25</span>
            </asp:LinkButton>
        </div>
        <div class="nav-buttons-container" runat="server" id="navButtonSignUp">
            <asp:LinkButton ID="btnSignInUP" runat="server" PostBackUrl="~/Pages/SignIn.aspx" CssClass="cyber-button bg-red fg-white">
                Sign In/Up
                <span class="glitchtext">Some edgy txt</span>
                <span class="tag">R25</span>
            </asp:LinkButton>
        </div>

        <br />
        <aside class="districts-container">
                        <div class="districts" id="districtsLeft" runat="server">
                            <asp:Repeater ID="rptDistrictsLeft" runat="server" OnItemDataBound="rptDistrictsLeft_ItemDataBound">
                                <ItemTemplate>
                                    <div class="district">
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%# Eval("ImageOne") %>' CommandArgument='<%# Eval("ID") %>' OnClick="ImageButton_Click" CssClass="district-image" />
                                        <div class="district-name"><%# Eval("Name") %></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="districts" id="districtsRight" runat="server">
                            <asp:Repeater ID="rptDistrictsRight" runat="server">
                                <ItemTemplate>
                                    <div class="district">
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl='<%# Eval("ImageOne") %>' CommandArgument='<%# Eval("ID") %>' OnClick="ImageButton_Click" CssClass="district-image" />
                                        <div class="district-name"><%# Eval("Name") %></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
        </aside>    
        <div id="districtDetails" class="district-details-container">
            <div id="contentArea" runat="server">
                <div class="content-districts">
                    <div id="selectedDistrict" runat="server" class="selectedDistrict">
                        <h1 id="selectedDistrictName" runat="server" class="selectedDistrictName"></h1>
                        <div class="selectedDistrictImageContainer">
                            <asp:Image ID="selectedDistrictImage" runat="server" CssClass="selectedDistrictImage" />
                            <div class="selectedDistrictDetails">
                                <div id="selectedDistrictDescription" runat="server" class="selectedDistrictDescription"></div>
                                <div class="selectedDistrictMapContainer">
                                    <asp:Image ID="selectedDistrictMap" runat="server" CssClass="selectedDistrictImageMap"/>
                                </div>
                            </div>
                        </div>
                        <div class="visit-district-form">
                            <div class="district-selection">
                                <h2>Select Your Visit Dates</h2>
                                <div class="date-selection">
                                    <label for="fromDate">From:</label>
                                    <input type="text" id="fromDate" class="date-input" placeholder="Select a date" readonly ClientIDMode="Static"/>
                                    <label for="toDate">To:</label>
                                    <input type="text" id="toDate" class="date-input" placeholder="Select a date" readonly ClientIDMode="Static"/>
                                    <button id="confirmDate" class="confirmDate-button">Confirm Date</button>
                                </div>
                            </div>
                        </div>
                    </div>                        
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" />
                </div>
            </div>
        </div>
    </form>
    <!-- Scripts -->
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="../assets/js/explore_page.js"></script>
    

</body>
</html>