<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTrainingList.aspx.cs" Inherits="CrewTrainingList" %>

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
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Training List</title> 
       
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>            
      </telerik:RadCodeBlock>
        
</head>
<body>
    <form id="frmCrewTrainingList" runat="server" submitdisabledcontrols="true">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlCrewTrainingListEntry">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew Training" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCrewTrainingList" runat="server" OnTabStripCommand="CrewTrainingList_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureCrewTraining" width="100%">
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblType" Text="Type" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Hard runat="server" ID="ucType" AutoPostBack="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ucType_Changed" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblCourseLicence" runat="server" Text="Course/Licence "></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Documents runat="server" ID="ucCourseLicence" DocumentType="COURSE" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                        </tr>
                         <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucStatus" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblInstitution" runat="server" Text="Institution "></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Institution runat="server" ID="ucInstitution" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblValidFromDate" runat="server" Text="Valid From Date"></asp:Literal>
                            </td>
                            <td style="width: 30%"> 
                                <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" /> 
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblValidToDate" runat="server" Text="Valid To Date "></asp:Literal>
                            </td>
                            <td style="width: 30%">  
                                <eluc:Date runat="server" ID="txtToDate" CssClass="input" />                                
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks "></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox runat="server" ID="txtRemarks" Width="60%" TextMode="MultiLine" Height="50px" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Literal ID="lblArchieve" runat="server" Text="Archive Y/N"></asp:Literal>
                            </td>
                            <td align="left" style="width: 30%"> 
                                <asp:CheckBox runat="server" ID="chkArchiveYN" />  
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
