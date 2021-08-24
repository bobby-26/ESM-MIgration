<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRequestActionPlanEdit.aspx.cs"
    Inherits="InspectionMOCRequestActionPlanEdit" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuMOCStatus" runat="server" OnTabStripCommand="MenuMOCStatus_TabStripCommand" Visible="false"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand">
        </eluc:TabStrip>
        <table id="tblDetails" runat="server" width="100%">
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lblActionTaken" runat="server" Text="Actions to be taken">
                    </telerik:RadLabel>
                </td>
                <td width="80%">
                    <telerik:RadTextBox ID="txtActionToBeTaken" runat="server"  Width="60%"
                        TextMode="MultiLine" Rows="4" Resize="Both">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldepartment" runat="server" Text="Department">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Department ID="ucDepartmentedit" runat="server" CssClass="gridinput" DepartmentList='<%#PhoenixRegistersDepartment.Listdepartment(1,null)%>'
                        Width="30%" AutoPostBack="true" AppendDataBoundItems="true" />
                    <telerik:RadLabel ID="lbldepartmentid" runat="server" Visible="false">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lbldept" runat="server" Visible="false">
                    </telerik:RadLabel>
                </td>
            </tr>
            <tr id="actionplancrewedit" runat="server">
                <td>
                    <telerik:RadLabel ID="lblPersonInChargeCrew" runat="server" Text="Person In Charge">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPersonInChargeactionplanEdit">
                        <telerik:RadTextBox ID="txtCrewNameEdit" runat="server"  Enabled="false"
                            MaxLength="50" Width="20%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCrewRankEdit" runat="server"  Enabled="false"
                            MaxLength="50" Width="20%">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInChargeEdit" Style="cursor: pointer;
                            vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtCrewIdEdit" runat="server"  MaxLength="20"
                            Width="0px">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr id="actionplanofficeedit" runat="server">
                <td>
                    <telerik:RadLabel ID="lblPlanPersonOffice" runat="server" Text="Person In Charge">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnActionPlanPersonOfficeEdit">
                        <telerik:RadTextBox ID="txtPersonNameEdit" runat="server"  Enabled="false"
                            MaxLength="50" Width="20%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPersonRankEdit" runat="server"  Enabled="false"
                            MaxLength="50" Width="20%">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonOfficeEdit" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtPersonOfficeIdEdit" runat="server" MaxLength="20" Width="0px"
                            Display="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPersonOfficeEmailEdit" runat="server" MaxLength="20" Width="0px"
                            Display="false">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target date">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtTargetdateEdit" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    <asp:Label ID="lblVesselId" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
