<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegisterPSCUSCGCodeAdd.aspx.cs" Inherits="Inspection_InspectionRegisterPSCUSCGCodeAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuPSCUSCGCodeAdd" runat="server" OnTabStripCommand="MenuPSCUSCGCodeAdd_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td Width="38%">
                        <telerik:RadLabel ID="lblSFICode" runat="server" Text="SFI Code"></telerik:RadLabel>
                    </td>
                    <td Width="60%">
                        <telerik:RadTextBox ID="txtSFICode" runat="server" Text="" Height="80px" Width="250px" Resize="Both" TextMode="MultiLine" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input_mandatory" Text="" Width="250px" MaxLength="500" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPSCMOU" runat="server" Text="PSC MOU"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPSCMOU" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUSCGMOU" runat="server" Text="USCG MOU"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUSCGMOU" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        <telerik:RadLabel ID="lblChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                    </td>
                   <td >
                       <telerik:RadComboBox ID="ddlChapter" runat="server" AutoPostBack="true" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Chapter" Width="70%" >
                        </telerik:RadComboBox>
                   </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblParent" runat="server" Text="Parent"></telerik:RadLabel>
                    </td>
                    <td >
                       <telerik:RadComboBox ID="ddlParent" runat="server" AutoPostBack="true" 
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Chapter" Width="70%" >
                        </telerik:RadComboBox>
                   </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="input_mandatory" 
                            Resize="Both" Width="350px" Height="70px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbActiveyn" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
