<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersYearEndExchangeRate.aspx.cs"
    Inherits="RegistersYearEndExchangeRate" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exchange Rate</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersExchangeRate" runat="server">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                      
               
                        <eluc:TabStrip ID="MenuMain" runat="server" TabStrip="true" OnTabStripCommand="MenuMain_TabStripCommand">
                        </eluc:TabStrip>
                 
                <br />
              
                    <table width="25%">
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblFinancialYear" Text="Financial Year"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadComboBox ID="ddlYear" RenderMode="Lightweight" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" EnableLoadOnDemand="true">
                                    <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2012" Text="2012"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2013" Text="2013"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2014" Text="2014"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2015" Text="2015"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2016" Text="2016"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2017" Text="2017"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2018" Text="2018"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2019" Text="2019"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2020" Text="2020"></telerik:RadComboBoxItem>
                                        </Items>
                                </telerik:RadComboBox>
                            </td>    
                        </tr>
                    </table>
             
                <br />
             
                    <eluc:TabStrip ID="MenuExchangeRate" runat="server" OnTabStripCommand="ExchangeRate_TabStripCommand">
                    </eluc:TabStrip>
            
                 <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="dgExchangerate" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="dgExchangerate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="dgExchangerate_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="98%" GroupingEnabled="false"  OnSelectedIndexChanging="dgExchangerate_SelectedIndexChanging"
                    OnItemDataBound="dgExchangerate_ItemDataBound" OnItemCommand="dgExchangerate_ItemCommand"
                    ShowFooter="true" ShowHeader="true" OnSortCommand="dgExchangerate_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >

                            <Columns>
                           <%-- <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Company Name">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblExchangeRateId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXCHANGERATEID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Financial Year End Date">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFinancialYearEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEARENDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FooterText="" HeaderText="Currency Code">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrencyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListAllActiveCurrency(iUserCode)%>'
                                        runat="server" ActiveCurrency="true" AppendDataBoundItems="true" CssClass="gridinput_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Base ExchangeRate">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBaseExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEEXCHANGERATE" ,"{0:f17}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnBaseExchangerateEdit">

                                        <eluc:Number ID="txtBaseExchangerateEdit" runat="server"  Width="120px"></eluc:Number>
                                      <%--  <asp:TextBox ID="txtBaseExchangerateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEEXCHANGERATE","{0:f17}") %>'
                                            CssClass="gridinput_mandatory txtNumber" MaxLength="25"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate2" runat="server" AutoComplete="false"
                                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number"
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtBaseExchangerateEdit" />--%>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                 

                                         <eluc:Number ID="txtBaseExchangerateAdd" runat="server"  Width="120px"></eluc:Number>
                                       <%-- <asp:TextBox ID="txtBaseExchangerateAdd" runat="server" CssClass="gridinput_mandatory txtNumber"
                                            MaxLength="25"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate3" runat="server" AutoComplete="false"
                                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number"
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtBaseExchangerateAdd" />--%>
                                   
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Report ExchangeRate">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                               
                                <ItemTemplate>
                                   <telerik:RadLabel ID="lblReportExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTEXCHANGERATE" ,"{0:f17}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnReportExchangerateEdit">
                                          <eluc:Number ID="txtReportExchangerateEdit" runat="server"  Width="120px"></eluc:Number>
                                      <%--  <asp:TextBox ID="txtReportExchangerateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTEXCHANGERATE","{0:f17}") %>'
                                            CssClass="gridinput_mandatory txtNumber" MaxLength="25"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate1" runat="server" AutoComplete="false"
                                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number"
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtReportExchangerateEdit" />--%>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnReportExchangerateAdd">
                                        <eluc:Number ID="txtReportExchangerateAdd" runat="server"  Width="120px"></eluc:Number>
                                       <%-- <asp:TextBox ID="txtReportExchangerateAdd" runat="server" CssClass="gridinput_mandatory txtNumber"
                                            MaxLength="25"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate4" runat="server" AutoComplete="false"
                                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number"
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtReportExchangerateAdd" />--%>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                
                                <FooterTemplate>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Center"  Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdCancel" runat="server" AlternateText="Cancel" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png%>" ToolTip="Cancel" />
                                </EditItemTemplate>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>


            