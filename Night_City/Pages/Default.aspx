<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Night_City.Pages.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Night City</title>
    <link rel="stylesheet" href="../Styles/styles.css" />    
    <link rel="stylesheet" href="../Styles/cyberpunk-css-main/cyberpunk.css" />
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1 class="glow" >Night City</h1>
            <h2 style=" position:absolute; right:0px; width:auto; margin:auto;  border: 3px solid cyan; padding: 5px;">
                Today's Death Count: 
                <asp:Label ID="lblDeathCount" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
            </h2>
        </header>
        <main>
           <div class="content">
                <div class="content-text">
                    <asp:Image ID="imgNightCityLogo" runat="server" Imageurl="~/Pictures/NC_Logo_Black_BG.PNG"/>
                    <p>Whether it’s your first time in Night City or you’ve lived here since the Kress administration,
                        this site has you covered. Get out and explore with our curated selection of the city’s top highlights.
                        Impress your friends with nuggets of trivia from our historical overviews of the city and its districts.
                        Navigate NCART like a pro with our TL;DR overview. Hit only the hip parts of Heywood and dodge the gangs
                        with safety tips straight from the NCPD. Whatever your Night City dream is, with this site to guide you,
                        it’s sure to come true.</p>
                </div>
                <div class="content-video">
                    <div class="aspect-ratio">
                        <iframe src="https://www.youtube.com/embed/gt943-HQzGc" title="Good morning, Night City! - Cyberpunk 2077 Intro" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
                    </div>
                </div>
            <div>
                <h2>History of Night City</h2>
                <p>Night City is a vibrant and sprawling metropolis located on the west coast of the United States in the state of California. It was founded as a small coastal town and rapidly transformed into a hub of technological advancement and cultural diversity. Throughout its history, Night City has been shaped by powerful mega-corporations, organized crime syndicates, and social inequality.</p>
                <p>Night City experienced a major technological boom, leading to the rise of cybernetic augmentations, artificial intelligence, and a hyper-connected network. The city is a beacon of opportunity and excess, attracting individuals from all walks of life.</p>
            </div>
            <div>
                <h2>Exploring Night City</h2>
                <p>Click the button below to explore our wonderful city:</p>
                <asp:LinkButton ID="btnLoginPage" runat="server" PostBackUrl="~/Pages/SignIn.aspx" CssClass="cyber-button bg-red fg-white">
                    Start Exploring
                    <span class="glitchtext">Some edgy txt</span>
                    <span class="tag">R25</span>
                </asp:LinkButton>
            </div>
        </div>
        </main>
    </form>
</body>
</html>
