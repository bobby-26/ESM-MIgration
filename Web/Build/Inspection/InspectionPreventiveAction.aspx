<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPreventiveAction.aspx.cs"
    Inherits="InspectionPreventiveAction" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubDepartment" Src="~/UserControls/UserControlSubDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </telerik:RadCodeBlock>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            function setScrollPosition(divname, hdnname) {
                var div = $get(divname);
                var hdn = $get(hdnname);
                hdn.value = div.scrollTop;
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }
            function EndRequestHandler(sender, args) {

                listBox = $get('<%= divVesselType.ClientID %>');
                hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divVessel.ClientID %>');
                hdn = $get('<%= hdnVesselScroll.ClientID %>');
                listBox.scrollTop = hdn.value;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand"></eluc:TabStrip>
            <table id="tblDetails" runat="server" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblControlActionNeeds" runat="server" Text="Control Action Needs"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCAN" runat="server" Height="50px" Rows="4" TextMode="MultiLine"
                            Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPreventiveAction" runat="server" Text="Preventive Action"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtPreventiveAction" runat="server" CssClass="input_mandatory" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTaskCategory" runat="server" Text="Task Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true"
                            Width="300px" OnSelectedIndexChanged="ddlCategory_Changed" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTaskSubCategory" runat="server" Text="Task Subcategory"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" id="tdddlSubcategory" runat="server">
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" Width="300px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td width="35%" id="tdspnPickListDocument" runat="server">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="300px"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowDocuments" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucTargetDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                            COMMANDNAME="TARGETDATE" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDepartmentAssignedTo" runat="server" Text="Department ( Assigned to )"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" 
                            Width="300px" AutoPostBack="true" OnTextChangedEvent="ucDept_TextChangedEvent" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSubDepartment" runat="server" Text="Sub Department"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:SubDepartment ID="ucSubDept" runat="server" AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblNote" runat="server" ForeColor="Blue" Font-Bold="true" Text="Note : If task category is 'Follow up with vessels', Please select vessels to distribute the task."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="divVesselType" runat="server" class="input" onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');"
                            style="overflow: auto; height: 200px;">
                            <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                            &nbsp;<telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkVesselType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed"
                                Direction="Vertical" Columns="2">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td colspan="2">
                        <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                            style="overflow: auto; height: 200px">
                            <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                            <telerik:RadCheckBoxList ID="chkVessel" runat="server" Columns="2" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
