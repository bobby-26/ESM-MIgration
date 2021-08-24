<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractDetails.aspx.cs"
    Inherits="CrewContractDetails" ValidateRequest="false" %>

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
                        <eluc:Title runat="server" ID="ucTitle" Text="Contract Letter" ShowMenu="false" />
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
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 13%;">
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td style="width: 22%;">
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="gridinput readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                        <td style="width: 10%;">
                            <asp:Literal ID="lbllastname" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td style="width: 22%;">
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td style="width: 11%;">
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                        </td>
                        <td style="width: 22%;">
                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFileNo" runat="server" Text=" File No."></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfileno" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRefno" runat="server" Text="Ref No."></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtrefno" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCDC" runat="server" Text="CDC No."></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeamanBook" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>
                            <asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="gridinput readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDOB" runat="server" Text="D.O.B."></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDOB" runat="server" CssClass="gridinput readonlytextbox" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPlaceofbirth" runat="server" Text="Place of Birth"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtplaceofbirth" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr><tr>
                        <td>
                            <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNationality" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="For Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvessel" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text=" Port of Engagement"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtportofEngagement" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                    </tr>  <tr>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner as per Registry"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOwnerName" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblOwnerAddress" runat="server" Text="Owner Address"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtOwnerAddress" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblContractPeriod" runat="server" Text="Contract Period"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtContractPeriod" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" />
                            +/-
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input readonlytextbox"
                                ReadOnly="true" />
                            (Months)
                        </td>
                        <td>
                            <asp:Literal ID="lblCBAApplied" runat="server" Text=" CBA Applied"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCBAApplied" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPaycommenceson" runat="server" Text=" Pay commences on"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="gridinput readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:Literal ID="lblGTDOTHrs" runat="server" Text=" GTD OT Hrs"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGTDOTHrs" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblOTrate" runat="server" Text="OT Rate"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOTRate" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <b>
                                <asp:Literal ID="lblRemuneration" runat="server" Text="Remuneration"></asp:Literal></b>
                        </td>
                        <td>
                            <b>
                                <asp:Literal ID="lblWages" runat="server" Text="Wages"></asp:Literal></b>
                        </td>
                        <td colspan="2">
                            <b>
                                <asp:Literal ID="lblpayment" runat="server" Text="Payment Frequency"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvContract" runat="server" AutoGenerateColumns="False" Width="100%"
                                CellPadding="3" ShowFooter="false" ShowHeader="false" EnableViewState="false"
                                GridLines="None">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLETYPE")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <b>
                                <asp:Literal ID="lblReimbursements" runat="server" Text="Reimbursements"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvReimbursements" runat="server" AutoGenerateColumns="False" Width="100%"
                                CellPadding="3" ShowFooter="false" ShowHeader="false" EnableViewState="false"
                                GridLines="None">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTAMOUNT")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLETYPE")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <b>
                                <asp:Literal ID="lbldeduction" runat="server" Text="Deduction"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvdeduction" runat="server" AutoGenerateColumns="False" Width="100%"
                                CellPadding="3" ShowFooter="false" ShowHeader="false" EnableViewState="false"
                                GridLines="None">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLETYPE")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <b>
                                <asp:Literal ID="lblTotalMonthlyWages" runat="server" Text="Total Monthly Wages"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Literal ID="lblMonthlyAmount" runat="server"></asp:Literal>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="6">
                            <b>
                                <asp:Literal ID="Literal1" runat="server" Text="Side Letter"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvsideletter" runat="server" AutoGenerateColumns="False" Width="100%"
                                CellPadding="3" ShowFooter="false" ShowHeader="false" EnableViewState="false"
                                GridLines="None">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponentName1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTAMOUNT")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASIS")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
