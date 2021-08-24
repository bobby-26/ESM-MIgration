<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTestAnswers.aspx.cs"
    Inherits="Registers_RegistersTestAnswers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                    <eluc:TabStrip ID="MenuTest" runat="server" OnTabStripCommand="MenuTest_TabStripCommand">
                    </eluc:TabStrip>
            
                <table>
                    <tr>
                    <td style="width:50px"></td>
                        <td>
                            <telerik:RadLabel ID="lblQuestion" runat="server" Text="Question"></telerik:RadLabel>
                        </td>
                         <td style="width:30px"></td>
                        <td>
                            <telerik:RadTextBox ID="txtQuestion" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="600px" Wrap="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
               
                    <eluc:TabStrip ID="MenuOffshoreAnswers" runat="server" OnTabStripCommand="MenuOffshoreAnswers_TabStripCommand">
                    </eluc:TabStrip>
                
                
                    <telerik:RadGrid ID="gvTestAnswers" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnItemCommand="gvTestAnswers_ItemCommand"
                        
                        ShowHeader="true" EnableViewState="false"  OnItemDataBound="gvTestAnswers_ItemDataBound" 
                       AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvTestAnswers_NeedDataSource" RenderMode="Lightweight" GroupingEnabled="false" EnableHeaderContextMenu="true">
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  DataKeyNames="FLDANSWERID" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Answer">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="50%"/>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"    />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAnswer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtAnswerEdit" runat="server" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'   Width="100%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtAnswerAdd" Width="100%" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sort Order">
                               <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="10%"/>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"    />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucSortEdit" Width="100%"  runat="server" CssClass="gridinput_mandatory"
                                        IsInteger="true" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSortAdd"  Width="100%" runat="server" CssClass="gridinput_mandatory"
                                        IsInteger="true" MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="CorrectAnswerYN">
                               
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="10%"/>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"    />
                               

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCorrectAnswerYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTANSWER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="chkCorrectAnswerYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCORRECTANSWERYN").ToString().Equals("1"))?true:false %>'>
                                    </telerik:RadCheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadCheckBox ID="chkCorrectAnswerYNAdd" runat="server" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                         
                              <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
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