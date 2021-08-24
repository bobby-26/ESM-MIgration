<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSearch.aspx.cs" Inherits="DashboardSearch" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Dashboard Search</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            Sys.Application.add_load(function () {
                resize();
            });
        </script>
        <script type="text/javascript">

            function txtSearch_KeyPressed() {
                var obj = document.getElementById("ifMoreInfo");
                obj.src = "../Dashboard/DashboardCrewListSearch.aspx?qSearch=" + document.getElementById("<%=txtCrewName.ClientID%>").value + "&Fileno=" + document.getElementById("<%=txtFileNo.ClientID%>").value;

              }
        </script>


    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 120 + "px";
        }
    </script>

</head>
<body onload="resize();" onresize="resize();">
    <form id="form1" runat="server">
        <%-- <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="uid" runat="server">
            <ContentTemplate>
                <%--onkeypress="javascript:RefreshIt(this);"--%>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuOptionChooseVessel" runat="server" OnTabStripCommand="OptionChooseVessel_TabStripCommand" Title="Crew Search"></eluc:TabStrip>
                <table cellpadding="8">
                    <tr>
                        <td colspan="4">
                            <telerik:RadLabel ID="lblTosearchkeyinpartofthecrewnameandtabouttoviewthefilteredlist" runat="server" Text="To search, key in part of the crew name and tab out to view the filtered list."></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCrewName" runat="server" MaxLength="200" CssClass="input" onkeyup="txtSearch_KeyPressed()">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" MaxLength="200" CssClass="input" OnTextChanged="txtSearch1_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <iframe id="ifMoreInfo" runat="server" width="99.5%" height="80%" src="DashboardCrewListSearch.aspx"></iframe>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
