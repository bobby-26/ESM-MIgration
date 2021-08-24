<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselCorrespondence.aspx.cs"
    Inherits="RegistersVesselCorrespondence" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>

<script runat="server">
    [System.Web.Services.WebMethod]
    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + sessionid + "/" + filename);
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="hdVesselCorrespondence" runat="server">
    <title>Vessel Correspondence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" CssClass="hidden" />
            <eluc:TabStrip ID="MenuCorrespondence" runat="server" OnTabStripCommand="Correspondence_TabStripCommand"></eluc:TabStrip>
            <%--<eluc:TabStrip ID="MenuCommon" runat="server" OnTabStripCommand="MenuCommon_TabStripCommand"></eluc:TabStrip>--%>
            <table id="tblConfigureVesselList" width="100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true" Width="24.5%" OnTextChangedEvent="ddlVessel_TextChangedEvent"
                            AppendDataBoundItems="true" AutoPostBack="true" AssignedVessels="true" EntityType="VSL" ActiveVessels="true" />
                    </td>
                    <td width="25%"></td>
                </tr>
            </table>
            <hr />
            <br />
            <table width="100%">
                <tr id="trFrom" runat="server">
                    <td width="15%">
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr id="trTO" runat="server">
                    <td width="15%">
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr id="trCC" runat="server">
                    <td width="15%">
                        <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr id="trBCC" runat="server">
                    <td width="15%">
                        <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr id="trAtt" runat="server">
                    <td style="text-align: right" width="15%">
                        <asp:LinkButton ID="lnkAttachment" OnClientClick="OpenWindow();" runat="server">Attachment</asp:LinkButton>
                    </td>
                    <td width="60%">
                        <asp:ListBox ID="lstAttachments" runat="server" CssClass="input" Width="500px"></asp:ListBox>
                        <asp:Repeater ID="rpAttachment" runat="server">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAtt" Target="_blank" Text='<%# Eval("FileName")%>' runat="server"
                                    NavigateUrl='<%#Session["sitepath"] + "/attachments/emailattachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblPressDELkeytoremovetheattachment" Width="100%" runat="server" Text="(Press &quot;DEL&quot; key to remove the attachment)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblCorrespondenceType" runat="server" Text="Correspondence Type"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory"
                            AppendDataBoundItems="true" Width="24.5%" />
                    </td>
                    <td width="25%"></td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td width="60%">
                        <telerik:RadTextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="25%"></telerik:RadTextBox>
                    </td>
                    <td width="25%"></td>
                </tr>
            </table>
            <telerik:RadEditor ID="txtBody" runat="server" Width="99%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
            <telerik:RadGrid ID="gvCorrespondence" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemCommand="gvCorrespondence_ItemCommand"
                CellSpacing="0" GridLines="None" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" Width="100%"
                OnItemDataBound="gvCorrespondence_ItemDataBound" CellPadding="3" ShowFooter="false" ShowHeader="true"
                OnNeedDataSource="gvCorrespondence_NeedDataSource" OnSelectedIndexChanged="gvCorrespondence_SelectedIndexChanged">
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCorrespondenceId" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDCORRESPONDENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCorrespondence" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>'
                                    CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%#DataBinder.Eval (Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Subject">
                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCORRESPONDENCETYPENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <%--<asp:ImageButton runat="server" AlternateText="E-Mail" ImageUrl="<%$ PhoenixTheme:images/email.png %>"
                                    CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdMail"
                                    ToolTip="E-Mail"></asp:ImageButton>--%>
                                <a runat="server" id="cmdMail" target="filterandsearch">
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/email.png %>" />
                                </a>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Lock" ID="cmdLock" ToolTip="Lock/Unlock" CommandName="LOCKUNLOCK" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-lock"></i></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <eluc:Status ID="ucStatus" runat="server" />
        <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
    </form>
</body>
</html>
