<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestPayment.aspx.cs"
    Inherits="CrewLicenceRequestPayment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence Request Payment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicReq" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="btnconfirm_Click" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewLicReq" runat="server" OnTabStripCommand="CrewLicReq_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuLicence" runat="server" OnTabStripCommand="MenuLicence_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="readonlytextbox" Width="75%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeName" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="ltVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVessel" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFlag" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" ReadOnly="true" CssClass="" Enabled="false" Width="75%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Consulate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtConsulate" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCurrency" ReadOnly="true" Width="75%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="75%"
                            Readonly="true" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BudgetCode runat="server" ID="ucBudgetCode" AppendDataBoundItems="true" AutoPostBack="true" Width="75%"
                            CssClass="dropdown_mandatory" BudgetCodeList='<%# PhoenixRegistersBudget.ListBudget() %>'></eluc:BudgetCode>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentBy" runat="server" Text="Payment By"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadRadioButtonList ID="rblPaymentBy" runat="server" Layout="Flow" Columns="2" Direction="Horizontal"
                            AutoPostBack="true">
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadRadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>

                        <asp:LinkButton runat="server" ID="imgBankInfo" CommandName="BANKINFO" OnClick="OnClickBankInfo"
                            ToolTip="Bank Info">
                                <span class="icon"><i class="fas fa-landmark"></i></span>
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" ID="imgAdvancePayement" CommandName="ADVANCEPAYMENT" OnClick="OnClickAdvancePayment"
                            ToolTip="Advance Payment">
                                <span class="icon"><i class="fa fa-coins"></i></span>
                        </asp:LinkButton>

                    </td>
                </tr>
            </table>
            <hr />
            <br />

            <telerik:RadLabel ID="ltNational" runat="server" Font-Bold="true" Text="National Documents"></telerik:RadLabel>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvMissingLicence" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvMissingLicence_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvMissingLicence_ItemDataBound"
                ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicence" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="Literal1" runat="server" Font-Bold="true" Text="Flag Documents"></telerik:RadLabel>
            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvFlag" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvFlag_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvFlag_ItemDataBound" OnUpdateCommand="gvFlag_UpdateCommand"
                ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="National Document">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicence" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestDetailIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                    MaxLength="14" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />

            <telerik:RadLabel ID="ltDCEList" runat="server" Font-Bold="true" Text="Dangerous Cargo Endorsements"></telerik:RadLabel>

            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvDCE" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvDCE_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvDCE_ItemDataBound" OnUpdateCommand="gvDCE_UpdateCommand"
                ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEELICENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLicence" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestDetailIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                    MaxLength="14" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="ltSeamansBook" runat="server" Font-Bold="true" Text="Seaman's Book List"></telerik:RadLabel>
            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSeamanBook" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvSeamanBook_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvSeamanBook_ItemDataBound" OnUpdateCommand="gvSeamanBook_UpdateCommand"
                ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLicence" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestDetailIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                    MaxLength="14" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="ltCourse" runat="server" Font-Bold="true" Text="Course List"></telerik:RadLabel>
            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCourse" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCourse_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCourse_ItemDataBound" OnUpdateCommand="gvCourse_UpdateCommand"
                ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestDetailIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDETAILID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                    MaxLength="14" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
