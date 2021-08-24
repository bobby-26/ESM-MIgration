<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkCalenderGeneral.aspx.cs"
    Inherits="VesselAccountsRHWorkCalenderGeneral" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Day</title>
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td colspan="4">
                    <table cellpadding="1" cellspacing="1" width="100%" class="input" style="border-style: none; color: blue;">
                        <tr>
                            <td>
                                <b>
                                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Notes :" CssClass="input"
                                        BorderStyle="None" ForeColor="Blue"></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lbl1" runat="server" Text="1."></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblSelecteitherInPortAtSeaorSeaPort" runat="server" Text="Select either In Port,At Sea or Sea/Port"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lbl2" runat="server" Text="2."></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblIfAdvancingorRetardingClocksonthisdateselectAdvanceorRetardandcheck05h10h20htoAdvanceorRetard" runat="server" Text="If Advancing or Retarding Clocks on this date, select Advance or Retard and check
                    0.5h,1.0h,2.0h to Advance or Retard."></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lbl3" runat="server" Text="3."></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblIfCrossingInternationalDateLineonthisdateselectIDLWEorIDLEW" runat="server" Text="If Crossing International Date Line on this date, select IDL W-E or IDL E-W"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lbl4" runat="server" Text="4."></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblSave" runat="server" Text="Save."></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtStartDate" runat="server" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <asp:Literal ID="lblHours" runat="server" Text="Hours"></asp:Literal>
                </td>
                <td>
                    <eluc:Number ID="txthours" runat="server" IsInteger="true" IsPositive="true" CssClass="input txtNumber"
                        Enabled="false" />
                    &nbsp;<asp:Label ID="lblNote" runat="server" Text="(Min - 22;Max - 26)" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblPreviousDate" runat="server" Text="Previous Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtPreviousDate" runat="server" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="lblNextDateCaption" runat="server" Text="Next Date"></asp:Label>
                </td>
                <td>
                    <eluc:Date ID="txtNextDate" runat="server" CssClass="input" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbtnadvanceretard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnadvanceretard_OnSelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="IDL W-E" Value="1"></asp:ListItem>
                        <asp:ListItem Text="IDL E-W" Value="2"></asp:ListItem>
                        <asp:ListItem Text="None" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbtnworkplace" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="In Port" Value="1"></asp:ListItem>
                        <asp:ListItem Text="At Sea" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Sea/Port" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbnhourchange" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbnhourchange_OnSelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Advance" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Retard" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Reset" Value="0" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbnhourvalue" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbnhourchange_OnSelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="0.5h" Value="1"></asp:ListItem>
                        <asp:ListItem Text="1.0h" Value="2"></asp:ListItem>
                        <asp:ListItem Text="2.0h" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <b>
                        <asp:Literal ID="lblWorkHour" runat="server" Text="Work Hour"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataList ID="dlstTimeList" runat="server" RepeatDirection="Horizontal" Width="35px"
                        Height="45px" BorderColor="ControlLight" GridLines="Both" RepeatLayout="Table"
                        BorderWidth="1px" OnItemDataBound="dlstTimeList_ItemDataBound">
                        <ItemTemplate>
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                            <asp:Button ID="btnhour" runat="server" Height="35px" ForeColor="Blue" CssClass="input"
                                BorderStyle="Inset" BorderColor="ControlLight" Width="35px" Text='<%# DataBinder.Eval(Container,"DataItem.Text") %>'
                                BackColor="LightGray" />
                            <center>
                                <asp:Label ID="lblsno" runat="server" CssClass="input" BorderStyle="None" Text='<%# DataBinder.Eval(Container,"DataItem.Hour") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
