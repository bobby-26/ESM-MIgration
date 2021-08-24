<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormFbCategoryChange.aspx.cs" Inherits="StandardFormFbCategoryChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spare Item Move</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSpareMove" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlSpareItemMove">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Category Change" ShowMenu="false"></eluc:Title>
                        </div>
                        <div>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuInventorySpareMove" runat="server" OnTabStripCommand="MenuInventorySpareMove_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divField" style="position: relative; z-index: 2">
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Literal ID="lblInStock" runat="server" Text="Form Name"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFormName" runat="server" Width="250px" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Literal ID="lblCurrentLocation" runat="server" Text="Current Category"></asp:Literal>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrenCategory" runat="server" ReadOnly="true" Width="250px" CssClass="readonlytextbox"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblLocationId" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lblformId" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Literal ID="lblMoveto" runat="server" Text="Move to"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategoryList" runat="server" CssClass="dropdown_mandatory" 
                                        DataTextField="FLDFORMCATEGORYNAME" DataValueField="FLDFORMCATEGORYID" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <eluc:Status runat="server" ID="ucStatus" Text="" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="cmdHiddenSubmit" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
