<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTestQuestions.aspx.cs"
    Inherits="Registers_RegistersTestQuestions" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Test Questions</title>  <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentTestQuestions" runat="server" submitdisabledcontrols="true">
   <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
                                  
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            
                       <%--<div class="navSelect" style="top: 0px; right: 0px; position:absolute ">--%>
            <eluc:TabStrip ID="MenuTest" runat="server" OnTabStripCommand="MenuTest_TabStripCommand"    />
        
                    
            
                    <%--<table id="tblFilter" runat="server" visible="false" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCourseFilter" runat="server" Text="Course"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlCourse" AutoPostBack="true" runat="server" CssClass="input" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"></telerik:RadComboBox>    
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblExanFilter" runat="server" Text="Exam"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlExam" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblQuestionName" runat="server" Text="Question"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtQuestion" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>   --%>
                    <table id="tblExam" runat="server" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text="Course"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCourse" runat="server" CssClass="input" Enabled="false" Width="70%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTestName" runat="server" Text="Test Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTestName" runat="server" CssClass="input_mandatory" Width="70%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMaxQuestions" runat="server" Text="Max No. of Questions"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="ucMaxQuestions" runat="server" Width="100px" MaxLength="6" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPassmark" runat="server" Text="Pass Mark"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="ucPassmark" runat="server" Width="100px" MaxLength="6" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>       
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPassPercentage" runat="server" Text="Pass %"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="ucPassPercentage" runat="server" DecimalPlace="2"  CssClass="input" ReadOnly="true" />
                                <%--<telerik:RadTextBox ID="txtPassPercentage" runat="server" CssClass="input" Enabled="false" ></telerik:RadTextBox>--%>
                            </td>
                        </tr>                      
                    </table>                 
                
                    <eluc:TabStrip ID="MenuRegistersDocumentTestQuestions" runat="server" OnTabStripCommand="RegistersDocumentTestQuestions_TabStripCommand">
                    </eluc:TabStrip>
                
                    <telerik:RadGrid ID="gvDocumentTestQuestions" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px"  CellPadding="3" OnItemCommand="gvDocumentTestQuestions_ItemCommand"
                        OnItemDataBound="gvDocumentTestQuestions_ItemDataBound"
                        ShowHeader="true" EnableViewState="false" OnSorting="gvDocumentTestQuestions_Sorting"
                        ShowFooter="true"          AllowSorting="true" 
                        AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvDocumentTestQuestions_NeedDataSource" RenderMode="Lightweight"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                       <%--OnSelectedIndexChanging="gvDocumentTestQuestions_SelectedIndexChanging" --%>
                      
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDQUESTIONID" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Exam" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'   Width="100%"    ></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblExamid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblExamidEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                    <telerik:RadComboBox ID="ddlExamEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"   Width="100%"    ></telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlExamAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"     Width="100%"    ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reference">
                               <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="20%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel      Width="100%"     ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQREFERENCENO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtReferenceEdit" runat="server"    Width="100%"     CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQREFERENCENO") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtReferenceAdd" runat="server"    Width="100%"    CssClass="input"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Question"   SortExpression="FLDQUESTION"    AllowSorting="true" >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION").ToString().Length>80 ? DataBinder.Eval(Container, "DataItem.FLDQUESTION").ToString().Substring(0, 80)+ "..." : DataBinder.Eval(Container, "DataItem.FLDQUESTION").ToString()  %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuestionid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")%> '></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipQuestion"    Width="100%"     runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDQUESTION") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblQuestionidEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtQuestionEdit" runat="server"    Width="100%"     CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtQuestionAdd" runat="server"    Width="100%"     CssClass="input_mandatory"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn    HeaderText="Sort Order">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSortOrder"    Width="100%"    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQSORTORDER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucSortEdit"    Width="100%"     runat="server" CssClass="gridinput_mandatory"
                                        IsInteger="true" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQSORTORDER") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSortAdd"   Width="100%"     runat="server" CssClass="gridinput_mandatory"
                                        IsInteger="true" MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                          <%--  <telerik:GridTemplateColumn HeaderText="Level">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblLevelHeader" runat="server" Text="Level"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                           <%-- <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCourseHeader" runat="server" Text="Course"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourse" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                                    <asp:ImageButton ID="ImgCourse" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:Tooltip ID="ucCourse" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            
                            <telerik:GridTemplateColumn HeaderText="ActiveYN">
                            <HeaderStyle Width="10%"    HorizontalAlign="Left" Wrap="true" />
                            <FooterStyle     HorizontalAlign="Left" Wrap="true" />
                            <ItemStyle   HorizontalAlign="Left" Wrap="true" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                                   
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadCheckBox  ID="chkActiveYNAdd" runat="server" Checked="true" />
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
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />

                                    <asp:LinkButton runat="server" AlternateText="Answer List"   CommandName="ANSWER" ID="cmdAnswer"  ToolTip="Answer List"   >
                                    <span class="icon"><i class="fa fa-list-ul" ></i></span>
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
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>