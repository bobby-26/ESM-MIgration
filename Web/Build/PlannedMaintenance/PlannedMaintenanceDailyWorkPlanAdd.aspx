<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanAdd.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanAdd" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript"> 
             function SetDate(dateVar, id) {
                 var datepicker = $find("<%= txtDate.ClientID %>");
                 datepicker.set_selectedDate(dateVar);
                 var calId = document.getElementById('hdnShipCalenderId');
                 calId.value = id;
             }
             function dayRender() {
                 document.getElementById('txtDate_calendar_NN').click();
                 setTimeout(function () { $find("<%= txtDate.ClientID %>").togglePopup(); }, 100);
             }             
         </script>
     </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>           
            <telerik:AjaxSetting AjaxControlID="divAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divAdd" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <div id="divAdd" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table border="0" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Daily Work Plan No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNo" runat="server" Enabled="false"
                        MaxLength="200" Width="180px">
                    </telerik:RadTextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>

                    <telerik:RadDatePicker ID="txtDate" runat="server" Width="120px" CssClass="input_mandatory">                        
                        <Calendar runat="server" OnDayRender="RadCalendar1_DayRender"
                            OnDefaultViewChanged="RadCalendar1_DefaultViewChanged" AutoPostBack="true">
                        </Calendar>
                        <DateInput Enabled="false" runat="server"></DateInput>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList ID="rblVesselStatus" runat="server" CssClass="input_mandatory"
                        Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="In Port" Value="1" />
                            <telerik:ButtonListItem Text="At Sea" Value="2" />
                            <telerik:ButtonListItem Text="Both" Value="3" />
                        </Items>
                    </telerik:RadRadioButtonList>                   
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChangeTime" runat="server" Text="Change Time"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTimePicker ID="tpChangeTime" runat="server"></telerik:RadTimePicker>                    
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:HiddenField ID="hdnShipCalenderId" runat="server" />                    
                    <telerik:RadButton ID="btnCreate" Text="Create" runat="server" OnClick="btnCreate_Click"></telerik:RadButton>
                </td>
            </tr>            
        </table>
        </div>
    </form>
</body>
</html>
