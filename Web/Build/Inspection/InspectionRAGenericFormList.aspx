<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAGenericFormList.aspx.cs" Inherits="InspectionRAGenericFormList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>    
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment Generic</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Generic" ShowMenu="true" />
            </div>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuGeneric" runat="server" OnTabStripCommand="MenuGeneric_TabStripCommand"></eluc:TabStrip>
        </div>
        <asp:GridView ID="gvRiskAssessmentGeneric" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowDataBound="gvRiskAssessmentGeneric_RowDataBound"
            OnRowCommand="gvRiskAssessmentGeneric_RowCommand">
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="5px" />
            <Columns>
                <asp:TemplateField HeaderText="Ref Number">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                       <asp:Label ID="lblRefNoHeader" runat="server">Ref Number</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField >
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                      <asp:Label ID="lblDateHeader" runat="server">Date</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                     <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField >
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                      <asp:Label ID="lblTypeHeader" runat="Server">Type</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Risks/Aspects">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                       <asp:Label ID="lblRisksAspectsHeader" runat="server"> Risks/Aspects</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGenericFormid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTGENERICFORMID")  %>' Visible="false"></asp:Label>
                          <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTGENERICFORMID")  %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKACTIVITY")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblIntendedWorkDateHeader" runat="server">Intended Work Date</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                      <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:label ID="lblActionHeader" runat="server">Action</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDITROW" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
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
    </form>
</body>
</html>
