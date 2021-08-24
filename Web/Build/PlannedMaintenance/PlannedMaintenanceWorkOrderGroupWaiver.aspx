<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupWaiver.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderGroupWaiver" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Waive</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function CloseModal() {
                GetRadWindow().close();
            }
            function refreshParent() {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow();
            }
        </script>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWaiver" runat="server" Text="Do you want to Waive this?"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkWaiver" runat="server" Checked="true" Enabled="false"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvWaiver" runat="server" RenderMode="Lightweight" OnNeedDataSource="gvWaiver_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridBoundColumn DataField="FLDWAIVERTYPE" HeaderText="Item"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FLDREMARKS" HeaderText="Remarks"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FLDWAIVEDBYNAME" HeaderText="Waived By"></telerik:GridBoundColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
            </MasterTableView>
        </telerik:RadGrid>
    </form>
</body>
</html>
