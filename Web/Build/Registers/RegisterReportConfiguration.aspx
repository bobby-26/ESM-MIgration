<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterReportConfiguration.aspx.cs" Inherits="Registers_RegisterReportConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report Configuration</title>
    
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    </head>
<body>
    <form id="frmprospercategory" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    
                                
                                <%--<asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />--%>
                            <table id="tblprospermm"    width="50%" >
                                <tr>
                                    <td width="20%">
                                        <b><telerik:RadLabel ID="lblType" runat="server" Text="Report Type"    ></telerik:RadLabel></b>
                                    </td>
                                    <td width="30%">
                                        <telerik:RadComboBox ID="ddlreporttype" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">
                                            <Items>
                                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"> </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Shell Matrix" Value="1">   </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Petronas Matrix" Value="2"></telerik:RadComboBoxItem>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td width="20%">
                                       <b> <telerik:RadLabel ID="lbldocumenttype" runat="server" Text="Document Type"  ></telerik:RadLabel></b>
                                    </td>
                                    <td width="30%">
                                        <telerik:RadComboBox ID="ddldocumenttype" runat="server"   Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">
                                            <Items>
                                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Vaccination" Value="1" ></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Medical" Value="2"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Courses and Endorsements" Value="3"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Other Document" Value="4"></telerik:RadComboBoxItem>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        
                            <eluc:TabStrip ID="MenuRegistersConfig" runat="server" OnTabStripCommand="RegistersConfig_TabStripCommand"></eluc:TabStrip>
                            <telerik:RadGrid ID="gvreportconfiguration" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" ShowFooter="true"
                                ShowHeader="true" EnableViewState="false" AllowSorting="true"
                                OnItemDataBound="gvreportconfiguration_ItemDataBound" OnItemCommand="gvreportconfiguration_ItemCommand" 
                                AllowPaging="true"  AllowCustomPaging="true" OnNeedDataSource="gvreportconfiguration_NeedDataSource" RenderMode="Lightweight"
                                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSorting="gvreportconfiguration_Sorting" >
                                
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames ="FLDREPORTCONFIGID" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                        
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="DocumentID" AllowSorting="true" SortExpression="FLDDOCUMENTNAME">
                                     <HeaderStyle Width="30%" HorizontalAlign="Left" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                     <FooterStyle HorizontalAlign="Left" Wrap="true" />   
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblreportconfigid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCONFIGID") %>'   ></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lnkdocumentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'  ></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblreportconfigidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCONFIGID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbldocumentidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lnkdocumentnameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' Width="100%" ></telerik:RadLabel>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddldocumentadd" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true"  Width="100%" 
                                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"
                                             ></telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                       <telerik:GridTemplateColumn Visible="false" >
                                      <HeaderStyle Width="20%" HorizontalAlign="Left" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                     <FooterStyle HorizontalAlign="Left" Wrap="true" />   
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lnkdoctypenum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lnkdoctypenumedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>' Width="100%" ></telerik:RadLabel>
                                        </EditItemTemplate>
                                      
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Display Name"   AllowSorting="true" SortExpression="FLDDISPLAYNAME">
                                      <HeaderStyle Width="20%" HorizontalAlign="Left" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                     <FooterStyle HorizontalAlign="Left" Wrap="true" />    
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lnkdisplayname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtdisplaynameedit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYNAME") %>'   Width="100%"   ></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtdisplaynameadd" runat="server" CssClass="gridinput_mandatory"  Width="100%"     ></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Data To Display"    AllowSorting="true" SortExpression="FLDDATATODISPLAY">
                                        <HeaderStyle Width="20%" HorizontalAlign="Left" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                     <FooterStyle HorizontalAlign="Left" Wrap="true" />   
                                        
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lnkdatadisplayid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATODISPLAY").ToString()  == "1" ? "Hold Document" : "Expiry Date"%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lnkdatadisplayidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATODISPLAY") %>'></telerik:RadLabel>
                                            <telerik:RadComboBox ID="ddldatadiaplayedit" runat="server"  Width="100%"    Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">
                                                <Items>
                                                <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="Hold Document" Value="1"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="Expiry Date" Value="2"></telerik:RadComboBoxItem>
                                                </Items>
                                           </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddldatadiaplayadd" runat="server"  Width="100%"     Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category" >
                                                <Items>
                                                <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="Hold Document" Value="1"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="Expiry Date" Value="2"></telerik:RadComboBoxItem>
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Order" AllowSorting="true" SortExpression="FLDORDER">
                                        <HeaderStyle Width="20%" HorizontalAlign="Left" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                     <FooterStyle HorizontalAlign="Left" Wrap="true" />   
                                        
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lnkordernum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>' Width="100%" ></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:num ID="txtorderedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>' MaxLength="3" CssClass="gridinput_mandatory"  Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:num ID="txtorderadd" runat="server" MaxLength="3" CssClass="gridinput_mandatory"  Width="100%"  />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
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