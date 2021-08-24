<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffShoreReliefPlanFilter.aspx.cs"
    Inherits="CrewOffShoreReliefPlanFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationalityList" Src="../UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZoneList" Src="../UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselTypeList" Src="../UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPoolList" Src="../UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlPortList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Applicant Query Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="DivHeader" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="PlanRelieveeFilterMain" runat="server" OnTabStripCommand="PlanRelieveeFilterMain_TabStripCommand"></eluc:TabStrip>

            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server"  />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromTo" runat="server"  AutoPostBack="true" OnTextChangedEvent="CalulateDays" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server"  Width="150px" MaxLength="200"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRelefDuePlanRelief" runat="server" Text="Relief Due / Plan Relief"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtReliefDue" runat="server" CssClass="input txtNumber" MaxLength="3"
                                IsInteger="true" AutoPostBack="false" OnTextChangedEvent="CalculateDatetime" Width="150px" />
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="(Days)"></telerik:RadLabel>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" 
                                AutoPostBack="true" OnTextChangedEvent="ucPrincipal_TextChangedEvent" AppendDataBoundItems="true"
                                Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" Width="150px"  VesselsOnly="true" AppendDataBoundItems="true"
                                AssignedVessels="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlVesselTypeList ID="lstVesselType" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="false"  OnTextChangedEvent="ucPrincipal_TextChangedEvent" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlZoneList ID="ucZone" Width="150px" AppendDataBoundItems="true" runat="server"
                                 />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server"  AppendDataBoundItems="true" Width="150px"  />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlPoolList ID="ucPool" AppendDataBoundItems="true" runat="server" Width="150px"
                                 />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeStatus" runat="server" Text="Employee Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlEmployeeStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="150px"
                                OnTextChanged="ddlEmployeeStatus_TextChanged" Filter="Contains" EmptyMessage="Type to slect status" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOffshoreStage" runat="server" Text="Offshore Stage"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlOffshoreStage" runat="server" AppendDataBoundItems="true" HardTypeCode="99"
                                Width="150px"  />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExpectedOn" runat="server" Text="Expected Joining From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtExpectedDate" runat="server"  />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExpectedto" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtExpectedtoDate" runat="server"  />
                        </td>
                        <%--<td colspan="2">
                                <table>
                                    <tr>
                                        
                                       
                                         
                                    </tr>
                                </table>
                               
                            </td>--%>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblJoiningPort" runat="server" Text="Joining Port"></telerik:RadLabel>
                            </td>
                            <td><%-- <eluc:Port ID="ucPort" runat="server"  AppendDataBoundItems="true"
                                    Width="80%" />--%>
                                <eluc:MultiPort ID="ucPort" runat="server"  Width="150px" />
                            </td>
                        </tr>

                    </tr>

                </table>

            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
