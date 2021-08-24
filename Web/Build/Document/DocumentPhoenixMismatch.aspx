<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentPhoenixMismatch.aspx.cs"
    Inherits="DocumentPhoenixMismatch" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tables" Src="~/UserControls/UserControlVesselTables.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="50%">
                <tr>
                    <td>Upload File
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" CssClass="input" Text="Upload Zip File" />
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>Comparison From
                    </td>
                    <td>
                        <eluc:Date ID="txtCompareFrom" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>Comparison Date
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>Vessel
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>Tables
                    </td>
                    <td>
                        <eluc:Tables ID="ddlTables" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlTables_TextChangedEvent" AuditTableYn="false" Width="150px" />
                    </td>
                    <td>Mismatch Column
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMismatchColumn" runat="server" CssClass="input_mandatory"
                            DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>Select Column
                    </td>
                    <td>
                        <div runat="server" id="divSelectCol" class="input" style="overflow: auto; width: 300px; height: 120px">
                            <asp:CheckBoxList runat="server" ID="cblSelectCol" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME"
                                RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td runat="server" id="tdUpdateHeader">Update Column
                    </td>
                    <td runat="server" id="tdUpdater">
                        <div runat="server" id="divUpdateColumn" class="input" style="overflow: auto; width: 300px; height: 120px">
                            <asp:CheckBoxList runat="server" ID="cblUpdateColumn" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME"
                                RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <table cellpadding="1" cellpadding="1" width="100%">
                <tr>
                    <td align="right">Office Count
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblOfficeCount" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td align="right">Vessel Count
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblVesselCount" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td align="right">Last Import Date
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblLastImportDate" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td align="right">Data Not in Vessel Count based on Last Import
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblDataNotinVesselCount" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td align="right">Data Not in Office Count based on Last Import
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblDataNotinOfficeCount" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td align="right">Data Mismatch Count based on Last Import
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblMismatchCount" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <div runat="server" id="divNotinVessel">
                <b>Data Not in Vessel</b>
                <eluc:TabStrip ID="MenuNotinVessel" runat="server" OnTabStripCommand="MenuMismatch_TabStripCommand"></eluc:TabStrip>
                <div style="overflow: auto; width: 100%; height: 120px">
                    <telerik:RadGrid ID="gvNotInVessel" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                    </telerik:RadGrid>
                </div>
            </div>
            <br />
            <div runat="server" id="divNotinOffice">
                <b>Data Not in Office</b>
                <eluc:TabStrip ID="MenuNotinOffice" runat="server" OnTabStripCommand="MenuMismatch_TabStripCommand"></eluc:TabStrip>
                <div style="overflow: auto; width: 100%; height: 300px">
                    <telerik:RadGrid ID="gvNotInOffice" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                    </telerik:RadGrid>
                </div>
            </div>
            <div runat="server" id="divMismatch">
                <b>Data Mismatch between Office and Vessel : </b>
                <eluc:TabStrip ID="MenuMismatch" runat="server" OnTabStripCommand="MenuMismatch_TabStripCommand"></eluc:TabStrip>
                <div style="overflow: auto; width: 100%; height: 300px">
                    <telerik:RadGrid ID="gvMismatch" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Image runat="server" ID="imgMismatch" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png %>" Visible='<%#((DataRowView)Container.DataItem)["FLDUPDATEYN"].ToString() =="1" ? true : false %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
