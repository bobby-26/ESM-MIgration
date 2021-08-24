<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCorrespondence.aspx.cs"
    Inherits="CrewCorrespondence" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>       
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCorrespondence" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
       
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
             <eluc:TabStrip ID="MenuCorrespondence" runat="server" OnTabStripCommand="Correspondence_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />            
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
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
                        <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trTO" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtTO" Width="500px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trCC" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtCC" Width="500px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trBCC" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server"></telerik:RadTextBox>
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
                                    NavigateUrl='<%#Session["sitepath"] + "/attachments/emailattachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPressDELkeytoremovetheattachment" runat="server" Text="(Press &quot;DEL&quot; key to remove the attachment)"></telerik:RadLabel>
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
                        <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDate" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
            </table>
            <telerik:RadEditor ID="txtBody" runat="server" Width="99%"  EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">                
            </telerik:RadEditor>          
            <eluc:TabStrip ID="MenuCrewCorrespondence" runat="server" OnTabStripCommand="CrewCorrespondence_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrespondence" runat="server" Height="37%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnSelectedIndexChanged="gvCorrespondence_SelectedIndexChanged"
                OnNeedDataSource="gvCorrespondence_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCorrespondence_ItemDataBound"
                OnItemCommand="gvCorrespondence_ItemCommand" OnEditCommand="gvCorrespondence_EditCommand" ShowFooter="false" OnDeleteCommand="gvCorrespondence_DeleteCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDCORRESPONDENCEID" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                     <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCorrespondenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRESPONDENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCorrespondence" runat="server"
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}")) %>'
                                    CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Subject" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRESPONDENCETYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Corres/Email" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCorresEmail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Mail" ID="cmdMail" ToolTip="Email" CommandName="EMAIL" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Lock" ID="cmdLock" ToolTip="Lock/Unlock" CommandName="LOCKUNLOCK" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-lock"></i></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus1" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
