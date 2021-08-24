<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsDocumentSerial.aspx.cs"
    Inherits="OptionsDocumentSerial" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocType" Src="~/UserControls/UserControlDocmentsType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    </head>
<body>

     <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <form id="frmDocumentSerial" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          <%--  <eluc:Title runat="server" ID="ucTitle" Text="Serial" ShowMenu="false"></eluc:Title>--%>
             <eluc:Status ID="ucStatus" runat="server"></eluc:Status>

                    <table id="tblConfigureCountry" Width="100%">
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></Telerik:RadLabel>

                                 <eluc:DocType ID="ucDocType" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="dropdown_mandatory" Width="350px"/>
                            </td>
                        </tr>
                    </table>

               <eluc:TabStrip ID="MenuDocumentSerial" runat="server" OnTabStripCommand="MenuDocumentSerial_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentSerial" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentSerial_ItemCommand" OnItemDataBound="gvDocumentSerial_ItemDataBound" OnNeedDataSource="gvDocumentSerial_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSERIALID">
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
                        <telerik:GridTemplateColumn HeaderText="Document Type" AllowSorting="true" SortExpression="FLDDOCUMENTTYPE">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                  <Telerik:RadLabel ID="lblDocumentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></Telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Company" >
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblCompanyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkCompanyName" runat="server" CommandName="EDIT"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>

                               <eluc:Company ID="ucCompanyEdit" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="250px"
                                        AppendDataBoundItems="true" CssClass="gridinput" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <eluc:Company ID="ucCompanyAdd" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="250px"
                                        AppendDataBoundItems="true" CssClass="gridinput" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Vessel" >
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                    <eluc:Vessel ID="ucVesselEdit" runat="server" VesselList='<%# PhoenixRegistersVessel.ListVessel()%>' Width="250px"
                                        AppendDataBoundItems="true" CssClass="gridinput" />
                                </EditItemTemplate>
                            <FooterTemplate>
                                 <eluc:Vessel ID="ucVesselAdd" runat="server" CssClass="gridinput" VesselList='<%# PhoenixRegistersVessel.ListVessel()%>' Width="250px"
                                        AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Serial" AllowSorting="true" SortExpression="FLDSERIAL">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblSerialId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALID") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkDocumentType" runat="server" CommandName="EDIT"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIAL") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                     <Telerik:RadLabel ID="lblSerialEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALID") %>'></Telerik:RadLabel>
                                     <eluc:Number ID="txtSerialEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIAL") %>'
                                        CssClass="gridinput_mandatory" ></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                              <%--  <Telerik:RadTextBox ID="txtSerialAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Serial Number"></Telerik:RadTextBox>--%>
                                    <eluc:Number ID="txtSerialAdd" runat="server" CssClass="gridinput_mandatory" Width="100px" ></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="20%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
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


