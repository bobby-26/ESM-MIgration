<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2TechnicalPMS.aspx.cs" Inherits="Dashboard_DashboardOfficeV2TechnicalPMS" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

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
                <telerik:AjaxSetting AjaxControlID="GvPMS">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GvPMS" UpdatePanelHeight="99%"></telerik:AjaxUpdatedControl>                         
                    </UpdatedControls>
                </telerik:AjaxSetting>               
            </AjaxSettings>            
        </telerik:RadAjaxManager>
         <telerik:RadPivotGrid ID="GvPMS" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="GvPMS_NeedDataSource"
            AllowPaging="false" AllowFiltering="true" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="GvPMS_CellDataBound"
            ShowColumnHeaderZone="true" ShowDataHeaderZone="true" ShowRowHeaderZone="true" Height="99%" TotalsSettings-RowsSubTotalsPosition="None">
            <ClientSettings>   
                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                <Scrolling SaveScrollPosition="true" AllowVerticalScroll="true" />
            </ClientSettings>            
            <Fields>                                
                <telerik:PivotGridColumnField DataField="Vessel" Caption="Vessel">
                    <CellTemplate>
                        <asp:Label ID="Label1" runat="server" CssClass="rotate" ToolTip='<%# Container.DataItem %>'>
                            <%# Container.DataItem %>
                        </asp:Label>
                    </CellTemplate>
                </telerik:PivotGridColumnField>              
                <telerik:PivotGridRowField DataField="Measure" Caption="Measure"  CellStyle-Width="250px" SortOrder="None">
                </telerik:PivotGridRowField>
                <telerik:PivotGridRowField DataField="FLDMEASUREID" Caption="Code" CellStyle-Width="1px" CellStyle-CssClass="hidden">
                </telerik:PivotGridRowField>   
                <telerik:PivotGridAggregateField DataField="Count" Caption="Count" Aggregate="Sum" IgnoreNullValues="true" CellStyle-Width="50px"></telerik:PivotGridAggregateField>
            </Fields>                             
        </telerik:RadPivotGrid>
    </form>
</body>
</html>
