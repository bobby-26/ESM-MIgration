<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchEvaluationAttachment.aspx.cs" Inherits="Crew_CrewBatchEvaluationAttachment" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Evaluation Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <script type="text/javascript">
            function fileUploaded(sender, args) {
                document.getElementById("<%= btnattachment.ClientID %>").click();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>         
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBatchNo" runat="server" Text="Batch No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBatchNo" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBatchStatus" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Visible="false"></telerik:RadTextBox>
                        <asp:HiddenField ID="hdndtkey" runat="server" />
                        <asp:HiddenField ID="hdnenrollmentid" runat="server" />
                        <asp:HiddenField ID="hdnrecommendedId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachmentType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblAttachmentType" runat="server" CssClass="input-mandatory"
                            Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblAttachmentType_SelectedIndexChanged">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>

            <br />
            <hr />
            <table id="tblFiles">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnattachment" runat="server"></asp:LinkButton>
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="txtFileUpload1" OnClientFileUploaded="fileUploaded"
                            OnFileUploaded="txtFileUpload1_FileUploaded" />
                        <%--  <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />--%>
                    </td>
                </tr>
            
            </table>
            <table runat="server" visible="false" id="tblAssessmentResule">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblResule" runat="server" Text="Assessment Result"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblResult" runat="server" Direction="Horizontal" CssClass="input-mandatory">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <table runat="server" visible="false" id="tblCertificate">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCertificateNo" runat="server" Text="Certificate No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertificateNo" CssClass="input_mandatory" runat="server"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPlaceOfIssue" CssClass="input_mandatory" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtNationality" CssClass="input_mandatory" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIssueDate" runat="server" Text="Issue Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtIssueDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="SubMenuAttachment" runat="server"></eluc:TabStrip>           
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAttachment_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAttachment_ItemDataBound" OnUpdateCommand="gvAttachment_UpdateCommand"
                OnItemCommand="gvAttachment_ItemCommand" ShowFooter="false" AutoGenerateColumns="false" OnDeleteCommand="gvAttachment_DeleteCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" HeaderStyle-Width="6%">
                            <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgfiletype" Enabled="false" Width="15px" Height="15px" Visible="true">
                                 <span class="icon" id="imgFlagcolor"><i class="fas fa-file"></i></span>      
                                </asp:LinkButton>                                
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File Name" AllowSorting="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblFileName" runat="server" CommandName="SELECT" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></asp:LinkButton>                                
                                <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px" ToolTip="Download File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Size(in KB)" AllowSorting="false" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sync(Yes/No)" AllowSorting="false" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFileNameEdit" runat="server" Visible="false" Enabled="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%#Bind("FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="UPDATE" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="CANCEL" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
