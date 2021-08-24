<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreArticlesAdd.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreArticlesAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew articles add</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmDocumentCourseList" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div>
            <telerik:RadTextBox runat="server" ID="txtLevel" Style="text-align: right;" Width="360px"
                CssClass="input" Visible="false">
            </telerik:RadTextBox>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuCrewArticel" runat="server" OnTabStripCommand="MenuCrewArticel_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="CrewArticelGeneral" runat="server" OnTabStripCommand="CrewArticelGeneral_TabStripCommand"></eluc:TabStrip>

            <div>
                <table cellpadding="1" cellspacing="10" width="50%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblupdateddate" runat="server" Text="Updated Date"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblnewarticleid" runat="server" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtupdateddate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblArticaltype" runat="server" Text="Update Done"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucArticalType" AppendDataBoundItems="true" QuickTypeCode="153"
                                QuickList='<%# PhoenixRegistersQuick.ListQuick(0, 2) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblvessellist" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="divflag" runat="server" style="overflow: auto;">
                                <telerik:RadListBox CheckBoxes="true" ID="chkvessel"   RepeatDirection="Vertical" RepeatColumns="2" runat="server" CssClass="input_mandatory">
                                </telerik:RadListBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Remarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" Width="300px" TextMode="MultiLine" ></telerik:RadTextBox>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
