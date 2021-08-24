<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockGeneralServiceAdd.aspx.cs" Inherits="DryDockGeneralServiceAdd" %>

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
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
             
        <script type="text/javascript" language="javascript">
            function fnJobEdit(jobid) {
                location.href = 'DryDockGeneralService.aspx?GENERALSERVICEID=' + jobid;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmGeneralServiceAdd" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
              
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              
               
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                   
                        <eluc:TabStrip ID="MenuGeneralServicesSpecification" runat="server" OnTabStripCommand="GeneralServicesSpecification_TabStripCommand">
                        </eluc:TabStrip>
                  
                </div>
                <table width="100%" cellpadding="1" cellspacing="3">
                    <tr>                        
                        <td>
                           <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ucJobType" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" DefaultMessage="--Select--">
                                
                            </telerik:RadDropDownList>
                        </td>
                        <td rowspan="9">
                           <%-- <b>Include</b>--%>
                            <telerik:RadCheckBoxList ID="cblInclude" runat="server" RepeatDirection="Vertical" Visible="false">
                            </telerik:RadCheckBoxList>
                        </td>                           
                    </tr>                
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                            
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
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                                Width="360px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                           <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                            
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                                TextMode="MultiLine" Rows="6"></asp:TextBox>
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlResponsibilty" runat="server" CssClass="input" AppendDataBoundItems="true" DefaultMessage="--Select--"/>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                           <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration (Hrs)"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="txtDuration" CssClass="input" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"  DefaultMessage="--Select--">
                               
                            </telerik:RadDropDownList>
                        </td>
                    </tr>                      
                    <tr runat="server" visible="false">
                        <td>
                           <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadCheckBoxList ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadCheckBoxList>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                           <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadCheckBoxList ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                            </telerik:RadCheckBoxList>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">                        
                        <td>
                           <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadCheckBoxList ID="cblEnclosed" runat="server" dataRepeatDirection="Horizontal">
                            </telerik:RadCheckBoxList>
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
                
                </div>
                 <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status ID="ucStatus" runat="server" />
           
  
    </form>
</body>
</html>
