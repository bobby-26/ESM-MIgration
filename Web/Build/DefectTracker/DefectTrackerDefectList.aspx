<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerDefectList.aspx.cs"
    MaintainScrollPositionOnPostback="true" Inherits="DefectTracker_DefectTrackerDefectList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPBugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPType" Src="~/UserControls/UserControlSEPBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPSeverity" Src="~/UserControls/UserControlSEPBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPPriority" Src="~/UserControls/UserControlSEPBugPriority.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Defect List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<form id="form1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
</ajaxToolkit:ToolkitScriptManager>
<asp:updatepanel runat="server" id="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="Title1" Text="Defect Analysis" ShowMenu="true"/> 
                </div>
            </div>
            <div id="divFind" style="margin-top: 0px;">
                <table width="100%" cellpadding="0" cellspacing="0" border="1">
                    <tr>
                        <td width="30%">
                            <table>
                                <tr>
                                    <td>
                                        Bug ID
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIDSearch" runat="server" CssClass="input" OnTextChangedEvent="Filter_Changed" AppendDataBoundItems="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reported By
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReportedBy" runat="server" MaxLength="100" CssClass="input" OnTextChangedEvent="Filter_Changed" AppendDataBoundItems="true"></asp:TextBox>
                                    </td>
                                </tr>
                               
                                 <tr>
                                    <td>
                                        Opened Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromOpenDate" runat="server" CssClass="input"/>
                                        <eluc:Date ID="ucToOpenDate" runat="server" CssClass="input"/>
                                    </td>
                                </tr>   
                                
                                <tr>
                                    <td>
                                        Fixed Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromFixedDate" runat="server" CssClass="input"/>
                                        <eluc:Date ID="ucToFixedDate" runat="server" CssClass="input"/>
                                    </td>
                                </tr>   
                                
                                 <tr>
                                    <td>
                                        Closed Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromClosedDate" runat="server" CssClass="input"/>
                                        <eluc:Date ID="ucToClosedDate" runat="server" CssClass="input"/>
                                    </td>
                                </tr>   
                                
                            </table>
                        </td>
                        <td width="25%">
                            Module<br />
                            <eluc:SEPModule ID="ucSEPModule" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed"  />
                        </td>
                        <td>
                            Status<br />
                            <eluc:SEPStatus ID="SEPStatus" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td align="left">
                            Severity<br />
                            <eluc:SEPStatus ID="SEPSeverity" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed"  />
                        </td>
                    </tr>
                </table>
            </div>
                        <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="MenuDefectTracker_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <table width="100%">
                <td colspan="4">
                    <asp:GridView ID="gvDefectList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowCreated="gvDefectList_RowCreated">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="6%"></ItemStyle>
                                <HeaderTemplate>
                                    Bug Id
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBugid" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                                <HeaderTemplate>
                                    Severity
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITYNAME") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Module Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblModuleName" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODULENAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Reported By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReportedBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDUSERNAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                                <HeaderTemplate>
                                    Present Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPresentStatus" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Open Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOpenedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDOPENDATE")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Fixed Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFixedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDFIXEDDATE")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Reopen Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReopenedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREOPENDATE")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Closed Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClosedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")%>'></asp:Label>
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
                </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:updatepanel>
</form>
</html>
