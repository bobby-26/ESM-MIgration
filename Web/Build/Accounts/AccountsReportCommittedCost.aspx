<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportCommittedCost.aspx.cs"
    Inherits="AccountsReportCommittedCost" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Committed Cost</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmmsg.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()">
    <form id="frmReportCommittedCost" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
         <asp:Button ID="ucConfirmmsg" runat="server" OnClick="ucConfirmmsg_Click" CssClass="hidden" />

        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

            <asp:Button runat="server" ID="cmdHiddenSubmit" />
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                OnSelectedIndexChanged="VesselAccount_Changed" AutoPostBack="true" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="" Selected="True"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>

                            <asp:ImageButton runat="server" Visible="false" AlternateText="GoodsReceivedate" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                                CommandName="GoodsReceiveDate" ID="cmdGoodsReceiveDate" ToolTip="Goodsreceivedate Update" OnClick="cmdGoodsReceiveDate_Click"
                                ></asp:ImageButton>

                            <%--<eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                            CssClass="dropdown_mandatory" />--%>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" DateTimeFormat="dd/MMM/yyyy" />
                            <%--  <telerik:RadTextBox ID="ucDate" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="ucDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDspOwnerBudget" runat="server" Text="Display Owner Budget Code"></telerik:RadLabel>
                        </td>
                        <%-- <td>
                            <asp:RadioButtonList ID="rblDspOwnerBC" runat="server">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>--%>


                        <td>
                            <telerik:RadComboBox ID="rblDspOwnerBC" runat="server" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Owner budget code" Selected="True" Value="1"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Budget code" Value="2"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Budget code with owner code" Value="3"></telerik:RadComboBoxItem>
                                </Items>

                            </telerik:RadComboBox>
                        </td>

                        <td />
                        <td />
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCommittedCostVoucherNo" runat="server" Text="Committed Cost Voucher No:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                CommandName="ATTACHMENT" ID="cmdAtt" Visible="false"
                                ToolTip="Attachment"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlOrdereddate" runat="server" GroupingText="Ordered Date" Width="340px">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucOrderedFromDate" runat="server" DateTimeFormat="dd/MMM/yyyy" />
                                            <%--  <telerik:RadTextBox ID="ucOrderedFromDate" runat="server" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                                Enabled="True" TargetControlID="ucOrderedFromDate" PopupPosition="TopLeft">
                                            </ajaxToolkit:CalendarExtender>--%>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucOrderedToDate" runat="server" DateTimeFormat="dd/MMM/yyyy" />
                                            <%--<telerik:RadTextBox ID="ucOrderedToDate" runat="server" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MMM/yyyy"
                                                Enabled="True" TargetControlID="ucOrderedToDate" PopupPosition="TopLeft">
                                            </ajaxToolkit:CalendarExtender>--%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px; width: 100%;"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
