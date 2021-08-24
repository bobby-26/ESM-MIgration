<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAImpactGeneral.aspx.cs" Inherits="InspectionRAImpactGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Severity" Src="~/UserControls/UserControlRASeverityExtn.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Impact</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {                              
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';                
            }           
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Details"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblnote" runat="server" Text="* Except Score and Consequence all fields are mandatory." ForeColor="Red"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblCategory" runat="server" Font-Bold="true" Text="Category"></telerik:RadLabel>
                        </td>
                        <td width="85%">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true" Width="270px"
                                AutoPostBack="True" DataTextField="FLDHAZARDTYPE" DataValueField="FLDHAZARDTYPEID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblImpact" runat="server" Font-Bold="true" Text="Impact"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtImpact" runat="server" Width="270px"
                                TextMode="MultiLine" Rows="2" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblseverity" runat="server" Font-Bold="true" Text="Severity"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Severity ID="ucSeverity" runat="server" AppendDataBoundItems="true" Width="270px"
                                Type="2" SeverityList='<%# PhoenixInspectionRiskAssessmentSeverityExtn.ListRiskAssessmentSeverity() %>'></eluc:Severity>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblscore" runat="server" Font-Bold="true" Text="Score"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtscore" runat="server" Width="90px"
                                CssClass="readonlytextbox" ReadOnly="true" Enabled="false">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcons" runat="server" Font-Bold="true" Text="Consequence"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtconsequence" runat="server" Width="90px" MaxLength="1" Enabled="false">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px" CssClass="rfdValidationSummaryControl alignleft"
                                BorderWidth="1px" HeaderText="List of errors"></asp:ValidationSummary>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

