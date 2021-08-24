<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsDocumentNumberEntry.aspx.cs"
    Inherits="OptionsDocumentNumberEntry" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="../UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmDocumentNumberEntry" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDocumentNumberEntry">
        <ContentTemplate>
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Document Number" ShowMenu="false"></eluc:Title>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="Menudocument" runat="server" OnTabStripCommand="Menudocument_TabStripCommand">
                </eluc:TabStrip>
                
            </div>
            <br clear="all" />
            <div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblDocumentType" runat="server" Text="Document Type :"></asp:Literal>
                        </td>
                        <td colspan="3">
                                   <asp:DropDownList ID="ddlDocumentType" runat="server" Height="22px" Width="250px"
                                    AppendDataBoundItems="True" DataTextField="FLDDOCUMENTTYPE" DataValueField="FLDCODE">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTestNumber" runat="server" Text="Test Number:"></asp:Literal>
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="txtRules" runat="server" Width="286px" ReadOnly="True" TextMode="MultiLine"
                                Style="text-align: left"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFieldList" runat="server" Text="Field List"></asp:Literal>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2" style="text-align: center">
                            <b><asp:Literal ID="lblSelectedFieldList" runat="server" Text="Selected Field List"></asp:Literal> </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbColumnsList" runat="server" Height="192px" Width="170px" Style="font-family: Arial;
                                font-size: x-small;">
                                <asp:ListItem Value="FLDCOMPANYSHORTCODE">Company Short Code</asp:ListItem>
                                <asp:ListItem Value="FLDCURRENCY">Currency</asp:ListItem>
                                <asp:ListItem Value="FLDDOCUMENTSHORTCODE">Document Short Code</asp:ListItem>
                                <asp:ListItem Value="FLDTOTAL">Total</asp:ListItem>
                                <asp:ListItem Value="FLDVESSELSHORTCODE">Vessel Short Code</asp:ListItem>
                                <asp:ListItem Value="FLDYEARFORMATE">Year(2)</asp:ListItem>
                                <asp:ListItem Value="FLDYEARFORMATE4">Year(4)</asp:ListItem>
                            </asp:ListBox>
                        </td>
                        <td colspan="2" valign="top" align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblCustomText" runat="server" Text="Custom Text"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCustom" runat="server" Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCustom" runat="server" Text="Add Custom" Height="26px" OnClick="btnCustom_Click"
                                            Width="77px" Style="font-size: xx-small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text="&gt;" Height="26px" OnClick="btnAdd_Click"
                                            Width="48px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnRemove" runat="server" Text="&lt;" OnClick="btnRemove_Click" Width="49px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnClearAll" runat="server" Text="&lt;&lt;" OnClick="btnClearAll_Click"
                                            Width="55px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:ListBox ID="lbColumnsAssign" runat="server" Height="186px" Width="197px" Style="font-family: Arial;
                                font-size: x-small;"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSerialNumber" runat="server" Text="Serial Number :"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtserial" runat="server" Width="84px"></asp:TextBox>
                        </td>
                        <td colspan="2" style="text-align: center">
                            <asp:CheckBox ID="CheckBox1" runat="server" Style="font-weight: 700" Text="Is Initial Values in Year" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Visible="false" Text="Save" Width="66px"
                                            OnClick="btnSave_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" Visible="false" runat="server" Text="Cancel" Width="65px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnTest" runat="server" Text="Test Document" OnClick="btnTest_Click"
                                            Visible="False" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:HiddenField ID="hdnSeparator" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:HiddenField ID="hdnColumns" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
