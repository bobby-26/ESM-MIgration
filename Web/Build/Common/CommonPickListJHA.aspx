<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListJHA.aspx.cs" Inherits="CommonPickListJHA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazards</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersPortAgent" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
         <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
      
            <asp:Button ID="cmdHiddenSubmit" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

          

                <eluc:TabStrip ID="MenuPortAgent" runat="server" Title="Job Hazards" OnTabStripCommand="MenuPortAgent_TabStripCommand"></eluc:TabStrip>

               
                    <table id="tblConfigurePortAgent" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblKeyword" runat="server" Text="Keyword"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtJob" runat="server"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true"
                                    DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" EmptyMessage="Type to select category" Filter="Contains" MarkFirstMatch="true">
                                    <%-- <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>--%>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <eluc:VesselByCompany ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                    Width="200px" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
             

               
                    <%-- <asp:GridView ID="gvPortAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPortAgent_RowCommand" OnRowDataBound="gvPortAgent_RowDataBound"
                        OnRowEditing="gvPortAgent_RowEditing" ShowHeader="true" AllowSorting="true" OnSorting="gvPortAgent_Sorting">

                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPortAgent" runat="server" Height="85%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvPortAgent_NeedDataSource"
                        OnItemCommand="gvPortAgent_ItemCommand"
                        OnItemDataBound="gvPortAgent_ItemDataBound"
                        OnSortCommand="gvPortAgent_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                      
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                                
                                <telerik:GridTemplateColumn HeaderText="Hazard Number">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblJobHazardid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID")  %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRefNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER")  %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="Select" CommandArgument="<%# Container.DataSetIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                  
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")  %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY")  %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Job">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB")  %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
             

         
      
    </form>
</body>
</html>
