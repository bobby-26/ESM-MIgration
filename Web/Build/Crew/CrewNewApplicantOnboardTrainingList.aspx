<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantOnboardTrainingList.aspx.cs" Inherits="CrewNewApplicantOnboardTrainingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OnboardTrainingTopic" Src="~/UserControls/UserControlOnboardTrainingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Onboard Training List</title> 
       
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>            
        </telerik:RadCodeBlock>
        
</head>
<body>
    <form id="frmCrewOnboardTrainingList" runat="server" submitdisabledcontrols="true">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlCrewOnboardTrainingListEntry">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew Onboard Training" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <%--<eluc:TabStrip ID="MenuCrewOnboardTrainingList" runat="server" OnTabStripCommand="CrewOnboardTrainingList_TabStripCommand">
                    </eluc:TabStrip>--%>
                </div>                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureCrewOnboardTrainingList" width="100%">
                        <tr>
                            <td style="width: 10%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" CssClass="readonlytextbox" EntityType="VSL" ActiveVessels="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date "></asp:Literal>
                            </td>
                            <td style="width: 30%">  
                                <eluc:Date runat="server" ID="txtFromDate" CssClass="readonlytextbox" ReadOnly="true" />                                   
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblToDate" runat="server" Text="To Date "></asp:Literal>
                            </td>
                            <td style="width: 30%">  
                                <eluc:Date runat="server" ID="txtToDate" CssClass="readonlytextbox" ReadOnly="true" />                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal runat="server" ID="lblDuration" Text="Duration"></asp:Literal>
                            </td>
                            <td style="width: 30%">  
                                <asp:TextBox ID="txtDuration" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>                              
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <asp:Literal ID="lblSubject" runat="server" Text="Subject"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:OnboardTrainingTopic runat="server" ID="ucSubject" CssClass="readonlytextbox" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubjectName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblTrainerRank" runat="server" Text="Trainer Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank runat="server" ID="ucRank" CssClass="readonlytextbox" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblTrainerName" runat="server" Text="Trainer Name"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox runat="server" ID="txtTrainerName" Width="60%" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox runat="server" ID="txtRemarks" Width="60%" TextMode="MultiLine" Height="50px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
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
