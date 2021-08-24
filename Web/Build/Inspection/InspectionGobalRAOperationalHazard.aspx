<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionGobalRAOperationalHazard.aspx.cs" Inherits="InspectionGobalRAOperationalHazard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>New Aspect</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="InspectionMapping" runat="server">
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="New Aspect"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <div id="divFind">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblElement" runat="server" Text="Process"></telerik:RadLabel>
                        </td>
                        <td width="85%">
                            <telerik:RadTextBox ID="txtElement" runat="server" Width="360px"
                                Visible="false">
                            </telerik:RadTextBox>
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Width="360px" AppendDataBoundItems="true"
                                AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblAspect" runat="server"  Text="Aspect"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAspect" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="2" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblopertaionalhazard" runat="server"  Text="Hazards / Risks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtopertaionalhazard" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="8" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblcontrolprecautions" runat="server"  Text="Controls / Precautions "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtcontrolprecautions" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="8" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
