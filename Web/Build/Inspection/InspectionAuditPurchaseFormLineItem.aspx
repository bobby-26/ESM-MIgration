<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditPurchaseFormLineItem.aspx.cs"
    Inherits="InspectionAuditPurchaseFormLineItem" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="Currency" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="UserControlQuick" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItem" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuLineItemGeneral" runat="server" OnTabStripCommand="MenuLineItemGeneral_TabStripCommand"></eluc:TabStrip>
            <br clear="all" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickItem">
                            <eluc:MaskNumber runat="server" ID="txtItemNumber" MaxLength="20" ReadOnly="true" Width="90px" />
                            <telerik:RadTextBox ID="txtServiceNumber" runat="server" Width="90px" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="cmdShowItem" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        </span>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" Width="120px" ReadOnly="True"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmakerRef" runat="server" Text="Maker Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMakerReference" runat="server" Width="120px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPartName" runat="server" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceiptstatus" runat="server" Text="Receipt status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="ucReciptstatus" AppendDataBoundItems="true" 
                            runat="server" Width="120px" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtExtraNumber" runat="server" Width="90px"  Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trComponent" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickComponent">
                            <telerik:RadTextBox ID="txtComponent" runat="server" Width="90px" Enabled="false"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" Width="180px" Enabled="false"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="cmdShowComponent" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentID" runat="server" Width="16px" />
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDrawingNo" runat="server" Text="Drawing No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDrawingNo" runat="server" Width="120px" MaxLength="50"></telerik:RadTextBox>
                        <telerik:RadCheckBox ID="chkBudgetedPurchase" runat="server" Text=" Budgeted Purchase" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPosition" runat="server" Text="Position"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPosition" runat="server" Width="120px" Enabled="false"></telerik:RadTextBox>
                        <telerik:RadCheckBox ID="chkIncludeOnForm" runat="server" Text="Include On Form" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPrice" runat="server">Price </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" Width="92px" 
                            runat="server" Visible="false" />
                        <eluc:Decimal ID="txtPrice" runat="server" ReadOnly="true" Mask="999,999,999,999.99" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestedQTY" runat="server" Text="Requested Qty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtRequestedQty" runat="server" Width="120px" Mask="99999" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderQty" runat="server" Text="Order Qty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtOrderQty" runat="server" Width="120px" Mask="99999" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Unit ID="ucUnit" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" Width="120px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceivedQTY" runat="server" Text="Received Qty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtRecivedQty" runat="server" Width="120px" Mask="99,999" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCancelledQTY" runat="server" Text="Cancelled Qty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtCanceledQty" runat="server" Width="120px" Mask="99,999" 
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvLineItem_ItemCommand" OnItemDataBound="gvLineItem_ItemDataBound"
                OnUpdateCommand="gvLineItem_UpdateCommand" OnNeedDataSource="gvLineItem_NeedDataSource"
                AllowSorting="True" EnableViewState="false" OnSorting="gvLineItem_Sorting">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID">
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
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="3%" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkSelect" OnCheckedChanged="CheckBoxClicked" AutoPostBack="true"></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="1%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblIsSelectedHeader" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                <telerik:RadLabel ID="lblIsFormNotes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTES") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsItemDetails" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILFLAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S. No.">
                            <HeaderStyle Width="3%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <HeaderStyle Width="6%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maker Reference">
                            <HeaderStyle Width="6%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMakerReference" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="16%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPartId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="SELECTITEM"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton><br />
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB">
                            <HeaderStyle Width="4%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROBQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested Qty">
                            <HeaderStyle Width="7%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrequestedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Order">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPLITFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Qty">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtOrderQtyEdit" runat="server" Width="90px" Mask="99999" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <HeaderStyle Width="7.5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="btnShowBudgetEdit" ImageAlign="AbsMiddle" Text="..">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Owner Budget Code">
                            <HeaderStyle Width="7.5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'
                                        MaxLength="20" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="btnShowOwnerBudgetEdit" ImageAlign="AbsMiddle" Text="..">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Receipt Status">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceiptStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChkRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHKREMARKS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                    CommandName="ViewRecord" ID="imgReceiptRemarks"
                                    ToolTip="View"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="Details" ID="cmdDetail" ToolTip="Item Details">
                                    <%--<span class="icon"><i class="fas fa-file-pdf"></i></span>--%>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table width="100%" border="0" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/detail-flag.png %>" /><telerik:RadLabel
                            ID="lblMessage" runat="server" ForeColor="Red">
                            Line item Details.
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
