<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAwards.aspx.cs" Inherits="CrewOffshoreAwards" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Qualificaiton" Src="~/UserControls/UserControlQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvAwardAndCertificate.ClientID %>"));
}, 200);
}
window.onresize = window.onload = Resize;

function pageLoad(sender, eventArgs) {
Resize();
fade('statusmessage');
}
</script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewAcademic" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:TabStrip ID="TabStrip1" runat="server" Title="Award and Certificate"></eluc:TabStrip>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

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
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File Number"></telerik:RadLabel>
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

            <eluc:TabStrip ID="CrewAwardandCertificate" runat="server" OnTabStripCommand="AwardandCertificateMenu_TabStripCommand"></eluc:TabStrip>

                
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAwardAndCertificate" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAwardAndCertificate_NeedDataSource"
                    OnItemCommand="gvAwardAndCertificate_ItemCommand"
                    OnItemDataBound="gvAwardAndCertificate_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                   
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                           
                            <telerik:GridTemplateColumn HeaderText="Sl no">
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                              <HeaderStyle Width="50px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Award/Certificate">
                                <HeaderStyle Width="200px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblAwardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblCertificate" runat="server" CommandArgument='<%#Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' CommandName="EDIT"></asp:LinkButton>
                            </itemtemplate>
                                <edititemtemplate>
                                <eluc:Quick Width="100%"  ID="ddlCertificateEdit" runat="server" CssClass="input_mandatory" QuickTypeCode="109"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                            </edititemtemplate>
                                <footertemplate>
                                <eluc:Quick Width="100%" ID="ddlCertificateAdd" runat="server" QuickTypeCode="109" CssClass="input_mandatory"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issue Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               <HeaderStyle Width="150px" />
                                <itemtemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDISSUEDATE", "{0:dd/MMM/yyyy}")%>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:RadLabel ID="lblAwardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                <eluc:Date Width="100%" runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                            </edititemtemplate>
                                <footertemplate>
                                <eluc:Date Width="100%" runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <itemstyle wrap="true" horizontalalign="Left"></itemstyle>
                               <HeaderStyle Width="250px" />
                                <itemtemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:RadTextBox Width="100%" ID="txtRemarksEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    MaxLength="200"></telerik:RadTextBox>
                            </edititemtemplate>
                                <footertemplate>
                                <telerik:RadTextBox Width="100%" ID="txtRemarksAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                              
                                <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                <itemtemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" 
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdXEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdXDelete"
                                    ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Attachment" 
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdXAtt"
                                    ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>

                                </asp:LinkButton>
                            </itemtemplate>
                                <edititemtemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton runat="server" AlternateText="Cancel" 
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </edititemtemplate>
                                <footerstyle horizontalalign="Center" />
                                <footertemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" 
                                    CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New">
                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </footertemplate>
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

            <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
