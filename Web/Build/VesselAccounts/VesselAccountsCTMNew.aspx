<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMNew.aspx.cs"
    Inherits="VesselAccountsCTMNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
  <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <asp:UpdatePanel runat="server" ID="pnlCTM">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Cash Request" ShowMenu="<%# Title1.ShowMenu %>">
                        </eluc:Title>
                        <%--  <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
                    </div>
                </div>
                <%-- <div style="position: relative; overflow: hidden; clear: right;">
                    <iframe runat="server" id="ifMoreInfo" style="min-height: 300px; width: 100%; overflow-x: hidden">
                    </iframe>
                </div>--%>
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvCTM" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvCTM_RowDataBound"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvCTM_Sorting"
                        OnRowCommand="gvCTM_RowCommand" OnRowDeleting="gvCTM_RowDeleting" DataKeyNames="FLDCAPTAINCASHID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lnkDateHeader" runat="server" Text="Date"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEditable" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVEYN"]%>'></asp:Label>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:dd/MMM/yyyy}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPortHeader" runat="server" Text="Required Port"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEAPORTNAME"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblETAHeader" runat="server" Text="ETA"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETA", "{0:dd/MMM/yyyy}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPortAgentHeader" runat="server" Text="Port Agent"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSUPPLIERNAME"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CTM Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDRECEIVEDAMOUNT"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSTATUS"]%>
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
                                    <asp:ImageButton runat="server" AlternateText="View" ImageUrl="<%$ PhoenixTheme:images/view-task.png %>"
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSelect"
                                        ToolTip="View"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <eluc:MaskNumber ID="txtnopage" MaxLength="9" runat="server" Width="20px" IsInteger="true"
                                    CssClass="input" />
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
