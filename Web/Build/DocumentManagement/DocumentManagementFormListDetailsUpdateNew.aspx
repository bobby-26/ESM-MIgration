<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormListDetailsUpdateNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementFormListDetailsUpdateNew" %>

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
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" Title="Form Design Details" OnTabStripCommand="MenuCommentsEdit_TabStripCommand"></eluc:TabStrip>
        <br />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table width="100%">
                <tr>
                    <td widith="10%">
                        <telerik:RadLabel ID="lblFormNumber" runat="server" Text="Form No."></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="gridinput" TextMode="SingleLine"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblFormName" runat="server" Text="Form Name"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="gridinput" TextMode="SingleLine"
                            Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rListAdd" runat="server" RepeatDirection="Horizontal" ValidationGroup="type">
                            <asp:ListItem Text="Desined Form" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Upload" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListCategory">
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="250px" CssClass="input">
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
                        <telerik:RadLabel ID="lblActiveyn" runat="server" Text="Active (Y/N)"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActiveYN" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="gridinput" TextMode="Multiline"
                            Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDisplayfms" runat="server" Text="Display in FMS"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="radchckfmsyn" runat="server" AutoPostBack="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="gridinput" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShipDept" runat="server" Text="Ship Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucGroupEdit" runat="server" AppendDataBoundItems="true" HardTypeCode="51"
                            SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>' Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOfficeDept" runat="server" Text="Office Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department ID="ucOfficeDept" runat="server" Width="200px" AppendDataBoundItems="true"
                            CssClass="input"></eluc:Department>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblGroupRank" runat="server" Text="Team Composition"></asp:Literal>
                    </td>
                    <td>

                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlgroupfunction" runat="server"
                            DataTextField="FLDFUNCTIONNAME" DataValueField="FLDFUNCTIONID" HighlightTemplatedItems="true" DropDownWidth="400" EnableScreenBoundaryDetection="true" Width="300"
                            EmptyMessage="select from the dropdown " EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                            EnableVirtualScrolling="true" DropDownCssClass="virtualRadComboBox">
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
                    <td colspan="2">

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountryPort" runat="server" Text="Country/Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucPort" runat="server" CssClass="gridipput" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTiming" runat="server" Text="Time Interval"></telerik:RadLabel>
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
                    <td>
                        <telerik:RadLabel ID="lblPMSComponent" runat="server" Text="PMS Component"></telerik:RadLabel>
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
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px" OnTextChanged="txtComponentId_TextChanged">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEquipment" runat="server" Text="Equipment Maker/Model"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Equipmentmake ID="ucEquipment" runat="server" CssClass="gridipput" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJob" runat="server" Text="Job Mapping"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divjobs" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="cbJobs" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblProcedure" runat="server" Text="Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="dvPROCEDURE" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblPROCEDURE" runat="server">
                            </table>
                        </div>
                    </td>

                    
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblra" runat="server" Text="RA">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <div id="dvRA" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblRA" runat="server">
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJHA" runat="server" Text="JHA"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="dvJHA" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblJHA" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblimage" runat="server" Text="Image"></telerik:RadLabel>
                    </td>
                    <td>
                         <asp:Image ID="imgPhoto" runat="server" Height="50px" 
                                Width="120px" />

                        <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                    <td colspan="2"></td>
                </tr>           
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
