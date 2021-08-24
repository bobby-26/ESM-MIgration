<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRTankPlanLocation.aspx.cs"
    Inherits="RegistersDMRTankPlanLocation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR Tank Plan Location</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLocation" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
            <%--<eluc:Title runat="server" ID="ucTitle" Text="Tank Plan Location"></eluc:Title>--%>
        <table>
            <tr><td><telerik:RadLabel ID="lblvesselname" runat="server" Text="Vessel Name"></telerik:RadLabel> </td>
                <td> <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                VesselsOnly="true" AppendDataBoundItems="true" />  </td></tr>
        </table>
           
            <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid ID="gvLocation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvLocation_ItemCommand" OnItemDataBound="gvLocation_ItemDataBound"
                AllowSorting="true" OnSorting="gvLocation_Sorting" ShowFooter="true" OnNeedDataSource="gvLocation_NeedDataSource"
                ShowHeader="true" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AllowPaging="true" AllowCustomPaging="true">
                <%--   <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Horizontal Value" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblRowValue" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDHORIZONTALVALUE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRowName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHORIZONTALNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlHorizontalValueEdit" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true">
                                       <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port Inner" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Center" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd Inner" Value="4"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd" Value="5"></telerik:RadComboBoxItem>
                                     <%--   <telerik:RadComboBoxItem Text="6" Value="6"></telerik:RadComboBoxItem>--%>
                                    </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlHorizontalValueAdd" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true" Width="100%">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port Inner" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Center" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd Inner" Value="4"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd" Value="5"></telerik:RadComboBoxItem>
                                        <%--<telerik:RadComboBoxItem Text="6" Value="6"></telerik:RadComboBoxItem>--%>
                                        </Items>
                                   </telerik:RadComboBox>
                                </FooterTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vertical Value" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblColumnValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERTICALVALUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlVerticalValueEdit" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" ></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="1" Value="1" ></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="2" Value="2" />
                                        <telerik:RadComboBoxItem Text="3" Value="3" />
                                        <telerik:RadComboBoxItem Text="4" Value="4" />
                                        <telerik:RadComboBoxItem Text="5" Value="5" />
                                        <telerik:RadComboBoxItem Text="6" Value="6" />
                                        <telerik:RadComboBoxItem Text="7" Value="7" />
                                        <telerik:RadComboBoxItem Text="8" Value="8" />
                                        <telerik:RadComboBoxItem Text="9" Value="9"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="10" Value="10" />
                                        <telerik:RadComboBoxItem Text="11" Value="11" />
                                        <telerik:RadComboBoxItem Text="12" Value="12" />
                                        <telerik:RadComboBoxItem Text="13" Value="13" />
                                        <telerik:RadComboBoxItem Text="14" Value="14" />
                                        <telerik:RadComboBoxItem Text="15" Value="15" />
                                        </Items>
                                   </telerik:RadComboBox>
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlVerticalValueAdd" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true" Width="100%">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Text="1" Value="1" />
                                        <telerik:RadComboBoxItem Text="2" Value="2" />
                                        <telerik:RadComboBoxItem Text="3" Value="3" />
                                        <telerik:RadComboBoxItem Text="4" Value="4" />
                                        <telerik:RadComboBoxItem Text="5" Value="5" />
                                        <telerik:RadComboBoxItem Text="6" Value="6" />
                                        <telerik:RadComboBoxItem Text="7" Value="7" />
                                        <telerik:RadComboBoxItem Text="8" Value="8" />
                                        <telerik:RadComboBoxItem Text="9" Value="9" />
                                        <telerik:RadComboBoxItem Text="10" Value="10" />
                                        <telerik:RadComboBoxItem Text="11" Value="11" />
                                        <telerik:RadComboBoxItem Text="12" Value="12" />
                                        <telerik:RadComboBoxItem Text="13" Value="13" />
                                        <telerik:RadComboBoxItem Text="14" Value="14" />
                                        <telerik:RadComboBoxItem Text="15" Value="15" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Short code" FooterText="New Short" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                          
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtShortNameEdit" runat="server" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' Width="50%" />
                               </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10"
                                       Width="100%" />
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Description" FooterText="New Dmr" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                           
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadLabel ID="lblLocationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtTaskNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Width="90%"></telerik:RadTextBox>
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadTextBox ID="txtTaskNameAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                       </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Methanol Tank Y/N"  Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblMethanolTankYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETHANOLTANKYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadCheckBox runat="server" ID="chkMethanolTankYNEdit" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDMETHANOLTANK").ToString().Equals("1") ? true : false %>' />
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadCheckBox runat="server" ID="chkMethanolTankYNAdd" />
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblTankLoc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTORSTBD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlTankLocEdit" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port" Value="PORT"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd" Value="STBD"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlTankLocAdd" CssClass="dropdown_mandatory"
                                        AppendDataBoundItems="true">
                                         <Items>
                                         <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Port" Value="PORT"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Stbd" Value="STBD"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="200px">
                            <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                          
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAMELIST") %>'></telerik:RadLabel>
                                 <%--<asp:LinkButton ID="ImgVesselList" runat="server" CommandName="view"   ToolTip="ViewVesselList">
                                 <span class="icon"><i class="fas fa-glasses"></i></span></asp:LinkButton>--%>
                                
                                <%--ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"--%>
                                 
                                </ItemTemplate>
                            <EditItemTemplate>
                                  
                                        <telerik:RadLabel ID="lblVesselEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselEditID"  Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                   
                                </EditItemTemplate>
                            <FooterTemplate >
                                  <%--ColumnWidthChanged --%>
                                        <asp:CheckBoxList ID="chkVesselListAdd" RepeatDirection="Horizontal" RepeatColumns="5"
                                            runat="server">
                                       </asp:CheckBoxList>
                                 
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Product Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblProductType" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadLabel ID="lblProductTypeShortname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPESHORTNAME") %>'></telerik:RadLabel>
                                    <telerik:RadRadioButtonList  ID="rblProductTypeEdit" runat="server" RepeatDirection="Horizontal">
                                        <Items>
                                        <telerik:ButtonListItem     Value="DB" Text="Dry Bulk"></telerik:ButtonListItem >
                                        <telerik:ButtonListItem  Value="WB" Text="Liquid Bulk"></telerik:ButtonListItem >
                                        <telerik:ButtonListItem  Value="MNL" Text="Methanol">  </telerik:ButtonListItem >
                                        </Items>
                                   </telerik:RadRadioButtonList>
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <telerik:RadRadioButtonList  ID="rblProductTypeAdd" runat="server" RepeatDirection="Horizontal">
                                       <Items>
                                        <telerik:ButtonListItem Value="DB" Text="Dry Bulk"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="WB" Text="Liquid Bulk"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="MNL" Text="Methanol"></telerik:ButtonListItem>
                                        </Items>
                                    </telerik:RadRadioButtonList>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort order" FooterText="Sort Order">
                          <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                          
                            <ItemTemplate>
                                    <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            <EditItemTemplate>
                                    <eluc:Number ID="txtSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                        CssClass="input_mandatory" />
                                </EditItemTemplate>
                            <FooterTemplate>
                                    <eluc:Number ID="txtSortOrderAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            
                            <ItemStyle Wrap="False" HorizontalAlign="Center" width="100px"></ItemStyle>
                            <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Delete" 
                                        CommandName="DELETE" ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" 
                                        CommandName="Add"  ID="cmdAdd"
                                        ToolTip="Add New">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
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
