<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersFlag.aspx.cs"
    Inherits="RegistersFlag" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Flag</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
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
    <form id="frmRegistersFlag" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersFlag" runat="server" OnTabStripCommand="MenuRegistersFlag_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" Width="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="RadGrid1_DeleteCommand" OnSortCommand="RadGrid1_SortCommand" Height="93%" ShowFooter="true"
                OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" EnableViewState="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDFLAGID" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDABBREVIATION">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAbbreviationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="20%" AllowSorting="true" SortExpression="FLDFLAGNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFlagName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFlagIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                <eluc:Country runat="server" ID="ucCountryEdit" Width="100%" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    CountryList='<%# PhoenixRegistersCountry.ListCountry(1) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country runat="server" ID="ucCountryAdd" Width="100%" CountryList='<%# PhoenixRegistersCountry.ListCountry(1) %>' CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Application form" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblhardEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODEID") %>'></telerik:RadLabel>
                                <eluc:Hard runat="server" ID="ucReportCodeEdit" Width="100%" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 122) %>' SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTCODEID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ucReportCodeAdd" Width="100%" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 122) %>' HardTypeCode="122" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical Requires Y/N" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMedicalRequiresYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALREQUIRESYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkMedicalRequiresYNEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMEDICALREQUIRES").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkMedicalRequiresYNAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag SIB Y/N" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlagSibYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSIB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkFlagSibYNEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGSIBYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkFlagSibYNAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag Endorsement Y/N" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlagEndorsement" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGENDORSMENTYESNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkFlagEndorsementEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGENDORSMENTYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkFlagEndorsementAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Flags matching your search criteria"
                        PageSizeLabelText="Flags per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="450px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
