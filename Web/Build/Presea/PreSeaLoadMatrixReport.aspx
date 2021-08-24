<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaLoadMatrixReport.aspx.cs"
    Inherits="PreSeaLoadMatrixReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaExamVenue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Load Matrix Report</title>
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
                        <eluc:Title runat="server" ID="Title1" Text="Load Matrix Report" ShowMenu="<%# Title1.ShowMenu %>" />
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPreSeaScoreCradSummary" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table style="width: 80%">
                    <tr>
                        <td>
                            Report Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="dropdown_mandatory">
                                <asp:ListItem Text="Load Matrix Proposed" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Load Matrix Actual" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Year
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown_mandatory">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Month
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Week
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlWeek" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                DataTextField="FLDWEEKNAME" DataValueField="FLDWEEKPERIOD">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Batch
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input" AppendDataBoundItems="true" />
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
                                <asp:Label ID="lblFacultyId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDFACULTY"].ToString()%>'></asp:Label>
                                <asp:Label ID="lblSNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDROWNUMBER"].ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Height="15px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblCrew" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFACULTYNAME"].ToString()%>'></asp:Label>
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
