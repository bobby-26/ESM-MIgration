<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogItemTransfer.aspx.cs" Inherits="Log_ElectricLogItemTransfer" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>

</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:Status runat="server" ID="ucStatus" />--%>
        <telerik:radwindowmanager rendermode="Lightweight" id="RadWindowManager1" runat="server" enableshadow="true">
        </telerik:radwindowmanager>

        <eluc:TabStrip ID="MenuSludgeTransfer" runat="server" OnTabStripCommand="MenuSludgeTransfer_TabStripCommand"></eluc:TabStrip>

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server">
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtOperationDate" runat="server" DatePicker="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd-MM-yyyy}") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCode" runat="server" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Transferred From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" style="width:100%" AutoPostBack="true" ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                            OnTextChanged="ddl_TextChanged" OnDataBound="ddlQuick_DataBound"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" visible="true" Text="Transfer To"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadComboBox DropDownPosition="Static" style="width:100%" AutoPostBack="true" visible="true" ID="ddlTransferTo" runat="server" DataTextField="FLDNAME" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                            OnTextChanged="ddl_TextChanged" OnDataBound="ddlQuick_DataBound"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="ddlTransferTo_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Before From ROB"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox  ID="txtBfrTrnsROBFrom" AutoPostBack ="true"  RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" style="text-align:right;" Type="Number">
                            <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="2" />
                            <ClientEvents OnBlur="Blur" />
                        </telerik:RadNumericTextBox> 
                        <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" visible="true" Text="Before To ROB"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadNumericTextBox ID="txtBfrTrnsROBTo" RenderMode="Lightweight" visible="true" runat="server" MaxLength="22" Width="120px" style="text-align:right;" Type="Number">
                            <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="2" />
                             <ClientEvents OnBlur="Blur" />
                        </telerik:RadNumericTextBox> 
                        <telerik:RadLabel ID="lbltounit" runat="server" visible="true" Text="m3"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAfrTrnsROBFrom" runat="server" Text="After From ROB"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="txtAfrTrnsROBFrom"  RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" style="text-align:right;" Type="Number">
                            <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="2" />
                            <ClientEvents OnBlur="Blur" />
                        </telerik:RadNumericTextBox> 
                        <telerik:RadLabel ID="lblaftrfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAfrTrnsROBTo" runat="server" visible="true" Text="After To ROB"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadNumericTextBox ID="txtAfrTrnsROBTo" visible="true" Enabled="false" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" style="text-align:right;" Type="Number">
                            <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="2" />
                        </telerik:RadNumericTextBox> 
                        <telerik:RadLabel ID="lblaftrtoUnit" runat="server" visible="true" Text="m3"></telerik:RadLabel>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel ID="lblStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="startTime" runat="server"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEndTime" runat="server" Text="End Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="endTime" runat="server"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOtherInfo1" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherInfo1" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOtherInfo2" runat="server" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherInfo2" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:radajaxpanel>
        <script type="text/javascript">
            function Blur(sender, eventArgs) {
                __doPostBack('txtBfrTrnsROBFrom', 'OnBlur');
            }
        </script>
    </form>
</body>
</html>


