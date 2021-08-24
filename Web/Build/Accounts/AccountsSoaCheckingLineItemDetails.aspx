<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingLineItemDetails.aspx.cs"
    Inherits="AccountsSoaCheckingLineItemDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <br />
        <div runat="server" id="divFind">
            <table width="100%">
                <tr>
                    <td style="width: 50%;">
                        <table>
                            <tr>
                                <td colspan="4">
                                    <b><asp:Literal ID="lblAccountCodeDescriptionCaption" runat="server" Text="Account Code/Description"></asp:Literal></b>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lblAccountId" Visible="false" runat="server"></asp:Label>
                                    <asp:Label ID="lblAccountCOde" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblAccountCOdeDescription" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <b><asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal></b>
                                </td>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnkOwnerBudgetCode" runat="server"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <b><asp:Literal ID="lblBudgetCodeDescriptionCaption" runat="server" Text="Budget Code Description"></asp:Literal></b>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lblBudgetCodeDescription" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <b><asp:Literal ID="lblStatementReferenceCaption" runat="server" Text="Statement Reference"></asp:Literal></b>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lblStatementReference" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 40%; vertical-align:top; ">
                        <asp:DataList runat="server" ID="VoucherLevelAttachmentLink" RepeatDirection="Vertical"  EnableViewState="false"
                            OnItemCommand="VoucherLevelAttachmentLink_RowCommand">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="VLlnkAttachment" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                     Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>' >
                                </asp:LinkButton> &nbsp; &nbsp;
                            </ItemTemplate>
                        </asp:DataList>
                        <br />
                        <asp:DataList runat="server" ID="dlAttachmentLink" RepeatDirection="Vertical"  EnableViewState="false"
                            OnItemCommand="dlAttachmentLink_RowCommand">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkAttachment" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                     Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>' >
                                </asp:LinkButton> &nbsp; &nbsp;
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>
                            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 750px; width: 100%" frameborder="0">
                            </iframe>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
