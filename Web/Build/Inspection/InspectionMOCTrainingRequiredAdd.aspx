<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTrainingRequiredAdd.aspx.cs"
    Inherits="InspectionMOCTrainingRequiredAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tbltrainingadd"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%" EnableAJAX="false">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuMOC" runat="server" OnTabStripCommand="MOC_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
        <table id="tbltrainingadd" runat="server">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblShipOrShoreid" runat="server" Text="Ship/Shore">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ddltraining" runat="server" Width="100px"
                        OnSelectedIndexChanged="ddltraining_SelectedIndedChanged" AutoPostBack="true"
                        DropDownHeight="80px">
                        <Items>
                            <telerik:DropDownListItem Text="Ship Staff" Value="1"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Text="Shore Staff" Value="2"></telerik:DropDownListItem>
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPIC" runat="server" Text="Person In Charge">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPersonInCharge" runat="server">
                        <asp:TextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="50%"></asp:TextBox>
                        <asp:TextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="20%"></asp:TextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInCharge"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:TextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                    </span><span id="spnPersonInChargeOffice" runat="server">
                        <asp:TextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="50%"></asp:TextBox>
                        <asp:TextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="23%"></asp:TextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInChargeOffice"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:TextBox runat="server" ID="txtPersonInChargeOfficeId" CssClass="input" Width="0px"
                            MaxLength="20"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtPersonInChargeOfficeEmail" CssClass="input" Width="0px"
                            MaxLength="20"></asp:TextBox>
                    </span>
                    <asp:Label ID="lblDtkey" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTrainingRequired" runat="server" Text="Training Required">
                    </telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtTrainingRequired" runat="server" Height="50px" Rows="4"
                        Resize="Both" CssClass="input_mandatory" TextMode="MultiLine" Width="95%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadLabel ID="lblDepartmentTrained" runat="server" Text="Dept./Persons to be trained">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDepartmentAdd" runat="server" Height="50px" Rows="2" Resize="Both"
                        CssClass="input_mandatory" TextMode="MultiLine" Width="95%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtTargetdateAdd" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
