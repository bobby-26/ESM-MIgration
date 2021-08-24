<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerDebitCreditNoteGenerateSearch.aspx.cs"
    Inherits="AccountsOwnerDebitCreditNoteGenerateSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debit/Credit Note</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand"></eluc:TabStrip>

                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                        <tr>
                            <td width="20%">
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td width="30%">
                                <eluc:Vessel ID="ddlVessel" runat="server"  VesselsOnly="true" AppendDataBoundItems="true"
                                    AutoPostBack="false" Width="200px" />
                            </td>
                            <td width="20%"></td>
                            <td width="30%"></td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReferenceNo" runat="server"  Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBillingCompany" runat="server" Text="Billing Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    AutoPostBack="true"  runat="server" AppendDataBoundItems="true"
                                    OnTextChangedEvent="ddlBillToCompany_Changed" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlType" runat="server"  DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 140)%>'
                                    DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlType_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBank" runat="server" Text="Bank receiving funds"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlBank" runat="server"  DataTextField="FLDBANKACCOUNTNUMBER"
                                    DataValueField="FLDSUBACCOUNTID" OnDataBound="ddlBank_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                </telerik:RadComboBox>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOpenCloseHeader" runat="server" Text="Open/Close"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlOpenClose" runat="server"  Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="Open" Text="Open"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="Close" Text="Close"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblsubtype" runat="server" Text="Sub-Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlSubtype" runat="server"  DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 154)%>'
                                    DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlSubtype_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromAmount" runat="server" Text="From Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtFromAmount" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblToAmount" runat="server" Text="To Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtToAmount" runat="server"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server"  DatePicker="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server"  DatePicker="true" />
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
