<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSupdtEventFeedback.aspx.cs"
    Inherits="InspectionSupdtEventFeedback" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
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
        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupdtEventFeedback" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuSupdtEventFeedback" runat="server" OnTabStripCommand="MenuSupdtEventFeedback_TabStripCommand" Title="Supt Event Feedback"></eluc:TabStrip>
            <table id="tblSupdtEventFeedback" width="100%">
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" MaxLength="200"
                            Width="200px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEventDate" runat="server" Text="Event Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucEventDate" runat="server" CssClass="input_mandatory" DatePicker="true" AutoPostBack="true" OnTextChangedEvent="cmdHiddenSubmit_Click"></eluc:Date>
                        <asp:LinkButton ID="ImageButton1" runat="server" OnClick="cmdHiddenSubmit_Click">
                            <span class="icon"><i class="fas fa-search"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEvent" runat="server" Text="Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlEvent" runat="server" CssClass="input_mandatory" Width="198px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucRecordedDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupdt" runat="server" Text="Supt"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupdtName" runat="server" CssClass="readonlytextbox" MaxLength="200"
                            Width="200px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvFeedback" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnNeedDataSource="gvFeedback_NeedDataSource"
                Width="100%" Height="77.4%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnDeleteCommand="gvFeedback_DeleteCommand"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvFeedback_ItemCommand" OnItemDataBound="gvFeedback_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnUpdateCommand="gvFeedback_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEFEEDBACKID">
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
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeFeedbackId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFEEDBACKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeFeedbackName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeFeedbackIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFEEDBACKID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlFeedbackCategoryEdit" runat="server" CssClass="input_mandatory"
                                    Width="100%" Filter="Contains">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFeedbackCategoryAdd" runat="server" CssClass="input_mandatory"
                                    Width="100%" Filter="Contains">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SubCategory">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedbackSubcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlFeedbackSubCategoryEdit" runat="server" CssClass="input"
                                    Width="100%" Filter="Contains">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFeedbackSubCategoryAdd" runat="server" CssClass="input"
                                    Width="100%" Filter="Contains">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Key Anchor">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblKeyAnchor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEYANCHORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucKeyAnchorEdit" runat="server" AppendDataBoundItems="true" Width="100%"
                                    CssClass="dropdown_mandatory" QuickTypeCode="155" QuickList='<%#PhoenixRegistersQuick.ListQuick(1,155)%>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterTemplate>
                                <eluc:Quick ID="ucKeyAnchorAdd" runat="server" AppendDataBoundItems="true" Width="100%"
                                    CssClass="dropdown_mandatory" QuickTypeCode="155" QuickList='<%#PhoenixRegistersQuick.ListQuick(1,155)%>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Assigned To">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignonoffIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKANDNAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAssignedToName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlAssignedToAdd" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true" Width="100%" Filter="Contains">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supt Remarks">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length > 200 ? (DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 200) + "...") : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" CssClass="input_mandatory" runat="server" TextMode="MultiLine"
                                    onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLength(this,700)" Rows="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Width="97%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" CssClass="input_mandatory" Width="97%" TextMode="MultiLine"
                                    onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLength(this,700)" Rows="4" runat="server">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" Width="60px" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="ADD" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
    </form>
</body>
</html>
