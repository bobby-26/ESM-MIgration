<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementCommentsList.aspx.cs"
    Inherits="DocumentManagementCommentsList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Comments</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function resizeDiv() {
                var obj = document.getElementById("divGrid");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";
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
            function confirmarchive(args) {
                if (args) {
                    __doPostBack("<%=confirmarchive.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function confirmReview(args) {
                if (args) {
                    __doPostBack("<%=confirmReview.UniqueID %>", "");
                }
            }
        </script>

        <style type="text/css">
            .lblheader {
                font-weight: bold;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body onresize="resizeDiv();">
    <form id="form1" runat="server">
        <eluc:Title runat="server" ID="ucTitle" Text="Comments" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager2"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table width="100%">
                <tr>
                    <td width="15%">File
                    </td>
                    <td width="35%">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="285px" CssClass="input"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowDocuments" runat="server" Text=".." >
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td width="15%">Posted By
                    </td>
                    <td width="35%">
                        <span id="spnPickListPostedBy">
                            <telerik:RadTextBox ID="txtPostedByName" runat="server" CssClass="input" MaxLength="200"
                                Width="35%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPostedByDesignation" runat="server" CssClass="input" MaxLength="50"
                                Width="25%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgPostedBy" OnClientClick="return showPickList('spnPickListPostedBy', 'codehelp1', '', 'Common/CommonPickListUser.aspx', true); " >
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtPostedBy" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPostedByEmailHidden" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td width="15%">Archived By
                    </td>
                    <td width="35%">
                        <span id="spnPickListArchivedBy">
                            <telerik:RadTextBox ID="txtArchivedByName" runat="server" CssClass="input" MaxLength="200"
                                Width="35%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtArchivedByDesignation" runat="server" CssClass="input" MaxLength="50"
                                Width="25%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgArchivedBy" OnClientClick="return showPickList('spnPickListArchivedBy', 'codehelp1', '', 'Common/CommonPickListUser.aspx', true); " >
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtArchivedBy" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtArchivedByEmailHidden" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td width="15%">Section has been reviewed and no changes required
                    </td>
                    <td width="35%">
                        <telerik:RadCheckBox ID="chknochangesrequired" runat="server" />
                    </td>

                </tr>
                <tr>
                    <td width="15%">Comment From
                    </td>
                    <td width="35%">
                        <eluc:Date ID="ucCommentFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td width="15%">Comment To
                    </td>
                    <td width="35%">
                        <eluc:Date ID="ucCommentToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">Archived From
                    </td>
                    <td width="35%">
                        <eluc:Date ID="ucArchivedFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td width="15%">Archived To
                    </td>
                    <td width="35%">
                        <eluc:Date ID="ucArchivedToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">Accepted
                    </td>
                    <td width="35%">
                        <telerik:RadCheckBox ID="chkacceptedyn" runat="server" />
                    </td>
                    <td width="15%">Completed
                    </td>
                    <td width="35%">
                        <telerik:RadCheckBox ID="chkcompletedyn" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">Include Archived
                    </td>
                    <td width="35%">
                        <telerik:RadCheckBox ID="chkArchivedYN" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:HiddenField ID="hdnScroll" runat="server" />
            <eluc:TabStrip ID="MenuComments" runat="server" OnTabStripCommand="MenuComments_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComments" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%" Height="50%" CellPadding="3" OnNeedDataSource="gvComments_NeedDataSource"
                OnItemDataBound="gvComments_ItemDataBound" OnUpdateCommand="gvComments_UpdateCommand"
                OnItemCommand="gvComments_ItemCommand" 
                OnRowDeleting="gvComments_RowDeleting" EnableViewState="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true"  
                    DataKeyNames="FLDDMSCOMMENTSID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />                    
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText ="Check">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="40px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllComments" runat="server" Text="" AutoPostBack="true" OnClick="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="120px"></HeaderStyle>
                            <HeaderTemplate>
                                File
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefName" runat="server" CommandName="FILEEDIT" 
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFILENAME").ToString().Length > 30 ? (DataBinder.Eval(Container, "DataItem.FLDFILENAME").ToString().Substring(0, 30) + "...") : DataBinder.Eval(Container, "DataItem.FLDFILENAME").ToString()%>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucRefName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENAME") %>' TargetControlId="lnkRefName" />
                                <telerik:RadLabel ID="lblsectionno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONNUMBER")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblsectionid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblformid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblformno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNO")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblcategoryid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReviewedByLink" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREVIEWEDBY")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comment">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                Comment
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkComments" runat="server" CommandName="EDIT" 
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMMENTS").ToString().Length > 30 ? (DataBinder.Eval(Container, "DataItem.FLDCOMMENTS").ToString().Substring(0, 30) + "...") : DataBinder.Eval(Container, "DataItem.FLDCOMMENTS").ToString()%>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>' TargetControlId="lnkComments" />
                                <telerik:RadLabel ID="lblCommentId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDMSCOMMENTSID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferenceId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFERENCEID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Source
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMMMENTSOURCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Posted By">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Posted By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpostedby" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDBYNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Accepted">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                            <HeaderTemplate>
                                Accepted
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAcceptanceYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACCEPTEDYNSTATUS")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCommentsIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDMSCOMMENTSID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferenceIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFERENCEID")%>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:CheckBox ID="chkAcceptanceYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACCEPTEDDOCUMENTYN").ToString().Equals("1"))?true:false %>'>
                                </asp:CheckBox>
                            </EditItemTemplate>
                            --%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Due
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <eluc:Date ID="ucDueDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                        DatePicker="true" CssClass="input" AutoPostBack="true"/>
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office Remarks">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                Office Remarks
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Length > 30 ? (DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Substring(0, 30) + "...") : DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucOfficeRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREMARKS") %>' TargetControlId="lblOfficeRemarks" />
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOfficeRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREMARKS") %>'
                                    CssClass="input" Visible = "false"/>
                                <telerik:RadLabel ID="lblOfficeRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Length > 25 ? (DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString().Substring(0, 25) + "...") : DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS").ToString()%>'></telerik:RadLabel>
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Office Remarks" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                 CommandName="OFFICEREMARKS" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdOfficeRemarks"
                                 ToolTip="Office Remarks"></asp:ImageButton>
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reviewed By">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Reviewed By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReviewedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reviewed On">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Reviewed On
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREVIEWEDDATE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed On">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>                                
                                    Completed On                               
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                    DatePicker="true" CssClass="input" AutoPostBack="true" Width="80px"/>
                            </EditItemTemplate>
                            --%>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridTemplateColumn HeaderText="Archived By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Archived By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDARCHIVEDBYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Archived On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Archived On
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDARCHIVEDDATE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="More Info"
                                    CommandName="INFO" ID="cmdMoreInfo"
                                    ToolTip="More Info"><span class="icon"><i class="fas fa-file"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Archive"
                                    CommandName="ARCHIVE" ID="cmdArchive"
                                    ToolTip="Archive"><span class="icon"><i class="fa fa-download"></i></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Review"
                                    CommandName="REVIEW" ID="cmdReview"
                                    ToolTip="Review"><span class="icon"><i class="fa fa-check-square"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>--%>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true"/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="confirmarchive" runat="server" Text="confirm" OnClick="confirmarchive_Click" />
            <asp:Button ID="confirmReview" runat="server" Text="confirm" OnClick="confirmReview_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
<%--            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
                CancelText="No" />
            <eluc:Confirm ID="ucConfirmArchive" runat="server" OnConfirmMesage="ucConfirmArchive_Click"
                OKText="Yes" CancelText="No" />
            <eluc:Confirm ID="ucConfirmReview" runat="server" OnConfirmMesage="ucConfirmReview_Click"
                OKText="Yes" CancelText="No" />
            <eluc:Confirm ID="ucConfirmBulkReview" runat="server" OnConfirmMesage="ucConfirmBulkReview_Click"
                OKText="Yes" CancelText="No" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
