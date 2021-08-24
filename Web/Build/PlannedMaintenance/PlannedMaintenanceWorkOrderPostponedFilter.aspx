<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderPostponedFilter.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceWorkOrderPostponedFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentJob" Src="../UserControls/UserControlMultiColumnComponents.ascx" %>
<%@ Register TagPrefix="eluc" TagName="status" Src="~/UserControls/UserControlHard.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            document.onkeydown = function (e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    __doPostBack('MenuComponentFilter$dlstTabs$ctl00$btnMenu', '');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuComponentFilter" runat="server" OnTabStripCommand="MenuComponentFilter_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlDiscussion">
            <ContentTemplate>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtnumber" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Component"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:ComponentJob ID="txtComponent" runat="server" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlResponsibility" runat="server" DataTextField="FLDDISCIPLINENAME"
                                DataValueField="FLDDISCIPLINEID" DefaultMessage="Select discipline">
                            </telerik:RadDropDownList>

                        </td>
                    </tr>
                    <tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Class Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtclasscode" runat="server" Width="240px"></telerik:RadTextBox>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblComponentCategory" runat="server" Text="Component Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlQuick ID="ucComponentCategory" runat="server" QuickTypeCode="166" AppendDataBoundItems="true" Width="240px" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text="Work Order Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:status ID="ucstatus" runat="server" HardTypeCode="10" AppendDataBoundItems="true" Width="240px" ShortNameFilter="ISS,POP,IPG,RPP,CVR,SVP" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Postponement "></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblpostpone" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Approved" Value="1" />
                                        <telerik:ButtonListItem Text="Pending" Value="0" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>

                        </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
