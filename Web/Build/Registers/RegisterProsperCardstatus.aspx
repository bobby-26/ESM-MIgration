<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperCardstatus.aspx.cs" Inherits="Registers_RegisterProsperCardstatus" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Measure</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmprospercardstatus" runat="server">
        <div>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                  
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblprospermeasure" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCode" runat="server" Text="Cardstatus Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtcardstatuscode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblName" runat="server" Text="Cardstatus Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtcardstatusname" runat="server" MaxLength="100"
                                        CssClass="input" Width="240px">
                                    </telerik:RadTextBox>
                                </td>

                            </tr>
                        </table>
                    </div>

                    <eluc:TabStrip ID="CardstatusRegistersProsper" runat="server" OnTabStripCommand="RegistersProsper_TabStripCommand"></eluc:TabStrip>

                    <div id="divGrid" style="position: relative; z-index: 0">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvprospercardstatus" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvprospercardstatus_NeedDataSource"
                            OnItemCommand="gvprospercardstatus_ItemCommand" 
                            OnItemDataBound="gvprospercardstatus_ItemDataBound"
                            OnSortCommand="gvprospercardstatus_SortCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                          
                            AutoGenerateColumns="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" ShowFooter="false" TableLayout="Fixed" DataKeyNames="FLDCARDSTATUSID">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Code">
                                  
                                        <itemtemplate>
                                            <telerik:RadLabel ID="lblcardstatusid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARDSTATUSID") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkcardstatusCode" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARDSTATUSCODE") %>' CommandName="EDIT"></asp:LinkButton>

                                        </itemtemplate>
                                        <%--<edititemtemplate>
                                            <telerik:RadLabel ID="txtcardstatusCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARDSTATUSCODE") %>'
                                                MaxLength="10" Width="98%"></telerik:RadLabel>

                                        </edititemtemplate>--%>
                                        <footertemplate>

                                            <telerik:RadTextBox ID="txtcardstatusCodeAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                                MaxLength="10"></telerik:RadTextBox>

                                        </footertemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name">
                                      
                                        <itemtemplate>
                                            <telerik:RadLabel ID="lnkcardstatusName" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARDSTATUSNAME") %>' CommandName="EDIT"></telerik:RadLabel>

                                        </itemtemplate>
                                        <%--<edititemtemplate>

                                            <telerik:RadTextBox ID="txtcardstatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARDSTATUSNAME") %>'
                                                CssClass="gridinput_mandatory" MaxLength="200" Width="98%"></telerik:RadTextBox>
                                        </edititemtemplate>--%>
                                        <footertemplate>

                                            <telerik:RadTextBox ID="txtcardstatusNameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                                MaxLength="200"></telerik:RadTextBox>
                                        </footertemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                      
                                        <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                        <itemtemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" 
                                                CommandName="EDIT" ID="cmdEdit"
                                                ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete"
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                ToolTip="Delete">
                                                <span class="icon"><i class="fa fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </itemtemplate>
                                        <%--<edititemtemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save"
                                                CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                           
                                            <asp:LinkButton runat="server" AlternateText="Cancel" 
                                                CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </edititemtemplate>--%>
                                        <footerstyle horizontalalign="Center" />
                                        <footertemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" 
                                                CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </footertemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                </div>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>