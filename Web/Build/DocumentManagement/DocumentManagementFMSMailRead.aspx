<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSMailRead.aspx.cs"
    ValidateRequest="false" Inherits="DocumentManagementFMSMailRead_MailRead" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailManager</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGeneric" runat="server"
            DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuMailRead" runat="server" OnTabStripCommand="MenuMailRead_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divinstruction"></telerik:RadFormDecorator>
            <table id="tblGeneric" runat="server" width="100%">
                 <tr>
                    <td width="10%">Mail Received on
                    </td>
                    <td width="90%">
                       <telerik:RadTextBox ID="txtreceivedon" runat="server" ReadOnly="true" CssClass="input" Width="40%"></telerik:RadTextBox>
                        <%-- <eluc:Date ID="txtreceivedon" runat="server" ReadOnly="true" />--%>
                    </td>
                </tr>
                <tr>
                    <td width="10%">File Number
                    </td>
                    <td width="90%">
                        <%--<asp:TextBox ID="txtFileNumber" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>--%>
                        <telerik:RadComboBox ID="ddlfileno" runat="server" DataTextField="FLDFILENODESCRIPTION" DataValueField="FLDFILENO"
                            EmptyMessage="Type to select file no" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="500px" OnItemsRequested="ddlfileno_ItemsRequested">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td width="10%">From
                    </td>
                    <td width="90%">
                        <asp:TextBox ID="txtFrom" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>To
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Cc
                    </td>
                    <td>
                        <asp:TextBox ID="txtCc" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Subject
                    </td>
                    <td>
                         <asp:TextBox ID="txtSubject" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadGrid ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvAttachment_RowCommand" ShowHeader="true" OnItemDataBound="gvAttachment_RowDataBound"
                            EnableViewState="false" OnNeedDataSource="gvAttachment_NeedDataSource" ShowFooter="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <NoRecordsTemplate>
                                    <table id="Table2" runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                                    Font-Bold="true">
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="File Name">
                                        <HeaderStyle Width="90%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMESSAGEID").ToString() %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                                Height="14px" ToolTip="Download File">
                                                <span class="icon"><i class="fas fa-download"></i></span>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                       <%-- <asp:TextBox ID="txtMessage" runat="server" ReadOnly="true" TextMode="MultiLine"
                            Rows="18" Columns="180" Width="100%" Height="100%" CssClass="input" Font-Size="Small"></asp:TextBox>--%>
                         <asp:TextBox ID="txtMessage" runat="server" ReadOnly="true" TextMode="MultiLine"
                            Rows="18" Columns="180" Width="100%" Height="100%" CssClass="input" Font-Size="Small"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
