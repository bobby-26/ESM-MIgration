<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOcimfLoginAccount.aspx.cs" Inherits="CrewOcimfLoginAccount" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ocimf Login</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
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

</head>
<body onload="center();">
    <form id="frmCrewBatchList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:TabStrip ID="OcimfLogin" runat="server" OnTabStripCommand="OcimfLoginTabs_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellspacing="0" cellpadding="1" border="0" style="position: absolute; border-collapse: collapse;"
                id="phoenixLogin" class="Loginbox_background">
                <tr>
                    <td>
                        <table cellpadding="0" border="0" style="width: 325px;">
                            <tr>
                                <td align="center" class="login_text" >
                                    <telerik:RadLabel ID="lblLogin" runat="server" Text="Log In"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="userinstruction_text">
                                    <telerik:RadLabel ID="lblhelptext" runat="server" Text="Please Select the Account and Log In"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAccountID" runat="server" Text="Account ID"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="ddlAccount" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                        OnDataBound="ddlAccount_DataBound" DataTextField="FLDACCOUNTID" OnTextChanged="ddlAccount_TextChanged"
                                        DataValueField="FLDOCIMFID" Width="240px"
                                        EnableLoadOnDemand="True" EmptyMessage="Type to select" >
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                           <tr>
                                <td>
                                    <telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtUserName" runat="server" Width="240px" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPassword" runat="server" Text="Password"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtPassword" runat="server" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="login_button" Text="Log In" OnClick="btnSubmit_OnClick" />

                                    <%--<telerik:RadLinkButton ID="btnSubmit" runat="server" CssClass="login_button" Text="Log In" OnClick="btnSubmit_OnClick"></telerik:RadLinkButton>--%>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
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
