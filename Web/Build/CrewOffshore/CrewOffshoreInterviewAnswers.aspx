<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreInterviewAnswers.aspx.cs" Inherits="CrewOffshoreInterviewAnswers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

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
    <form id="frmOffshoreAnswers" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <table id="tblFilter" runat="server">
                        <tr>
                            <td>
                                <b><telerik:RadLabel ID="lblQuestionFilter" runat="server" Text="Question"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlQuestion" AutoPostBack="true" runat="server" AppendDataBoundItems="true"  Width="300px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"  ></telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>                    
               
                
                    <eluc:TabStrip ID="MenuOffshoreAnswers" runat="server" OnTabStripCommand="MenuOffshoreAnswers_TabStripCommand">
                    </eluc:TabStrip>
               
                    <telerik:RadGrid ID="gvOffshoreAnswers" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnItemCommand ="gvOffshoreAnswers_ItemCommand" 
                       
                        ShowHeader="true" EnableViewState="false" 
                        OnItemDataBound="gvOffshoreAnswers_ItemDataBound" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true"   
                      OnSorting="gvOffshoreAnswers_Sorting" OnNeedDataSource="gvOffshoreAnswers_NeedDataSource" RenderMode="Lightweight"
                        GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" >

                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames ="FLDANSWERID" >
                    <NoRecordsTemplate>
                        
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true"  >
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                        <Columns>
                           
                            <telerik:GridTemplateColumn HeaderText="Question"    AllowSorting="true" SortExpression="FLDQUESTION">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderStyle Wrap="true"    HorizontalAlign="Left"  Width="35%"  />

                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblQuestion" runat="server"  Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <telerik:RadComboBox ID="ddlQuestionEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"   Width="100%"   ></telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlQuestionAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"   Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"  Width="100%"   ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Answer">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderStyle Wrap="true"    HorizontalAlign="Left" Width="35%"   />

                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblAnswer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <telerik:RadTextBox ID="txtAnswerEdit" runat="server"  CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'  Width="100%"  ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtAnswerAdd" runat="server" CssClass="input_mandatory"  Width="100%"  ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowSorting="true" SortExpression="FLDSCORE"   HeaderText="Score">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderStyle Wrap="true"    HorizontalAlign="Left"  Width="20%"  />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"    Width="100%"  
                                        MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucScoreAdd"   Width="100%"  runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                        MaxLength="3" />
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