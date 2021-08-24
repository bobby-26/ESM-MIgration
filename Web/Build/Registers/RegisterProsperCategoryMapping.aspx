<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperCategoryMapping.aspx.cs" Inherits="Registers_RegisterProsperCategoryMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Category Mapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmTemplateMapping" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               
             
                    <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"></eluc:TabStrip>
              
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblevent" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtevent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblsource" runat="server" Text="External Audit"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="divSource" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblextaudit" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal2" runat="server" Text="Internal Audit"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="div2" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblintaudit" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal3" runat="server" Text="Vetting"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="div3" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblextvetting" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal4" Visible="false" runat="server" Text="Internal Vetting"></telerik:RadLabel>
                        </td>
                        <td>

                            <asp:CheckBoxList Visible="false" ID="cblintvetting" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                RepeatColumns="5">
                            </asp:CheckBoxList>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal5" runat="server" Text="Incident"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="div5" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblincident" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal6" runat="server" Text="Feedback"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="div6" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblfeedback" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr></tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal1" runat="server" Text=""></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="divVesseltype" runat="server" visible="false" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblvesseltype" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="5">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
