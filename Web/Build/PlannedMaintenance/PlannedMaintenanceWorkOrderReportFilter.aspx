<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportFilter.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
   <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkOrderReportFilter" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
     <asp:UpdatePanel runat="server" ID="pnlFilter">
        <ContentTemplate>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="div2" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Report Work Filter" ShowMenu="True">
            </eluc:Title>
        </div>
    </div>
     <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuWorkOrderReportFilter" runat="server" OnTabStripCommand="MenuWorkOrderReportFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>  
   
   
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" MaxLength="12"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComponentNumber" runat="server" CssClass="input" MaxLength="9"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtComponentNumber"
                            Mask="999.99.99" MaskType="Number" InputDirection="LeftToRight">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Literal ID="lblComponentName" runat="server" Text="Component Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblResponsibleDiscipline" runat="server" Text="Responsible Discipline"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDueBetweenDate" runat="server" Text="Due Between Date"></asp:literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="input" Width="60px"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateFrom" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        -
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="input" Width="60px"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateTo" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Literal ID="lblJobClasses" runat="server" Text="Job Classes"></asp:Literal>
                    </td>
                    <td>
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; width: 70%;
                            height: 100px">
                            <asp:CheckBoxList ID="chkClasses" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <asp:Literal ID="lblPlanning" runat="server" Text="Planning"></asp:Literal>
                    </td>
                    <td valign="top">
                        <div runat="server" id="dvPlaning" class="input" style="overflow: auto; width: 70%;
                            height: 100px">
                            <asp:CheckBoxList ID="ckPlaning" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblMaintenanceCause" runat="server" Text="Maintenance Cause"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkUnexpected" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                    </td>
                    <td>
                        <div runat="server" id="dvStatus" class="input" style="overflow: auto; width: 70%;
                            height: 100px">
                            <asp:CheckBoxList ID="chkStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                    </td>
                    <td valign="top">
                        <eluc:Decimal runat="server" ID="txtPriority" Mask="9" CssClass="input" Width="60px" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
