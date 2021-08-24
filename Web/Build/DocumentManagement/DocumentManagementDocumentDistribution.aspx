<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentDistribution.aspx.cs" Inherits="DocumentManagementDocumentDistribution" MaintainScrollPositionOnPostback="true" %>

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
    <title>Document</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script language="javascript" type="text/javascript">
            function resizeDiv(obj) {
                var height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 120;

                if (obj.id == 'divVessel' || obj.id == 'divVesselType')
                    obj.style.height = ((height - 60) / 2) + "px";
                else
                    obj.style.height = ((height - 60)) + "px";
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
                var listBox = $get('<%= divForm.ClientID %>');
                var hdn = $get('<%= hdnFormScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divJHA.ClientID %>');
                hdn = $get('<%= hdnJHAScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divRA.ClientID %>');
                hdn = $get('<%= hdnRAScroll.ClientID %>');
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
<body onresize="resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divRA);resizeDiv(divVesselType);resizeDiv(divVessel);">
    <form id="frmDocument" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuVessel" runat="server" Title="Distribution - General" OnTabStripCommand="MenuVessel_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
           <%-- <eluc:TabStrip ID="MenuBulk" runat="server" OnTabStripCommand="MenuBulk_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDocumentList" runat="server" OnTabStripCommand="MenuDocumentList_TabStripCommand"></eluc:TabStrip>
            <table id="tblDocument" width="100%" runat="server" align="center" style="table-layout: fixed;">
                <tr>
                    <td Width="25%"><b>Form by Category</b>
                    </td>
                    <td Width="10%"><b>JHA by Category</b>
                    </td>
                    <td Width="15%"><telerik:RadRadioButtonList ID="rblJHA" runat="server" Columns="2" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="rblJHA_SelectedIndexChanged" AutoPostBack="true" >
                            <Items>
                                <telerik:ButtonListItem Text="Old" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="New" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td Width="10%"><b>RA by Category</b></td>
                    <td Width="15%"><telerik:RadRadioButtonList ID="rblRA" runat="server" Columns="2" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="rblRA_SelectedIndexChanged" AutoPostBack="true" >
                            <Items>
                                <telerik:ButtonListItem Text="Old" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="New" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td Width="25%"><b>Vessel Type</b>
                    </td>
                </tr>
                <tr>
                    <td width="25%" rowspan="3">
                        <telerik:RadComboBox ID="ddlFormCategory" runat="server" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true"
                            AutoPostBack="true" CssClass="input" OnSelectedIndexChanged="ddlFormCategory_Changed"
                            Width="100%">
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <div id="divForm" runat="server" class="input" onscroll="javascript:setScrollPosition('divForm','hdnFormScroll');"
                            style="overflow: auto; height: 145px">
                            <asp:HiddenField ID="hdnFormScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkFormAll" runat="server" Text="SELECT ALL" AutoPostBack="true"
                                OnCheckedChanged="chkFormAll_Changed" />
                            <telerik:RadCheckBoxList ID="chkForm" runat="server" Width="500px" AutoPostBack="true" OnTextChanged="chkForm_Changed">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%" rowspan="3" colspan="2">
                        <telerik:RadComboBox ID="ddlJHACategory" runat="server" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true"
                            AutoPostBack="true" CssClass="input" OnSelectedIndexChanged="ddlJHACategory_Changed"
                            Width="100%">
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <div id="divJHA" runat="server" class="input" onscroll="javascript:setScrollPosition('divJHA','hdnJHAScroll');"
                            style="overflow: auto; height: 290px">
                            <asp:HiddenField ID="hdnJHAScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkJHAAll" runat="server" Text="SELECT ALL" AutoPostBack="true"
                                OnCheckedChanged="chkJHAAll_Changed" />
                            <telerik:RadCheckBoxList ID="chkJHA" Width="500px" runat="server" AutoPostBack="true">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%" rowspan="3" colspan="2">
                        <telerik:RadComboBox ID="ddlRACategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true"
                            CssClass="input" OnSelectedIndexChanged="ddlRACategory_Changed" Width="100%">
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <div id="divRA" runat="server" class="input" onscroll="javascript:setScrollPosition('divRA','hdnRAScroll');"
                            style="overflow: auto; height: 290px">
                            <asp:HiddenField ID="hdnRAScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkRAAll" runat="server" Text="SELECT ALL" AutoPostBack="true"
                                OnCheckedChanged="chkRAAll_Changed" />
                            <telerik:RadCheckBoxList ID="chkRA" Width="1000px"  runat="server" AutoPostBack="true">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="25%">
                        <div id="divVesselType" runat="server" class="input" style="overflow: auto; height: 150px"
                            onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');">
                            <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" Text="SELECT ALL" AutoPostBack="true"
                                OnCheckedChanged="chkVesselTypeAll_Changed" />
                            <telerik:RadCheckBoxList ID="chkVesselType" Width="500px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="25%" align="center">
                        <b>Vessel</b>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <div id="divVessel" runat="server" class="input" style="overflow: auto; height: 150px"
                            onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');">
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
    </form>
</body>
</html>
