<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIEngineParameters.aspx.cs" Inherits="Log_ElectronicLogAnnexVIEngineParameters" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record book of Engine parameters</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .user-info {
                float: right;
            }
        </style>
        <%--<script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvParameter.ClientID %>"));

                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table class="user-info">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblUsername"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnItemSelected="ddl_TextChanged"></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="Tabstrip" runat="server" OnTabStripCommand="Tabstrip_TabStripCommand" />
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvParameter" AutoGenerateColumns="false"  Height="40%"
                AllowPaging="false" OnNeedDataSource="gvParameter_NeedDataSource" OnItemCommand="gvParameter_ItemCommand"
                OnItemDataBound="gvParameter_ItemDataBound" OnUpdateCommand="gvParameter_UpdateCommand">
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDCOMPONENTID" AutoGenerateColumns="false" EnableViewState="true"
                    EnableColumnsViewState="false" TableLayout="Fixed" ShowFooter="false" HeaderStyle-Font-Bold="true"
                    ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="None">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="cg" HeaderText="Record Book of Engine Parameters(in accordance to MARPOL,Annex VI and Nox Technical Code ,EIAPP Certificate)" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
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
                        <telerik:GridTemplateColumn HeaderText="Engine" ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="radlblenginename" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPARAMETERID")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radcomponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Make" ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="radlblenginemake" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAKE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="radtbenginemakeentry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAKE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Model" ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="radlblenginemodel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODEL")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="radlblenginemodelentry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODEL")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Serial Number" ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="radlblengineserial" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSERIALNUMBER")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="radlblengineserialentry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSERIALNUMBER")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EIAPP certificate No." ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="radlblengineeiapp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEIAAPCERTIFICATENO")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="radlblengineeiappentry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEIAAPCERTIFICATENO")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" ColumnGroupName="cg">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" CommandName="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" ID="btnUpdate" ToolTip="Update" CommandName="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                &nbsp &nbsp
                              <asp:LinkButton runat="server" ID="btncancel" ToolTip="Cancel" CommandName="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                              </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvParameterRecord" AutoGenerateColumns="false" Height="48%"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvParameterRecord_NeedDataSource"
                OnItemDataBound="gvParameterRecord_ItemDataBound" OnItemCommand="gvParameterRecord_ItemCommand">
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDRECORDID" AutoGenerateColumns="false"
                    EnableColumnsViewState="false" TableLayout="Fixed" CommandItemDisplay="None" ShowFooter="true"
                    ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="par" HeaderText="" />
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="par" HeaderStyle-Width="5%">

                            <ItemStyle HorizontalAlign="Center" Width="5%" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="radsno" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSNO")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radrid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECORDID")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="raddtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'>
                                </telerik:RadLabel>
                                 <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISBACKDATEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" ID="btngreenlock" ToolTip="Entry UnLocked" Visible="false">
                                            <span class="icon"><img src="../css/Theme1/images/unlock_3.png" /></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnredlock" ToolTip="Entry Locked" Visible="false" CommandName="Unlock">
                                            <span class="icon"><img src="../css/Theme1/images/lock_2.png" /></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Adjustment Made" ColumnGroupName="par" HeaderStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="25%" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="radadjustment" runat="server" Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" ColumnGroupName="par" HeaderStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="25%" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="radremarks" runat="server" Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="par" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="10%" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="raddate" runat="server" Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="OiC Sign" ColumnGroupName="par" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="10%" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="radoicsign" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOICSIGN")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radoicsigntext" runat="server" Visible="false" Text="OiC Signed">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="btnoicsign" runat="server" Visible="false" Text=" Sign" CommandName="sign" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CE Sign" ColumnGroupName="par" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="10%" />

                            <ItemTemplate>

                                <telerik:RadLabel ID="radcesign" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCESIGN")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radcesigntext" runat="server" Visible="false" Text="Verified">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="btncesign" runat="server" Visible="false" Text="Verify" CommandName="CHIEFENGGSIGN" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" ColumnGroupName="par" HeaderStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit1" ToolTip="Edit" CommandName="AMEND">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btndelete1" ToolTip="Delete" CommandName="Delete">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>

                                <asp:ImageButton ID="btnattachments" runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ToolTip="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ImageUrl='Session["images"]/no-attachment.png'></asp:ImageButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
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
