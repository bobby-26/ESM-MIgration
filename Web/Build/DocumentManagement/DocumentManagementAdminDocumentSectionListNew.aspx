<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementAdminDocumentSectionListNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementAdminDocumentSectionListNew" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Section</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
        <script type="text/javascript">
            function ConfirmApprove(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmApprove.UniqueID %>", "");
                }
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
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvDocument.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDocument.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <input type="hidden" id="hiddenkey" name="hiddenkey" runat="server" value="" />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDocumentGeneral" TabStrip="false" runat="server" OnTabStripCommand="MenuDocumentGeneral_TabStripCommand"></eluc:TabStrip>
            <table id="tblFind" runat="server" width="75%">
                <tr style="height: 50px">
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblSection" runat="server" Text="Section"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSectionNo" runat="server" CssClass="input" Width="200px" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtName" runat="server" CssClass="input" Width="200px" MaxLength="100"></telerik:RadTextBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlStatus" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" runat="server" Width="200px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Preparation" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Pending Approval" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Approved" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Published" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocument" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" Font-Size="11px" Width="100%" Height="73%" CellPadding="3" OnNeedDataSource="gvDocument_NeedDataSource"
                OnItemCommand="gvDocument_ItemCommand" OnItemDataBound="gvDocument_ItemDataBound" ShowFooter="false"
                OnRowDeleting="gvDocument_RowDeleting" OnUpdateCommand="gvDocument_UpdateCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDSECTIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllSection" runat="server" Text="" AutoPostBack="true" OnClick="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" Visible="false">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <img id="imgSection" alt="" runat="server" src="<%$ PhoenixTheme:images/28.png %>"
                                    width="3" visible="false" />
                                <telerik:RadLabel ID="lblParentSectionYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTSECTIONYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            <HeaderTemplate>
                                Section
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                           
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSectionNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="60px" onkeypress="return isNumberKey(event)" MaxLength="50">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="250px">
                            <HeaderTemplate>
                                Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                           
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSectionNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="300">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <HeaderTemplate>
                                Active
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                           
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Checked="true"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWREVISIONNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLastRevision" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTREVISIONYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtVersionNumberAdd" runat="server" CssClass="gridinput"
                                    Visible="false" onkeypress="return isNumberKey(event)">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Draft">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <HeaderTemplate>
                                Draft
                            </HeaderTemplate>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="70px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDraftRevisionNo" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISION") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblDraftRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <HeaderTemplate>
                                Status
                            </HeaderTemplate>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office Remarks">
                            <HeaderStyle Width="125px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Length > 25 ? (DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Substring(0, 25) + "...") : DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucOfficeRemarks" runat="server" TargetControlId="lblOfficeRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Questions </br> Approved YN">
                            <HeaderStyle Width="75px"></HeaderStyle>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuestionsApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDQUESAPPROVEDYN").ToString()%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDREADINGUSER").ToString()%>' ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reading Required">
                            <HeaderStyle Width="125px"></HeaderStyle>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReading" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREADINGUSER").ToString()%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDREADINGUSER").ToString()%>' ></telerik:RadLabel>
                                <telerik:RadLabel ID="lblForms" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMSPOSTERCHECKLISTIDS").ToString()%>' Visible ="false" ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="220px"></HeaderStyle>
                            <HeaderTemplate>
                                Action                               
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="sub section" CommandName="SUBSECTION" ID="cmdSubSection"
                                    ToolTip="Add sub section"><span class="icon"><i class="fas fa-angle-double-down"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Edit Current Version" CommandName="EDITDOCUMENT" ID="cmgEditContent"
                                    ToolTip="Edit Current Revision"><span class="icon"><i class="fas fa-user-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="View Current Version" CommandName="VIEWDOCUMENT" ID="cmgViewContent"
                                    ToolTip="View Current Revision" Visible="false"></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="APPROVE" CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Approve"><span class="icon"><i class="fas fa-clipboard-check"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="View Versions" CommandName="VIEWREVISON" ID="cmdRevison"
                                    ToolTip="View Revisions"><span class="icon"><i class="fas fa-copy"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Set Parent Section" CommandName="PARENTSECTION" ID="cmdParentSection"
                                    ToolTip="Set Parent Section"><span class="icon"><i class="fas fa-angle-double-up"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Questions" CommandName="QUESTIONS" ID="cmdQuestion"
                                    ToolTip="Questions"><span class="icon"><i class="fas fa-Question"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="User Configuration" CommandName="CONFIGURATION" ID="cmdConfig"
                                    ToolTip="Reading User Configuration"><span class="icon"><i class="fas fa-user-tie"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="cmdForms"
                                    ToolTip="Linked Forms and Checklist"><span class="icon"><i class="fa-file-contract-af"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="ReadUnread" CommandName="READUNREAD" ID="lnkReadUnread"
                                    ToolTip="Read/Unread User List"><span class="icon"><i class="fa-coins-ei"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Data" CommandName="BASE64" ID="cmdbase64"
                                    ToolTip="Image Base64 Update"><span class="icon"><i class="fa-check-select"></i></span></asp:LinkButton>

                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmApprove" runat="server" Text="confirmApprove" OnClick="ucConfirmApprove_Click" />
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="RadFormDecorator2" DecorationZoneID="RadAjaxPanel2"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindow ID="modalPopup" runat="server" Width="550px" Height="300px" Modal="true"
            OnClientClose="CloseWindow" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1"
                    OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table border="0" style="width: 100%">
                        <tr id="trDocuments">
                            <td>
                                <telerik:RadLabel ID="lblDocuments" runat="server" Text="Forms and Checklists">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnShowDocuments">
                                    <telerik:RadTextBox ID="txtDocumentname" runat="server" CssClass="input" Width="280px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtDocumentId" runat="server" CssClass="hidden" Width="0px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Documents">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkDocumentAdd" runat="server" OnClick="lnkDocumentAdd_Click"
                                        ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <div id="divHSEQUA" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 330px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                        <table id="tblDocuments" runat="server">
                                        </table>
                                    </div>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
