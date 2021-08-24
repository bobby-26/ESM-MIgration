<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDirectPORemainingBudget.aspx.cs"
    Inherits="AccountsDirectPORemainingBudget" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
    <style>
            .table {
                border-collapse: collapse;
                
            }

            .table td,th {
                border: 1px solid black;
            }
        </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
            <div class="subHeader" style="position: relative">
                <div id="div2" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Budget Group" ShowMenu="false">
                    </eluc:Title>
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                border: none; width: 100%">

                <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true" >
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane" runat="server">
                        <Header>
                            <a href="" class="accordionLink"><asp:Label runat="server" ID="lblBudgetGroup"></asp:Label> </a>
                        </Header>
                        <Content>
                            <table width="80%" class="table" style="margin-left:10px;" >
                                <tr>
                                    <td style="width:80%">
                                        <asp:Literal ID="lblVarianceDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right">
                                        <asp:Label ID="lblVariance" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80%">
                                        <asp:Literal ID="lblBudgetDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right">
                                        <asp:Label ID="lblBudget" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80%">
                                        <asp:Literal ID="lblChaCommittedDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right">
                                        <asp:Label ID="lblChaCommitted" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80%">
                                        <asp:Literal ID="lblActualDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right;color:red;">
                                        <asp:Label ID="lblActual" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80%">
                                        <asp:Literal ID="lblCurrentDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right;color:red;">
                                        <asp:Label ID="lblCurrent" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td style="width:80%">
                                        <asp:Literal ID="lblAppNotOrderDesc" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width:20%;text-align:right;color:red;">
                                        <asp:Label ID="lblAppNotOrder" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
                </ajaxToolkit:Accordion>
                
                <br />
                <table width="90%"  class="table" >
                    <tr>
                        <td style="width: 80%">
                            <asp:Literal ID="lblMonthlyRemainigDesc" runat="server"></asp:Literal>
                        </td>
                        <td style="width:20%;text-align:right;">
                            <asp:Label ID="lblMonthlyRemainig" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 80%">
                            <asp:Literal ID="lblYTDRemainigDesc" runat="server"></asp:Literal>
                        </td>
                        <td style="width:20%;text-align:right;">
                            <asp:Label ID="lblYTDRemainig" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td style="width: 80%">
                            <asp:Literal ID="lblTechMonthlyRemainigDesc" runat="server"></asp:Literal>
                        </td>
                        <td style="width:20%;text-align:right;">
                            <asp:Label ID="lblTechMonthlyRemainig" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td style="width: 80%">
                            <asp:Literal ID="lblTechYTDRemainigDesc" runat="server"></asp:Literal>
                        </td>
                        <td style="width:20%;text-align:right;">
                            <asp:Label ID="lblTechYTDRemainig" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
