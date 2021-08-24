<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMatrixCompliance.aspx.cs" Inherits="CrewMatrixCompliance" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           
            function pageLoad() {
                var code = document.querySelector('[title="Code"]');
                if (code != null) {
                    code.parentElement.style.display = "none";
                }

                var headerRow = document.querySelectorAll(".rpgRowHeaderZoneDiv table tbody tr");
                var dataRow = document.querySelectorAll(".rpgContentZoneDiv table tbody tr");
                for (var i = 0; i < headerRow.length; i++) {
                    var row = headerRow[i]
                    var data = dataRow[i];
                    row.style.height = row.offsetHeight + "px";
                    data.style.height = row.offsetHeight + "px";
                }
            }

        </script>


    </telerik:RadCodeBlock>
      <style type="text/css">
        .mlabel {
            display: inline;
            padding: .2em .6em .3em;
            font-size: 82%;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: .25em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="GvCrewMatrix">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GvCrewMatrix" UpdatePanelHeight="88%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
   <%--     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContract" runat="server" Text="Contract"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlContract" runat="server" SelectedHard="440" HardTypeCode="101" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewOilMajorMatrix" runat="server" OnTabStripCommand="MenuCrewOilMajorMatrix_TabStripCommand"></eluc:TabStrip>
            <telerik:RadPivotGrid ID="GvCrewMatrix" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="GvCrewMatrix_NeedDataSource"
                AllowPaging="false" AllowFiltering="true" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="GvCrewMatrix_CellDataBound"
                ShowColumnHeaderZone="true" ShowDataHeaderZone="true" ShowRowHeaderZone="true" Height="95%" TotalsSettings-RowsSubTotalsPosition="None">
                <ClientSettings>
                    <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                    <Scrolling SaveScrollPosition="true" AllowVerticalScroll="true" />
                </ClientSettings>
                <Fields>

                    <telerik:PivotGridColumnField DataField="FLDOILMAJORNAME" Caption="Oil Major">
                    </telerik:PivotGridColumnField>

                    <telerik:PivotGridRowField DataField="FLDVESSELNAME" Caption="Vessel" CellStyle-Width="150px">
                    </telerik:PivotGridRowField>

                    <telerik:PivotGridRowField DataField="FLDVESSELCATEGORYNAME" Caption="Category" CellStyle-Width="150px">
                    </telerik:PivotGridRowField>
                    <telerik:PivotGridRowField DataField="FLDVESSELID" Caption="Code" CellStyle-Width="1px" CellStyle-CssClass="hidden">
                    </telerik:PivotGridRowField>                    
                    <telerik:PivotGridAggregateField DataField="Count" Caption="Count" Aggregate="Sum" IgnoreNullValues="true" CellStyle-Width="50px"></telerik:PivotGridAggregateField>
                </Fields>
            </telerik:RadPivotGrid>
 <%--       </telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
