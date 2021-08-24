<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersWorkingGearSetDetails.aspx.cs"
    Inherits="RegistersWorkingGearSetDetails" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SetDetails" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Set Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="WorkingGearSetDetails" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkingGearSet" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkingGearSetDetails">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
                <div id="divFind" style="position: relative; z-index: 2">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="lblApplicableRanks" runat="server" Text="Applicable Ranks"></asp:Literal>
                            </td>
                            <td>
                                <div id="divRankList" class="input" style="overflow: auto; width: 60%; height: 100px">
                                    <asp:CheckBoxList runat="server" ID="cblRank" Height="100%" RepeatColumns="1" AutoPostBack="true"
                                        OnSelectedIndexChanged="ApplicableRanksSelection" RepeatDirection="Horizontal"
                                        DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td valign="top">
                                <asp:Literal ID="lblApplicableVessels" runat="server" Text="Applicable Vessels"></asp:Literal>
                            </td>
                            <td>
                                <div id="divVesselList" class="input" style="overflow: auto; width: 60%; height: 100px">
                                    <asp:CheckBoxList runat="server" ID="cblVessel" Height="100%" RepeatColumns="1" AutoPostBack="true"
                                        OnSelectedIndexChanged="ApplicableVesselsSelection" RepeatDirection="Horizontal"
                                        DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <br style="clear: both" id="divGridw" />
                <div class="navSelect" style="position: relative; width: 15px" id="divGrqid">
                    <eluc:TabStrip ID="MenuWorkingGearSetDetails" runat="server" OnTabStripCommand="MenuWorkingGearSetDetails_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvWorkingGearSetItems" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvWorkingGearSetItems_RowCommand"
                        OnRowDataBound="gvWorkingGearSetItems_ItemDataBound" OnRowCancelingEdit="gvWorkingGearSetItems_RowCancelingEdit"
                        OnRowDeleting="gvWorkingGearSetItems_RowDeleting" OnRowEditing="gvWorkingGearSetItems_RowEditing"
                        kkShowFooter="true" ShowHeader="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    &nbsp;</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARSETID") %>'></asp:Label>
                                    <asp:Label ID="lblSetItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARSETITEMID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSetIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARSETID") %>'></asp:Label>
                                    <asp:Label ID="lblSetItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARSETITEMID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lnkItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblNoofUnits" runat="server" Text="No of Units"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfUnits" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFUNITS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucNoOfUnitEdit" runat="server" CssClass="input_mandatory" IsPositive="true"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFUNITS") %>' IsInteger="true"
                                        DefaultZero="true" />
                                </EditItemTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript"> 
        // Maintain scroll position on list box. 
        var xPosVessel, yPosVessel; 
        var xPosRank, yPosRank; 
        var prm = Sys.WebForms.PageRequestManager.getInstance(); 

        function BeginRequestHandler(sender, args) 
        { 
            var VesselCbl = $get('divVesselList'); 
            var RankCbl = $get('divRankList'); 

            if (VesselCbl != null) 
            { 
                xPosVessel = VesselCbl.scrollLeft; 
                yPosVessel = VesselCbl.scrollTop; 
            } 
            if (RankCbl != null) 
            { 
                xPosRank = RankCbl.scrollLeft; 
                yPosRank = RankCbl.scrollTop; 
            } 
        } 

        function EndRequestHandler(sender, args) 
        { 
             var VesselCbl = $get('divVesselList'); 
            var RankCbl = $get('divRankList'); 

            if (VesselCbl != null) 
            { 
                VesselCbl.scrollLeft = xPosVessel; 
                VesselCbl.scrollTop = yPosVessel; 
            } 
            if (RankCbl != null) 
            { 
                RankCbl.scrollLeft = xPosRank; 
                RankCbl.scrollTop = yPosRank; 
            } 
        } 

        prm.add_beginRequest(BeginRequestHandler); 
        prm.add_endRequest(EndRequestHandler); 
    </script>

    </form>
</body>
</html>
