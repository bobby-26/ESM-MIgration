<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportRHRankWiseNonComplianceSummary.aspx.cs" Inherits="VesselAccountsReportRHRankWiseNonComplianceSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rest Hours Rank Wise NonCompliance Summary</title>
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
    <form id="frmRankWiseNonComplianceSummary" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlRankWiseNonCompliance">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Rank Wise NonCompliance Summary" ShowMenu="True"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportRankWiseNonCompliance" runat="server" OnTabStripCommand="MenuReportRankWiseNonCompliance_TabStripCommand"></eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="2" width="100%" style="height: 336px">
                    <tr>
                        <td>
                            <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input_mandatory">
                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="input_mandatory">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselTypes" runat="server" Text="Vessel Types"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvVesselType" class="input" style="overflow: auto; width: 73%; height: 101px">
                                <asp:CheckBoxList runat="server" ID="cblVesselType" Height="111%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" Width="290px">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvRank" class="input" style="overflow: auto; width: 71%; height: 99px">
                                <asp:CheckBoxList ID="cblRank" runat="server" Height="111%" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" Width="356px">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvPrincipal" class="input" style="overflow: auto; width: 73%; height: 106px">
                                <asp:CheckBoxList runat="server" ID="cblPrincipal" Height="109%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" Style="margin-left: 0px" Width="257px">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <asp:Literal ID="lblReason" runat="server" Text="Reason"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvReason" class="input" style="overflow: auto; width: 71%; height: 106px">
                                <asp:CheckBoxList runat="server" ID="chkReason" Height="95%" RepeatColumns="1" RepeatDirection="Horizontal"
                                    DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" Width="359px">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
