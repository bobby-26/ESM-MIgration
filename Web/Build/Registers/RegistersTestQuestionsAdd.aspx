<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTestQuestionsAdd.aspx.cs"
    Inherits="Registers_RegistersTestQuestionsAdd" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inteview Questions</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript">
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }    
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersTestQst" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTestQst">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblTestQst" runat="server" Text="Test Questions"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRegistersTestQstAdd" runat="server" OnTabStripCommand="MenuRegistersTestQstAdd_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divControls">
                    <table width="100%" cellspacing="15">
                        <tr>
                            <td>
                                <asp:Literal ID="lblQuestionName" runat="server" Text="Question"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuestion" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                    onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="500px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                      <%--  <tr>
                            <td>
                                <asp:Literal ID="lblLevel" runat="server" Text="Level"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="ucLevel" runat="server" QuickTypeCode="137" CssClass="input_mandatory"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,137)%>' AppendDataBoundItems="true" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <div style="height: 100px; overflow: auto; width: 400px;" class="input">
                                    <asp:CheckBoxList ID="chkCourse" RepeatDirection="Vertical" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblActiveYn" runat="server" Text="ActiveYN"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkActiveYN" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
