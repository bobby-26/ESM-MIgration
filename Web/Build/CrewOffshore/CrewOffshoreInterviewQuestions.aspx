<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreInterviewQuestions.aspx.cs" Inherits="CrewOffshoreInterviewQuestions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
        <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOffshoreQuestions" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
                    <eluc:TabStrip ID="MenuOffshoreQuestions" runat="server" OnTabStripCommand="MenuOffshoreQuestions_TabStripCommand">
                    </eluc:TabStrip>
            
                    <telerik:RadGrid ID="gvOffshoreQuestions" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnItemCommand ="gvOffshoreQuestions_ItemCommand" 
                       ShowHeader="true" EnableViewState="false"  
                        OnItemDataBound="gvOffshoreQuestions_ItemDataBound"  AllowSorting="true" AllowPaging="true" AllowCustomPaging="true"   
                      OnSorting="gvOffshoreQuestions_Sorting" OnNeedDataSource="gvOffshoreQuestions_NeedDataSource" RenderMode="Lightweight"
                        GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" >

                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDQUESTIONID">
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                     
                        <Columns>
                            
                            <telerik:GridTemplateColumn HeaderText="Question"   AllowSorting="true" SortExpression="FLDQUESTION">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="60%" />

                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblQuestion" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <telerik:RadTextBox ID="txtQuestionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'
                                        CssClass="gridinput_mandatory"  Width="100%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtQuestionAdd" runat="server" CssClass="gridinput_mandatory"   Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="30%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAMELIST") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="ImgRankList" runat="server" >
                                    <span class="icon">
                                    <i class="fas fa-glasses"></i>
                                    </span>
                                    </asp:LinkButton>
                                    <eluc:Tooltip ID="ucRankList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAMELIST") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                    
                                    <div style="height: 200px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkRankListEdit" RepeatDirection="Vertical" 
                                            runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>                                    
                                    <div style="height: 70px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkRankListAdd" RepeatDirection="Vertical" 
                                            runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn> 


                            <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                    ID="cmdAdd">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                       </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>