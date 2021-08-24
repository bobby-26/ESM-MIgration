<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewListFilter.aspx.cs" Inherits="CrewListFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewListFilter" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewListFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="CrewListFilterMain" runat="server" OnTabStripCommand="CrewListFilterMain_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 20%;">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%;">

                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true"
                            AssignedVessels="true" Entitytype="VSL" ActiveVesselsOnly="true" Width="180px" />
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadLabel ID="lblOnDate" runat="server" Text="On Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%;">
                        <eluc:Date ID="txtOnDate" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSailOnly" runat="server" Text="Sail Only"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadCheckBox ID="chkSailOnly" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="lstRank" runat="server" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="lstNationality" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeafarersabovetheselectedscalewouldbehighlighted" runat="server" Text="Seafarers above the selected scale would be highlighted"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadRadioButtonList ID="rblExtraCrew" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Actual" Value="0" Selected="true" />
                                <telerik:ButtonListItem Text="Safe Scale" Value="1" />
                                <telerik:ButtonListItem Text="Owner Scale" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShowRankExp" runat="server" Text="Show Rank Exp"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadCheckBox ID="chkRankExp" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
