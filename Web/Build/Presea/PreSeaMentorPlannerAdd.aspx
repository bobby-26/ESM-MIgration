<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaMentorPlannerAdd.aspx.cs" Inherits="Presea_PreSeaMentorPlannerAdd" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mentor Planner Add</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlMentorPlanner">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader">
                        <eluc:Title runat="server" ID="titlepreseaMentorAdd" Text="Mentor Plan" ShowMenu="false" TabStrip="true"></eluc:Title>
                        <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                    </div>
                    <%--<div class="subHeader" style="position: relative;" runat="server">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="MenuTrainingSchedule" runat="server" OnTabStripCommand="MenuTrainingSchedule_TabStripCommand"></eluc:TabStrip>
                        </span>
                    </div>--%>
                    <br />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblStaff" runat="server" Text="Staff"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFaculty" runat="server" AppendDataBoundItems="true" 
                                    CssClass="input" Width="120px">
                                    <asp:ListItem Value="DUMMY" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <%--   <span id="spnFacultyAdd">
                                    <asp:TextBox ID="txtFaculty" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="120px"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgFaculty" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClick="imgFaculty_Click" />
                                    <asp:TextBox ID="txtFacultyDesignation" runat="server" CssClass="input" Width="0px"
                                        Enabled="false" MaxLength="50"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyId" CssClass="input" Width="0px" MaxLength="20"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAFFID") %>'></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyEmail" CssClass="input" Width="0px"
                                        MaxLength="20"></asp:TextBox>
                                </span>--%>
                                <asp:TextBox ID="txtmentorPlanId" runat="server"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Literal ID="lblStrength" runat="server" Text="Strength"></asp:Literal>
                            </td>
                            <td>
                                 <eluc:Number ID="txtStrength" runat="server" Width="120px" IsInteger="true" CssClass="input" />                               
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" Width="120px"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblClassRoom" runat="server" Text="Class Room"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlClassRoomAdd" runat="server" CssClass="input" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblsession" runat="server" Text="Session"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsession" runat="server" CssClass="input_mandatory">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1st Session" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2st Session" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine" Rows="3" Columns="25"></asp:TextBox>
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
