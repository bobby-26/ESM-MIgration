<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceOrderInformationAdd.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceOrderInformationAdd" %>
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
            function checkall(sender, eventArgs) {                
                var list = $find("<%=ddlCrewList.ClientID%>");                
                for (var i in list.get_items()) {
                    var item = list.get_items()[i];                 
                    if (item.get_enabled())
                        item.set_selected(sender.get_checked());
                }
                sender.set_cancel(true);
            }           
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>   
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <table border="0" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtDate" runat="server" Width="120px">
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDetail" runat="server" Text="Details"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDetail" runat="server" TextMode="MultiLine" Width="400px" Rows="7" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblApplicableTo" runat="server" Text="Applicable To"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkAll" runat="server" Text="Check All" OnClientClicked="checkall" /><br />
                    <telerik:RadCheckBoxList ID="ddlCrewList" runat="server" CssClass="input_mandatory" Columns="3" AutoPostBack="false" OnItemDataBound="ddlCrewList_ItemDataBound">
                        <Databindings DataTextField="FLDEMPLOYEENAME" DataValueField="FLDEMPLOYEEID" DataToolTipField="FLDEMPLOYEENAME" />                             
                    </telerik:RadCheckBoxList>
                </td>
            </tr>
        </table>        
    </form>
</body>
</html>
