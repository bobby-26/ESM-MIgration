<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportsRHNCByOwner.aspx.cs" Inherits="VesselAccountsReportsRHNCByOwner" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NC</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNC" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNC">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="NC/Owner" ShowMenu="true"></eluc:Title>
                            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                                <eluc:TabStrip ID="MenuNCGeneral" runat="server" OnTabStripCommand="MenuNCGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                            </div>
                        </div>
                    </div>
                    <table id="tblNC" width="100%" align="center" runat="server">
                        <tr>
                            <td style="width: 15px">
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                            </td>
                            <td style="width: 35px">
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td style="width: 15px">
                                <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            </td>
                            <td style="width: 35px">
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15px">
                                <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                            </td>
                            <td style="width: 35px">
                                <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input" AppendDataBoundItems="true" Width="80%" />
                            </td>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuNC" runat="server" OnTabStripCommand="MenuNC_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvNC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvNC_ItemDataBound" ShowHeader="true"
                            EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblNoOfNCs" runat="server" Text="No. Of NCs"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                    <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                            <eluc:Status runat="server" ID="ucStatus" />
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
