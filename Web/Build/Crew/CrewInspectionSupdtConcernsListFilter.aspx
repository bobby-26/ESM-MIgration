<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInspectionSupdtConcernsListFilter.aspx.cs"
    Inherits="Crew_CrewInspectionSupdtConcernsListFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList"
    TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVesselCommon.ascx" TagName="UserControlVessel" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList"
    TagPrefix="eluc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filters</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupdtConcernsList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:TabStrip ID="MainMenuSupdtConcernsList" runat="server" OnTabStripCommand="MainMenuSupdtConcernsList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">




                <div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td colspan="4">
                                <font color="blue"><b>Note: </b>For embeded search, use '%' symbol. (Eg. Name: %xxxx)</font>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="250px" MaxLength="200"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFileNumber" Text="File Number" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFileNumber" runat="server" CssClass="input" Width="250px" MaxLength="10"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblPresentRank" Text="Present Rank" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlRankList ID="lstRank" runat="server" AppendDataBoundItems="true"
                                    Width="250px" />
                                <br />
                                <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVesselSailed" Text="Vessel" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                                    VesselsOnly="true" Width="250px" Entitytype="VSL" AssignedVessels="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlVesselTypeList ID="ddlVesselType" runat="server" AppendDataBoundItems="true"
                                    Width="250px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFeedbackCategory" runat="server">Feedback Category</telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFeedbackCategory" runat="server" Filter="Contains" EmptyMessage="Type to select feedback category" MarkFirstMatch="true"
                                    Width="250px">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFeedbackSubCategory" runat="server">SubCategory</telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFeedbackSubCategory" runat="server" Width="250px" Filter="Contains" EmptyMessage="Type to select subcategory" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEvent" runat="server" Text="Event"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlEvent" runat="server" Width="250px" Filter="Contains" EmptyMessage="Type to select event" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Event Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucRecordedDate" runat="server" />
                            </td>
                            <td colspan="2">
                                <asp:Panel ID="pnlPeriod" runat="server" GroupingText="Period" Width="380px">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucFromDate" runat="server" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucToDate" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
