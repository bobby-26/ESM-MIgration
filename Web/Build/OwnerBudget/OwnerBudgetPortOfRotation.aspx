<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetPortOfRotation.aspx.cs" Inherits="OwnerBudgetPortOfRotation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvContactType.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmContactType" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxPanel runat="server" ID="pnlContactType">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table id="tblCauseSearch" width="70%">
                <tr>
                    <td>
                        <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Nationality runat="server" ID="ucNationality" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuInspectionContactType" runat="server" OnTabStripCommand="InspectionContactType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvContactType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="true" AllowSorting="true" AllowCustomPaging="true" AllowPaging="true"
                ShowHeader="true" EnableViewState="false" OnSortCommand="gvContactType_SortCommand" OnNeedDataSource="gvContactType_NeedDataSource"
                OnItemCommand="gvContactType_ItemCommand"
                OnItemDataBound="gvContactType_ItemDataBound"
                OnUpdateCommand="gvContactType_UpdateCommand"
                OnDeleteCommand="gvContactType_DeleteCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Seq" HeaderStyle-Width="315px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPortofRotation" runat="server" Text="Port of Rotation"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortOfRotationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTOFROTATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPortOfRotation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ROTATIONPORT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRotationPort" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROTATIONPORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucPortofRotaion" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    QuickTypeCode="115" Width="300px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="325px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAirFare" runat="server" Text="AirFare"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirfare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAirfare" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDAIRFARE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAirfareAdd" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="325px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAgencyFee" runat="server" Text="Agency Fee"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgencyFee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENCYFEE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAgencyFee" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDAGENCYFEE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAgencyFeeAdd" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="20px" Wrap="false" />
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
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        setTimeout(function () {
                            TelerikGridResize($find("<%= gvContactType.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
