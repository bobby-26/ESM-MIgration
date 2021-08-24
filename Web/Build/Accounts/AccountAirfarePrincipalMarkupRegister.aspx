<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountAirfarePrincipalMarkupRegister.aspx.cs" Inherits="Accounts_AccountAirfarePrincipalMarkupRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AIRFARE PRINCIPAL REGISTER</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAirfarePrincipalRegister" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
       <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <%--  <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">--%>
                      
                        <eluc:TabStrip ID="MenuAirfarePrincipalMain" runat="server" OnTabStripCommand="MenuAirfarePrincipalMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                 
               
                                        <eluc:TabStrip ID="MenuAirfarePrincipal" runat="server" OnTabStripCommand="MenuAirfarePrincipal_TabStripCommand">
                                        </eluc:TabStrip>
                                   

                    <table style="width:50%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipalId" runat="server" Visible="false"></telerik:RadLabel>
                                <telerik:RadTextBox runat="server" ID="txtPrincipal" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="75%"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>

             <table style="width: 100%;border:1px">
                        <tr>
                            <td style="vertical-align:top; width:50%">
                              
                                    <table id="tblAirfarePrincipal" width="50%">
                                        
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblDefault" runat="server" Text="Using Airfare Default"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDefault" runat="server" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblMaxPrice" runat="server" Text="Max Markup Price:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                 <eluc:Number ID="txtMaxPrice" runat="server"  Width="120px"></eluc:Number>
                                               <%-- <asp:TextBox runat="server" ID="txtMaxPrice" CssClass="input_mandatory" Style="text-align: right;"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskMaxPrice" runat="server" TargetControlID="txtMaxPrice"
                                                    Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                                </ajaxToolkit:MaskedEditExtender>--%>
                                            </td>
                                        </tr>
                                        
                                    </table>
                              
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvAirfare" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAirfare" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvAirfare_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvAirfare_SelectedIndexChanging"
                    OnItemDataBound="gvAirfare_ItemDataBound" OnItemCommand="gvAirfare_ItemCommand"
                    ShowFooter="true" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >     
                        <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Price Range(USD)" Name="Price" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                       
                         </ColumnGroups>      
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="Price">
                                                  <HeaderStyle  HorizontalAlign="Right"/>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                               
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblMarkupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPRANGEID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFromAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMAMOUNT","{0:###,###,###,##0.00}") %>'
                                                        ></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadLabel ID="lblMarkupIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPRANGEID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFromAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblFromAmountAdd" runat="server"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="Price">
                                                  <HeaderStyle  HorizontalAlign="Right"/>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                               
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblToAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                     <eluc:Number ID="txtToAmountEdit" runat="server"  Width="120px"></eluc:Number>
                                                  <%--  <asp:TextBox ID="txtToAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOAMOUNT","{0:###,###,###,##0.00}") %>'
                                                        CssClass="gridinput_mandatory" MaxLength="26" Width="100px"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskToAmountEdit" runat="server" TargetControlID="txtToAmountEdit"
                                                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                                    </ajaxToolkit:MaskedEditExtender>--%>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <eluc:Number ID="txtToAmountAdd" runat="server"  Width="120px"></eluc:Number>
                                                   <%-- <asp:TextBox ID="txtToAmountAdd" runat="server" CssClass="gridinput_mandatory" Width="100px"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskToAmountAdd" runat="server" TargetControlID="txtToAmountAdd"
                                                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                                    </ajaxToolkit:MaskedEditExtender>--%>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText=" Markup %" ColumnGroupName="Price">
                                                  <HeaderStyle  HorizontalAlign="Right"/>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                               
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblMarkupAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAMOUNT","{0:#0.00}") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                     <eluc:Number ID="txtMarkupAmountEdit" runat="server"  Width="120px"></eluc:Number>
                                                   <%-- <asp:TextBox ID="txtMarkupAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAMOUNT","{0:#0.00}") %>'
                                                        CssClass="gridinput_mandatory" MaxLength="26" Width="50px"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskMarkupAmountEdit" runat="server" TargetControlID="txtMarkupAmountEdit"
                                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                                    </ajaxToolkit:MaskedEditExtender>--%>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>

                                                     <eluc:Number ID="txtMarkupAmountAdd" runat="server"  Width="120px"></eluc:Number>
                                                  <%--  <asp:TextBox ID="txtMarkupAmountAdd" runat="server" CssClass="gridinput_mandatory"
                                                        Width="50px"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskMarkupAmountAdd" runat="server" TargetControlID="txtMarkupAmountAdd"
                                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                                    </ajaxToolkit:MaskedEditExtender>--%>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Action">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                               
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/move_items.png %>"
                                                        CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                                        ToolTip="Group"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                        ToolTip="Add New"></asp:ImageButton>
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
                <eluc:Status runat="server" ID="ucStatus" />
                 
                                     
                          </td>
                            <td style="vertical-align:top;">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblBillToCompanySetting" runat="server" Text="Bill To Company Setting"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ucBillToCompanySetting" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" CssClass="input_mandatory" Width="150px" AutoPostBack="true" EnableLoadOnDemand="true">
                     
                                           </telerik:RadComboBox>
                                           <%-- <asp:DropDownList id="ucBillToCompanySetting" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" CssClass="input_mandatory" Width="150px" >
                                            </asp:DropDownList>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text="Bill to Company:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                                CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                     
             
             
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVessel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvVessel_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvVessel_SelectedIndexChanging"
                    OnItemDataBound="gvVessel_ItemDataBound" OnItemCommand="gvVessel_ItemCommand"
                    ShowFooter="true" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >                                         
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                               
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>' Visible="false"></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Bill To">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                              
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblBillTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:UserControlCompany ID="ucBillTo" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                                        CssClass="input_mandatory" runat="server" Width="150px" AppendDataBoundItems="true" SelectedCompany='<%# DataBinder.Eval(Container,"DataItem.FLDBILLTOCOMPANY") %>'/>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Action">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                               
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                  
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
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
                    
                     </td>
                        </tr>
                    </table>

                    
           
                    
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>

                         
