<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRequestActionPlanComment.aspx.cs"
    Inherits="InspectionMOCRequestActionPlanComment" %>

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
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <eluc:Status ID="ucStatus" runat="server" Text="" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:TabStrip ID="MenuMOCPlanComment" runat="server" OnTabStripCommand="MenuMOCPlanComment_TabStripCommand">
    </eluc:TabStrip>
    <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all">
    </telerik:RadFormDecorator>
    <br />
    <table border="0" style="width: 100%">
        <tr>
            <td  width="20%">
                <telerik:RadLabel ID="lbldepartment" runat="server" Text="Department">
                </telerik:RadLabel>
            </td>
            <td  width="80%">
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
                    <telerik:RadTextBox ID="txtCrewNameEdit" runat="server" CssClass="input" Enabled="false"
                        MaxLength="50" Width="20%">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCrewRankEdit" runat="server" CssClass="input" Enabled="false"
                        MaxLength="50" Width="20%">
                    </telerik:RadTextBox>
                    <asp:LinkButton runat="server" ID="imgPersonInChargeEdit" Style="cursor: pointer;
                        vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                    </asp:LinkButton>
                    <telerik:RadTextBox ID="txtCrewIdEdit" runat="server" CssClass="hidden" MaxLength="20"
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
                    <telerik:RadTextBox ID="txtPersonNameEdit" runat="server" CssClass="input" 
                        MaxLength="50" Width="20%">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtPersonRankEdit" runat="server" CssClass="input" 
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
    </form>
</body>
</html>
