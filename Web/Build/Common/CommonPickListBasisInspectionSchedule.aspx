<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListBasisInspectionSchedule.aspx.cs" Inherits="CommonPickListBasisInspectionSchedule" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Basis Inspection Schedule</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Literal ID="lblBasis" runat="server" Text="Basis"></asp:Literal>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="MenuAccount_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlAccount">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvAccount" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvAccount_RowCommand" OnRowDataBound="gvAccount_RowDataBound"
                    OnRowEditing="gvAccount_RowEditing" ShowFooter="false" ShowHeader="true" Width="100%"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvAccount_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyName" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblCompanyId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblInspection" runat="server" Text="Inspection"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblInspectionShortCode" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblReferenceNumber" runat="server" Text="Reference Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblScheduleId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDBASISSCHEDULEID") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lblRefNo" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISSREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblCompletionDate" runat="server" Text="Completion Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbldate" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE") %>'></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateField>                                               
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="gvAccount" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
