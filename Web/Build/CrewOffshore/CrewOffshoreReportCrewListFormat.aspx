<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReportCrewListFormat.aspx.cs"
    Inherits="CrewOffshoreReportCrewListFormat" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simple Crew List</title>

    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
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
<body>
    <form id="frmReportCrewList" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />

            <asp:Button runat="server" ID="cmdHiddenSubmit" />

            <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                                CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFormat" runat="server" Text="Format"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblFormat" runat="server" AutoPostBack="false" CssClass="input_mandatory" Columns="6">
                                <Items>
                                    <telerik:ButtonListItem Value="1" Text="Default" />
                                    <telerik:ButtonListItem Value="2" Text="IMO" />
                                    <telerik:ButtonListItem Value="3" Text="Shell Weekly" />
                                    <telerik:ButtonListItem Value="4" Text="Shell Quarterly" />
                                    <telerik:ButtonListItem Value="5" Text="OVID" />
                                    <telerik:ButtonListItem Value="6" Text="OVID" />
                                    <telerik:ButtonListItem Value="7" Text="Weekly Monitor" />
                                    <telerik:ButtonListItem Value="8" Text="Petronas Matrix" />
                                    <telerik:ButtonListItem Value="9" Text="Shell Weekly New" />

                                </Items>

                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>

                </table>
            </div>
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="height: 550px; width: 99%;"></iframe>
            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
