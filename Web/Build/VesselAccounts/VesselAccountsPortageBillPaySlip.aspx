<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPortageBillPaySlip.aspx.cs"
    Inherits="VesselAccountsPortageBillPaySlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script language="javascript" type="text/javascript">
            function cmdPrint_Click() {
                document.getElementById('cmdPrint');
                window.print();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Payslip" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPB" runat="server"></eluc:TabStrip>
                    </div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRankNationality" runat="server" Text="Rank / Nationality"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="120px"></asp:TextBox>
                                /
                            <asp:TextBox ID="txtNationality" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="SeamanBookNo" runat="server" Text="Seaman Book No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSeamanBook" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPortofEngagement" runat="server" Text="Port of Engagement"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPort" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblContractPayCommencementDate" runat="server" Text="Contract/ Pay Commencement Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblContractPeriod" runat="server" Text="Contract Period"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContractPeriod" runat="server" CssClass="readonlytextbox" Width="90px" />
                                +/-
                            <asp:TextBox ID="txtPlusMinusPeriod" runat="server" CssClass="readonlytextbox" Width="90px" />
                                (Months)
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselJoining" runat="server" Text="Vessel Joining"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtStartDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblEndDate" runat="server" Text="End Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtEndDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <b>
                            <h3>
                                <td>
                                    <asp:Literal ID="lblPayslipfortheMonthof" runat="server" Text="Payslip for the Month of"></asp:Literal>
                                    <%= DateTime.Parse(Request.QueryString["pbd"]).ToString("MMM yyyy") %>
                            </h3>
                        </b>
                    </center>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvPB" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true"
                            OnRowDataBound="gvPB_RowDataBound" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Earnings">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDECOMPONENTNAME"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Literal ID="lblTotalEarnings" runat="server" Text="Total Earnings"></asp:Literal>
                                    </FooterTemplate>
                                    <FooterStyle Wrap="false" HorizontalAlign="Left" Font-Bold="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(USD)">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDEAMOUNT"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                    <FooterStyle Wrap="false" HorizontalAlign="Right" Font-Bold="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deductions">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDDCOMPONENTNAME"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Literal ID="lblTotalDeductions" runat="server" Text="Total Deductions"></asp:Literal>
                                    </FooterTemplate>
                                    <FooterStyle Wrap="false" HorizontalAlign="Left" Font-Bold="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(USD)">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDDAMOUNT"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                    <FooterStyle Wrap="false" HorizontalAlign="Right" Font-Bold="true" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
