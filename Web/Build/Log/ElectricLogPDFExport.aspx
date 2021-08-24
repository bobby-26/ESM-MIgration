<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogPDFExport.aspx.cs" Inherits="Log_ElectricLogPDFExport" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDF Export</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenugvCounterUpdate" runat="server" OnTabStripCommand="MenugvCounterUpdate_TabStripCommand"></eluc:TabStrip>

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" height="95%">

        <table runat="server" id="tbldaterange">
            <tr>
                <td colspan="2">
                    <telerik:RadRadioButtonList runat="server" ID="radPdfExport" OnSelectedIndexChanged="chkCurrentPage_CheckedChanged" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="Current Page" Value="currentPage" />
                            <telerik:ButtonListItem Text="Date Range" Value="dateRange" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
            <tr runat="server" id="rowdaterange" style="display:none;">
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="txtFromDate" CssClass="ui-control">
                         <DateInput DateFormat="dd/MM/yyyy"> 
                        </DateInput> 
                    </telerik:RadDatePicker>
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="txtToDate" CssClass="ui-control">
                         <DateInput DateFormat="dd/MM/yyyy"> 
                        </DateInput> 
                    </telerik:RadDatePicker>
                </td>
            </tr>
        </table>
            </telerik:radajaxpanel>
    </form>
</body>
</html>
