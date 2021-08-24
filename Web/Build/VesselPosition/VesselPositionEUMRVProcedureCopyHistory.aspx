<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedureCopyHistory.aspx.cs" Inherits="VesselPositionEUMRVProcedureCopyHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EUMRV Procedure History</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id = "div1" runat ="server" >
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRangeConfig" runat="server" >
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>

        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Label ID="lblheader" runat="server" Text="Procedure Copy History"></asp:Label>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server">
                        </eluc:TabStrip>
                    </div>
                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblRangeConfig" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCommencedFromDate" runat="server" Text="Date From - To"></asp:Literal>
                            </td>
                            <td>
                            <eluc:Date runat="server" ID="txtFromDate" CssClass="input" DatePicker="true" />
                            <asp:Literal ID="lblTo" runat="server" Text=" - "></asp:Literal>
                            <eluc:Date runat="server" ID="txtToDate" CssClass="input" DatePicker="true" />
                            </td>
                            <td><asp:Literal ID="lblFromVessel" runat="server" Text="From Vessel"></asp:Literal></td>
                            <td><eluc:Vessel ID="UcFromVessel" runat="server" CssClass="input" AppendDataBoundItems="true" SyncActiveVesselsOnly="True" VesselsOnly="true" /></td>
                            <td><asp:Literal ID="lblToVessel" runat="server" Text="To Vessel"></asp:Literal></td>
                            <td><eluc:Vessel ID="UcToVessel" runat="server" CssClass="input" AppendDataBoundItems="true"  SyncActiveVesselsOnly="True" VesselsOnly="true" /></td>
                        </tr>
                    </table>
                </div>
                <eluc:User ID="UcUser" runat="server" CssClass="input" AppendDataBoundItems="true" Width="150px" Visible="false" />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuHistoryList" runat="server" OnTabStripCommand="HistoryList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvProcedureCopyHistory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" AllowSorting="true" 
                                OnSorting="gvProcedureCopyHistory_Sorting" ShowHeader="true" EnableViewState="false"  DataKeyNames="FLDPROCEDURECOPYLOGID">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lnkCopyDateHeader" Text="Date" runat="server"></asp:Label>
                                            <img id="FLDCOPYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCopyDate" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOPYDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOPYDATE", "{0:HH:mm}") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Label ID="lnkCopiedByHeader" Text="Copied By" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCopiedBy" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOPYPERSON") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Label ID="lnkFromVesselHeader" Text="From Vessel" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromVessel" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOPYFROMVESSEL") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Label ID="lnkToVesselHeader" Text="To Vessel" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblToVessel" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOPYTOVESSEL") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
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
                                <eluc:Number ID ="txtnopage" runat="server" CssClass = "input" MaxLength="9" Width="20px" IsInteger="true" />
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>

    </form>
</body>
</html>
