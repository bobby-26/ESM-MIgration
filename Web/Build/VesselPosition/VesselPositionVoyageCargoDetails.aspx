<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionVoyageCargoDetails.aspx.cs" Inherits="VesselPositionVoyageCargoDetails" %>

<!DOCTYPE html >

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargotype" Src="~/UserControls/UserControlCargoTypeMappedVesselType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cargo Operations</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoyageCargoData" runat="server">
    <telerik:RadScriptManager  ID="ToolkitScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel runat="server" ID="pnlVoyageData">
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="Cargo Operation"></eluc:Title> 

                <telerik:RadGrid ID="gvVoyagePort" runat="server" AutoGenerateColumns="False" Font-Size="11px"  GridLines="None"
                    Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemCommand="gvVoyagePort_RowCommand" 
                    OnItemDataBound="gvVoyagePort_ItemDataBound" OnNeedDataSource="gvVoyagePort_NeedDataSource" OnSortCommand="gvVoyagePort_SortCommand" 
                    EnableHeaderContextMenu="true" GroupingEnabled="false" ShowFooter="true" ShowHeader="true" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVOYAGECARGOID"> 
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>                        
                        <telerik:GridTemplateColumn Visible="false" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPortHeader" Visible="true" Text=" Port " runat="server">                                        
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoyageCargoId" runat="server" Width="10%" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOYAGECARGOID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPort" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="uclblPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
         
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblBerthHeader" Text="Berth" Visible="true" runat="server">                                        
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBerth" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALBERTH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtBerthEdit" runat="server" CssClass = "input" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTUALBERTH") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtBerthAdd" runat="server" Visible="true" CssClass = "input"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCargoHeader" Text="Cargo" Visible="true" runat="server">                                        
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVoyageCargoIdEdit" runat="server" Width="10%" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOYAGECARGOID") %>'></telerik:RadLabel>
                                <eluc:Cargotype ID="ucCargoEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" SelectedCargoType='<%# DataBinder.Eval(Container,"DataItem.FLDCARGO") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Cargotype ID="ucCargo" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn >
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLoadedHeader" Text="Qty To Load" runat="server">                                        
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLoadedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTONNESLOADED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQtyLoadedEdit" runat="server" CssClass="input" MaxLength="10"
                                    DecimalPlace="2" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTONNESLOADED") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtQtyLoadedAdd" runat="server" CssClass="input" MaxLength="10"
                                    DecimalPlace="2" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn >
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDischargedHeader" Text="Qty To Discharge" runat="server">                                        
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDischargedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTONNESDISCHARGED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQtyDischargedEdit" runat="server" CssClass="input" MaxLength="10"
                                    DecimalPlace="2" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTONNESDISCHARGED") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtQtyDischargedAdd" runat="server" CssClass="input" MaxLength="10"
                                    DecimalPlace="2" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                            </HeaderTemplate>
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
