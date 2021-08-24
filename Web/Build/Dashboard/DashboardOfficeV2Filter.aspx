<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2Filter.aspx.cs"
    Inherits="Dashboard_DashboardOfficeV2Filter" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlFleet">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlFleet"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlVeselType"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlVessel"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>               
            </AjaxSettings>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlOwner">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlOwner"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlFleet"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlVeselType"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlVessel"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>               
            </AjaxSettings>
             <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlVeselType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlOwner"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlFleet"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlVeselType"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlVessel"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>               
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <table align="center">
            <tr id="owner" runat="server">
                <td>
                    <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ddlOwner" AddressType="<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>"
                        Width="300px" AutoPostBack="true" OnTextChangedEvent="ddlOwner_TextChangedEvent" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                        EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        OnItemChecked="ddlFleet_ItemChecked" Width="300px">
                    </telerik:RadComboBox>                   
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVeselType" runat="server" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID" AutoPostBack="true"
                        EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        OnItemChecked="ddlVeselType_ItemChecked" Width="300px">
                    </telerik:RadComboBox>    
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                        EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                    </telerik:RadComboBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblGroupRank" runat="server" Text="Group Rank" Visible="false"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlGroupRank" runat="server" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID" Visible="false"
                        EmptyMessage="Type to select group rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                    </telerik:RadComboBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblzone" runat="server" Text="Zone" Visible="false" ></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlzone" runat="server" DataTextField="FLDZONE" DataValueField="FLDZONEID" Visible="false"
                        EmptyMessage="Type to select group Zone" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                    </telerik:RadComboBox>
                </td>
            </tr>
			<tr>
                <td>
                    <asp:Literal Text="View All" runat="server" ID="lblViewAll"></asp:Literal>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkViewAll" runat="server" OnCheckedChanged="chkViewAll_CheckedChanged" AutoPostBack="true">
                    </telerik:RadCheckBox>

                </td>
            </tr>
             
        </table>
    </form>
</body>
</html>
