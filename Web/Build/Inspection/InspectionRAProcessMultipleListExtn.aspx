<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessMultipleListExtn.aspx.cs" Inherits="InspectionRAProcessMultipleListExtn" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment Process</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
          <eluc:Status runat="server" ID="ucStatus" />
        <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <div class="subHeader" style="position: relative; right: 0px">
                <eluc:Title runat="server" ID="ucTitle" Text="Multiple RA" ShowMenu="true" />
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 450px; width: 100%">
        </iframe>    
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <asp:GridView ID="gvRiskAssessmentProcess" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" 
            DataKeyNames="FLDRISKASSESSMENTPROCESSMULTIPLEID" OnRowEditing="gvRiskAssessmentProcess_RowEditing"
            OnRowDataBound="gvRiskAssessmentProcess_RowDataBound" OnRowDeleting="gvRiskAssessmentProcess_RowDeleting"
            OnRowCommand="gvRiskAssessmentProcess_RowCommand" OnSelectedIndexChanging="gvRiskAssessmentProcess_SelectedIndexChanging">
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="5px" />
            <Columns>
                <asp:ButtonField Text="DoubleClick" CommandName="EDIT" Visible="false" />
                <asp:TemplateField HeaderText="Ref Number">
                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:label ID="lblActivityConditionHeader" runat="server">Activity/Condition</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblProcessMultipleId" Visible="false" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSMULTIPLEID")  %>'></asp:Label>
                        <asp:Label ID="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITION")  %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:label ID="lblCategoryHeader" runat="server">Category</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblProcessName" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:label ID="lblDateHeader" runat="server">Date</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCREATEDDATE"])%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="100px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:label ID="lblActionHeader" runat="server">Action</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>
                        <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                            ToolTip="Delete"></asp:ImageButton>
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
