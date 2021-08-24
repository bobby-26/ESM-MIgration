<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportExtraMeals.aspx.cs" Inherits="VesselAccountsReportExtraMeals" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Extra Meals Report For Charterers And Owners</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 75 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resize();" onresize="resize();">
    <form id="frmBond" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuReportExtraMeals" runat="server" OnTabStripCommand="MenuReportExtraMeals_TabStripCommand"></eluc:TabStrip>
            <table width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExtraMealsReportFor" runat="server" Text="Extra Meals Report For:"></telerik:RadLabel>
                    </td>
                    <td> <telerik:RadComboBox DropDownPosition="Static" ID="ddlReportFor" runat="server" EnableLoadOnDemand="True"
                                EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem  Text="Owners" Value="-1" />
                                    <telerik:RadComboBoxItem Text="Charterer's" Value="-2" />
                                   
                                </Items>
                            </telerik:RadComboBox>
                    
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of"></telerik:RadLabel>
                    </td>
                    <td>
                      <eluc:Month ID="ddlMonth" runat="server" Width="140px"></eluc:Month>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Year ID="ddlYear" runat="server" Width="140px" OrderByAsc="false"></eluc:Year>
                    </td>
                </tr>
            </table>

            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 200px;  height:80%;width: 99.5%;"></iframe>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
