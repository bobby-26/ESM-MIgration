<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditNonConformityMaster.aspx.cs" Inherits="InspectionAuditNonConformityMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Observation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionObservationAndResponselink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <body>
        <form id="frmAccounts" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlAccount">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;"
                    id="divAccounts">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="div2">
                            <eluc:Title runat="server" ID="ucTitle" Text="Record"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuInspectionObsGeneral" TabStrip="true" runat="server" OnTabStripCommand="InspectionObsGeneral_TabStripCommand">
                            </eluc:TabStrip>
                    </div>
                   <%-- <div class="subHeader" style="position: relative;">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            
                        </span>
                    </div>--%>
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 680px; width: 100%">
                    </iframe>
                </div>
                </div>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
        </form>
    </body>
</body>
</html>
