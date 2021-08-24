<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormListDetailsUpdate.aspx.cs"
    Inherits="DocumentManagement_DocumentManagementFormListDetailsUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Equipmentmake" Src="~/UserControls/UserControlEquipmentMakerModel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Design Upload</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
       <style type="text/css">
        html .rcbHeader ul,
        html .rcbFooter ul,
        html .rcbItem ul,
        html .rcbHovered ul {
            margin: 0 !important;
            padding: 0 !important;
            width: 90% !important;
            display: inline-block;
            list-style-type: none;
        }
 
        html div.RadComboBoxDropDown .rcbItem > label,
        html div.RadComboBoxDropDown .rcbHovered > label {
            display: inline-block;
        }
    </style>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" Text="" />
    <eluc:TabStrip ID="MenuCommentsEdit" runat="server" Title="Form Design Details" OnTabStripCommand="MenuCommentsEdit_TabStripCommand">
    </eluc:TabStrip>
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <table width="80%">
            <tr>
                <td>
                    Form No.
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="gridinput" TextMode="SingleLine"
                        Width="150px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    Form Name
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="gridinput" TextMode="SingleLine"
                        Width="200px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Form Type
                </td>
                <td>
                    <asp:RadioButtonList ID="rListAdd" runat="server" RepeatDirection="Horizontal" ValidationGroup="type">
                        <asp:ListItem Text="Desined Form" Value="0"  ></asp:ListItem>
                        <asp:ListItem Text="Upload" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    Category
                </td>
                <td>
                    <span id="spnPickListCategory">
                        <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="input">
                        </telerik:RadTextBox>
                        <asp:ImageButton ID="btnShowCategory" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="Top" Text=".." />
                        <telerik:RadTextBox ID="txtCategoryid" runat="server" Width="0px" CssClass="hidden">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    Active (Y/N)
                </td>
                <td>
                    <asp:CheckBox ID="chkActiveYN" runat="server" />
                </td>
                <td>
                    Remarks
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="gridinput" TextMode="Multiline"
                        Width="200px">
                    </telerik:RadTextBox>
                </td>
            </tr>
             <tr>
                <td>
                    Display in FMS
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="radchckfmsyn" runat="server" AutoPostBack="false"/>
                </td>
               
            </tr>
            <tr>
                <td>
                    Company
                </td>
                <td>
                    <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="gridinput" />
                </td>
                <td>
                    Ship Department
                </td>
                <td>
                    <eluc:Hard ID="ucGroupEdit" runat="server" AppendDataBoundItems="true" HardTypeCode="51"
                        SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>' Width="200px" />
                    <%--   <eluc:Department ID="ucShipDept" DepartmentFilter="1" runat="server" Width="200px" AppendDataBoundItems="true" CssClass="input"></eluc:Department>--%>
                </td>
                <%--  <td>File No</td>
                <td>
                    <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
            </tr>
            <%--  <tr>
                <td>Primary Owner Ship</td>
                <td>
                    <eluc:Rank ID="ucRankPrimary" runat="server" Width="200px" AppendDataBoundItems="true" CssClass="input"></eluc:Rank>
                </td>
                <td>Secondary Owner Ship</td>
                <td>
                    <eluc:Rank ID="ucRankSecondary" runat="server" Width="200px" AppendDataBoundItems="true" CssClass="input"></eluc:Rank>
                </td>
            </tr>--%>
            <%--<tr>
                <td>Other Participants</td>
                <td>
                    <telerik:RadTextBox ID="txtOtherParticipants" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>
                <td>Primary Owner Office</td>
                <td>
                    <telerik:RadTextBox ID="txtPrimaryOffice" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>
            </tr>--%>
            <%--  <tr>--%>
            <%--<td>Secondary Owner Office</td>
                <td>
                    <telerik:RadTextBox ID="txtSecondaryOffice" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
            <%--<td>Ship Department</td>
                <td>
                    <eluc:Department ID="ucShipDept" runat="server" Width="200px" AppendDataBoundItems="true" CssClass="input"></eluc:Department>
                </td>
            </tr>--%>
            <tr>
                <td>
                    Office Department
                </td>
                <td>
                    <eluc:Department ID="ucOfficeDept" runat="server" Width="200px" AppendDataBoundItems="true"
                        CssClass="input"></eluc:Department>
                </td>
                <%--  <td>Ship Type</td>
                <td>
                    <telerik:RadTextBox ID="txtShipType" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
                <%-- </tr>
            <tr>--%>
                <td>
                    Country/Port
                </td>
                <td>
                    <eluc:MultiPort ID="ucPort" runat="server" CssClass="gridipput" Width="200px" />
                </td>
            </tr>
            <tr>
                <td>
                    Equipment Maker/Model
                </td>
                <td>
                    <eluc:Equipmentmake ID="ucEquipment" runat="server" CssClass="gridipput" Width="250px" />
                    <%-- <telerik:RadTextBox ID="txtEquMaker" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
                <td>
                    PMS Component
                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" Width="270px">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px">
                        </telerik:RadTextBox>
                    </span>
                    <%-- <span id="spnpicklistcomponent">
                    <telerik:RadTextBox ID="txtPMSCompWO" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox>
                      <asp:ImageButton ID="btnShowpmscomponent" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="Top" Text=".." />
                        <telerik:RadTextBox ID="txtpmsid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>--%>
                </td>
            </tr>
            <tr>
                <td>
                    Activity/Operation
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCategory" runat="server" Width="250px" AppendDataBoundItems="true"
                        DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" EmptyMessage="Type or select"
                        Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                    </telerik:RadComboBox>
                </td>
                <%-- <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
                <%--</tr>
            <tr>--%>
                <td>
                    Time Interval
                </td>
                <td>
                    <telerik:RadComboBox runat="server" AppendDataBoundItems="true" EmptyMessage="Select Timeinterval"
                        ID="txtTimeInterval" CssClass="input">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                            <telerik:RadComboBoxItem Text="Daily" Value="1" />
                            <telerik:RadComboBoxItem Text="Weekly" Value="2" />
                            <telerik:RadComboBoxItem Text="Monthly" Value="3" />
                            <telerik:RadComboBoxItem Text="quarterly" Value="4" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <%-- <td>Time Interval</td>
                <td>
                    <telerik:RadTextBox ID="txtTimeInterval" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblra" runat="server" Text="RA">
                    </telerik:RadLabel>
                </td>
                <%-- </td>
                <td>--%>
                <%--      <asp:LinkButton ID="txtRA" runat="server" Text="RA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                <%--  <br />--%>
                <%--                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 35%; height: 80px;">
                            <telerik:RadCheckBoxList ID="txtRA" runat="server" Height="99.9%" 
                                RepeatLayout="Flow" AutoPostBack="false" Columns="1" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>--%>
                <td>
                    <div id="dvRA" runat="server" style="height: 100px; width: 300px; border-width: 1px;
                        border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblRA" runat="server">
                        </table>
                    </div>
                </td>
                <td>
                    JHA
                </td>
                <td>
                    <%--   <td>--%>
                    <asp:LinkButton ID="lnkImportJHA" runat="server" Text="Import JHA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <div id="dvJHA" runat="server" style="height: 100px; width: 300px; border-width: 1px;
                        border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblJHA" runat="server">
                        </table>
                    </div>
                </td>
                <%--
                 <asp:LinkButton ID="lnkImportJHA" runat="server" Text="Import JHA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 300px; height: 80px;">
                            <telerik:RadCheckBoxList ID="chkImportedJHAList" runat="server" Height="99.9%"  OnSelectedIndexChanged="chkImportedJHAList_Changed"
                                RepeatLayout="Flow" AutoPostBack="true" Columns="1" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>
                        </td>--%>
                <%--
                <td>Procedure</td>
               
                       <td>
                                <div id="dvPROCEDURE" runat="server" style="height: 100px; width:300px ; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">

                        <table id="tblPROCEDURE" runat="server">
                        </table>
                    </div>
                        </td>--%>
                <%-- <telerik:RadTextBox ID="txtProcedure" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
                <%-- </tr>
            <tr>--%>
                <%--    <td >
                <telerik:RadLabel ID="lblra" runat="server" Text="RA"></telerik:RadLabel>
                </td>
               <%-- </td>
                <td>--%>
                <%--      <asp:LinkButton ID="txtRA" runat="server" Text="RA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                <%--  <br />--%>
                <%--                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 35%; height: 80px;">
                            <telerik:RadCheckBoxList ID="txtRA" runat="server" Height="99.9%" 
                                RepeatLayout="Flow" AutoPostBack="false" Columns="1" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>--%>
                <%--<td>
                    <div id="dvRA" runat="server" style="height: 100px; width:300px ; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblRA" runat="server">
                        </table>
                    </div>
                    </td>--%>
                <%--    </td>--%>
            </tr>
            <%-- <td>RA</td>
                <td>
            --%>
            <%--   <telerik:RadTextBox ID="txtRA" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
            <tr>
                <td>
                    Procedure
                </td>
                <td>
                    <div id="dvPROCEDURE" runat="server" style="height: 100px; width: 300px; border-width: 1px;
                        border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblPROCEDURE" runat="server">
                        </table>
                    </div>
                </td>

                         <td>
                    <asp:Literal ID="lblGroupRank" runat="server" Text="Team Composition"></asp:Literal>
                </td>
                             <td>
                
                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlgroupfunction" runat="server" 
     DataTextField="FLDFUNCTIONNAME" DataValueField="FLDFUNCTIONID" HighlightTemplatedItems="true" DropDownWidth="400" EnableScreenBoundaryDetection="true" Width="300"
    EmptyMessage="select from the dropdown " EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains"  EnableCheckAllItemsCheckBox="true" CheckBoxes="true" 
    EnableVirtualScrolling="true"  DropDownCssClass="virtualRadComboBox" >

       <HeaderTemplate>
      
        <ul>
            <li class="col2">Department</li>
            <li class="col2">Function</li>
        </ul>
    </HeaderTemplate>

        <ItemTemplate>
        <ul>
            <li class="col2">  
                <%# DataBinder.Eval(Container.DataItem, "FLDDEPARTMENTNAME")%> </li>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDFUNCTIONNAME")%> </li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>

                </td>


                <%--
                <td>JHA</td>
                <td>
                 <asp:LinkButton ID="lnkImportJHA" runat="server" Text="Import JHA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 300px; height: 80px;">
                            <telerik:RadCheckBoxList ID="chkImportedJHAList" runat="server" Height="99.9%"  OnSelectedIndexChanged="chkImportedJHAList_Changed"
                                RepeatLayout="Flow" AutoPostBack="true" Columns="1" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>
                        </td>--%>
                <%-- <td>
                                <div id="dvJHA" runat="server" style="height: 100px; width:300px ; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblJHA" runat="server">
                        </table>
                    </div>
                        </td>--%>
                <%--  <td>
                    <telerik:RadTextBox ID="txtJHA" runat="server" CssClass="gridinput" Width="200px"></telerik:RadTextBox></td>--%>
            </tr>
             <tr>
            <td>
                    <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListJob">
                        <telerik:RadTextBox ID="txtJobCode" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                            ReadOnly="false" Width="60px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtJobName" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                            ReadOnly="false" Width="210px">
                        </telerik:RadTextBox>
                        <img id="imgJob" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobId" runat="server" CssClass="input hidden" Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdJobClear_Click" />
                            <telerik:RadTextBox ID="txtcodeId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
