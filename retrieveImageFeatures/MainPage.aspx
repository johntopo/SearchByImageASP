<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="retrieveImageFeatures.MainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css_js/bootstrap/Content/bootstrap.css" rel="stylesheet"/>
    <link href="css_js/MyStyle.css" rel="stylesheet"/>
    <script src="css_js/bootstrap/Scripts/jquery-1.9.1.js"></script>


</head>
<body>
<div class="container-fluid">
    <div class="jumbotron" style="border-radius: 25px; text-align: center; border: 2px black; box-shadow: 0px 2px 5px #ccc; align-content: center; align-items: center">
        <h1>
            Search by Image
        </h1>
    </div>
    <form id="form" runat="server">

        <div class="row">
            <div class="col-lg-4 text-center">
                <div class="fuLabel">Find Similar Images</div>
                <asp:fileupload ID="fuSearch" AllowMultiple="False" runat="server" onchange="this.form.submit()" CssClass="center-block fu" ClientIDMode="Static"/>
                <asp:Image ID="imageUploadedByUser" runat="server" Height="74px"/>
            </div>


            <div class="col-lg-4 text-center">
                <asp:DropDownList ID="dplNumOfImagesToReturn" runat="server" CssClass="col-lg-4">
                    <asp:ListItem Enabled="true" Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-4 text-center">
                <div class="fuLabel">Insert Images In Database</div>
                <asp:FileUpload ID="fuUploadImagesToDb" AllowMultiple="True" runat="server" onchange="this.form.submit()" CssClass="center-block fu"/>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="buttonResearch" runat="server" OnClick="buttonResearch_Click" Text="Research" Visible="False" CssClass="center-block fu"/>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-offset-2 col-xm-8 text-center">
                <asp:placeholder ID="placeholderResearchImages" runat="server"/>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-center">
                <h1>Result</h1>
            </div>
        </div>
        <div class="row">
            <asp:CheckBoxList ID="imageCheckBoxList" runat="server" CssClass="col-sm-12" RepeatDirection="Horizontal" RepeatLayout="Flow"/>
        </div>


    </form>
</div>


</body>


<script src="css_js/MyScript.js"></script>
</html>