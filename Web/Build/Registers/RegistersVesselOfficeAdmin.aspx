<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselOfficeAdmin.aspx.cs" Inherits="RegistersVesselOfficeAdmin" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mobile" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Phone" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Admin</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .scrolpan {
                overflow-y: auto;
                height: 80%;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegisterVesselCommunication" DecoratedControls="All" />
    <form id="frmRegisterVesselCommunication" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuVesselOfficeAdmin" runat="server" OnTabStripCommand="MenuVesselOfficeAdmin_TabStripCommand"></eluc:TabStrip>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />

            <table cellpadding="1" width="100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <asp:Panel ID="pnlSatcom" runat="server" GroupingText="Accounts" Width="100%" Height="100px">
                            <table width="100%">
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadTextBox runat="server" ID="txtVesselName" Text="" Width="250px" ReadOnly="true" Visible="false"
                                            CssClass="readonlytextbox">
                                        </telerik:RadTextBox>
                                        <telerik:RadLabel ID="lblAccountsExecutive" runat="server" Text="Executive"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucAccountsExecutive" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadCheckBox ID="chkSupplierConfig" runat="server" Text="Invoice Posting As Per Supplier Configuration" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="width: 50%" valign="top">
                        <asp:Panel ID="pnlPurchase" runat="server" GroupingText="Purchase" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblPurchaseSuperintendent" runat="server" Text="Superintendent"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucPurchaseSupdt" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlQuality" runat="server" GroupingText="Quality" Width="100%" Height="40%">
                            <table width="100%">
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblQualityGeneralManager" runat="server" Text="General Manager"></telerik:RadLabel>
                                    </td>
                                    <td valign="baseline" style="width: 78%">
                                        <eluc:MCUser ID="ucQualityGM" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblQualitySuperintendent" runat="server" Text="Superintendent"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucQualitySupdtDesignation" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblQualityDirector" runat="server" Text="Director"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucQualityDirector" runat="server" Width="100%" emailrequired="true" designationrequired="true" />

                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlTravel" runat="server" GroupingText="Travel" Width="100%" Height="40%">
                            <table width="100%">
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblTravelPIC2" runat="server" Text="Travel PIC 2"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucTravelPIC2" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 22%">
                                        <telerik:RadLabel ID="lblTravelPIC3" runat="server" Text="Travel PIC 3"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 78%">
                                        <eluc:MCUser ID="ucTravelPIC3" runat="server" Width="100%" emailrequired="true" designationrequired="true" />
                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselAdminUser" Height="40%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvVesselAdminUser_ItemCommand" OnItemDataBound="gvVesselAdminUser_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvVesselAdminUser_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Invoice Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Status">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceStautsName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Designation Name">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDesignationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Designation ID="ucDesignationEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="gridinput_mandatory" DesignationList="<%#PhoenixRegistersDesignation.ListDesignation()%>" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Person In Charge">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkSupplierId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel Visible="false" ID="lblVesselAdminUserMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel Visible="false" ID="lblVesselAdminUserMapCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel Visible="false" ID="lblDesignationInvoiceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONINVOICEID") %>'></telerik:RadLabel>
                                <eluc:MCUser ID="UcPersonOfficeId" runat="server" Width="100%" emailrequired="false" designationrequired="false" SelectedValue='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

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
