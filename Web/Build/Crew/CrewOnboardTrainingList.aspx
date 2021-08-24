<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOnboardTrainingList.aspx.cs"
    Inherits="CrewOnboardTrainingList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OnboardTrainingTopic" Src="~/UserControls/UserControlOnboardTrainingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Onboard Training List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewOnboardTrainingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureCrewOnboardTrainingList" width="100%">
                <tr>
                    <td style="width: 10%">                        
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Vessel runat="server" ID="ucVessel" Width="40%" AppendDataBoundItems="true" Enabled="false"
                            EntityType="VSL" />                    
                    </td>
                </tr>
                <tr>
                    <td>                        
                        <telerik:RadLabel ID="lblNameofthetraining" runat="server" Text="Name of the Training"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OnboardTrainingTopic runat="server" ID="ucSubject"  Width="40%" Enabled="false"
                            AutoPostBack="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td width="30%">
                        <telerik:RadLabel ID="lblTrainingNamenotinlist" runat="server" Text="Name of the Training (If not there in the Training List)"></telerik:RadLabel>                                                    
                    </td>
                    <td>                        
                        <telerik:RadTextBox ID="txtSubjectName" Width="40%" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                         <telerik:RadLabel ID="lblFromdate" runat="server" Text="From Date"></telerik:RadLabel>                        
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtFromDate" Width="40%" CssClass=""
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">                        
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date" ></telerik:RadLabel>    
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtToDate" Width="40%"  ReadOnly="true" CssClass="" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>                           
                    </td>
                    <td style="width: 30%;">                        
                        <telerik:RadTextBox ID="txtDuration" Width="40%" runat="server" Style="text-align: right" ReadOnly="true"></telerik:RadTextBox>
                                             
                        <telerik:RadLabel ID="lblHrs" runat="server" Text="Hrs"></telerik:RadLabel>                       
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">                        
                            <telerik:RadLabel ID="lblTrainerRank" runat="server" Text="Training Conducted By"></telerik:RadLabel>    
                    </td>
                    <td colspan="2">

                        <telerik:RadTextBox runat="server" ID="txtTrainerName" Width="40%"  ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtTrainerRank" Width="40%"  ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblTrainerNameIfnotthereintheTrainingConductedByList" runat="server" Text="Trainer Name (If not there in the Training Conducted By List)"></telerik:RadLabel>                            
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTrainerNameNotinList" runat="server" Width="40%"  ReadOnly="true"> </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>                         
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRemarks" Width="82%" TextMode="MultiLine" Height="50px" ReadOnly="true"
                            ></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
