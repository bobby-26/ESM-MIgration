<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPortagebillHistory.aspx.cs"
    Inherits="CrewPortagebillHistory" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlContractCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contract</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInActive" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlContract">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Summary" ShowMenu="false" />
                    </div>
                </div>
                <div runat="server" id="divSubHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="" Width="360px"></asp:Label>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuCrewContract" runat="server" OnTabStripCommand="CrewContract_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divCrewCompanyExperience" style="position: relative; z-index: +1">
                    <table id="tblCrewCompanyExperience" width="100%">
                        <tr>
                            <td style="width: 10%">
                                First Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Middle Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Last Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                File No.
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Current Rank
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Sign On Rank
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtsignonRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                Vessel
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtvessel" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Portage Bill Between
                            </td>
                            <td style="width: 10%">
                                <eluc:Date ID="txtfromdate" runat="server" CssClass="readonlytextbox" />
                                <eluc:Date ID="txtTodate" runat="server" CssClass="readonlytextbox" />
                            </td>
                            <td style="width: 10%">
                                Days
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtdays" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <table cellpadding="1" cellspacing="1" width="70%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvPortagebill" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="70%" CellPadding="3" OnRowDataBound="gvPortagebill_RowDataBound" ShowHeader="true"
                                ShowFooter="true" EnableViewState="false" GridLines="None">
                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" CssClass="datagrid_footerstyle">
                                </FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <b>Total</b></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Earnings" HeaderStyle-HorizontalAlign="Right">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <%#",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deductions" HeaderStyle-HorizontalAlign="Right">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <%#!",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div id="div2" runat="server">
                                <table width="70%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            Total Earnings
                                        </td>
                                        <td>
                                            (+)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEarning" runat="server" CssClass="readonlytextbox txtNumber"
                                                ReadOnly="true" Width="90px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total Deductions
                                        </td>
                                        <td>
                                            (-)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="readonlytextbox txtNumber"
                                                ReadOnly="true" Width="90px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Balance of Wages
                                        </td>
                                        <td>
                                            (=)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbalance" runat="server" CssClass="readonlytextbox txtNumber"
                                                ReadOnly="true" Width="90px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
