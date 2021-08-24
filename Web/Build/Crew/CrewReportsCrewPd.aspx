<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsCrewPd.aspx.cs"
    Inherits="Crew_CrewReportsCrewPd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Details PD Format</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()" onresize="resizeFrame()">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
              
                    <b style="color: Blue; font-size: small">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Please Select JSU as the Union since this report is for JSU Vessels Only.">
                        </telerik:RadLabel>
                    </b>
            
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Period Between"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                                    <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblUnion" runat="server" Text="Union"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlUnion" runat="server" AutoPostBack="true" AppendDataBoundItems="true" EmptyMessage="Type to Select Union" ToolTip="Type to Select Union" Filter="Contains" MarkFirstMatch="true"
                                        CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlUnion_SelectedIndexChanged" Width="100%">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel" Width="50%"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlVessel" runat="server" CssClass="input_mandatory" AutoPostBack="false" EmptyMessage="Type to Select Vessel" ToolTip="Type to Select Vessel" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Selected="True" Value="0" Text="--Selected--"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                
                <iframe runat="server" id="ifMoreInfo" style="min-height: 450px; width: 100%; border: 0"
                    scrolling="yes"></iframe>
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
