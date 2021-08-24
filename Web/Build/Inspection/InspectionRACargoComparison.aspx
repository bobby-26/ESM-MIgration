<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACargoComparison.aspx.cs" Inherits="InspectionRACargoComparison" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Generic</title>
    <div runat="server" id="Div1">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
    <style type="text/css">
        table.Hazard
        {
            border-collapse: collapse;
        }
        table.Hazard td, table.Hazard th
        {
            border: 1px solid black;
            padding: 5px;
        }
    </style>

    <script language="javascript" type="text/javascript">
    function TxtMaxLength(text,maxLength)
    {
       text.value = text.value.substring(0,maxLength);
    }
    </script>

</head>
<body>
    <form id="frmGeneric" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGen" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <asp:UpdatePanel runat="server" ID="pnlGeneric">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                <div id="tblGeneric">
                    <table border="1" width="100%" cellpadding="1" cellspacing="1" id="tblGen">
                        <tr>
                            <td>
                                
                            </td>
                            <td>
                                <asp:Label ID="lblstandardtemplate" runat="server" Text="Standard Template" Font-Bold="true"></asp:Label>                                
                            </td>
                            <td>
                                <asp:Label ID="lbltemplate" runat="server" Text="Amended" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblnumber" runat="server" Text="Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtstnumber" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                               <asp:TextBox ID="txtgenericnumber" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>                      
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblReason" runat="server" Text="Reason for Assessment"></asp:Label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstReason" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtReason" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblalternativework" runat="server" Text="Alternative Method Considered for Work "></asp:Label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstalternativework" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtalternativework" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                           
                        </tr>
                        <tr>
                            <td>
                                <asp:label ID="lblnoofpeople" runat="server" Text="No of people involved in activity / Affected"></asp:label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtstnoofpeople" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtnoofpeople" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>    
                        
                        <tr>
                            <td>
                                <asp:label ID="lblduration" runat="server" Text="Duration of work / activity"></asp:label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtstduartaion" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtduartaion" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>                         
                        <tr>
                            <td>
                                <asp:label ID="lblFrequency" runat="server" Text="Frequency of work / activity"></asp:label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtstFrequency" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="360px" CssClass="input" ID="txtFrequency" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr> 
                        
                        <tr>
                            <td>
                                <asp:label ID="lblRisks" runat="server" Text="Risks/Aspects"></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstRisks" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtRisks" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                        </tr> 
                         <tr>
                            <td>
                                <asp:label ID="lblhealth" runat="server" Text="Health and Safety Impact"></asp:label>
                            </td>
                             <td>
                                <eluc:CustomEditor ID="txtsthealth" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txthealth" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                             
                        </tr>                            
                        <tr>
                            <td>
                                <asp:label ID="lblEnvironmental" runat="server" Text="Environmental Impact"></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstEnvironmental" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtEnvironmental" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                             
                        </tr>                         
                        <tr>
                            <td>
                                <asp:label ID="lblEconomic" runat="server" Text="Economic Impact"></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstEconomic" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtEconomic" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                            
                        </tr> 
                        
                        <tr>
                            <td>
                                <asp:label ID="lblWorstCase" runat="server" Text="Worst Case Scenario"></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstWorstCase" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtWorstCase" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                             
                        </tr>
                        
                        <tr>
                            <td>
                                <asp:label ID="lblProposedControls" runat="server" Text="Proposed Controls To Reduce Risk "></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstProposedControls" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtProposedControls" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                            
                        </tr> 
                        
                        <tr>
                            <td>
                                <asp:label ID="lblAdditionalSafety" runat="server" Text="Additional Safety Procedures (Emergency)"></asp:label>
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtstAdditionalSafety" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                            <td>
                                <eluc:CustomEditor ID="txtAdditionalSafety" runat="server" Width="100%" Height="100px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>                                                   
                        </tr>                  
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>