<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRATemplateMapping.aspx.cs"
    Inherits="InspectionRATemplateMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
                       <asp:Literal ID="lbltemplateMapping" runat="server" Text="Template Mapping"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand">
                    </eluc:TabStrip>
                </div>
               
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 10%">
                           <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" CssClass="input" MaxLength="100" Width="80%"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>                           
                            <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 50%; height: 60px;">
                                <asp:CheckBoxList ID="chkVeselTypeList" runat="server" Height="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="BindCheckBox" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucAddrOwner" AddressType='<%# Enum.GetName(typeof(SouthNests.Phoenix.Common.PhoenixAddressType),Convert.ToInt32(Eval("OWNER"))) %>'
                                Width="80%" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" OnTextChangedEvent="BindCheckBox" />
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblPrinciple" runat="server" Text="Principle"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input"
                                AppendDataBoundItems="true" Width="80%" AutoPostBack="true" OnTextChangedEvent="BindCheckBox" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkCheckAll" runat="server" Text="Check All" AutoPostBack="true" OnCheckedChanged="SelectAll"/>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRATemplate" runat="server" OnTabStripCommand="MenuRATemplate_TabStripCommand">
                    </eluc:TabStrip>
                       
                </div>
              
                <div>
                    <asp:CheckBoxList ID="cblVesselName" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                        RepeatColumns="6">
                    </asp:CheckBoxList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
