<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIWelfare.aspx.cs"
    Inherits="InspectionPNIWelfare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNI Welfare</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Welfare" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PNIListMain" runat="server" OnTabStripCommand="PNIListMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblInspectionPNI" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblMedicalFitdate" runat="server" Text="Medical Fit Date"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="txtFitDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblTimeLimitForTheCase" runat="server" Text="Time Limit for the Case"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txttimelimit" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCoveringCBA" runat="server" Text="Covering CBA"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCBA" runat="server" CssClass="readonlytextbox" Width="80%"></asp:TextBox>
                            </td>
                            <td>
                               <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucWelfarestatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="175" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:literal ID="lblCaseClosureDateofOPSIndia" runat="server" Text="Case Closure date of Ops, India"></asp:literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtClosureDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                    Width="80%" Height="75px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:LinkButton ID="lnkPNIChecklist" runat="server" Text="PNI Checklist" Font-Bold="true"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
