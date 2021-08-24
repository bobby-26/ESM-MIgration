<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockProjectAdd.aspx.cs"
    Inherits="DryDockProjectAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Project</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmProjectAdd" runat="server" autocomplete="off">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             
                    <eluc:TabStrip ID="MenuProject" runat="server" OnTabStripCommand="Project_TabStripCommand">
                    </eluc:TabStrip>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblProjectID" runat="server" Text="Project ID"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucCreatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTitle" runat="server" Width="480px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblDockingCategory" runat="server" Text="Docking Category"></telerik:RadLabel>
                            
                        </td>
                       <td>
                            <telerik:RadDropDownList ID="ddlCategory" runat="server" CssClass="input_mandatory" DataTextField="FLDCATEGORYNAME"
                                DataValueField="FLDCATEGORYID">
                            </telerik:RadDropDownList>
                       </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblFromDate" runat="server" Text="Est From"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblToDate" runat="server" Text="Est To"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblActualStartDate" runat="server" Text="Actual Start Date"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="txtStartDate" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text="Actual Finish Date"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="txtFinishDate" runat="server"  DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblHFOCostMT" runat="server" Text="HFO Cost/MT"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <table width="70%">
                                <tr>
                                    <td>
                                        <eluc:Number ID="txtHFOCost" runat="server" MaxLength="8" />
                                    </td>
                                    <td>
                           <telerik:RadLabel ID="lblHFOConsumptionDay" runat="server" Text="HFO Consumption/Day"></telerik:RadLabel>
                                        
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHFOConsumption" runat="server" MaxLength="8" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblMDOCostMT" runat="server" Text="MDO Cost/MT"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <table width="70%">
                                <tr>
                                    <td>
                                        <eluc:Number ID="txtMDOCost" runat="server" MaxLength="8" />
                                    </td>
                                    <td>
                           <telerik:RadLabel ID="lblMDOConsumptionDay" runat="server" Text="MDO Consumption/Day"></telerik:RadLabel>
                                        
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtMDOConsumption" runat="server" MaxLength="8" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ucOrderStatus" runat="server" CssClass="input_mandatory" DataTextField="FLDDESCRIPTION"
                                DataValueField="FLDDRYDOCKSTATUS">
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblPurchaseWindowMonths" runat="server" Text="Purchase Window (Months)"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="ucWindowPeriod" MaxLength="2" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRemarks" runat="server"  TextMode="MultiLine"
                                Height="60px" Width="320px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
        
           
            <eluc:Status ID="ucStatus" runat="server" />
        
 
    </form>
</body>
</html>
