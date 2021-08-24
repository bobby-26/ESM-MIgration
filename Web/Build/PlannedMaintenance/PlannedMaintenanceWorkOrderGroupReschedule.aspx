<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupReschedule.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderGroupReschedule" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function refresh() {                
                if (typeof parent.CloseUrlModelWindow == 'function') {
                    parent.CloseUrlModelWindow();
                } else {
                    top.closeTelerikWindow('postpone', 'maint');
                }                
            }
            function callConfirm() {
                radconfirm('Are you sure want to cancel this wo?', confirmCallBackFn);
            }
            function confirmCallBackFn(arg) {
                var ajaxManager = $find("<%=RadAjaxManager1.ClientID%>");
                if (arg) {
                    ajaxManager.ajaxRequest('ok');
                }
                else {
                    ajaxManager.ajaxRequest('cancel');
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
        
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>        
        <eluc:TabStrip ID="MenuPostpone" runat="server" OnTabStripCommand="MenuPostpone_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlanDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtPlannedDate" runat="server" Enabled="false" Width="120px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblpostponeDate" runat="server" Text="Reschedule Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtpostponeDate" runat="server" CssClass="input_mandatory" Width="120px" />                    
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPostponeRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="input_mandatory" Width="140px"></telerik:RadTextBox>                    
                </td>
            </tr>              
        </table>  
        <table>
            <tr runat="server" id="trpostpone">
                <td colspan="2" style="font-weight: bold">
                    Note: To reschedule the work order, select the “New date”, enter reason for date change in remarks field and click "Save".
                </td>
            </tr>            
            <tr runat="server" id="trpostponecancel" visible="false">
                <td colspan="2" style="font-weight: bold">                    
                Note: To cancel the work order click on the "Cancel WO". The Work order will be canceled, and the Jobs will be released. 
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvJobList" runat="server" OnNeedDataSource="gvJobList_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridBoundColumn DataField="FLDCOMPONENTNUMBER" HeaderText="Comp. No"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FLDWORKORDERNAME" HeaderText="Job Code & Title"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="76px" AllowSorting="true" ShowFilterIcon="false"
                        ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" FilterDelay="200">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

    </form>
</body>
</html>
