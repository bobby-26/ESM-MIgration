<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerReportFileAttachment.aspx.cs" Inherits="OwnerReportFileAttachment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessageVoucherAttachment.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="AttachmentHead" runat="server">
    <title>Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">        
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.Telerik.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/scripts.js"></script>
        <script type="text/javascript">
  
                (function () {
                    var $;
                    var attachment = window.attachment = window.attachment || {};

                    attachment.initialize = function () {
                        $ = $telerik.$;
                    };

                    window.validationFailed = function (radAsyncUpload, args) {
                        var $row = $(args.get_row());
                        var erorMessage = getErrorMessage(radAsyncUpload, args);
                        var span = createError(erorMessage);
                        $row.addClass("ruError");
                        $row.append(span);
                    }

                    function getErrorMessage(sender, args) {
                        var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
                        if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                            if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                                return ("This file type is not supported.");
                            }
                            else {
                                return ("This file exceeds the maximum allowed size.");
                            }
                        }
                        else {
                            return ("not correct extension.");
                        }
                    }

                    function createError(erorMessage) {
                        var input = '<span class="ruErrorMessage">' + erorMessage + ' </span>';
                        return input;
                    }
            })();
            function closeError(obj) {
                obj.parentElement.style.display='none';
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvAttachment.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
        <style type="text/css">
             .ruError {
        padding: 5px 0px;
        margin: 10px 0px;
        border: 1px solid #ef0000;
        background: #f9e8e8;
    }
             .ruError .ruErrorMessage {
            display: block;
            font-variant: small-caps;
            text-transform: lowercase;
            margin-left: 5px;
        }
             .qsf-list {
        margin: 0;
        padding: 1em 0 1em 150px;
        list-style: none;
        background: #ced1d3;
    }
          .qsf-error .qsf-list {
        background-color: #f9e8e8;
        margin-bottom: 1em;
    }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnablePageMethods="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand" TabStrip="false"></eluc:TabStrip>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtFileUpload1" />
                        <telerik:AjaxUpdatedControl ControlID="gvAttachment" />
                        <telerik:AjaxUpdatedControl ControlID="divPager" />  
                        <telerik:AjaxUpdatedControl ControlID="InvalidFiles" /> 
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvAttachment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvAttachment" />
                        <telerik:AjaxUpdatedControl ControlID="divPager" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>              
        <table id="tblFiles" runat="server" style="width: 100%">
            <tr runat="server" id="tr1">
                <td colspan="2">
                    <strong>Allowed file types:</strong> <asp:Literal id="spnAllowedFileType" runat="server"></asp:Literal>.
                    <strong>Allowed overall upload size:</strong> <asp:Literal id="spnUploadSize" runat="server"></asp:Literal>.
                    <asp:Panel ID="InvalidFiles" Visible="false" runat="server" CssClass="qsf-error">
                        <span tabindex="0" class="ruButton ruRemove" style="float:right;cursor:pointer;" onclick="closeError(this);">Remove</span>
                        <h3>The Upload failed for:</h3>
                        <ul class="qsf-list ruError" runat="server" id="InValidFilesList">
                            <li>
                                <p class="ruErrorMessage">The size of your overall upload exceeded the maximum of size.</p>
                            </li>
                        </ul>
                    </asp:Panel>
                    <telerik:RadAsyncUpload runat="server" ID="txtFileUpload1" MultipleFileSelection="Automatic"
                        DropZones=".DropZone1" OnClientFilesUploaded="OnClientFilesUploaded" OnFileUploaded="txtFileUpload1_FileUploaded" OnClientValidationFailed="validationFailed"/>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false">Status</telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="49"
                        CssClass="input_mandatory" Visible="false" ShortNameFilter="APP,NAP,CAP" />
                </td>
                <td>
                    <telerik:RadProgressManager runat="server" ID="RadProgressManager1" />
                </td>
                <td>
                    <telerik:RadProgressArea runat="server" ID="RadProgressArea1" />
                </td>
            </tr>
            <tr runat="server" id="tr2">
                <td colspan="6">
                    <div class="DropZone1">
                        <p>DROP FILES HERE</p>
                    </div>
                </td>
            </tr>            
            <tr>
                <td colspan="6">
        <eluc:TabStrip ID="SubMenuAttachment" runat="server" Visible="false"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Width="100%" Height="50px" ShowHeader="true" EnableViewState="false"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" EnableHeaderContextMenu="true"
            OnItemCommand="gvAttachment_ItemCommand" OnItemDataBound="gvAttachment_ItemDataBound"
            OnNeedDataSource="gvAttachment_NeedDataSource" OnSortCommand="gvAttachment_SortCommand">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed" EnableHeaderContextMenu="true">
                <Columns>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkExport" runat="server" Width="10px" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkExportEdit" runat="server" Width="10px" Visible="false" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                        <ItemTemplate>
                            <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File Name" HeaderStyle-Width="50%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAttachmentType" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSyncyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString() %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblFileNameEdit" runat="server" Visible="false" Enabled="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtFileNameEdit" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                Height="14px" ToolTip="Download File">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Size(in KB)">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFilesize" runat="server" Text='<%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Synch(Yes/No)">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                            <asp:CheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="40px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
        </telerik:RadGrid>
        </td>
        </tr>
        <tr>
             <td colspan="6">
                 <div>
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 600px; width: 100%" frameborder="0">
                    </iframe>
                 </div>
             </td>
         </tr>
        </table>
        <eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OKText="Delete" OnConfirmMesage="CheckMapping_Click" />
        <telerik:RadScriptBlock runat="server">
            <script type="text/javascript">
                //<![CDATA[
                serverID("ajaxManagerID", "<%= RadAjaxManager1.ClientID %>");
                                   
                    Sys.Application.add_load(function () {
                        attachment.initialize();
                });               
                //]]>
            </script>
        </telerik:RadScriptBlock>
    </form>
</body>
</html>

