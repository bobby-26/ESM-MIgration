<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOffshoreVesselManningScale.aspx.cs" Inherits="RegistersOffshoreVesselManningScale" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagName="AddrType" TagPrefix="eluc" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel ManningScale</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ManningScalelink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersManningScale" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlManningScaleEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Vessel Manning Scale" ShowMenu="false" /> 
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: +1;">
                    <table id="tblConfigureManningScale" width="100%">
                        <tr>
                            <td>
                                Vessel
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="260px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Owner Scale Total
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOwnerScaleTotal" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                Safe Scale Total
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSafeScaleTotal" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <b><asp:Literal ID="lblRev" runat="server" Text="Revision"></asp:Literal></b>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuRegistersRevision" runat="server" OnTabStripCommand="MenuRegistersRevision_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div1" style="position: relative; z-index: 2">
                    <asp:GridView ID="gvManningScaleRevision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvManningScaleRevision_RowCommand"
                        ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDREVISIONID" OnRowDataBound="gvManningScaleRevision_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEffectiveDateHeader" runat="server" Text="Effective Date"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                      
                                    <asp:LinkButton ID="lnkEffectiveDate" CommandName="SELECTREVISION" CommandArgument="<%# Container.DataItemIndex %>" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucEffectiveDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>' CssClass="input_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucEffectiveDateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>    
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRevNoHeader" runat="server" Text="Revision No"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                         
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITREVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETEREVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Copy Manning Scale from previous revision" ImageUrl="<%$ PhoenixTheme:images/copy.png %>"
                                        CommandName="COPY" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCopy"
                                        ToolTip="Copy Manning Scale from previous revision"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATEREVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCELREVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADDREVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPageR" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberR" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesR" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsR" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousR" runat="server" OnCommand="PagerButtonClickR" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextR" OnCommand="PagerButtonClickR" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageR" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoR" runat="server" Text="Go" OnClick="cmdGoR_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <b><asp:Literal ID="lblManningScale" runat="server" Text="Manning Scale"></asp:Literal></b>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuRegistersManningScale" runat="server" OnTabStripCommand="RegistersManningScale_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvManningScale" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvManningScale_RowCommand" OnRowDataBound="gvManningScale_ItemDataBound"
                        OnRowCancelingEdit="gvManningScale_RowCancelingEdit" OnRowDeleting="gvManningScale_RowDeleting"
                        OnRowEditing="gvManningScale_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" OnSorting="gvManningScale_Sorting" AllowSorting="true"
                        OnRowCreated="gvManningScale_RowCreated" OnSelectedIndexChanging="gvManningScale_SelectedIndexChanging"
                        OnRowUpdating="gvManningScale_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                        ForeColor="White">Rank&nbsp;</asp:LinkButton>
                                    <img id="FLDRANKNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDRANKNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblManningScaleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGSCALEID") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkRankEdit" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblManningScaleIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGSCALEID") %>'></asp:Label>
                                    <eluc:Rank ID="ucRank" RankList='<%# PhoenixRegistersRank.ListRank() %>' runat="server"
                                        CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANK") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Rank ID="ucRankAdd" RankList='<%# PhoenixRegistersRank.ListRank() %>' runat="server"
                                        CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblEquivalentRank" runat="server" Text="Equivalent Ranks"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <img id="imgGroupList" runat="server" src="<%$ PhoenixTheme:images/te_view.png %>"
                                        onmousedown="javascript:closeMoreInformation()" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Owner Scale">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOwnerScaleHeader" runat="server">Owner Scale&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOwnerScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOwnerScaleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtOwnerScaleEdit"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOwnerScaleAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Owner Scale" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtOwnerScaleAdd"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Safe Scale">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSafeScaleHeader" runat="server">Safe Scale&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSafeScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSAFESCALE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSafeScaleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSAFESCALE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtSafeScaleEdit"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSafeScaleAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Safe Scale" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtSafeScaleAdd"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CBA Scale">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCBAScaleHeader" runat="server">CBA Scale&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCBAScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCBASCALE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCBAScaleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCBASCALE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtCBAScaleEdit"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCBAScaleAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter CBA Scale" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtCBAScaleAdd"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contract Period">
                                <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblContractPeriod" runat="server"> Contract Period(days)&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIODDAYS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtContractPeriodEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIODDAYS") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtContractPeriodEdit"
                                        Mask="999" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtContractPeriodAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Contract Period in Days" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtContractPeriodAdd"
                                        Mask="999" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemarksHeader" runat="server">Remarks&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        CssClass="gridinput" MaxLength="100"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput" MaxLength="100"></asp:TextBox>
                                </FooterTemplate>
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
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="MapEquivalentRank" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdEquivalentRank" ToolTip="Map Equivalent Rank"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
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
