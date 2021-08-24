<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentReport.aspx.cs"
    Inherits="InspectionIncidentReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            p {
                margin: 0;
                padding: 0;
                text-align: left;
                max-width: 950px;
                max-width: 900px\9;
                display: block;
            }

            table {
                table-layout: fixed; /*border: 1px solid #f00;*/
                word-wrap: break-word;
                max-width: 950px;
                max-width: 900px\9;
                margin: 0;
                padding: 0;
            }

            img {
                table-layout: fixed;
                max-width: 950px;
                max-width: 900px\9;
                margin: 0;
                padding: 0;
                display: block;
            }

            span {
                margin: 0;
                padding: 0;
                word-wrap: break-word;
                max-width: 950px;
                max-width: 900px\9;
                *zoom: 1;
                *display: inline;
            }

                span span {
                    margin: 0;
                    padding: 0;
                    word-wrap: break-word;
                    max-width: 950px;
                    max-width: 900px\9;
                    *zoom: 1;
                    *display: inline;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="divForm" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top; position: static; height: auto; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand"></eluc:TabStrip>

            <%--<img id="imgSearch" runat="server" src="<%$ PhoenixTheme:images/icon_print.png %>" onclick="printPreviewDiv(document.getElementById('divForm'));return false;"
                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px; width:25px; height:25px;" />--%>
        </div>
        <br />
        <br />
        <br />
        <div id="divForm" runat="server" style="padding-left: 2%; display: inline-block; width: 100%; table-layout: fixed;">
            <%--<span id="span1" runat="server" title="View" style="display: inline-block;"></span>--%>
            <table id="tblIncident" runat="server">
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdExecSummaryHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdExecsummary" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdDescHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdDesc" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdTimelineHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdTimeline" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdImmediateActionHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdImmediateAction" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdPostIncidentHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdPostIncident" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdInvestigationHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdInvestigation" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdRCAHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdRCA" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdCAHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdCA" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr style="text-align: left; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                    <td id="tdPAHeader" runat="server"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td id="tdPA" runat="server"></td>
                </tr>
            </table>
        </div>
        <br />
        <br />
    </form>
</body>
</html>
