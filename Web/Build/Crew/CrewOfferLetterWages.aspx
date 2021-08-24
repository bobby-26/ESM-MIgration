<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOfferLetterWages.aspx.cs"
    Inherits="CrewOfferLetterWages" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Agreed component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewOfferLetter" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewOfferLetter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" />

            <telerik:RadGrid ID="gvCrew" Width="99%" EnableViewState="true" ShowFooter="true" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound" runat="server"
                OnNeedDataSource="gvContract_NeedDataSource" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Left" Width="35%" />
                            <ItemTemplate>
                                 <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblContractCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                                </telerik:RadLabel>

                                <telerik:RadLabel ID="lblShortCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:CrewComponents ID="ddlCrewComponentsAdd" runat="server" CrewComponentsList='<%#Component()%>' AppendDataBoundItems="true" Width="99%" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" Width="12%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' MaxLength="8" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input" MaxLength="8" Text="" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">

                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Left" Width="12%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="dropdown_mandatory" Width="60px"
                                    CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' AppendDataBoundItems="true"
                                    SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCY") %>' />

                                <telerik:RadLabel ID="lblCurrencyEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" AutoPostBack="false"
                                    CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' SelectedCurrency="10" Width="60px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payable">
                            <HeaderStyle Width="18%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASICDESC")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox
                                    ID="TxtPayableEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASICDESC") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblPayableAdd" runat="server" Text=""></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
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
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                                      <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>


            <eluc:Status runat="server" ID="ucStatus" />
            <asp:HiddenField runat="server" ID="hdnappraisalcomplatedyn" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
