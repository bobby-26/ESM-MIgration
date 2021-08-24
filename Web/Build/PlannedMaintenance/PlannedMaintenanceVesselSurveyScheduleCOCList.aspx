<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyScheduleCOCList.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleCOCList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey COC</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscripts">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSurveyScheduleComplte" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyScheduleComplte">
        <ContentTemplate>
            <div class="subHeader">
                <asp:Literal ID="lblSurveyScheduleComplte" runat="server" Text="Survey COC"></asp:Literal>
            </div>
            <div id="divControls">
                <table width="100%" cellspacing="15">
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyNumber" runat="server" Text="Survey Number"></asp:Literal>
                            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyNumber" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurvey" runat="server" Text="Survey"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurvey" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyType" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurveyorNameDesignation" runat="server" Text="Surveyor Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyorName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyorDesignation" runat="server" Text="Surveyor Designation"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyorDesignation" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAttendingSupt" runat="server" Text="Attending Supt"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSuperdentName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div id="divGrid" style="position: relative; z-index: 0">
                                <asp:GridView ID="gvCertificatesCOC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvCertificatesCOC_RowDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblItemHeader" runat="server">COC</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDITEM")%>'></asp:Label>
                                                <eluc:ToolTip ID="ucItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTOOLTIP") %>'
                                                    Width="250px" />
                                                <asp:Label ID="lblCocID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></asp:Label>
                                                <asp:Label ID="lblDtkey" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                                    Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDescriptionHeader" runat="server"> Description</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'></asp:Label>
                                                <eluc:ToolTip ID="ucDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTIONTOOLTIP") %>'
                                                    Width="300px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDueDateHeader" runat="server">Due Date</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Width="80px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblStatusName" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCompletedDateHeader" runat="server">Completed Date</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompletedDate" runat="server" Width="80px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
