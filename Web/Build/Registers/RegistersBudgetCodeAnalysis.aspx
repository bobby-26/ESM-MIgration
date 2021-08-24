<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudgetCodeAnalysis.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="RegistersBudgetCodeAnalysis" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gears Set</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBudgetAnalysis" runat="server">
  
           <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlWorkingGearSet" Height="100%">
         
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                   <br />
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblTypeName" runat="server" Text="Type"></asp:Literal>

                                <eluc:Quick ID="ddlType" runat="server" CssClass="input_mandatory" QuickTypeCode="157" AutoPostBack="true" Width="240px" OnTextChangedEvent="BindData" AppendDataBoundItems="true"></eluc:Quick>
                            </td>
                            </tr>
                    </table>
                    <br />
                   
                        <eluc:TabStrip ID="MenuBudgetCodeAnalaysis" runat="server" OnTabStripCommand="MenuBudgetCodeAnalaysis_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetCodeAnalaysis" runat="server" AutoGenerateColumns="False" Font-Size="11px" Height="83%"
                            OnUpdateCommand="gvBudgetCodeAnalaysis_OnUpdateCommand" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            Width="100%" CellPadding="3" OnItemCommand="gvBudgetCodeAnalaysis_OnItemCommand" OnItemDataBound="gvBudgetCodeAnalaysis_ItemDataBound"
                             OnDeleteCommand="gvBudgetCodeAnalaysis_OnDeleteCommand"    OnSortCommand="gvBudgetCodeAnalaysis_OnSortCommand" OnNeedDataSource="gvBudgetCodeAnalaysis_OnNeedDataSource"
                            ShowFooter="true" ShowHeader="true"  EnableViewState="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="" >
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                            <FooterStyle CssClass="datagrid_footerstyle" Width="100%"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                       
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="115px">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnBudgetEdit">
                                            <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Width="60px" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="180px" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                                       <asp:LinkButton ID="ImgOwnerBudgetEdit" runat="server" OnClientClick="return showPickList('spnBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx',true); " >
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtBudgetIdEdit" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>' runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnBudget">
                                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                                        <asp:LinkButton ID="ImgOwnerID" runat="server" OnClientClick="return showPickList('spnBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx',true); " >
                                                      <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="90px">
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblownerheader" runat="server">Owner</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListOwnerEdit">
                                            <telerik:RadTextBox ID="txtOwnerCodeEdit" runat="server" CssClass="gridinput_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCODE") %>'
                                                Width="60px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtOwnerNameEdit" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>' runat="server" CssClass="gridinput_mandatory"
                                                Width="120px"></telerik:RadTextBox>
                                          <asp:LinkButton ID="ImgOwnerEdit" runat="server" OnClientClick="return showPickList('spnPickListOwnerEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx',true); " >
                                                      <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtOwnerIdEdit" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>' runat="server" CssClass="input hidden"></telerik:RadTextBox>

                                        </span>&nbsp;
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListOwner">
                                            <telerik:RadTextBox ID="txtOwnerCode" runat="server" CssClass="gridinput_mandatory"
                                                Width="60px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtOwnerName" runat="server" CssClass="gridinput_mandatory"
                                                Width="120px"></telerik:RadTextBox>
                                           <asp:LinkButton ID="ImgOwner" runat="server" OnClientClick="return showPickList('spnPickListOwner', 'codehelp1', '', '../Common/CommonPickListAddress.aspx',true); " >
                                                      <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtOwnerId" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>&nbsp;
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn >
                                <telerik:GridTemplateColumn HeaderText="Variant Budget Code" HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container,"DataItem.FLDVARIANTSUBACCOUNT") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>

                                        <span id="spnVariantBudgetEdit">
                                            <telerik:RadTextBox ID="txtVariantBudgetCodeEdit" runat="server" Width="60px" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVARIANTSUBACCOUNT") %>'></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVariantBudgetNameEdit" runat="server" Width="180px" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVARIANTDESCRIPTION") %>'></telerik:RadTextBox>
                                              <asp:LinkButton ID="ImgVariantBudgetEdit" runat="server" OnClientClick="return showPickList('spnVariantBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx',true); " >
                                                      <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtVariantBudgetIdEdit" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVARIANTBUDGETID") %>' runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVariantBudgetgroupIdEdit" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnVariantBudget">
                                            <telerik:RadTextBox ID="txtVariantBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVariantBudgetName" runat="server" Width="160px" CssClass="input_mandatory"></telerik:RadTextBox>
                                       <asp:LinkButton ID="ImgVariantID" runat="server" OnClientClick="return showPickList('spnVariantBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx',true); " >
                                                      <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtVariantBudgetId" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVariantBudgetgroupId" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Criteria" HeaderStyle-Width="70px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="height: 100px; width: 150px; overflow: auto;" class="input">
                                            <telerik:RadCheckBoxList ID="chkCriteriaEdit" Visible="true" RepeatDirection="Vertical" Enabled="true" AppendDataBoundItems="true"
                                                runat="server">
                                            </telerik:RadCheckBoxList>
<%--                                        </div>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <div style="height: 100px; width: 150px; overflow: auto;" class="input">
                                            <telerik:RadCheckBoxList ID="chkCriteriaAdd" RepeatDirection="Vertical" Enabled="true"
                                                runat="server">
                                            </telerik:RadCheckBoxList>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCriteriaHeader" runat="server" Text="Criteria"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDCAPTAINCASHCOMPONENT") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadDropDownList ID="ddlCaptaincashEdit" runat="server" CssClass="gridinput_mandatory"></telerik:RadDropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadDropDownList ID="ddlCaptaincashAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadDropDownList>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="20px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                                        <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Add"  CommandName="Add"  ID="cmdAdd"
                                        ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                            </Columns>
                       <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                             <clientsettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                   </clientsettings>                                                
                  </telerik:RadGrid>                     
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
