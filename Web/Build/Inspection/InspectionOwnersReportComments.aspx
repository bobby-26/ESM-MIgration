<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOwnersReportComments.aspx.cs" Inherits="InspectionOwnersReportComments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizediv() {
                var tbl = document.getElementById("tblComments");
                if (tbl != null) {
                    for (var i = 0; i < tbl.rows.length; i++) {
                        tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuRAComments" runat="server" OnTabStripCommand="MenuRAComments_TabStripCommand"></eluc:TabStrip>
            <table width="99%">
                <tr>
                    <td align="left" colspan="4">
                        <font color="blue" size="0">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Post your comments here." ForeColor="Blue"></telerik:RadLabel>
                    </font>
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblComment" runat="server" Text="Comment"></telerik:RadLabel>
                    </td>
                    <td align="left" style="vertical-align: top;">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                            Height="49px" TextMode="MultiLine" Width="600px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <%--<table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%" id="tblComments">
            <asp:Repeater ID="repDiscussion" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblPosted" runat="server" Text="Posted By"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblPostedname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                        <td class="input" width="60%">
                            <telerik:RadLabel ID="lblcomment" runat="server" Text="Comment - "></telerik:RadLabel>
                            <br />
                            <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                <telerik:RadLabel ID="lblcommentname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMENTS")%>' Font-Bold="true"></telerik:RadLabel>
                            </div>
                        </td>
                        <td width="12%">
                            <telerik:RadLabel ID="lblpostedby" runat="server" Text="Name"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblpostedbyname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblpostedDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>--%>
            <telerik:RadGrid ID="gvComments" runat="server" AutoGenerateColumns="False" Width="100%" OnItemCommand="gvComments_ItemCommand"
                AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                OnNeedDataSource="gvComments_NeedDataSource" OnItemDataBound="gvComments_ItemDataBound" ShowHeader="false">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="false" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td width="10%">
                                    <telerik:RadLabel ID="lblPosted" runat="server" Text="Posted By"></telerik:RadLabel>
                                    <br />
                                    <telerik:RadLabel ID="lblPostedname" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                </td>
                                <td class="input" width="60%">
                                    <telerik:RadLabel ID="lblcomment" runat="server" Text="Comment - "></telerik:RadLabel>
                                    <br />
                                    <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                        <telerik:RadLabel ID="lblcommentname" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                    </div>
                                </td>
                                <td width="15%">
                                    <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                                    <br />
                                    <telerik:RadLabel ID="lblpostedDate" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Posted By" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPosted" runat="server" Text="Posted By"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblPostedname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>' Font-Bold="true"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comment -" HeaderStyle-Width="65%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="commentsid" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOWNERSREPORTCOMMENTID")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcomment" runat="server" Text="Comment - "></telerik:RadLabel>
                                <br />
                                <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                    <telerik:RadLabel ID="lblcommentname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMENTS")%>' Font-Bold="true"></telerik:RadLabel>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="commentsEditid" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOWNERSREPORTCOMMENTID")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMENTS")%>'
                                    Height="49px" TextMode="MultiLine" Width="600px" Resize="Both">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Posted By" HeaderStyle-Width="13%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblpostedDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>' Font-Bold="true"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Resizing AllowColumnResize="true" />
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
