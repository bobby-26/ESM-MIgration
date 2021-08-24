<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerCallnumberTrack.aspx.cs"
    Inherits="DefectTrackerCallnumberTrack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>...</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader">
        <div id="div1" style="vertical-align: top">
            <eluc:Title runat="server" Text="Call Tracker" ShowMenu="true" ID="ucCallNumber">
            </eluc:Title>
        </div>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <table width="50%">
        <tr>
            <td>
                Vessel Name
            </td>
            <td>
                <eluc:SEPVessel ID="ucVessel" AppendDataBoundItems="true" runat="server" CssClass="input"
                    AutoPostBack="true" />
            </td>
            <td>
                Call Number
            </td>
            <td>
                <asp:TextBox ID="txtCallNumber" runat="server" CssClass="input" />
            </td>
        </tr>
        <tr>
            <td>
                Open date
            </td>
            <td>
                <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
            </td>
            <td>
                Last Mail
            </td>
            <td>
                <asp:RadioButtonList ID="rbthIsVessel" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Text="SEP" Value="0" Selected="True" />
                    <asp:ListItem Text="Vessel" Value="1" />
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div class="navSelect" style="position: relative; width: 15px">
        <eluc:TabStrip ID="MenuMailManager" runat="server" OnTabStripCommand="MenuMailManager_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </div>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="gvCallTracker" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="99%" OnRowDataBound="CallNumber_ItemDataBound" CellPadding="3" EnableViewState="False"
                    AllowSorting="True">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="From">
                            <HeaderTemplate>
                                Call Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnCallNumber" CommandName="SELECT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALLNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Vessel
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Sep-Vessel
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSepToVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEPTOVESSEL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Vessel-Sep
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselToSep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTOSEP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Opened on
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOpenDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Last Mail on
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClosedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Last Mail
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLastMail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTMAIL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Duration
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCallDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALLDURATION") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                </asp:GridView>
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
