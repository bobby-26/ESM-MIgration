<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentBulkDistribution.aspx.cs" Inherits="DocumentManagementDocumentBulkDistribution" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Distribution</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script language="javascript" type="text/javascript">
            function resizeDiv(obj) {
                var height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 120;

                if (obj.id == 'divManual')
                    obj.style.height = (height - 35) + "px";
                else if (obj.id == 'divVessel' || obj.id == 'divVesselType')
                    obj.style.height = ((height - 60) / 2) + "px";
                else
                    obj.style.height = ((height - 65)) + "px";

            }
        </script>

        <script type="text/javascript" language="javascript">

            // Maintain scroll position on list box.                 
            function setScrollPosition(divname, hdnname) {
                var div = $get(divname);
                var hdn = $get(hdnname);
                hdn.value = div.scrollTop;
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }

            function EndRequestHandler(sender, args) {
                var listBox = $get('<%= divManual.ClientID %>');
                var hdn = $get('<%= hdnManualScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divFormCategory.ClientID %>');
                hdn = $get('<%= hdnFormCategoryScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divJHACategory.ClientID %>');
                hdn = $get('<%= hdnJHACategoryScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divVesselType.ClientID %>');
                hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divVessel.ClientID %>');
                hdn = $get('<%= hdnVesselScroll.ClientID %>');
                listBox.scrollTop = hdn.value;
            }

        </script>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onresize="resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);">
    <form id="frmDocument" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDistribution" runat="server" Title="Distribution - Bulk" OnTabStripCommand="MenuDistribution_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <%--<eluc:TabStrip ID="MenuBulk" runat="server" OnTabStripCommand="MenuBulk_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuFilter" runat="server" OnTabStripCommand="MenuFilter_TabStripCommand"></eluc:TabStrip>
            <table id="tblDocument" width="100%" align="center" runat="server" style="table-layout: fixed;">
                <tr>
                    <td class="style2" Width="25%"><b>Manuals</b>
                    </td>
                    <td Width="10%"><b>JHA / RA Category</b>
                    </td>
                    <td Width="15%"><telerik:RadRadioButtonList ID="rblJHARA" runat="server" Columns="2" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="rblJHARA_SelectedIndexChanged" AutoPostBack="true" >
                            <Items>
                                <telerik:ButtonListItem Text="Old" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="New" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList></td>
                    <td Width="25%"><b>Form Category</b>
                    </td>
                    <td Width="25%"><b>Vessel Type</b>
                    </td>
                </tr>
                <tr>
                    <td width="25%" rowspan="3" >
                        <div id="divManual" runat="server" class="input" onscroll="javascript:setScrollPosition('divManual','hdnManualScroll');"
                            style="overflow: auto; height: 330px; ">
                            <asp:HiddenField ID="hdnManualScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkManualAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkManualAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkManual" Width="500px"  runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%" rowspan="3" colspan="2">                        
                        <telerik:RadComboBox ID="ddlRADistributionType" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="100%" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="ddlRADistributionType_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                            <telerik:RadComboBoxItem Text="Distribute JHA only" Value="1"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Distribute RA only" Value="2" Selected="True"></telerik:RadComboBoxItem>
<%--                            <telerik:RadComboBoxItem Text="Distribute Both JHA & RA" Value="3" ></telerik:RadComboBoxItem>--%>
                                </Items>
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <div id="divJHACategory" runat="server" class="input" onscroll="javascript:setScrollPosition('divJHACategory','hdnJHACategoryScroll');"
                            style="overflow: auto; height: 297px">
                            <asp:HiddenField ID="hdnJHACategoryScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkJHACategoryAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkJHACategoryAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkJHACategory" Width="500px" runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%" rowspan="3" >
                        <telerik:RadComboBox ID="ddlParentFormCategory" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlParentFormCategory_Changed" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <div id="divFormCategory" runat="server" class="input" onscroll="javascript:setScrollPosition('divFormCategory','hdnFormCategoryScroll');"
                            style="overflow: auto; height: 297px">
                            <asp:HiddenField ID="hdnFormCategoryScroll" runat="server" />
                            <telerik:RadCheckBoxList ID="chkFormCategory" Width="500px" runat="server" OnSelectedIndexChanged="chkFormCategory_Changed" AutoPostBack="true" >
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%">
                        <div id="divVesselType" runat="server" class="input" onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');"
                            style="overflow: auto; height: 145px;">
                            <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkVesselType" Width="500px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" width="25%">
                        <b>Vessel</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                            style="overflow: auto; height: 145px">
                            <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                            <telerik:RadCheckBoxList ID="chkVessel" Width="500px" runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
                <eluc:Status runat="server" ID="ucStatus" />
                <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>

<%--        <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
            CancelText="No" />--%>

    </form>
</body>
</html>
