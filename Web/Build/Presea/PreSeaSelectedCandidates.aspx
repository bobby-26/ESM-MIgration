<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaSelectedCandidates.aspx.cs"
    Inherits="PreSeaSelectedCandidates" EnableEventValidation="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaExamVenue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Selected Candidates</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSea">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Score card summary" ShowMenu="<%# Title1.ShowMenu %>" />
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPreSeaScoreCradSummary" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            Batch
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            Exam Venue
                        </td>
                        <td>
                            <eluc:PreSeaExamVenue ID="ucExamVenue" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                        <td>
                            Entrance Roll No
                        </td>
                        <td>
                            <eluc:Number ID="txtRollNumber" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            Score Between (%)
                        </td>
                        <td>
                            <eluc:Number ID="txtScoreFrom" runat="server" CssClass="input txtNumber" MaxLength="5"
                                IsInteger="false" />
                            to
                            <eluc:Number ID="txtScoreTo" runat="server" CssClass="input txtNumber" MaxLength="5"
                                IsInteger="false" />
                        </td>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br style="clear: both" />
                <h3 id="HeaderTitle" runat="server">
                </h3>
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaGrid" runat="server" OnTabStripCommand="MenuPreSeaGrid_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvPreSea_RowDataBound" ShowHeader="true"
                    ShowFooter="false" EnableViewState="false">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Height="15px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="LblInterviewId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDINTERVIEWID"].ToString()%>'></asp:Label>
                                <asp:Label ID="LblBatchId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDBATCHID"].ToString()%>'></asp:Label>
                                <asp:Label ID="LblCandidateId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCANDIDATEID"].ToString()%>'></asp:Label>
                                <asp:Label ID="LblSNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDROWNUMBER"].ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Height="15px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCrew" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"].ToString()%>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
