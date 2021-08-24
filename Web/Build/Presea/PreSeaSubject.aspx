<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaSubject.aspx.cs" Inherits="PreSeaSubject" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pre Sea Subject</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
         </telerik:RadCodeBlock>
    <script language="Javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }       
    </script>
       
</head>
<body>
    <form id="frmPreSeaSubject" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreseaSubject">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Subject"></eluc:Title>
            </div>
        </div>
        <div style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuMainSub" runat="server" OnTabStripCommand="MenuMainSub_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <eluc:Status ID="ucStatus" runat="server" />
        <div style="overflow: scroll; width: 30%; float: left; height: 680px;" id="divLocation">
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuPreSeaSubject" runat="server" OnTabStripCommand="MenuPreSeaSubject_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <table style="float: left; width: 100%;">
                <tr style="position: absolute">
                    <eluc:TreeView runat="server" ID="tvwLocation" OnSelectNodeEvent="ucTree_SelectNodeEvent">
                    </eluc:TreeView>
                    <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                </tr>
            </table>
        </div>
        <eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divLocation" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <br clear="right" />
        <div style="position: relative; float: left; margin: 5px; width: auto">
            <table width="100%" cellpadding="5">
                <tr>
                    <td>
                        Subject Name
                        <asp:Label ID="lblSubjectID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                        <asp:Label ID="lblMainSubjectID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINSUBJECTID") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'
                            CssClass="input_mandatory" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Subject Type
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSubjectType" runat="server" CssClass="input_mandatory">
                            <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Theoretical"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Pratical"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
        OKText="Yes" CancelText="No" />
    </form>
</body>
</html>
