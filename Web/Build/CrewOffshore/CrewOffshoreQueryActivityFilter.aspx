<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreQueryActivityFilter.aspx.cs" Inherits="CrewOffshoreQueryActivityFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Src="../UserControls/UserControlNationalityList.ascx" TagName="UserControlNationalityList"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlZone.ascx" TagName="UserControlZone"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="UserControlRank"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlEngineType.ascx" TagName="UserControlEngineType"   TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlDocuments.ascx" TagName="UserControlDocuments"   TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlDate.ascx" TagName="UserControlDate"   TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="OtherCompany" Src="~/UserControls/UserControlOtherCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register Src="~/UserControls/UserControlPoolList.ascx" TagName="UserControlPool"
    TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="UserControlVessel"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand">
        </eluc:TabStrip>        
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNewApplicantEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td colspan="4">
                            <font color="blue"><b>Note: </b>For embeded search, use '%' symbol. (Eg. Name: %xxxx)</font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" Text="Name" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="200px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                             <asp:Literal ID="lblFileNumber" Text="File Number" runat="server"></asp:Literal>
                          
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileNumber" runat="server" CssClass="input" Width="200px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Literal ID="lblPassportNumber" Text="Passport Number" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassortNo" runat="server" CssClass="input" Width="200px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCDCNumber" Text="CDC Number" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeamanbookNo" runat="server" CssClass="input" Width="200px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAppliedBetween" Text="Applied Between" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtAppliedStartDate" CssClass="input" Width="80px" runat="server" />
                            &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtAppliedEndDate" CssClass="input" Width="80px" runat="server" />
                        </td>
                        <td>
                             <asp:Literal ID="lblZone" Text="Zone" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                             <asp:Literal ID="lblDateofAcailability" Text="Date of Availability Between" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtDOAStartDate" CssClass="input" Width="80px" runat="server" />
                            &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtDOAEndDate" CssClass="input" Width="80px" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDateofBirth" Text="Date of Birth Between" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtDOBStartDate" CssClass="input" Width="80px" runat="server" />
                            &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtDOBEndDate" CssClass="input" Width="80px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                             <asp:Literal ID="lblPresentRank" Text="Rank" runat="server"></asp:Literal>
                         
                        </td>
                        <td>
                            <eluc:UserControlRankList ID="lstRank" runat="server" CssClass="input" />
                            <br />
                            <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblNationality" Text="Nationality" runat="server"></asp:Literal>
                          
                        </td>
                        <td>
                            <eluc:UserControlNationalityList ID="lstNationality" runat="server" CssClass="input" />
                            <br />
                            <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCountry" Text="Country" runat="server"></asp:Literal>
                           
                        </td>
                        <td colspan="3">
                            <eluc:Country ID="ddlCountry" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                                CssClass="input" OnTextChangedEvent="ddlCountry_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblState" Text="State" runat="server"></asp:Literal>
                         
                        </td>
                        <td>
                            <eluc:State ID="ddlState" CssClass="input" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                        </td>
                        <td>
                             <asp:Literal ID="lblCity" Text="City" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblBatch" Text="Batch" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:BatchList ID="lstBatch" AppendDataBoundItems="true" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselSailed" Text="Vessel Sailed" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                                CssClass="input" VesselsOnly="true" />
                        </td>
                        <td>
                             <asp:Literal ID="lblPool" Text="Pool" Visible="false" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:UserControlPool ID="ddlPool" Visible="false" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td>
                            Name(nok/family)
                        </td>--%>
                        <td>
                            <asp:TextBox ID="txtNOKName" runat="server" CssClass="input" Width="200px" MaxLength="200"
                                Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                            <b><asp:Literal ID="lblExperience" Text="Experience" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSailedRank" Text="Sailed Rank" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlRank ID="ddlSailedRank" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselType" Text="Vessel Type" runat="server"></asp:Literal>
                            
                        </td>
                        <td rowspan="2">
                            <eluc:UserControlVesselTypeList ID="ddlVesselType" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                            <br />
                            <asp:CheckBox ID="chkIncludepastexp" runat="server" Text="Include past experience" />
                            <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEngineType" Text="Engine Type" runat="server"></asp:Literal>
                            
                        </td>
                        <td colspan="2">
                            <eluc:UserControlEngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <font color="blue">Note:if checked, the filter will search for vessel type experiece
                            in full past experience,if not will check only last vessel/onboard vessel
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                            <b><asp:Literal ID="lblDocuments" Text="Documents" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" Text="Course" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlDocuments ID="ddlCourse" runat="server" DocumentType="COURSE" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblLicenses" Text="Licenses" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlDocuments ID="ddlLicences" runat="server" DocumentType="LICENCE"
                                AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVisa" Text="Visa" runat="server"></asp:Literal>
                           
                        </td>
                        <td>
                            <eluc:Country ID="ddlVisa" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td> 
                        <td>
                            <asp:Literal ID="lblFlag" Text="Flag" runat="server" Visible="true"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Flag ID="ddlFlag" runat="server" CssClass="input" Visible="true" AppendDataBoundItems="true" FlagList='<%#PhoenixRegistersFlag.ListFlag() %>' />
                        </td>                          
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPreviousCompany" Text="Previous Company" runat="server"></asp:Literal>
                           
                        </td>
                        <td colspan="3">
                            <eluc:OtherCompany ID="ddlPrevCompany" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActiveInActive" Text="Active / In-Active" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlInActive" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="rblInActive_SelectedIndex">
                                <asp:ListItem Value="">All</asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">In-Active</asp:ListItem>
                            </asp:DropDownList>
                          <%--  <asp:RadioButtonList ID="rblInActive" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblInActive_SelectedIndex">
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>--%>
                        </td>
                        <td>
                            <asp:Literal ID="lblStatus" Text="Status" runat="server"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:ListBox ID="lstStatus" runat="server" CssClass="input" SelectionMode="Multiple"
                                DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"></asp:ListBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
