<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCorrespondence.aspx.cs"
    Inherits="InspectionCorrespondence" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<script runat="server">
    [System.Web.Services.WebMethod]
    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + sessionid + "/" + filename;
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <script language="javascript" type="text/javascript">
        function DeleteFiles(e, sessionid) {
            var kc = e == null ? event.keyCode : e.keyCode;
            if (kc == 46) {
                var LeftListBox = document.forms[0].lstAttachments;
                for (var i = (LeftListBox.options.length - 1) ; i >= 0; i--) {
                    if (LeftListBox.options[i].selected) {
                        PageMethods.Message(sessionid, LeftListBox.options[i].text);
                        LeftListBox.options[i] = null;
                    }
                }
            }
        }
    </script>
    <form id="frmCorrespondence" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />

                <eluc:TabStrip ID="MenuCorrespondence" runat="server" OnTabStripCommand="Correspondence_TabStripCommand"></eluc:TabStrip>

                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <br />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr id="trFrom" runat="server">
                        <td>
                            <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trTO" runat="server">
                        <td>
                            <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trCC" runat="server">
                        <td>
                            <telerik:RadLabel ID="lblCC" runat="server" Text="Cc"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trBCC" runat="server">
                        <td>
                            <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trAtt" runat="server">
                        <td style="text-align: right">
                            <asp:LinkButton ID="lnkAttachment" OnClientClick="OpenWindow();" runat="server">Attachment</asp:LinkButton>
                        </td>
                        <td colspan="2">
                            <asp:ListBox ID="lstAttachments" runat="server" CssClass="input" Width="500px"></asp:ListBox>
                            <asp:Repeater ID="rpAttachment" runat="server">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkAtt" Target="_blank" Text='<%# Eval("FileName")%>' runat="server"
                                        NavigateUrl='<%# PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPressDel" runat="server" Text="(Press 'DEL' key to remove the attachment)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCorrespondenceType" runat="server" Text="Correspondence Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="104" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadEditor ID="txtBody" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                                <Modules>
                                    <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                    <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                                </Modules>
                            </telerik:RadEditor>

                            <div runat="server" id="txtBodyDiv" style="width: 100%; height: 275px; overflow: auto" class="readonlytextbox">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <font color="blue">
                                <telerik:RadLabel ID="lblNote" runat="server" Text="Note: &nbsp;For searching click on the filter button below"></telerik:RadLabel>
                            </font>
                            <eluc:TabStrip ID="MenuCrewCorrespondence" runat="server"
                                OnTabStripCommand="CrewCorrespondence_TabStripCommand" />
                        </td>
                    </tr>
                </table>



                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%--  <asp:GridView ID="gvCorrespondence" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowEditing="gvCorrespondence_RowEditing" OnRowDataBound="gvCorrespondence_RowDataBound"
                    OnRowDeleting="gvCorrespondence_RowDeleting" CellPadding="3" ShowFooter="false"
                    ShowHeader="true" EnableViewState="false">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrespondence" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvCorrespondence_NeedDataSource"
                        OnItemCommand="gvCorrespondence_ItemCommand"
                        OnItemDataBound="gvCorrespondence_ItemDataBound"
                        OnEditCommand="gvCorrespondence_EditCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />

                            <Columns>
                                <telerik:GridButtonColumn Text="Click" CommandName="Edit" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDateHeader" runat="server">Date</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCorrespondenceId" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDCORRESPONDENCEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblLockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkCorrespondence" runat="server" CommandArgument='<%#Container.DataSetIndex%>'
                                            Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE"))%>'
                                            CommandName="EDIT"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblSubjectHeader" runat="server">Subject</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTypeHeader" runat="server">Type</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCORRESPONDENCETYPENAME")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblUserHeader" runat="server">User</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCorresEmailHeader" runat="server">Corres/Email</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDTYPEOF")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <a runat="server" id="cmdMail" target="filterandsearch">
                                            <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>


                </div>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
