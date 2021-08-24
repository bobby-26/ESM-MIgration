<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormUploadList.aspx.cs" Inherits="DocumentManagementFormUploadList"  MaintainScrollPositionOnPostback="true"%>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Upload</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body style="margin: 0; padding: 0px;">
    <form id="frmCategory" runat="server" autocomplete="off">
    <table class="loginpagebackground" width="80%" align="center" cellpadding="0" cellspacing="0"
        height="60px">
        <tr>
            <td align="left" valign="top">
                &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" />&nbsp; &nbsp;&nbsp;<font class="application_title"><%=Application["softwarename"].ToString() %><asp:Literal
                        runat="server" ID="litTitle" Text=""></asp:Literal></font>
            </td>
            <td align="right" valign="top">
                <font class="loginpage_companyname"><b>
                    <%=Session["companyname"]%></b></font>
            </td>
        </tr>
    </table>
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;
        width: 80%;">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="ucTitle" Text="Office Forms and Checklists" ShowMenu="false">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuForm" runat="server" OnTabStripCommand="MenuForm_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div class="navSelect* " style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuFormByCategory" runat="server" OnTabStripCommand="MenuFormByCategory_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0">
            <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvForm_RowCommand" OnRowDataBound="gvForm_ItemDataBound"
                OnRowCancelingEdit="gvForm_RowCancelingEdit" OnRowDeleting="gvForm_RowDeleting"
                OnRowCreated="gvForm_RowCreated" OnRowUpdating="gvForm_RowUpdating" OnRowEditing="gvForm_RowEditing"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                OnSelectedIndexChanging="gvForm_SelectedIndexChanging" OnSorting="gvForm_Sorting"
                DataKeyNames="FLDFORMID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <asp:TemplateField HeaderText="Purpose">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Form No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sort Order">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkFormName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>' Visible="false"></asp:LinkButton>
                            <asp:Label ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblFormRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                Visible="false"></asp:Label>
                            <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>'
                                Height="14px" ToolTip="Download Form">
                                    
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Label" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Category
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                            <asp:Label ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active YN">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Active YN
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purpose">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Purpose
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--         <asp:TemplateField HeaderText="Added date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Added Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Added By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Added By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddedByName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Revison Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Revision
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="divPage" style="position: relative;">
            <table width="100%" border="0" class="datagrid_pagestyle">
                <tr>
                    <td nowrap="nowrap" align="center">
                        <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                        <asp:Label ID="lblPages" runat="server"> </asp:Label>
                        <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                    </td>
                    <td nowrap="nowrap" align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap="nowrap" align="center">
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
                <eluc:Status runat="server" ID="ucStatus" />
            </table>
        </div>
    </div>
    <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
    </form>
</body>
</html>
