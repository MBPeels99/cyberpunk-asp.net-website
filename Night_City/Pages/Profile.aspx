<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Night_City.Pages.Profile" EnableEventValidation="false" %>
<!DOCTYPE html>
<html>
<head>
    <title>Welcome to Night City</title>
    <link rel="stylesheet" href="../Styles/styles.css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1 class="glow" id="welcomeMessage" runat="server"></h1>
        </header>

        <div class="nav-buttons-container">
            <asp:Button runat="server" CssClass="nav-button" ID="updateAccount" OnClick="updateAccount_Click" Text="Update Account" />
            <asp:Button runat="server" CssClass="nav-button" ID="deleteAccount" OnClick="deleteAccount_Click" Text="Delete Account" />
            <asp:Button runat="server" CssClass="nav-button" ID="btnLogout" OnClick="btnLogOut_Click" Text="Log Out" />
        </div>
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
                                    <input type="text" id="fromDate" class="date-input" placeholder="Select a date" readonly />
                                    <label for="toDate">To:</label>
                                    <input type="text" id="toDate" class="date-input" placeholder="Select a date" readonly />
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
<script type="text/javascript">
        $(document).ready(function () {
            $("#fromDate").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#toDate").datepicker({ dateFormat: 'yy-mm-dd' });

            $('#confirmDate').click(function (event) {
                event.preventDefault();
                var from = $('#fromDate').val();
                var to = $('#toDate').val();

                if (from && to) { // Making sure both dates are selected
                    $.ajax({
                        type: "POST",
                        url: "Profile.aspx/ConfirmDates", // Make sure this matches your [WebMethod] in code-behind
                        data: JSON.stringify({ startDate: from, endDate: to }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d === "Success") {
                                // If successful, redirect to the ConfirmationPage.aspx
                                window.location.href = "ConfirmationPage.aspx";
                            } else {
                                // If the method returns any message other than "Success", alert the message
                                alert(response.d);
                            }
                        },
                        error: function (error) {
                            alert("Error: " + error.responseText);
                            // Handle errors
                        }
                    });
                } else {
                    alert("Please select both 'From' and 'To' dates.");
                }
            });
        });

    $(document).ready(function () {
        $('.district-image').click(function () {
            $('html, body').animate({
                scrollTop: $("#districtDetails").offset().top
            }, 1000);
        });
    });

</script>

</body>
</html>