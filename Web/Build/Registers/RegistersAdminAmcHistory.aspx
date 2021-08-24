<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAmcHistory.aspx.cs" Inherits="Registers_RegistersAdminAmcHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset AMC</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="assetamc" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAssetAMC" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAssetAMC">
        <ContentTemplate>            
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="AMC" />
                    </div>
                </div>
                <div style="top: 0px; right: 2px; position: absolute">
                    <eluc:TabStrip ID="MenuAsset" runat="server" OnTabStripCommand="Asset_TabStripCommand" 
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divAssetAMC" style="position: relative; z-index: 2">
                    <table id="tblAssetAMC" width="100%">
                        <tr>
                            <td width= "50px">
                                <asp:literal ID="lblAssetName" runat="server" Text="Asset"></asp:literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAssetName" runat="server" CssClass="input" Width="150px"></asp:TextBox>                              
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuAdminAssetAMC" runat="server" OnTabStripCommand="MenuAdminAssetAMC_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAMC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAMC_RowCommand" OnRowDataBound="gvAdminAMC_ItemDataBound"
                        OnRowCancelingEdit="gvAdminAMC_RowCancelingEdit" OnRowDeleting="gvAdminAMC_RowDeleting"
                        OnRowEditing="gvAdminAMC_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnRowUpdating="gvAdminAMC_RowUpdating" OnSorting="gvAdminAMC_Sorting" OnSelectedIndexChanging="gvAdminAMC_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="16%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetNameHeader" runat="server">
                                    Asset
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetAMCID" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMCID")) %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblAssetName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNAME")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">
                                    Address 
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSNAME")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                 <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">
                                    PO Reference 
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOReference" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPOREFERENCE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle> 
                                <FooterStyle HorizontalAlign="Right"/>                              
                                 <HeaderTemplate>
                                        <asp:label ID="lblDurationHeader" runat="server">Duration(In Months) </asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDURATION")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNextDueHeader" runat="server" CommandName="SORT" CommandArgument="FLDNEXTDUE"
                                        ForeColor="White">Next Due</asp:Label>
                                    <img id="FLDNEXTDUE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNextDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEXTDUE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDoneDateHeader" runat="server" CommandName="SORT" CommandArgument="FLDDONEDATE"
                                        ForeColor="White">Done Date</asp:Label>
                                    <img id="FLDDONEDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDONEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>                       
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative; z-index:0">
                    <table width="100%" border="0" class="datagrid_pagestyle">
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
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
