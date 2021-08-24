<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreGMInterviewAnswers.aspx.cs" Inherits="CrewOffshore_CrewOffshoreGMInterviewAnswers" %>
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
    
<telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock></head>
<body>
    <form id="frmOffshoreAnswers" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server" >
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               <table id="tblFilter" runat="server" >
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblQuestionFilter" runat="server" Text="Question" Font-Bold="true" ></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlQuestion" AutoPostBack="true"  Width="300px" runat="server" OnTextChanged="ddlQuestion_SelectedValueChanged" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"  ></telerik:RadComboBox>
                                
                            </td>
                        </tr>
                    </table>                    
                
               
                
                    <eluc:TabStrip ID="MenuOffshoreAnswers" runat="server" OnTabStripCommand="MenuOffshoreAnswers_TabStripCommand">
                    </eluc:TabStrip>

                    <telerik:RadGrid ID="gvOffshoreAnswers" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnItemCommand ="gvOffshoreAnswers_ItemCommand" 
                       ShowHeader="true" EnableViewState="false" OnItemDataBound="gvOffshoreAnswers_ItemDataBound"  AllowPaging="true"   
                       AllowCustomPaging="true" OnNeedDataSource="gvOffshoreAnswers_NeedDataSource" RenderMode="Lightweight"
                        GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSorting="gvOffshoreAnswers_Sorting" AllowSorting="true">
                     
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames ="FLDANSWERID" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                        <Columns>
                            
                            <telerik:GridTemplateColumn HeaderText="Question"  >
                                <ItemStyle Wrap="true" HorizontalAlign="Left" ></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left"/>
                                <HeaderStyle Wrap="true" Width="50%"    HorizontalAlign="Left" />
                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblQuestion" runat="server" Width="100%" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'  ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <telerik:RadComboBox ID="ddlQuestionEdit" Width="100%" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"  ></telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlQuestionAdd" Width="100%" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category" ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Answer" >
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle    Wrap="true" HorizontalAlign="Left" />
                                <HeaderStyle    Wrap="true" HorizontalAlign="Left" Width="25%" />
                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblAnswer" runat="server" Width="100%" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'  ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <telerik:RadTextBox ID="txtAnswerEdit" runat="server" Width="100%" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>' ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtAnswerAdd" runat="server" Width="100%" CssClass="input_mandatory"    ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Score"  AllowSorting="true" SortExpression="FLDSCORE" >
                           <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="10%" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                               <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'   ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucScoreEdit" Width="100%" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                        MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucScoreAdd" Width="100%" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                        MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
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