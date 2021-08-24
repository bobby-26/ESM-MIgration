<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJobGeneralAdd.aspx.cs" Inherits="DryDockJobGeneralAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
 
        <script type="text/javascript" language="javascript">
            function fnJobEdit(jobid) {
                location.href = 'DryDockJobGeneral.aspx?STANDARDJOBID=' + jobid;
            }
        </script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStandardJobGeneral" runat="server" autocomplete="off">
     <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
              
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              
             
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                
                        <eluc:TabStrip ID="MenuStandardJobSpecification" runat="server" OnTabStripCommand="StandardJobSpecification_TabStripCommand">
                        </eluc:TabStrip>
                   
                <table width="100%" cellpadding="1" cellspacing="3">
                    <tr>                        
                        <td>
                           <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList RenderMode="Lightweight" DefaultMessage="--Select--" ID="ucJobType" runat="server" CssClass="dropdown_mandatory">
                              
                            </telerik:RadDropDownList>
                        </td>
                        <td rowspan="9">
                            <b>
                           <telerik:RadLabel ID="lblInclude" runat="server" Text="Include"></telerik:RadLabel>
                            </b>
                            <telerik:RadListBox CheckBoxes="true" ID="cblInclude" runat="server" RepeatDirection="Vertical" >
                            </telerik:RadListBox  >
                        </td>                         
                    </tr>                
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="10" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox  RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                                Width="360px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                           <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                            
                        </td>
                        <td colspan="5">
                            <telerik:RadTextBox  RenderMode="Lightweight" ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                                TextMode="MultiLine" Rows="6"></telerik:RadTextBox>
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadListBox CheckBoxes="true" ID="cblResponsibilty" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                           <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration (Hrs)"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="txtDuration"  Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadDropDownList RenderMode="Lightweight" DefaultMessage="--Select--" ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                              
                            </telerik:RadDropDownList>
                        </td>
                    </tr>                                          
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadListBox RenderMode="Lightweight" CheckBoxes="true" ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadListBox RenderMode="Lightweight" CheckBoxes="true" ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                           <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadListBox RenderMode="Lightweight" CheckBoxes="true" ID="cblEnclosed" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblbudgetcode" runat="server" Text="Budgetcode"></telerik:RadLabel>
                        </td>
                        <td>
                           <telerik:RadComboBox runat="server" ID="radddlbudgetcode"  AllowCustomText
                                ="true" EmptyMessage="Type to select Budgetcode" Width="180px"/>
                        </td>
                    </tr>
                </table>
                
                
                 <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status ID="ucStatus" runat="server" />
           
 
    </form>
</body>
</html>
