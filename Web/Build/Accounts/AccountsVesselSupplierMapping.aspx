<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselSupplierMapping.aspx.cs"
    Inherits="AccountsVesselSupplierMapping" %>

<!DOCTYPE html >
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselSupplierMapping" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <%--<eluc:Title runat="server" ID="ucTitle" Text="Vessel Supplier Mapping" />--%>
            <%--  <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>

            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuGeneralSub" runat="server" OnTabStripCommand="MenuGeneralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <table id="SupplierMapTable" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselSupplierCodeName" runat="server" Text="Vessel Supplier Code & Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupplierCode" Width="90px" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSupplierName" Width="200px" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                        <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                            ID="cmdMoreInfoVesselSupplier" ToolTip="More Info"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMappedSupplierCodeName" runat="server" Text="Mapped Supplier Code & Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVenderCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" BorderWidth="1px" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickVender" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132,130,131&txtsupcode='+ document.getElementById('txtVenderCode').value +'&txtsupname='+ document.getElementById('txtVenderName').value, true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtVenderID" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton runat="server" AlternateText="Map Supplier" ImageUrl="<%$ PhoenixTheme:images/owner.png %>"
                            ID="imgSupplier" ToolTip="Map Supplier" ImageAlign="Middle" OnClick="imgSupplier_Map"></asp:ImageButton>
                        <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                            ID="cmdMoreInfoMappedSupplier" ToolTip="More Info" ImageAlign="Middle"></asp:ImageButton>
                        <asp:ImageButton runat="server" AlternateText="Unmap Supplier" ImageUrl="<%$ PhoenixTheme:images/off-signer.png %>"
                            ID="imgUnMap" ToolTip="Unmap Supplier" OnClick="imgUnMap_Click"></asp:ImageButton>
                    </td>
                    <%--<td>
                                <Telerik:RadTextBox ID="txtMappedSupplierCode" Width="90px" runat="server" ReadOnly="true"
                                    CssClass="input"></Telerik:RadTextBox>
                                <Telerik:RadTextBox ID="txtMappedSupplierName" Width="200px" runat="server" ReadOnly="true"
                                    CssClass="input"></Telerik:RadTextBox>
                                <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                    ID="cmdMoreInfoMappedSupplier" ToolTip="More Info"></asp:ImageButton>
                            </td>--%>
                </tr>
            </table>

            <hr />
            <b>
                <telerik:RadLabel ID="lblAdvancePayment" runat="server" Text="Advance Payment"></telerik:RadLabel>
            </b>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrderNumber" runat="server" Text="Order Number :"></telerik:RadLabel>
                        &nbsp;&nbsp; &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOrderNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderPartPaid" Height="70%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvOrderPartPaid_ItemCommand" OnItemDataBound="gvOrderPartPaid_ItemDataBound" OnNeedDataSource="gvOrderPartPaid_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblOrderPartPaidId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERPARTPAIDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDescription" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOrderPartPaidIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERPARTPAIDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="input_mandatory" MaxLength="200" Width="120px">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" Width="120px" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="35%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%-- <telerik:RadTextBox ID="txtAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                    Style="text-align: right" CssClass="input_mandatory" MaxLength="14">
                                </telerik:RadTextBox>--%>
                                <%--<ajaxtoolkit:maskededitextender id="mskAmount" runat="server" targetcontrolid="txtAmountEdit"
                                    oninvalidcssclass="MaskedEditError" mask="9,999,999.99" masktype="Number" inputdirection="RightToLeft"
                                    autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtAmountEdit" CssClass="input_mandatory" oninvalidcssclass="MaskedEditError" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                    targetcontrolid="txtAmountEdit" Width="120px" autocomplete="false" inputdirection="RightToLeft" MaxLength="14" Mask="#,###,###.##">
                                </telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<telerik:RadTextBox ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="14"
                                    Style="text-align: right">
                                </telerik:RadTextBox>--%>
                                <%-- <ajaxtoolkit:maskededitextender id="mskAmountAdd" runat="server" targetcontrolid="txtAmountAdd"
                                    oninvalidcssclass="MaskedEditError" mask="9,999,999.99" masktype="Number" inputdirection="RightToLeft"
                                    autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtAmountAdd" oninvalidcssclass="MaskedEditError" CssClass="input_mandatory" MaxLength="14"
                                    targetcontrolid="txtAmountAdd" Width="120px" autocomplete="false" inputdirection="RightToLeft" Mask="#,###,###.##">
                                </telerik:RadMaskedTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="true" SortExpression="FLDFIELDNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                <eluc:Currency ID="ucCurrencyEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ucCurrencyAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance Payment Number">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance Payment Date" AllowSorting="true" SortExpression="FLDVOUCHERDATE">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Adv. Pymt. Status">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdvPymtStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREJECTREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREJECTREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTREMARKS") %>' />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Approve"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

