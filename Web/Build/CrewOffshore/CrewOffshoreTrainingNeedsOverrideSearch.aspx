<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsOverrideSearch.aspx.cs" Inherits="CrewOffshoreTrainingNeedsOverrideSearch" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Needs</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Training Needs Search" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewQuery" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>    
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>                   
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name Contains"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFileNo" runat="server" Text="File No Contains"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileNo" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td>
                            <asp:Literal ID="lblCurrentRank" runat="server" Text="Current Rank"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                 
                  <%--  <tr>
                        <td>
                            <asp:Literal ID="lblShowArchived" runat="server" Text="Status" Visible="false"></asp:Literal>
                        </td>
                        <td>
                          <asp:DropDownList ID="ddlstatus" runat="server" >
                              <asp:ListItem Value="0">--SELECT--</asp:ListItem>
                               <asp:ListItem Value="PENDING">PENDING</asp:ListItem>
                               <asp:ListItem Value="OVERDUE">OVERDUE</asp:ListItem>
                               <asp:ListItem Value="BOTH">BOTH</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
