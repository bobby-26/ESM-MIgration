<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillJobRegister.aspx.cs" Inherits="Registers_DrillJobRegister" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drill Job</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
    </telerik:RadCodeBlock>



</head>
<body>

    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillJoblist,table1" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" DefaultLoadingPanelID = "RadAjaxLoadingPanel1" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvDrillJoblist">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDrillJoblist" UpdatePanelHeight="80%" />
            <telerik:AjaxUpdatedControl ControlID="ucError"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlvessellist">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDrillJoblist" UpdatePanelHeight="80%" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

        
        
            <eluc:TabStrip ID="Tabstripdrilljobregister" runat="server" OnTabStripCommand="drilljobregister_TabStripCommand" TabStrip="true">
            </eluc:TabStrip>
        
        <table id="table1">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="lblvesselname" runat="server" Text="Vessel Name" />
                    </th>
                    <th>
                        :
                    </th>
                    <td>
                        <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true"  ManagementType="FUL" 
                      Entitytype="VSL"  AutoPostBack = "true" ActiveVesselsOnly="true" OnTextChangedEvent="ddlvessellist_textchange" VesselsOnly="true"/>
                    </td>
                </tr>
            </tbody>
        </table>
        
        
            <eluc:TabStrip ID="Tabstripdrilljobregistermenu" runat="server" OnTabStripCommand="drilljobregistermenu_TabStripCommand" TabStrip="true">
            </eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillJoblist" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillJoblist_NeedDataSource" Height="100%"
            OnUpdateCommand="gvDrillJoblist_UpdateCommand" OnItemDataBound="gvDrillJoblist_ItemDataBound">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLJOBID" AutoGenerateColumns="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState ="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Drill">
                        <HeaderStyle HorizontalAlign="Center" Width="100px"/>
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Interval" >
                        <HeaderStyle HorizontalAlign="Center" Width="181px"/>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDrillFrequency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")+" "+DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                                <eluc:MaskNumber ID="tbfrequencyedit" runat="server" Width="35px" MaskText="##" MaxLength="2" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")%>'
                                    IsPositive="true" IsInteger="true" />
                                <eluc:Hard ID="Radddlfrequencyedit" runat="server" CssClass="input_mandatory" HardTypeCode="260"
                                    HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(260,1,null, null) %>' Width="100px"
                                    AutoPostBack="false" />
                            </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Fixed/Variable">
                            <HeaderStyle HorizontalAlign="Center" Width="131px"/>
                            <ItemStyle HorizontalAlign="Center"  Width="102px"/>
                            <FooterStyle HorizontalAlign="Center" Width="102px"/>
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfixedorvariable" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFIXEDORVARIABLE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <eluc:Hard ID="Radcbfixedorvariableedit" runat="server" CssClass="input_mandatory" HardTypeCode="258" HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(258,1,null, null) %>'
                                    Width="102px" AutoPostBack="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <HeaderStyle HorizontalAlign="Center" Width="160px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlbltype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>' >
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Hard ID="Radddltypeedit" runat="server" CssClass="input_mandatory" HardTypeCode="259" HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(259,1,null, null) %>'
                                Width="130px" AutoPostBack="false" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Affected by Crew Change?">
                            <HeaderStyle HorizontalAlign="Center" Width="98px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblaffectedbycrewchange" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAFFECTEDBYCREWCHANGEYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="Radcheckcrewchangeedit" runat="server" AutoPostBack="true" OnCheckedChanged="crewchangeeffect" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText=" Crew Change Percentage">
                            <HeaderStyle HorizontalAlign="Center" Width="94px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblaffectedbycrewchangepercentage" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPERCENTAGE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:MaskNumber ID="tbcrewpercentedit" runat="server" Width="35px" MaskText="##" MaxLength="2" CssClass="input"
                                    IsPositive="true" IsInteger="true"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPERCENTAGE")%>' ReadOnly="true"/>
                                <eluc:MaskNumber ID="MaskNumber1" runat="server" Width="35px" MaskText="##" MaxLength="2" CssClass="input" Visible="false"
                                    IsPositive="true" IsInteger="true"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPERCENTAGE")%>' ReadOnly="true"/>
                            </EditItemTemplate>
                            
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Photo Mandatory">
                            <HeaderStyle HorizontalAlign="Center" Width="115px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblphotoyn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPHOTOYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="Radcbphotoynedit" runat="server" AutoPostBack="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show in Dashboard">
                            <HeaderStyle HorizontalAlign="Center" Width="123px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDashboardyn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDASHBOARDYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="Radcbdashboardynedit" runat="server" AutoPostBack="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="150px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>--%>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" FrozenColumnsCount="1" EnableNextPrevFrozenColumns="true" />
                   
                </ClientSettings>
        </telerik:RadGrid>
   
    
    </form>
</body>
</html>
