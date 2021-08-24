<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTravelClaimapprovalPIC.aspx.cs"
    Inherits="RegistersTravelClaimapprovalPIC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" cssclass="hidden"/>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

       

            <eluc:TabStrip ID="MenuOfficestaff" runat="server" OnTabStripCommand="MenuOfficestaff_TabStripCommand"
                TabStrip="True"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>Employee No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNo" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>
                    <td>User Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtUsername" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Salutation</td>
                    <td>
                        <telerik:RadTextBox ID="txtSalutation" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td>First Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtfirstname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Middle Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtmiddlename" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>last Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtLastname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>Type</td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="98%" ID="ddlAddType" AutoPostBack="true" EnableLoadOnDemand="true">
                        </telerik:RadComboBox>
                        <%--   <asp:DropDownList ID="ddlAddType" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="98%"
                                        CssClass="dropdown_mandatory">
                                    </asp:DropDownList></td>--%>
                    </td>
                </tr>
            </table>

            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvCrewComponentTrack" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewComponentTrack" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewComponentTrack_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Height="82%" Width="100%" GroupingEnabled="false" 
                OnItemDataBound="gvCrewComponentTrack_ItemDataBound" OnItemCommand="gvCrewComponentTrack_ItemCommand"
                ShowFooter="true" ShowHeader="true" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCONFIGURATIONID">

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Approver">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblconfigurationid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONID") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblapproverstaffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOfficeStaffedit">
                                 

                                    <telerik:RadTextBox ID="txtEditStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        Width="80%" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtEditStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                        Width="5px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="imgEdituser" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ToolTip="Approver" />
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOfficeStaff">
                                    <telerik:RadTextBox ID="txtAddStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        Width="80%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAddStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                        Width="5px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="imgadduser" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ToolTip="Approver" />
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                           <telerik:RadLabel ID="lblconfigurationidedit"  Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONID") %>'></telerik:RadLabel>

                                <telerik:RadComboBox RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="300px" ID="ddleditlevel" AutoPostBack="true" EnableLoadOnDemand="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                        <telerik:RadComboBoxItem Text="1" Value="1" />
                                        <telerik:RadComboBoxItem Text="2" Value="2" />
                                        <telerik:RadComboBoxItem Text="3" Value="3" />
                                        <telerik:RadComboBoxItem Text="4" Value="4" />
                                    </Items>

                                </telerik:RadComboBox>
                              <%--  <asp:DropDownList ID="ddleditlevel" runat="server" CssClass="input_mandatory">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                </asp:DropDownList>--%>
                            </EditItemTemplate>
                            <FooterTemplate>

                                  <telerik:RadComboBox RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="300px" ID="ddlAddLevel" AutoPostBack="true" EnableLoadOnDemand="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                        <telerik:RadComboBoxItem Text="1" Value="1" />
                                        <telerik:RadComboBoxItem Text="2" Value="2" />
                                        <telerik:RadComboBoxItem Text="3" Value="3" />
                                        <telerik:RadComboBoxItem Text="4" Value="4" />
                                    </Items>

                                </telerik:RadComboBox>

                               <%-- <asp:DropDownList ID="ddlAddLevel" runat="server" CssClass="input_mandatory">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>

                                </asp:DropDownList>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                            <itemstyle wrap="False" horizontalalign="Center" ></itemstyle>
                            <itemtemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </itemtemplate>
                            <edititemtemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </edititemtemplate>
                            <footerstyle horizontalalign="Center" />
                            <footertemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </footertemplate>
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
<%--                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvCrewComponentTrack" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowCommand="gvCrewComponentTrack_RowCommand"
                            OnRowDataBound="gvCrewComponentTrack_RowDataBound" OnRowCancelingEdit="gvCrewComponentTrack_RowCancelingEdit"
                            OnRowUpdating="gvCrewComponentTrack_RowUpdating" OnRowEditing="gvCrewComponentTrack_RowEditing"
                            OnRowDeleting="gvCrewComponentTrack_RowDeleting"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDCONFIGURATIONID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Approver">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblapproverstaffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'></asp:Label>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListOfficeStaffedit">
                                            <asp:TextBox ID="txtEditStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                                Width="80%" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>'></asp:TextBox>
                                            <asp:TextBox ID="txtEditStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                                Width="5px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="imgEdituser" Style="cursor: pointer; vertical-align: top"
                                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" 
                                                ToolTip="Approver" />
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListOfficeStaff">
                                            <asp:TextBox ID="txtAddStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                                Width="80%"></asp:TextBox>
                                            <asp:TextBox ID="txtAddStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                                Width="5px"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="imgadduser" Style="cursor: pointer; vertical-align: top"
                                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" 
                                                ToolTip="Approver" />
                                        </span>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Level">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddleditlevel" runat="server" CssClass="input_mandatory">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>                                        
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlAddLevel" runat="server" CssClass="input_mandatory">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                           
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>--%>
