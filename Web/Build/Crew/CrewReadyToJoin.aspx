<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReadyToJoin.aspx.cs"
    Inherits="CrewReadyToJoin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="../UserControls/UserControlVessel.ascx" TagName="UserControlVessel"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlPool.ascx" TagName="UserControlPool"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="UserControlRank"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlSeaport.ascx" TagName="UserControlSeaport"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlDate.ascx" TagName="UserControlDate"
    TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Ready To Join</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReadyToJoin" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="MainDiv" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="titleCrewJoin" Text="Crew Ready To Join" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewJoinTab" runat="server" OnTabStripCommand="CrewJoinTab_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <br />
        <table width="90%">
            <tr>
                <td>
                    <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEmployeeNumber" runat="server" Text="Employee Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblPresentRank" runat="server" Text="Present Rank"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                 <td>
                    <asp:Literal ID="lblLastVessel" runat="server" Text="Last Vessel"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtLastVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">            
            <tr>
                <td>
                    <asp:Literal ID="lblSignOffDate" runat="server" Text="Sign Off Date"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtSingOffDate" runat="server" CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblAvailabilityDate" runat="server" Text="Availability Date"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtDOA" runat="server" CssClass="input" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblLastActivity" runat="server" Text="Last Activity"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtLastActivity" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtFromDate" runat="server" CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtTodate" runat="server" CssClass="input" />
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblJoiningVessel" runat="server" Text="Joining Vessel"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
                <td>
                    <asp:Literal ID="lblContract" runat="server" Text="Contract"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlContract" runat="server" CssClass="input">
                        <asp:ListItem Selected="True" Value="Dummy">--Select--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlPool ID="ddlPool" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblJoiningDate" runat="server" Text="Joining Date"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtJoiningDate" runat="server" CssClass="input_mandatory" />
                </td>
                <td>
                    <asp:Literal ID="lblJoiningRank" runat="server" Text="Joining Rank"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlRank ID="ddlJoiningRank" runat="server" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
                <td>
                    <asp:Literal ID="lblJoiningPort" runat="server" Text="Joining Port"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlSeaport ID="ddlJoiningPort" runat="server" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
