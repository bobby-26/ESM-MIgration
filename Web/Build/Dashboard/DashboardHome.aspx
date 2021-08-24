<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardHome.aspx.cs" Inherits="DashboardHome" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <%--  <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />--%>
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <link rel="stylesheet" type="text/css" href="fonts/fontawesome/css/all.min.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <style>
            .white {
                color: #fffff7 !important;
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div class="gray-bg">
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
                <table style="border-spacing: 50px; border-style: hidden;" border="0" align="center"
                    cellpadding="5" cellspacing="10" width="60%">
                    <tr>
                        <td style="width: 15%">
                            <div class="panel panel-success" style="height: 180px; width: 98%;">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Personal" Font-Bold="true" CssClass="white"></telerik:RadLabel>

                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="LinkButton1" ToolTip="Edit">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf500;</i></span>
                                    </asp:LinkButton>
                                    <%--  <asp:ImageButton runat="server" AlternateText="personal" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="Personal" ID="ImageButton1" ToolTip="Personal Master" Width="150px" Height="120px"></asp:ImageButton>--%>
                                </div>
                            </div>
                        </td>
                        <td style="width: 15%">

                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="Plan" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="cmdplan" ToolTip="Plan" OnClick="cmdplan_Click1">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fa fwm fa-7x whc">&#xf21a;</i></span>
                                    </asp:LinkButton>
                                    <%-- <asp:ImageButton runat="server" AlternateText="Plan" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="Plan" ID="cmdplan" ToolTip="Plan" Width="180px" Height="120px" OnClick="cmdplan_Click"></asp:ImageButton>--%>
                                </div>
                            </div>
                        </td>

                        <td style="width: 15%">
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="Profile" Font-Bold="true" CssClass="white"></telerik:RadLabel>

                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="cmdProfile" ToolTip="Profile" OnClick="cmdProfile_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct far fwm fa-7x whc">&#xf007;</i></span>
                                    </asp:LinkButton>
                                    <%-- <asp:ImageButton runat="server" AlternateText="Profile" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="Profile" ID="cmdProfile" ToolTip="Profile" Width="180px" Height="120px"></asp:ImageButton>--%>
                                </div>
                            </div>

                        </td>
                        <td style="width: 15%">
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Document" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="LinkButton2" ToolTip="Document" OnClick="LinkButton2_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct far fwm fa-7x whc">&#xf15c;</i></span>
                                    </asp:LinkButton>
                                    <%--   <asp:ImageButton runat="server" AlternateText="Document" OnClick="CmdDocument_Click" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="Document" ID="CmdDocument" ToolTip="Document" Width="180px" Height="120px"></asp:ImageButton>--%>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel6" runat="server" Text="Training" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="align-content: center">
                                    <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Training" OnClick="cmdEdit_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf51c;</i></span>
                                    </asp:LinkButton>
                                    <%-- <asp:ImageButton runat="server" AlternateText="Training" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="Training" ID="cmdTraining" ToolTip="Training" OnClick="cmdTraining_Click" Width="180px" Height="120px"></asp:ImageButton>--%>
                                </div>
                            </div>
                        </td>
                        <td>

                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel7" runat="server" Text="De Briefing" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="cmdDebriefing" ToolTip="De Briefing" OnClick="cmdDebriefing_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf007;</i></span>
                                    </asp:LinkButton>
                                    <%-- <asp:ImageButton runat="server" AlternateText="DeBriefing" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        CommandName="DeBriefing" ID="cmdDebriefing" ToolTip="De Briefing" Width="180px" Height="120px"></asp:ImageButton>--%>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Travel" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">

                                    <asp:LinkButton runat="server" ID="cmdTravel" ToolTip="Travel" OnClick="cmdTravel_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf5b0;</i></span>
                                    </asp:LinkButton>


                                </div>
                            </div>

                        </td>
                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel8" runat="server" Text="Payment" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="Cmdpayment" ToolTip="Payment">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fws fa-7x whc">&#xf155;</i></span>
                                    </asp:LinkButton>

                                </div>
                            </div>
                        </td>
                    </tr>



                    <tr>
                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel9" runat="server" Text="Appraisal" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" ID="CmdAppraisal" ToolTip="Appraisal" OnClick="CmdAppraisal_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas vro fws fa-7x whc">&#xf5a2;</i></span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </td>
                        <td>

                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel10" runat="server" Text="ESM News" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="CmdNews" ToolTip="Edit" OnClick="CmdNews_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fws fa-7x whc">&#xf0a1;</i></span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel11" runat="server" Text="Correspondence" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="CmdCorrespondence" ToolTip="Edit">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf46d;</i></span>
                                    </asp:LinkButton>

                                </div>
                            </div>

                        </td>
                        <td>
                            <div class="panel panel-success" style="height: 180px; width: 98%">
                                <div class="panel-heading gossip" style="text-align: center;">
                                    <telerik:RadLabel ID="RadLabel12" runat="server" Text="Settings" Font-Bold="true" CssClass="white"></telerik:RadLabel>
                                </div>
                                <div class="panel-body" style="height: 100%">

                                    <asp:LinkButton runat="server" ID="cmdSettings" ToolTip="Edit" OnClick="cmdSettings_Click">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="icon" ><i class="ct fas fwm fa-7x whc">&#xf509;</i></span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>

            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
