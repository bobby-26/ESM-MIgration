<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEPSSCourseExceptionList.aspx.cs"
    Inherits="CrewEPSSCourseExceptionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Activity</title>
      <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSCE.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmActivity" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              
              <table cellpadding="1" cellspacing="1" width="60%">
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblException" runat="server" Text="Exception List"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlExceptionList" runat="server"  width="180px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select Exception" >
                                <Items>
                                <telerik:RadComboBoxItem Value=""  text="--Select--"    />
                                <telerik:RadComboBoxItem Value="1"  text="Ready for Processing" />
                                <telerik:RadComboBoxItem Value="2"  text="File No not matched" />
                                <telerik:RadComboBoxItem Value="3"  text="Seagull Course name not matched with phoenix course" />
                                <telerik:RadComboBoxItem Value="4"  text="Last Date not specified" />
                                <telerik:RadComboBoxItem Value="5"  text="Last Date is not a valid date" />
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblFileNo" runat="server" Text="File No"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtfilenumber" runat="server"  width="180px" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                
                        <eluc:TabStrip ID="MenuCrewActivityLog" runat="server" OnTabStripCommand="MenuCrewActivityLog_TabStripCommand">
                    </eluc:TabStrip>
               
                    <telerik:RadGrid ID="gvSCE" runat="server" AutoGenerateColumns="False" 
                CellPadding="1" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvSCE_ItemCommand"
                OnItemDataBound="gvSCE_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvSCE_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDDTKEY">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>                 
                    <ItemStyle HorizontalAlign="Left"   />
                    <Columns>           
                        <telerik:gridtemplatecolumn headertext="File No">
                            
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="gridinput" MaxLength="20" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Course">
                           
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCourse" runat="server" CssClass="gridinput" MaxLength="800" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Exam Date">
                        
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXAMDATE"))%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtExamDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXAMDATE")%>' />
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Done On">
                          
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSHIP")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShip" runat="server" CssClass="gridinput" MaxLength="200" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSHIP")%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Duration">
                            
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDURATION")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDuration" runat="server"  MaxLength="50" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDURATION")%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Attempts">
                           
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDATTEMPTS")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAttempts" runat="server"  Text='<%#DataBinder.Eval(Container, "DataItem.FLDATTEMPTS")%>'
                                    IsInteger="true" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Message">
                           
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDMESSAGE")%>
                            </ItemTemplate>
                        </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn headertext="Action" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        
                            <ItemStyle Wrap="False"  Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"  CommandName="EDIT"  ID="cmdEdit"  ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>

                                </asp:LinkButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete"      CommandName="DELETE" ID="cmdDelete"      ToolTip="Delete">
                                 <span class="icon"><i class="fas fa-trash-alt"></i></span>

                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"  CommandName="Update" ID="cmdSave"       ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"  CommandName="Cancel" ID="cmdCancel"   ToolTip="Cancel">

                                     <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:gridtemplatecolumn>
                     </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>