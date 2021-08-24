<%@ Page Language="C#" AutoEventWireup="True" CodeFile="StandardformFBformCategory.aspx.cs"
    Inherits="StandardformFBformCategory" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                sender.set_height(browserHeight - 40);
                $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersFormCategory" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <asp:Literal ID="lblCategory" runat="server" Text="Category" Visible="false"></asp:Literal>
        <eluc:TabStrip ID="MenuSecurityLocation" runat="server" OnTabStripCommand="SecurityLocation_TabStripCommand">
        </eluc:TabStrip>
            <eluc:TabStrip ID="MenuExport" runat="server" OnTabStripCommand="MenuExport_TabStripCommand">
            </eluc:TabStrip>
           
                        Show All &nbsp&nbsp
                        <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowAll_CheckedChanged" />
                 
             
                    <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server"  Width="100%" Height="100%">
                        <telerik:RadPane ID="navigationPane" runat="server" Width="200" Height="97%">
                            <eluc:TreeView runat="server" ID="tvwCategory" OnNodeClickEvent="ucTree_SelectNodeEvent">
                            </eluc:TreeView>
                            <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                        </telerik:RadPane>
                        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                        </telerik:RadSplitBar>
                        
                  <telerik:RadPane ID="contentPane" runat="server">
        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
       
            <table width="100%" cellpadding="5">
                <tr>
                    <td width="50%">
                        <asp:Literal ID="lblCategoryCode" runat="server" Text="Code"></asp:Literal>
                        <telerik:RadLabel ID="lblCategoryID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMCATEGORYID") %>'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategoryCode" runat="server" CssClass="input" MaxLength="6">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCategoryname" runat="server" Text="Name"></asp:Literal>
                        <telerik:RadLabel ID="lblParentLocationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTCATEGORYID") %>'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMCATEGORYCODE") %>'
                            CssClass="input_mandatory" MaxLength="100" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblActive" runat="server" Text="Active"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkActive" runat="server" Checked="true" AutoPostBack="false">
                        </telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
