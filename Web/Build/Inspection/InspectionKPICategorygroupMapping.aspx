<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionKPICategorygroupMapping.aspx.cs" Inherits="Inspection_InspectionKPICategorygroupMapping" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmTemplateMapping" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlTemplateMapping">
        <ContentTemplate>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                  <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        <eluc:Title ID="ucTitle" runat="server" Text="Map Category" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblevent" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtevent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></asp:TextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblsource" runat="server" Text="External Audit"></asp:Literal>
                        </td>
                        <td>
                            <div id="divSource" runat="server" class="input" style="overflow-y:auto; overflow-x:auto; width: 100%;
                                height: 100%">
                                
                                <asp:CheckBoxList ID="cblextaudit" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="Internal Audit"></asp:Literal>
                        </td>
                        <td>
                            <div id="div2" runat="server" class="input" style="overflow-y:auto; overflow-x:auto; width: 100%;
                                height: 100%">
                                <asp:CheckBoxList ID="cblintaudit" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="Vetting"></asp:Literal>
                        </td>
                        <td>
                            <div id="div3" runat="server" class="input" style="overflow-y:auto; overflow-x:auto; width: 100%;
                                height: 100%">
                                <asp:CheckBoxList ID="cblextvetting" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                  
                 
                 
                     
                    <tr></tr>
                     
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

