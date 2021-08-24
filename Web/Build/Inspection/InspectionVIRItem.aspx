<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVIRItem.aspx.cs" Inherits="InspectionVIRItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection VIR Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    
    <script language="Javascript">
         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode;
             if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                 return false;

             return true;
         }       
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionVIRItem" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="VIR Item"></eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuInspectionVIRItemMain" runat="server" OnTabStripCommand="MenuInspectionVIRItemMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <br />
        <div style="overflow: scroll; width: 30%; float: left; height: 450px;" id="divInspectionVIRItem">
            <%--<div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuOwnerBGroupExport" runat="server" OnTabStripCommand="OwnerBGroupExport_TabStripCommand">
                </eluc:TabStrip>
            </div>--%>
            <table style="float: left; width: 100%;">
                <tr style="position: absolute">
                    <eluc:TreeView runat="server" ID="tvwInspectionVIRItem" OnSelectNodeEvent="ucTree_SelectNodeEvent">
                    </eluc:TreeView>
                    <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                    <asp:Label ID="lblItemId" runat="server"></asp:Label>
                </tr>
            </table>
        </div>
        <eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divInspectionVIRItem" />
        <div style="position: relative; float: left; margin: 5px; width: auto">
            <table width="100%" cellpadding="5">
                <tr>
                    <td>
                        <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" CssClass="input_mandatory" Width="300" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblItemNumber" runat="server" Text="Item Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemNumber" runat="server" CssClass="input"
                            onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>
                        <asp:literal ID="lblActiveYN" runat="Server" Text="Active Y/N"></asp:literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActiveyn" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
