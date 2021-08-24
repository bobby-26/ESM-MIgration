<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilyExperience.aspx.cs"
    Inherits="CrewFamilyExperience" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Family Experience</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmFamilyExperience" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuFamilyExperience" runat="server" OnTabStripCommand="CrewFamilyExperience_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <table cellpadding="3" cellspacing="3" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            VesselsOnly="true" EntityType="VSL" ActiveVessels="true" Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" Width="40%" 
                            AutoPostBack="true" OnTextChangedEvent="FilterSeaport" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign-On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign-On Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="40%" 
                            OnTextChangedEvent="CalculateReliefDue" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDurationMonths" runat="server" Text="Duration (Months)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDuration" runat="server" CssClass="input_mandatory txtNumber" MaskText="###" Width="40%" 
                            OnTextChangedEvent="CalculateReliefDue" AutoPostBack="true" MaxLength="3" IsInteger="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReliefDueDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="40%" 
                            OnTextChangedEvent="CalculateReliefDue" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnReason" runat="server" Text="Sign-On Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOnReason ID="ddlSignOnReason" runat="server" AppendDataBoundItems="true" Width="40%" 
                            CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOnRemarks" runat="server" Text="Sign-On Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignonRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="40%" 
                            MaxLength="200" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="Sign-Off Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory"  Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign-Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOffPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="40%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign-Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true" Width="40%" 
                            CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffRemarks" runat="server" Text="Sign-Off Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignOffRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="40%" 
                            MaxLength="200" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />            
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
