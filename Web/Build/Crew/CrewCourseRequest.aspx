<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseRequest.aspx.cs"
    Inherits="CrewCourseRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Course Reqt</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseReq" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MRMenu" runat="server" OnTabStripCommand="MRMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvReq_NeedDataSource" OnItemDataBound="gvReq_ItemDataBound"
                OnItemCommand="gvReq_ItemCommand" EnableViewState="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <%--  <HeaderTemplate>
                                <asp:LinkButton ID="lblRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFNO"
                                    ForeColor="White">Ref Number</asp:LinkButton>
                                <img id="FLDREFNO" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDREFNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <%--  <HeaderTemplate>
                                <asp:LinkButton ID="lblFirstHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                    ForeColor="White">Name</asp:LinkButton>
                                <img id="FLDNAME" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployee" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Accom Req.">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDACCOMYN")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vsl Pln">
                            <%-- <HeaderTemplate>
                                <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                    ForeColor="White">Vsl Pln</asp:LinkButton>
                                <img id="FLDVESSELNAME" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vsl Joined">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDJOINEDVESSEL")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Courses to do">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCourse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE").ToString()%> '></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Created By : " + DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") +"<br/>Created Date :  " + DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available From">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available To">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel RefNo">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDTRAVELNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested By">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDBY")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
