<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementAdminDocumentListNewUpdate.aspx.cs" Inherits="DocumentManagement_DocumentManagementAdminDocumentListNewUpdate" %>

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
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Equipmentmake" Src="~/UserControls/UserControlEquipmentMakerModel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


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
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" Title="Document Details Add" OnTabStripCommand="MenuCommentsEdit_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="100%">
            <tr>
                <td width="10%">S.No</td>
                <td width="40%">
                    <eluc:Number ID="txtSequenceNumberEdit" runat="server" CssClass="gridinput_mandatory"
                        IsInteger="true" MaxLength="3" IsPositive="true" Width="20%"></eluc:Number>
                </td>
                <td width="10%">Name</td>
                <td width="40%">
                    <telerik:RadTextBox ID="txtDocumentNameEdit" runat="server" CssClass="input_mandatory"
                        TextMode="SingleLine" Width="200px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Category</td>
                <td>
                    <span id="spnPickListCategory">
                        <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:ImageButton ID="btnShowCategory" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="Top" Text=".." />
                        <telerik:RadTextBox ID="txtCategoryidEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                </td>
                <td>Company</td>
                <td>
                    <eluc:Company ID="ucCompanyEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="198px" />
                </td>
            </tr>
            <tr>
                <td>Active (Y/N)                  
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked="true"></telerik:RadCheckBox>
                </td>
                <td>Published                 
                </td>
                <td>
                    <eluc:Date ID="ucPublishedDateEdit" runat="server" Width="25%" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE")) %>' />
                </td>
            </tr>
            <tr>
                <td>Whole Manual Read(Y/N)                  
                </td>
                <td colspan="3">
                    <telerik:RadCheckBox ID="cbWholeReadyn" runat="server" ></telerik:RadCheckBox>
                </td>
                
            </tr>
        </table>
    </form>
</body>
</html>
