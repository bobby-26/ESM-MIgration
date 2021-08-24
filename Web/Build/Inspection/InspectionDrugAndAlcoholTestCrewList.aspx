<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrugAndAlcoholTestCrewList.aspx.cs" Inherits="InspectionDrugAndAlcoholTestCrewList" %>

<!DOCTYPE html >

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvCrewList.ClientID %>"));
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="CrewTab" runat="server" OnTabStripCommand="CrewTab_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="search">
                <table cellpadding="1" cellspacing="1" width="75%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Testing Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" DatePicker="true" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAttachment" runat="server" Text="Attachment"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" AlternateText="Attachment" ID="imgAtt"
                                ToolTip="Attachment">                                
                                 <span class="icon"><i class="fas fa-paperclip"></i></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewList" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewList_NeedDataSource" OnItemDataBound="gvCrewList_ItemDataBound"
                OnItemCommand="gvCrewList_ItemCommand"
                OnSortCommand="gvCrewList_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDDRUGALCOHOLTESTCREWID">
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
                     <%--   <telerik:GridTemplateColumn HeaderStyle-Width="30px" Visible="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor" style="color:orange;" ><i class="fas fa-star"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />

                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblDrugAlcoholTestCrewId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDRUGALCOHOLTESTCREWID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblDrugAlcoholTestId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDRUGALCOHOLTESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblCrewId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblCrewName" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblsignonoffId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblRankId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONRANKID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblRank" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblNationality" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNATIONALITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadRadioButtonList ID="rblTest" runat="server" OnSelectedIndexChanged="rblTest_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Positive"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="Negative"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="3" Text="Not Tested" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtSampleTakenEdit" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSAMPLETAKEN") %>'
                                    Resize="None" Width="100%">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSampleTakenEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSAMPLETAKEN") %>' runat="server" Width="98%"
                                    CssClass="input_mandatory">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
         <%--   <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <span class="icon" style="color: orange;"><i class="fas fa-star-yellow"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTestConducted" runat="server" Text="* Test Conducted"></telerik:RadLabel>
                    </td>
                </tr>
            </table>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
