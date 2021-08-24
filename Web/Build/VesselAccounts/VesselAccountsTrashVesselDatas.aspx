<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsTrashVesselDatas.aspx.cs"
    Inherits="VesselAccountsTrashVesselDatas" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <div style="margin-left: auto; margin-right: auto; width: 100%;">
        <form id="frmProvisionNegativeCorrection" runat="server" autocomplete="off">
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <div class="navigation" id="navigation" style="margin-left: auto; margin-right: auto; width: 100%;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Trash Vessel Data" ShowMenu="false"></eluc:Title>
                    </div>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFormTrash" runat="server" OnTabStripCommand="MenuFormTrash_TabStripCommand"></eluc:TabStrip>
                </div>
                <table width="100%" style="border-style: none; color: blue;">
                    <tr>
                        <td colspan="3">
                            <b>
                                <asp:Label ID="lblnote" runat="server" EnableViewState="false" Text="Notes :" CssClass="input"
                                    BorderStyle="None" ForeColor="Blue"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lbl1" runat="server" Text="1."></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblNote1" runat="server"
                                Text="Enter the Date till when data is to be trashed."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lbl2" runat="server" Text="2."></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblNote2" runat="server"
                                Text="Click on the button (Bonded Stores/Provision/Brought Forward/Earning-Deduction/Phone Card) to trash specific data in vessel"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblOR" runat="server" Text="( OR )"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lbl3" runat="server" Text=" "></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblNote3" runat="server"
                                Text="Click on the button (All) to trash all data in vessel"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="20%">
                            <asp:Literal ID="lblTrashuptoDate" runat="server" Text="Trash upto Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                <br clear="all" />
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="Trash_Confirm" Visible="false" />
                <eluc:Confirm ID="ucConfirmBond" runat="server" OnConfirmMesage="Trash_ConfirmBond"
                    Visible="false" />
                <eluc:Confirm ID="ucConfirmProvision" runat="server" OnConfirmMesage="Trash_ConfirmProvision"
                    Visible="false" />
                <eluc:Confirm ID="ucConfirmPortagebillBroughtforward" runat="server" OnConfirmMesage="Trash_ConfirmPortagebillBroughtforward"
                    Visible="false" />
                <eluc:Confirm ID="ucConfirmPortagebillEarningDeduction" runat="server" OnConfirmMesage="Trash_ConfirmPortagebillEarningDeduction"
                    Visible="false" />
                <eluc:Confirm ID="ucConfirmPhoneCard" runat="server" OnConfirmMesage="Trash_ConfirmPhoneCard"
                    Visible="false" />
            </div>
            <div>
                <input type="button" runat="server" id="Button1" name="isouterpage" style="visibility: hidden" />
            </div>
        </form>
    </div>
</body>
</html>
