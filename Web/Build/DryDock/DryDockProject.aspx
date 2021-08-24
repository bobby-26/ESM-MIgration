<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockProject.aspx.cs" Inherits="DryDockProject"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DryDock" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Dry Dock Project</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <%-- <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvProjectLineItem.ClientID %>"));
                    
                }, 200);
           }
           window.onresize = window.onload = Resize;
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmProject" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuProjectGeneral" runat="server" OnTabStripCommand="ProjectGrid_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuProjectGrid" runat="server" OnTabStripCommand="ProjectGrid_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvProject" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="true"
                OnNeedDataSource="gvProject_NeedDataSource"
                OnItemDataBound="gvProject_ItemDataBound1" ViewStateCompression="off"
                OnItemCommand="gvProject_ItemCommand" ClientSettings-EnablePostBackOnRowClick="true"
                Height="40%"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>

                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />

                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Project ID" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Docking Category" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Est From" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Est To" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Started" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="7%" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEnddate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="180px" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>


                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                    
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Owner Export XL"
                                    CommandName="EXPORT2XL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdExport2XL"
                                    ToolTip="Owner Export XL">
                                    <span class="icon"><i class="fas fa-file-excel"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="SPARE"
                                    CommandName="SPARE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSpare"
                                    ToolTip="Spare">
                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="DryDock Estimate"
                                    CommandName="ESTIMATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEstimate"
                                    ToolTip="DryDock Estimate">
                               
                                    <span class="icon"><i class="fa fa-calculator"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Initiate Docking"
                                    CommandName="INITIATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdInitiate"
                                    ToolTip="Initiate Docking">
                                    <span class="icon"><i class="fab fa-docker"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdAttachment" runat="server" AlternateText="Attachment" CommandName="ATTACHMENT"
                                    CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>

                                </asp:LinkButton>
                                <asp:LinkButton ID="linkbtncostinccured" runat="server" AlternateText="Cost Incurred"
                                    ToolTip="Cost Incurred">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span>

                                </asp:LinkButton>
                                 <asp:LinkButton ID="linkbtncostsummary" runat="server" AlternateText="Cost Summary" Visible="false"
                                    ToolTip="Cost Summary">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Selecting-AllowRowSelect="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:TabStrip ID="MenuProjectLineItemGrid" runat="server" OnTabStripCommand="ProjectLineItemGrid_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvProjectLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvProjectLineItem_NeedDataSource" OnItemDataBound="gvProjectLineItem_ItemDataBound"
                OnItemCommand="gvProjectLineItem_ItemCommand" Height="43.25%" EnableViewState="true"
                OnUpdateCommand="gvProjectLineItem_UpdateCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBID">

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Est. Price" Name="Estprice" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" />

                    </ColumnGroups>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="12%" />
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderLineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobDetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                  <telerik:RadLabel ID="lblJobDetailidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescriptionedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="8%" Wrap="true" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                     
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlResponsibility" runat="server" AppendDataBoundItems="true"
                                    DataSource='<%# PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5)%>' DataTextField="FLDNAME" Width="100px"
                                    DataValueField="FLDMULTISPECID" DefaultMessage="--Select--">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="10%" Wrap="true" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblbudgetcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlbudgetcode" runat="server" AppendDataBoundItems="true"
                                    DataTextField="FLDBUDGETCODE" Width="100px" EnableDirectionDetection="true"
                                    DataValueField="FLDBUDGETID" DefaultMessage="--Select--">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Count" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTCOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Qty" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOrderLineidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'
                                    Width="50px" MaxLength="9" DecimalPlace="2"></eluc:Number>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" Width="120px"
                                    UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-HorizontalAlign="center" ColumnGroupName="Estprice">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPriceEdit" runat="server" Width="50px" CssClass="gridinput"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE","{0:n3}") %>' DecimalPlace="3"
                                    MaxLength="16" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gross" HeaderStyle-HorizontalAlign="center" ColumnGroupName="Estprice">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrossPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROSSPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtGrossPriceEdit" runat="server" Width="50px" CssClass="gridinput"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROSSPRICE","{0:n3}") %>' DecimalPlace="3"
                                    MaxLength="16" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Disc.(%)" HeaderStyle-HorizontalAlign="center" ColumnGroupName="Estprice">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="txtDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtDiscountEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'
                                    Mask="99.99" MaxLength="5" Width="50px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Net " HeaderStyle-HorizontalAlign="center" ColumnGroupName="Estprice">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtNetPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNETPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtNetPriceEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNETPRICE","{0:n2}") %>'
                                    Mask="99.99" Width="50px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Approved price " HeaderStyle-HorizontalAlign="center" >
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtapprovedprice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtapprovedpriceedit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDPRICE","{0:n2}") %>'
                                    Mask="99.99" Width="50px" />
                            </EditItemTemplate>
                       
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Exclude Y/N" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDOWNEREXCLUDEYN").ToString() =="1" ? "Yes" : "" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkOwnerExclude" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDOWNEREXCLUDEYN").ToString() =="1" ? true : false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manager Remarks" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="imgManagerRemarks" runat="server"><span class="icon"><i class="fas fa-glasses"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Remarks" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>

                                <asp:LinkButton ID="imgOwnerRemarks" runat="server"><span class="icon"><i class="fas fa-glasses"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supt Remarks" Visible="false">

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>

                                <asp:LinkButton ID="imgSuptRemarks" runat="server"><span class="icon"><i class="fas fa-glasses"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="5%" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Height="20px" Width="20px" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <%--  <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />--%>
                                <asp:LinkButton Height="20px" Width="20px" runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                 <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <EditItemTemplate>
                                <asp:LinkButton Height="20px" Width="20px" runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <%-- <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" alt="" />--%>
                                <asp:LinkButton Height="20px" Width="20px" runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
