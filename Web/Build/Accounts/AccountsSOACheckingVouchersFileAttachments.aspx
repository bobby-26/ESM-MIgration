<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOACheckingVouchersFileAttachments.aspx.cs" Inherits="Accounts_AccountsSOACheckingVouchersFileAttachments" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachment</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


        <script type="text/javascript">
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

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>


        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
      <%--  <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


            <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand"></eluc:TabStrip>

            <table id="tblFiles">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <a href="#" onclick="return addFile();">
                            <telerik:RadLabel ID="lblAddFile" runat="server" Text="Add File"></telerik:RadLabel>
                        </a>
                    </td>
                </tr>
            </table>
            <hr />
            <br />

            <eluc:TabStrip ID="SubMenuAttachment" runat="server"></eluc:TabStrip>

            <%-- <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvAttachment_RowUpdating"
                OnRowCancelingEdit="gvAttachment_RowCancelingEdit" OnRowDataBound="gvAttachment_RowDataBound"
                OnRowEditing="gvAttachment_RowEditing" OnRowDeleting="gvAttachment_RowDeleting"
                AllowSorting="true" OnSorting="gvAttachment_Sorting">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" 
                GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvAttachment_NeedDataSource"
                OnItemDataBound="gvAttachment_ItemDataBound"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                      
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmenttype" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE").ToString() %>'> </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblFileHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILENAME"
                                    ForeColor="White">File Name&nbsp;</asp:LinkButton>
                                <img id="FLDFILENAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFileNameEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtFileNameEdit" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'
                                    CssClass="input"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px" ToolTip="Download File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Size(in KB)">
                            <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSizeHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILESIZE"
                                    ForeColor="White">Size(in KB)&nbsp;</asp:LinkButton>
                                <img id="FLDFILESIZE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Synch(Yes/No)">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                                <asp:CheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By">
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%#Bind("FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date">
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                          
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:Splitter ID="ucSplitter" runat="server" TargetControlID="ifMoreInfo" />
        </div>
    </form>
</body>
</html>
