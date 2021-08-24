<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreArticlesSearch.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreArticlesSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="CrewQuery" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <table width="50%" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromdate" runat="server" Text=" Updated From "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromdate" runat="server"  DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text=" To "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTodate" runat="server"  DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblArticaltype" runat="server" Text="Update Done For "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucArticalType" AppendDataBoundItems="true" QuickTypeCode="153"
                            QuickList='<%# PhoenixRegistersQuick.ListQuick(0, 2) %>' Width="150PX" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server"  AppendDataBoundItems="true" Width="150PX"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblinactive" runat="server" Text="Include Archived"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkInactive" runat="server" />
                    </td>
                </tr>
            </table>
      
    </form>
</body>
</html>
