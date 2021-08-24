<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantInActive.aspx.cs" Inherits="CrewNewApplicantInActive" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew In-Active</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInActive" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInActive">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Status" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuInActive" runat="server" OnTabStripCommand="CrewInActive_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">                  
                    <tr>
                        <td>
                             <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                             <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeMiddleName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                             <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                              <asp:Literal ID="lblActiveInActive" runat="server" Text="Active / In-Active"></asp:Literal>
                           
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblInActive" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ActiveRemarks">
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="In-Active" Value="0" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>                       
                        <td>
                             <asp:Literal ID="lblReason" runat="server" Text="Reason"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:Hard ID="ddlInactiveReason" runat="server" AppendDataBoundItems="true"
                                CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlReason_TextChanged" />
                        </td>
                         <td>
                            <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                        </td>
                        <td>
                            <eluc:Date ID="txtInActiveDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                       
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInActiveRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                MaxLength="800" Width="300px"></asp:TextBox>                            
                        </td>
                    </tr>                   
                </table>
                <eluc:Status ID="ucStatus" runat="server" />             
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
