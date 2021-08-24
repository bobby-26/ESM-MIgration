<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationQuoteAgent.aspx.cs"
    Inherits="CrewCostEvaluationQuoteAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Agent Details </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>

    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-190 + "px";

        }
    </script>

</head>
    <body onload="resize()" onresize="resize()">
        <form id="frmCrewCostQuoteAgent" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlQuoteAgentList">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                    width: 100%">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Quotation Details"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuAgent" runat="server" TabStrip="true" OnTabStripCommand="MenuAgent_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div class="subHeader">
                        <div class="divFloat" style="clear: right">
                            <eluc:TabStrip ID="MenuCrewCostQuoteSub" runat="server" OnTabStripCommand="MenuCrewCostQuoteSub_TabStripCommand">
                            </eluc:TabStrip>
                        </div>
                    </div>
                    <table style="color: Blue">
                        <tr>
                            <td>
                                <asp:Literal ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></asp:Literal>
                            </td>
                            <td>
                                <img id="imgnotes" runat="server" src="<%$ PhoenixTheme:images/54.png %>" style="vertical-align: top;
                                    cursor: pointer" alt="NOTES" />
                                &nbsp;
                                <asp:Literal ID="lblbutton" runat="server" Text="button."></asp:Literal>
                                <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                                <td>
                        </tr>
                    </table>
                    <b>&nbsp;<asp:Literal ID="lblPortAgents" runat="server" Text="Port Agents"></asp:Literal></b>
                    <div class="navSelect* " style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuAgentList" runat="server" OnTabStripCommand="MenuAgentList_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" AllowPaging="true" ShowFooter="false" OnSorting="gvAgent_Sorting"
                            EnableViewState="false" AllowSorting="true" OnRowCommand="gvAgent_RowCommand"
                            OnRowDeleting="gvAgent_RowDeleting" OnRowDataBound="gvAgent_RowDataBound">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblImageHeader" runat="server"> </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" Checked="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAgentName" runat="server" Text="Agent Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPortAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGENTID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:LinkButton ID="lnkAgentName" CommandArgument='<%# Container.DataItemIndex %>'
                                            runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") + " - " + DataBinder.Eval(Container,"DataItem.FLDAGENTCODE")%> '></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSendDateHeader" runat="server" Text="Sent Date"> </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSendDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENDDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRecievedDateHeader" runat="server" Text="Received Date"> </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecievedDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <iframe runat="server" id="ifMoreInfo" style="width: 100%" scrolling="auto"></iframe>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAgent" />
            </Triggers>
        </asp:UpdatePanel>
        </form>
    </body>
</html>
