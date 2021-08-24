<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationVesselSummary.aspx.cs" Inherits="Inspection_InspectionRegulationVesselSummary" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Regulation Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
   <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        
        <%-- For Popup Relaod --%>
         <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />  

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <eluc:TabStrip ID="MenuDivRegulation" runat="server" OnTabStripCommand="plan_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvRegulation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None"
            OnNeedDataSource="gvRegulation_NeedDataSource"
            OnItemDataBound="gvRegulation_ItemDataBound"
            OnItemCommand="gvRegulation_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Regulation (Title)" AllowSorting="true">
                        <HeaderStyle Width="125px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRegulationID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblTitle" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRegulationDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDTKEY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true">
                        <HeaderStyle Width="118px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDescription" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="IssuedDate" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDISSUEDDATE">
                        <HeaderStyle Width="118px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Issued By" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDISSUEDBYNAME">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIssuedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ref Doc" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDISSUEDBYNAME">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%--<telerik:RadLabel ID="RadLabel1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBYNAME") %>'></telerik:RadLabel>--%>
                            <asp:LinkButton runat="server" AlternateText="attachment"
                                CommandName="ATTACHMENT" ID="lnkRegulationAttachment"
                                ToolTip="Reference Doc Attachment" Width="20PX" Height="20PX">
                                <span class="icon"> <i class="fa fa-paperclip"></i></span>
                            </asp:LinkButton>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action Required" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDACTIONREQUIRED">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActionRequired" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'  Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due Date" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDUEDATE">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblduedate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDUEDATE").ToString() =="" ? "N/A" :  DataBinder.Eval(Container,"DataItem.FLDDUEDATE","{0:dd-MM-yyyy}") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Alternate Survey" Visible="true"  AllowSorting="true" ShowSortIcon="true" SortExpression="FLDALTERNATESURVEY">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAlternateSurvey" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'  Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date Closed" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCLOSEDDATE">
                        <HeaderStyle Width="140px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDateClosed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="SUMMARYEDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                      <%--      <asp:LinkButton runat="server" AlternateText="attachment"
                                CommandName="ATTACHMENT" CommandArgument="<%# Container.DataSetIndex %>" ID="btnAttachment"
                                ToolTip="Evidence Attachment" Width="20PX" Height="20PX">
                                <span class="icon"> <i class="fa fa-paperclip"></i></span>
                            </asp:LinkButton>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                     <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
