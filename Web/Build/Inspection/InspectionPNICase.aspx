<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNICase.aspx.cs" Inherits="InspectionPNICase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNI Case</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLegal">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="PNI Case" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPNIGeneral" TabStrip="true" runat="server" OnTabStripCommand="MenuPNIGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="PNIListMain" runat="server" OnTabStripCommand="PNIListMain_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <div id="divFind" style="z-index: 2">
                    <table id="tblInspectionPNI" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                               <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" Enabled="false" />
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblMedicalCaseNo" runat="server" Text="Medical Case No"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtMedicalCaseNo" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblPIClub" runat="server" Text="P & I Club"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtPNIClub" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                    Width="80%"></asp:TextBox>
                                <asp:TextBox ID="txtPNIClubid" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                    Width="80%" Visible="false"></asp:TextBox>
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblReportedDate" runat="server" Text="Reported Date"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="txtReportedDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>                            
                            <td style="width: 15%">
                                <asp:Literal ID="lblESMOwner" runat="server" Text="Company/Owner"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Hard ID="ucESMOwner" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="176" AutoPostBack="true" OnTextChangedEvent="ucESMOwner_Changed" />
                            </td>
                            <td style="width: 15%"> 
                               <asp:Literal ID="lblPIRefNo" runat="server" Text="P & I Ref.No"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtRefNo" CssClass="readonlytextbox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:literal ID="lblCrewDeductible" runat="server" Text="Crew Deductible"></asp:literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtDeductible" CssClass="readonlytextbox" ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 15%">
                              <asp:Literal ID="lblQualityInCharge" runat="Server" Text=" Quality In-Charge"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtQualityincharge" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                    Width="80%"></asp:TextBox>
                                <asp:TextBox ID="txtQualityinchargeid" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                    Width="80%" Visible="false"></asp:TextBox>
                            </td>
                        </tr>  
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:LinkButton ID="lnkPNIChecklist" runat="server" Text="PNI Checklist" Font-Bold="true" ></asp:LinkButton>
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

