<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPlannedMaintainanceSFI.aspx.cs" Inherits="Registers_RegisterPlannedMaintainanceSFI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedTextBox" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Global-SFI</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                sender.set_height(browserHeight - 05);
                $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 35);
            }
        window.onresize = window.onload = PaneResized;
        function pageLoad() {            
            PaneResized();
        }
        document.onkeydown = function (e) {
            var keyCode = (e) ? e.which : event.keyCode;
            if (keyCode == 13) {
                __doPostBack("<%=btnSearch.UniqueID %>", "");
            }
        }
        function frameGridResize() {
            var frm = document.getElementById('ifMoreInfo').contentWindow;
            if (frm.Resize != null) {
                setTimeout(function () {
                    frm.Resize();
                }, 200);                
            }
        }
    </script>
        </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
    <div>
         <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="100%" Width="100%">
             <telerik:RadPane ID="navigationPane" runat="server" Width="350" OnClientResized="PaneResized">
                <eluc:TabStrip ID="Menusearch" runat="server" OnTabStripCommand="Menusearch_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                 <div>
                  <div class="rdTreeFilter" runat="server" id="divTreeFilter">
                    <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearch" ID="treeViewSearch" runat="server" Width="100%" EmptyMessage="Type to search " />
                </div>
                     <div class="rdTreeScroll">
                        <telerik:RadTreeView RenderMode="Lightweight" ID="tvwSFI" runat="server" OnNodeDataBound="tvwSFI_NodeDataBound" 
                            OnNodeClick="tvwSFI_NodeClickEvent" AllowNodeEditing="false">
                            <ExpandAnimation Type="None" />
                            <CollapseAnimation Type="None" />
                            <DataBindings>
                                <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                            </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                     </div>
<%--                <eluc:TreeView ID="tvwSFI" runat="server" OnNodeClickEvent="tvwSFI_NodeClickEvent"  EmptyMessage="Type to search"/>--%>
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="hidden" />
             </telerik:RadPane>
             <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" >
                <div id="ifMoreInfo" >
     <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="Menu_SFI" runat="server" OnTabStripCommand="Menu_SFI_TabStripCommand"
                />

         <telerik:RadNotification ID="radnotificationstatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
         <table width="100%">
             <tr>
                 <td>
                     Number
                 </td>
                 <td>
                     
                     <eluc:MaskedTextBox MaskText="###.##.##" runat="server" ID="tbsfinumber" Width="150px" CssClass="input_mandatory" />
                 </td>
            
             
                 <td>
                     Name
                 </td>
                 <td>
                     <telerik:RadTextBox ID="tbradsfiname" runat="server"  CssClass="input_mandatory" Width="150px"/>
                 </td>
             </tr>
             <tr>
                 <td>
                    Parent Number
                 </td>
                 <td>
                                          <eluc:MaskedTextBox MaskText="###.##.##" runat="server" ID="tbradsfiparentnumber" Width="150px" CssClass="input_mandatory" />

                 </td>
            
                 <td>
                     Category
                 </td>
                 <td>
                     <telerik:RadComboBox ID="radcbsficategory" runat="server" CssClass="input_mandatory" Width="150px"/>
                 </td>
             </tr>
             <tr>
                 <td>
                     Type
                 </td>
                 <td>
                     <eluc:Hard ID="ddsfitype" runat="server" HardTypeCode="278" Enabled="false" CssClass="input_mandatory" Width="150px"/>
                 </td>
             </tr>
         </table>
                    </div>
                </telerik:RadPane>
             </telerik:RadSplitter>
    </div>
    </form>
</body>
</html>
