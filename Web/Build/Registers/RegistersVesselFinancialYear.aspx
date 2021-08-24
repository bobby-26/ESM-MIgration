<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselFinancialYear.aspx.cs" Inherits="RegistersVesselFinancialYear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Financial Year Setup</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselFinancialYearSetup" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlFinancialYearSetup">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblContentFinancialYear" runat="server" Text="Financial Year"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblFinancialYearSetup" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox" Width="360px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblFinancialYear" runat="server" Text="Financial Year"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFinancialYear" runat="server" MaxLength="4" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditFinancialYear" runat="server" AutoComplete="false"
                                    InputDirection="RightToLeft" Mask="9999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtFinancialYear" />
                            </td>
                        </tr>
                    </table>
                </div>
                
                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPeriodLock" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="dgFinancialYearSetup" runat="server" AutoGenerateColumns="False"
                        CellPadding="3" Font-Size="11px" OnRowCommand="dgFinancialYearSetup_RowCommand"
                        OnRowDataBound="dgFinancialYearSetup_ItemDataBound" OnRowDeleting="dgFinancialYearSetup_RowDeleting"
                        OnSorting="dgFinancialYearSetup_Sorting" OnRowEditing="dgFinancialYearSetup_RowEditing"
                        OnRowCancelingEdit="dgFinancialYearSetup_RowCancelingEdit" OnRowCreated="dgFinancialYearSetup_RowCreated"
                        AllowSorting="true" ShowFooter="True" Style="margin-bottom: 0px" Width="100%"
                        EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle" />
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>
                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselnameHeader" runat="server">Vessel Name&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblFinancialYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></asp:Label>
                                    <asp:Label ID="lblVesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Financial Start Year">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblFinancialStartYearHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDFINANCIALSTARTYEAR" ForeColor="White">Start&nbsp;</asp:LinkButton>
                                    <img id="FLDFINANCIALSTARTYEAR" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFinancialStartYear" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <eluc:UserControlDate ID="txtFinancialStartYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'
                                        CssClass="input_mandatory" />
                                </EditItemTemplate>--%>
                                <FooterTemplate>
                                    <eluc:UserControlDate ID="txtFinancialStartYearAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Financial End Year">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblFinancialEndYearHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDFINANCIALENDYEAR" ForeColor="White">End&nbsp;</asp:LinkButton>
                                    <img id="FLDFINANCIALENDYEAR" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lnkFinancialEndYear" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlDate ID="txtFinancialEndYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'
                                        CssClass="input_mandatory" />
                                    <eluc:UserControlDate ID="txtFinancialStartYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'
                                        CssClass="input_mandatory" Visible="false" />
                                </EditItemTemplate>
                                <%--   <FooterTemplate>
                                    <eluc:UserControlDate ID="txtFinancialEndYearAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>--%>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Financial Year">
                                <HeaderTemplate>
                                    <asp:Label ID="lblFinancialYearHeader" runat="server" ForeColor="White">Financial Year&nbsp;</asp:Label>
                                    <img id="FLDFINANCIALENDYEARHEADER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                        Visible="false"></asp:TextBox>
                                    <asp:Label ID="lnkFinancialYear" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></asp:Label>
                                    <asp:Label ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                        Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtFinancialYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'
                                        CssClass="input" />
                                    <asp:Label ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFinancialYear" runat="server" CssClass="input"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveYNHeader" runat="server"> Status </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUS").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:TextBox>
                                    <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUS").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkActiveYNAdd" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <FooterTemplate>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>" ToolTip="Add New" />
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete"
                                        Visible="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
