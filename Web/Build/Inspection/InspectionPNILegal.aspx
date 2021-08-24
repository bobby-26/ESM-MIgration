<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNILegal.aspx.cs" Inherits="InspectionPNILegal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Legal</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />        
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Legal & Quality" ShowMenu="false"></eluc:Title>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="LegalTabStrip" runat="server" OnTabStripCommand="LegalTabStrip_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="SubmitToAcc_Click"  OKText="Yes" CancelText="No" /> 
                <div id="divMain" runat="server" class="style1">
                    <asp:Panel ID="pnlLegall" runat="server" GroupingText="Legal">
                     <table width="98%" cellpadding="1" cellspacing="1">
                       <tr>
                            <td width="15%">
                                <asp:Literal ID="lblLegalDeptRemarks" runat="server" Text="Legal Dept. Remarks"></asp:Literal>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" 
                                    TextMode="MultiLine" Width="80%" Height="75px"></asp:TextBox>
                            </td>
                           <td width="15%">
                                <asp:literal ID="lbldeadLineDate" runat="server" Text="Deadline Date"></asp:literal>
                            </td>
                           <td width="35%">
                                <eluc:Date ID="ucDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                       </tr>
                       <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:LinkButton ID="lnkPNIChecklist" runat="server" Text="PNI Checklist" Font-Bold="true" ></asp:LinkButton>
                            </td>
                        </tr>    
                    </table> 
                   </asp:Panel>  
                   <br />
                   <asp:Panel ID="pnlQuality" runat="server" GroupingText="Quality" Visible="false">
                    <table id="tblInspectionPNI" width="100%">
                        <tr>
                            <td style="width: 15%">
                               <asp:Literal ID="lblPIClub" runat="server"> P & I Club</asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtPNIClub" CssClass="readonlytextbox" ReadOnly="true" runat="server" Width="80%"></asp:TextBox>
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblTobeReportedtoPI" runat="server">To be Reported to P & I?</asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:CheckBox ID="chkPNIReportyn" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblReportedDate" runat="server" Text="Reported Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtReportedDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td>
                               <asp:literal ID="lblESMOwner" runat="server" Text="Company/Owner"></asp:literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucESMOwner" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="176" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCrewDeductible" runat="server" Text="Crew Deductible"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeductible" CssClass="readonlytextbox" ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                            <td>
                               <asp:Literal ID="lblPIRefNo" runat="server" Text=" P & I Ref.No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefNo" CssClass="input_mandatory" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <asp:literal ID="lblQualityInCharge" runat="server" Text=" Quality In-Charge"></asp:literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQualityincharge" CssClass="readonlytextbox" ReadOnly="true" runat="server" Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                   </asp:Panel>                     
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
