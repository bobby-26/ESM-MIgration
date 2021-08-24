<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMROilType.aspx.cs"
    Inherits="RegistersDMROilType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProductType" Src="~/UserControls/UserControlProductType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Oil Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterOilType" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:Title runat="server" ID="ucTitle" Text="Product"></eluc:Title>--%>
            <eluc:TabStrip ID="MenuRegistersOilType" runat="server" OnTabStripCommand="RegistersOilType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvOilType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvOilType_ItemCommand" OnItemDataBound="gvOilType_ItemDataBound"
                AllowSorting="true" OnSorting="gvOilType_Sorting" ShowFooter="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvOilType_NeedDataSource"
                RenderMode="Lightweight" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <%-- <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Conversion Factor" Name="Conversion" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Short Code" FooterText="New Short" AllowSorting="true" SortExpression="FLDSHORTNAME">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOilShortNameEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' Enabled="false" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOilShortNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="100%" ToolTip="Enter Oil Short Name" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Oil Type Description" FooterText="New Oil" AllowSorting="true" SortExpression="FLDOILTYPECODE">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOilTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOilTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'>
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtOilTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOilTypeNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="100%" ToolTip="Enter OilType Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%#PhoenixRegistersUnit.ListDMRProductUnit() %>'
                                    SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' CssClass="gridinput_mandatory" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ucUnitAdd" runat="server" AppendDataBoundItems="true" DataSource='<%#PhoenixRegistersUnit.ListDMRProductUnit() %>'
                                    DataTextField="FLDUNITNAME" DataValueField="FLDUNITID" CssClass="gridinput_mandatory" Width="100%">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Product type" AllowSorting="true" SortExpression="FLDPRODUCTTYPENAME">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProductTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPEID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblProductTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ProductType ID="ucProductTypeEdit" runat="server" AppendDataBoundItems="true"
                                    ProductTypeList='<%# PhoenixRegistersDMRProductType.ListProductType() %>'
                                    SelectedProductType='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:ProductType ID="ucProductTypeAdd" runat="server" AppendDataBoundItems="true" Width="100%"
                                    ProductTypeList='<%# PhoenixRegistersDMRProductType.ListProductType() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Specific Gravity" AllowSorting="true" SortExpression="FLDSPECIFICGRAVITY">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75px"></HeaderStyle>
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="100%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpecificGravity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPECIFICGRAVITY") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSpecificGravityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPECIFICGRAVITY") %>'
                                    DecimalPlace="2" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucSpecificGravityAdd" runat="server" DecimalPlace="2" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Conv Required">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequiredYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONREQUIRED") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox Width="100%" runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDCONVERSIONREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkRequiredAdd" Checked="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Conversion" HeaderText="LTR" FooterText="New Oil">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConvFactorLTR" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORLTR") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucConvFactorLTREdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORLTR") %>'
                                    DecimalPlace="2" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucConvFactorLTRAdd" runat="server" DecimalPlace="2" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Conversion"  HeaderText="M3" FooterText="New Oil">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConvFactorm3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORM3") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucConvFactorm3Edit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORM3") %>'
                                    DecimalPlace="2" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucConvFactorm3Add" runat="server" DecimalPlace="2" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Conversion"  HeaderText="BBL" FooterText="New Oil">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConvFactorBBL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORBBL") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucConvFactorBBLEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTORBBL") %>'
                                    DecimalPlace="2" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucConvFactorBBLAdd" runat="server" DecimalPlace="2" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Color">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <div id="divColor" runat="server" style='<%# "width:80px; height:10px; background-color:" + DataBinder.Eval(Container,"DataItem.FLDCOLOR") %>'>
                                </div>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <%--    <telerik:RadTextBox ID="txtColorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLOR") %>'>
                            </telerik:RadTextBox>--%>

                                <telerik:RadColorPicker RenderMode="Lightweight" runat="server"
                                    SelectedColor="#FFFF00" ID="txtColorEdit" ShowIcon="true" Preset="Standard">
                                </telerik:RadColorPicker>


                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<telerik:RadTextBox ID="txtColorAdd" runat="server"   Width="100%" >
                            </telerik:RadTextBox>--%>
                                <telerik:RadColorPicker RenderMode="Lightweight" runat="server"
                                    SelectedColor="#FFFF00" ID="txtColorAdd" ShowIcon="true" Preset="Standard">
                                </telerik:RadColorPicker>

                                <%--   <telerik:RadColorPicker ID="txtColorAdd_ColorPickerExtender" TargetControlID="txtColorAdd"   Width="100%" 
                                Enabled="True" runat="server">
                            </telerik:RadColorPicker>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order" AllowSorting="true" SortExpression="FLDSORTORDER">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilTypeSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                    IsInteger="true">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtOilTypeSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" IsInteger="true" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtOilTypeSortOrderAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" IsInteger="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
