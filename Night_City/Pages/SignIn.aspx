<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Night_City.Pages.PlanTrip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plan Trip to Night City</title>
    <link rel="stylesheet" href="../Styles/styles.css" />
    <link rel="stylesheet" href="../Styles/cyberpunk-css-main/cyberpunk.css" />
</head>
<body>
     <div class="cyber-background">
        <form id="form1" runat="server">
            <header>
                <h1 class="glow">Plan Your Trip to Night City</h1>
            </header>
            <main>
                <div class="login-intro">
                    <p>Join us in the digital frontier of Night City. Sign up to discover the endless possibilities that await you in this metropolis of dreams and neon. Your adventure begins with a click.</p>
                </div>
                <h2>Account Login</h2>
                <p>Do you have an account with us?</p>
                <asp:LinkButton ID="btnSignIn" runat="server" OnClick="btnSignIn_Click" CssClass="cyber-button bg-red fg-white cyber-button-inline">
                    Sign In
                    <span class="glitchtext">Some edgy txt</span>
                    <span class="tag">R25</span>
                </asp:LinkButton>

                <asp:LinkButton ID="btnSignUp" runat="server" OnClick="btnSignUp_Click" CssClass="cyber-button bg-red fg-white cyber-button-inline">
                    Sign Up
                    <span class="glitchtext">Some edgy txt</span>
                    <span class="tag">R25</span>
                </asp:LinkButton>

                <asp:LinkButton ID="btnHomePage" runat="server" PostBackUrl="~/Pages/Default.aspx" CssClass="cyber-button bg-red fg-white cyber-button-inline">
                    Return to Home Page
                    <span class="glitchtext">Some edgy txt</span>
                    <span class="tag">R25</span>
                </asp:LinkButton>

            
                <div id="divSignIn" runat="server" visible="false">
                    <h2>Sign In</h2>
                    <div class="form-group">
                        <label for="txtEmailSignIn">Email:</label>
                        <input type="text" id="txtEmailSignIn" runat="server" class="form-control" />
                    </div>

                    <div class="form-group">
                        <label for="txtPassword">Password:</label>
                        <input type="password" id="txtPassword" runat="server" class="form-control" />
                    </div>

                    <asp:LinkButton ID="btnConfirmSignIn" runat="server" OnClick="btnConfirmSignIn_Click" CssClass="cyber-button bg-red fg-white" >
                        Sign In
                        <span class="glitchtext">Some edgy txt</span>
                        <span class="tag">R25</span>
                    </asp:LinkButton>
                </div>
            
                <div id="divSignUp" runat="server" visible="false">
                    <h2>Sign Up</h2>
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
                            <asp:DropDownList ID="ddlCountry" runat="server" class="form-control" >
                                <asp:ListItem Value="USA" Text="USA"></asp:ListItem>
                                <asp:ListItem Value="Canada" Text="Canada"></asp:ListItem>
                                <asp:ListItem Value="Netherlands" Text="Netherlands"></asp:ListItem>
                                <asp:ListItem Value="South Africa" Text="South Africa"></asp:ListItem>
                        
                            </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtDateOfBirth">Date of Birth:</label>
                        <input type="date" id="txtDateOfBirth" runat="server" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtPassword">Password:</label>
                        <input type="Password" id="txtPasswordSignUp" runat="server" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtConfirmPassword">Confirm Password:</label>
                        <input type="password" id="txtConfirmPassword" runat="server" class="form-control" />
                    </div>
                    <asp:Button ID="btnConfirmSignUp" runat="server" Text="Sign Up" OnClick="btnConfirmSignUp_Click" CssClass="button" />
                </div>
                    <br>
                    <asp:Label ID="lblError" runat="server" Text="" Visible="true" CssClass="yellow-text"></asp:Label>
            
            </main>
        </form>
         <div class="ads-left">
            <!-- Animated GIF ads or styled divs go here -->
        </div>
        <div class="ads-right">
            <!-- Animated GIF ads or styled divs go here -->
        </div>
        <div class="ads-bottom">
            <!-- Animated GIF ads or styled divs go here -->
        </div>
    </div>
</body>
</html>
