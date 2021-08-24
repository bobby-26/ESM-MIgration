<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardPersonnelOnDuty.aspx.cs" Inherits="Dashboard_DashboardPersonnelOnDuty" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">            
            
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .strikethrough {
            text-decoration: line-through;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvTimeSheet">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvTimeSheet" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadPivotGrid ID="GvOnDuty" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="GvOnDuty_NeedDataSource"
            AllowPaging="false" AllowFiltering="false" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="GvOnDuty_CellDataBound"
            ShowColumnHeaderZone="false" ShowDataHeaderZone="false" ShowRowHeaderZone="false" Height="99%">
            <ClientSettings Scrolling-AllowVerticalScroll="true">
            </ClientSettings>
            <DataCellStyle Width="100px" />
            <Fields>
                <telerik:PivotGridColumnField DataField="FLDDEPARTMENT">
                </telerik:PivotGridColumnField>
                <telerik:PivotGridRowField DataField="FLDMANAGEMENT">
                </telerik:PivotGridRowField>
            </Fields>
        </telerik:RadPivotGrid>
    </form>
</body>
</html>
