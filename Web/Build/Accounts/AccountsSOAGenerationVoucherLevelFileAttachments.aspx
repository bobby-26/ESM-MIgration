<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAGenerationVoucherLevelFileAttachments.aspx.cs" Inherits="Accounts_AccountsSOAGenerationVoucherLevelFileAttachments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessageVoucherAttachment.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

<%--        <script type="text/javascript">
            var Template = new Array();
            Template[0] = '<a href="#" id="lnkRemoveFile{counter}" onclick="return removeFile(this);">Remove</a>';
            Template[1] = '<input id="txtFileUpload{counter}" name="txtFileUpload{counter}" type="file" class="input" />';
            Template[2] = 'Choose a file';
            var counter = 1;

            function addFile(description) {
                counter++;
                var tbl = document.getElementById("tblFiles");
                var rowCount = tbl.rows.length;
                var row = tbl.insertRow(rowCount - 1);
                var cell;

                for (var i = 0; i < Template.length; i++) {
                    cell = row.insertCell(0);
                    cell.innerHTML = Template[i].replace(/\{counter\}/g, counter).replace(/\{value\}/g, (description == null) ? '' : description);
                }
            }
            function removeFile(ctrl) {
                var tbl = document.getElementById("tblFiles");
                if (tbl.rows.length > 2)
                    tbl.deleteRow(ctrl.parentNode.parentNode.rowIndex);
            }

        </script>--%>
         <script type="text/javascript">
            function MainFn( args) {
            
                   var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                       if (shouldSubmit) {
                           alert("Do something more..then postback");
                           this.click();
                       } 
                    else {
                           alert("cancel selected. do nothing..");
                       }
                   });
                   var text = "Are you sure you want to submit the page?";
                   radconfirm(text, callBackFunction, 300, 160, null, "RadConfirm");
                   args.set_cancel(true);  // postback(radbutton) stopped
               }
                    </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1">            
           <Windows>
        <telerik:RadWindow ID="rw_customConfirm" Modal="true" Title="Confirm" Behaviors="Close, Move" VisibleStatusbar="false"
            Width="500px" Height="130px"  runat="server">
            <ContentTemplate>
                <div class="rwDialogPopup radconfirm">
                    <br />
                    <div class="rwDialogText">
                        <asp:Literal ID="confirmMessage" Text="" runat="server" />
                    </div>
                    <br />  
                    <div style="position:center">
                        <telerik:RadButton  runat="server" ID="voucher" Text="Remove from the entire voucher and all lineitems"  OnClick="voucherlineitem_Click">
                        </telerik:RadButton>
                        <telerik:RadButton  runat="server" ID="lineitem" Text="Remove only for this Voucher line item"  OnClick="voucherlineitem_Click">
                        </telerik:RadButton>
<%--                        <telerik:RadButton  runat="server" ID="rbConfirm_Cancel" Text="Cancel"  önClientClicked="closeCustomConfirm">
                        </telerik:RadButton>--%>
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
               </Windows>     
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand" TabStrip="false"></eluc:TabStrip>
        <table id="tblFiles">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false">Status</telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="49"
                        CssClass="input_mandatory" Visible="false" ShortNameFilter="APP,NAP,CAP" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <a href="#" onclick="return addFile();">Add File</a>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <eluc:TabStrip ID="SubMenuAttachment" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="true" OnUpdateCommand="gvAttachment_RowUpdating" Height="85%"
            OnItemDataBound="gvAttachment_ItemDataBound" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvAttachment_NeedDataSource"
            OnItemCommand="gvAttachment_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            AllowSorting="true" OnSortCommand="gvAttachment_Sorting">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
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
                    <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                        <ItemStyle HorizontalAlign="Left" Width="4%"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkExport" runat="server" Width="10px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="7%" Visible="false">
                        <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File Name" HeaderStyle-Width="25%" AllowSorting="true" SortExpression="FLDFILENAME" ShowSortIcon="true">
                        <ItemStyle HorizontalAlign="Left" Width="30%" Wrap="true"></ItemStyle>
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
                    <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                Height="14px" ToolTip="Download File">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Size(in KB)" HeaderStyle-Width="15%" AllowSorting="true" SortExpression="FLDFILESIZE" ShowSortIcon="true">
                        <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFilesize" runat="server" Text='<%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Synch(Yes/No)" HeaderStyle-Width="16%">
                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                            <telerik:RadCheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Created By" HeaderStyle-Width="16%">
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%#Bind("FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Created Date" HeaderStyle-Width="16%">
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Modified By" Visible="false">
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblModifiedBy" runat="server" Text='<%#Bind("FLDMODIFIEDBYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Modified Date" Visible="false">
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblModifieddate" runat="server" Text='<%#Bind("FLDMODIFIEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="17%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Convert2PDF" ImageUrl="<%$ PhoenixTheme:images/pdf.png %>"
                                CommandName="CONVERTTOPDF" CommandArgument='<%# Container.DataItem %>' ID="cmdConvert"
                                ToolTip="Convert2PDF"></asp:ImageButton>
                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="PDFDOWNLOAD" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                                CommandName="PDFDOWNLOAD" CommandArgument='<%# Container.DataItem %>' ID="cmdDownload"
                                ToolTip="Download Pdf"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="UPDATE" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
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
<%--            <asp:Button ID="voucherlineitem" runat="server" Text="hide" OnClick="voucherlineitem_Click" CssClass="hidden" />            
            <asp:Button ID="voucher" runat="server" Text="show" OnClick="voucher_Click" CssClass="hidden" />
            <asp:Button ID="close" runat="server" Text="close" OnClick="close_Click" CssClass="hidden" />--%>
        <%--<eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OKText="Delete" OnConfirmMesage="CheckMapping_Click" />--%>
    </form>
</body>
</html>

