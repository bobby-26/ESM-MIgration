<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsDocumentNumberFormat.aspx.cs"
    Inherits="OptionsDocumentNumberFormat" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fields" Src="~/UserControls/UserControlDocmentFields.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocType" Src="~/UserControls/UserControlDocmentsType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country</title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
     </head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           <%-- <eluc:Title runat="server" ID="ucTitle" Text="Format" ShowMenu="false"></eluc:Title>--%>
             <eluc:Status ID="ucStatus" runat="server"></eluc:Status>   
             <table id="tblConfigureCountry" width="100%">
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></Telerik:RadLabel>
                          
                             <eluc:DocType ID="ucDocType" runat="server" 
                                        AppendDataBoundItems="true" AutoPostBack="true"  CssClass="dropdown_mandatory" width="100%" />
                             
                            </td>
                        </tr>
                    </table>
                   <eluc:TabStrip ID="MenuDocumentNumberFormat" runat="server" OnTabStripCommand="MenuDocumentNumberFormat_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentNumberFormat" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="false"  AllowPaging="true" 
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentNumberFormat_ItemCommand" OnItemDataBound="gvDocumentNumberFormat_ItemDataBound" OnNeedDataSource="gvDocumentNumberFormat_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblDocumentTypeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPEID") %>' Visible="false"></Telerik:RadLabel>
                                  <Telerik:RadLabel ID="lblDocumentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                              <EditItemTemplate>

                               <Telerik:RadLabel ID="lblDocumentTypeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPEID") %>' Visible="false"></Telerik:RadLabel>
                               <Telerik:RadLabel ID="lblDocumentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></Telerik:RadLabel>
                            </EditItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Field Name" AllowSorting="true" SortExpression="FLDFIELDNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblFields" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Fields ID="ucFieldsEdit" runat="server" DocmentFieldsList ='<%# PhoenixCommanDocuments.ListDocumentNumberFields(null)%>'
                                        AppendDataBoundItems="true" CssClass="gridinput"  SelectedDocmentFields='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Fields ID="ucFieldsAdd" runat="server" CssClass="gridinput" DocmentFieldsList ='<%# PhoenixCommanDocuments.ListDocumentNumberFields(null)%>'
                                        AppendDataBoundItems="true" width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                         
                        <telerik:GridTemplateColumn HeaderText="Value" AllowSorting="true" SortExpression="FLDVALUE">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblValuesId" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></Telerik:RadLabel>
                                    <Telerik:RadLabel ID="lblValues" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                              <EditItemTemplate>

                                 <Telerik:RadLabel ID="lblValues" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></Telerik:RadLabel>
                                    <Telerik:RadTextBox ID="txtValuesEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'
                                        MaxLength="10"></Telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <Telerik:RadTextBox ID="txtValuesAdd" runat="server"
                                        MaxLength="10"></Telerik:RadTextBox>
                            </FooterTemplate>
                           
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Order" AllowSorting="true" SortExpression="FLDORDER">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblorder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>

                               <eluc:Number ID="txtorderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>'
                                        CssClass="gridinput" MaxLength="200"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                  <eluc:Number ID="txtorderAdd" runat="server" CssClass="gridinput"
                                        MaxLength="200"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
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
                      
                        
                       
 
