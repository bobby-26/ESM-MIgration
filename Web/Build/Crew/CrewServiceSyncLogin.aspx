<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewServiceSyncLogin.aspx.cs" Inherits="CrewServiceSyncLogin" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Login</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            body, html {
                width: 100%;
                height: 100%;
                margin: 0px;
                padding: 0px;
            }
        </style>
        <script type="text/javascript">
            function center() {
                var Element = document.getElementById("phoenixLogin");
                var rect = Element.getBoundingClientRect();
                w = rect.right - rect.left;
                h = rect.bottom - rect.top;
                var objh = parseFloat(h) / 2;
                var objw = parseFloat(w) / 2;
                Element.style.top = Math.floor(Math.round((document.documentElement.offsetHeight / 2) + document.documentElement.scrollTop) - objh) + 'px';
                Element.style.left = Math.floor(Math.round((document.documentElement.offsetWidth / 2) + document.documentElement.scrollLeft) - objw) + 'px';
            }
        </script>

        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>

    </telerik:RadCodeBlock>

</head>
<body onload="center();">
    <form id="frmCrewBatchList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MainTab" runat="server" Title="Phoenix Sync"></eluc:TabStrip>
        <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" Title="Service" runat="server" Width="350px" Height="250px" Modal="true" OffsetElementID="main"
            Style="z-index: 100001;">
            <ContentTemplate>
                <telerik:RadLabel ID="lblNote" runat="server" Font-Bold="true" Text="Please Select the Service to Sync"></telerik:RadLabel>
                <telerik:RadLabel ID="RadLabel1" runat="server" Text=""></telerik:RadLabel>
                <telerik:RadCheckBoxList ID="cblService" runat="server" RepeatDirection="Horizontal" Columns="2" AutoPostBack="false">
                    <Items>
                        <telerik:ButtonListItem Text="Employee" Value="EMPLOYEE" />
                        <telerik:ButtonListItem Text="Address" Value="EMPLOYEEADDRESS" />
                        <telerik:ButtonListItem Text="Experience" Value="EMPLOYEECOMPANYOTHEREXPERIENCE" />
                        <telerik:ButtonListItem Text="Travel Document" Value="TRAVELDOCUMENT" />
                        <telerik:ButtonListItem Text="Medical Test" Value="MEDICALTEST" />
                        <telerik:ButtonListItem Text="Medical Vaccination" Value="MEDICALVACCINATION" />
                        <telerik:ButtonListItem Text="Licence" Value="LICENCE" />
                        <telerik:ButtonListItem Text="Licence DCE" Value="LICENCEDCE" />
                        <telerik:ButtonListItem Text="Licence Flag" Value="LICENCEFLAG" />
                        <telerik:ButtonListItem Text="Course" Value="COURSE" />                    
                        <telerik:ButtonListItem Text="Academics" Value="ACADEMICS" />
                        <telerik:ButtonListItem Text="Awards & Certificate" Value="AWARDS" />
                        <telerik:ButtonListItem Text="Family" Value="FAMILYNOK" />
                        <telerik:ButtonListItem Text="Family Medical Test" Value="FAMILYMEDICALTEST" />
                        <telerik:ButtonListItem Text="Family Vaccination" Value="FAMILYMEDICALVACCINATION" />
                        <telerik:ButtonListItem Text="Family Travel Document" Value="FAMILYTRAVELDOCUMENT" />
                    </Items>
                </telerik:RadCheckBoxList>
                <telerik:RadButton ID="btnSync" runat="server" Text="Sync" OnClick="btnSync_Click"></telerik:RadButton>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellspacing="0" cellpadding="1" border="0" style="position: absolute; border-collapse: collapse;"
                id="phoenixLogin" class="Loginbox_background">
                <tr>
                    <td>
                        <table cellpadding="0" border="0" style="width: 325px;">
                            <tr>
                                <td align="center" class="login_text">
                                    <telerik:RadLabel ID="lblLogin" runat="server" Text="Log In"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="userinstruction_text">
                                    <telerik:RadLabel ID="lblService" runat="server" Text="Please enter your Service Login Credential."></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtUserName" runat="server" Width="240px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPassword" runat="server" Text="Password"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" Width="240px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <telerik:RadButton ID="btnSubmit" runat="server" Text="Log In" OnClick="btnSubmit_OnClick"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
        <eluc:Status ID="ucStatus" runat="server" />
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    center();
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
