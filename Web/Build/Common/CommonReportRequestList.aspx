<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonReportRequestList.aspx.cs" Inherits="Common_CommonReportRequestList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReportRequest List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionAudit" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDeficiencyXLReportRequestList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <%--<asp:UpdatePanel runat="server" ID="pnlInspectionScheduleSearchEntry">
        <ContentTemplate>--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="XL Report Request Status"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDeficiencyXLReportRequestList" runat="server" OnTabStripCommand="DeficiencyXLReportRequestList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%; ">
                    <asp:GridView ID="gvDeficiencyXLReportRequestList" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                        Width="100%" CellPadding="3" OnRowCommand="gvDeficiencyXLReportRequestList_RowCommand" OnRowDataBound="gvDeficiencyXLReportRequestList_ItemDataBound"
                        AllowSorting="true" OnSelectedIndexChanging="gvDeficiencyXLReportRequestList_SelectedIndexChanging"
                        DataKeyNames="FLDREPORTREQUESTID" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lnkReportNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDREPORTNAME"
                                        ForeColor="White">Report Name</asp:Label>
                                    <img id="FLDREPORTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReportRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTREQUESTID") %>'></asp:Label>
                                    <asp:Label ID="lblReportRequestDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblReportName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkReportRequest" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDREPORTNAME").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDREPORTNAME").ToString() %>'></asp:LinkButton>
                                    <eluc:ToolTip ID="ucToolTipName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedByHead" runat="server" ForeColor="White">Requested By</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblRequestedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedDateHead" runat="server" ForeColor="White">Requested Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblRequestedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReportGenerateHead" runat="server" ForeColor="White">Report Generated Y/N</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblReportGenerate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTGENERATED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMailSentHead" runat="server" ForeColor="White">Mail Sent Y/N</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblMailSent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAILSENT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")%>'></asp:Label>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompletedDateHead" runat="server" ForeColor="White">Completed Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                                                        
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Export XL" ImageUrl="<%$ PhoenixTheme:images/icon_xls.png %>"
                                        CommandName="EXPORT2XL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdExport2XL"
                                        ToolTip="Download XL"></asp:ImageButton>
                                </ItemTemplate>
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
    </form>
</body>
</html>
