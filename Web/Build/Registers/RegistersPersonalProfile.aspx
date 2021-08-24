<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPersonalProfile.aspx.cs"
    Inherits="RegistersPersonalProfile" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Personal Profile</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmprofile" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Personal Profile"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>          
            <table id="tblpersonalprofile" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlpersonalprofile" runat="server" HardTypeCode="91"
                            HardList='<%# PhoenixRegistersHard.ListHard(1,91) %>' AppendDataBoundItems="true"
                            Width="300px" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedTo" runat="server" Text="Applied To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlrank" runat="server" HardTypeCode="90" ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                            AppendDataBoundItems="true" Width="300px" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuPersonalProfile" runat="server" OnTabStripCommand="MenuPersonalProfile_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="GvPersonalProfile" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="GvPersonalProfile_ItemCommand" OnNeedDataSource="GvPersonalProfile_NeedDataSource" Height="99%"
                OnItemDataBound="GvPersonalProfile_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkcategory" runat="server" CommandName="Sort" CommandArgument="FLDPROFILECATEGORY">
                                                Category&nbsp;</asp:LinkButton>
                                <%--<img id="FLDPROFILECATEGORY" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILECATEGORY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblquestionid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlpersonalprofileedit" runat="server" CssClass="dropdown_mandatory"
                                    HardTypeCode="91" HardList='<%# PhoenixRegistersHard.ListHard(1, 91) %>' SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILECATEGORYID") %>'
                                    AppendDataBoundItems="true" />
                                <telerik:RadLabel ID="lblquestionidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlpersonalprofileinsert" runat="server" CssClass="dropdown_mandatory"
                                    HardTypeCode="91" HardList='<%# PhoenixRegistersHard.ListHard(1, 91) %>' AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Applied To">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkrank" runat="server" CommandName="Sort" CommandArgument="FLDPROFILERANKCLASSIFICATION">
                                                Applied To&nbsp;</asp:LinkButton>
                                <%--<img id="FLDPROFILERANKCLASSIFICATION" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILERANKCLASSIFICATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlrankedit" runat="server" CssClass="dropdown_mandatory" HardTypeCode="90"
                                    ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                    AppendDataBoundItems="true" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlrankinsert" runat="server" CssClass="dropdown_mandatory" HardTypeCode="90"
                                    ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                    AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank List" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Registers/RegistersToolTipAppraisalRankList.aspx?rankcategorylist=" + ","+DataBinder.Eval(Container,"DataItem.FLDRANKID").ToString()+"," %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Category" HeaderStyle-Width="155px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblheader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYDESC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucQuickEdit" runat="server" QuickList='<%# PhoenixRegistersQuick.ListQuick(1,161)%>'
                                    QuickTypeCode='161' SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>' AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucQuick" runat="server" AppendDataBoundItems="true" QuickList='<%# PhoenixRegistersQuick.ListQuick(1,161)%>'
                                    QuickTypeCode='161' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Evaluation Item" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                Evaluation Item
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtevaluationitemedit" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTION").ToString() %>'
                                    CssClass="gridinput_mandatory" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtevaluationiteminsert" Width="98%" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Sequence" HeaderStyle-Width="105px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkordersequence" runat="server" CommandName="Sort" CommandArgument="FLDORDERSEQUENCE">
                                                Order Sequence&nbsp;</asp:LinkButton>
                                <%--    <img id="FLDORDERSEQUENCE" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblordersequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="ucordersequenceEdit" CssClass="gridinput_mandatory" Width="80px"
                                    MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="ucordersequenceInsert" CssClass="gridinput_mandatory"
                                    MaxLength="2" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Score" HeaderStyle-Width="75px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkmaxscore" runat="server" CommandName="Sort" CommandArgument="FLDMAXMARK">
                                                Max Score&nbsp;</asp:LinkButton>
                                <%--<img id="FLDMAXMARK" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmaxscore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="ucmaxscoreEdit" CssClass="gridinput_mandatory" Width="60px"
                                    MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARK") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="ucmaxscoreInsert" CssClass="gridinput_mandatory" Width="60px"
                                    MaxLength="2" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblactiveHeader" runat="server" CommandName="Sort" CommandArgument="FLDSHORTCODE">
                                    Active Y/N&nbsp;
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblactive" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString() == "1" ? "Yes" : "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkactive" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")).ToString().Equals("1")?true:false %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkactiveadd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Map Vessel Type" ToolTip="Map Vessel Type" Width="20PX" Height="20PX"
                                    CommandName="VTMAP" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdmap">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Map Rank" ToolTip="Map Rank" Width="20PX" Height="20PX"
                                    CommandName="RMAP" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRankMap">
                                <span class="icon"><i class="fas fa-address-card"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Appraisal Remarks Mapping" ToolTip="Appraisal Remarks Mapping" Width="20PX" Height="20PX"
                                    CommandName="MAPPING" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdMapping">
                                <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Occasion Mapping" ToolTip="Occasion Mapping" Width="20PX" Height="20PX"
                                    CommandName="MAPPINGOCCASION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdoccasionmapping">
                                <span class="icon"><i class="fas fa-bars"></i></span>
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
