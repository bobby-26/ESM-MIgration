<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditInterfaceReport.aspx.cs" Inherits="InspectionAuditInterfaceReport" EnableEventValidation="false"%>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvReport.ClientID %>"));
                }, 200);
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
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuTitle" runat="server" TabStrip="true"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="MV/MT:" Width="40px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtMvMt" runat="server" Text="" Width="140px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Made By:" Width="50px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtmadeby" runat="server" Width="160px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormto" runat="server" Font-Bold="true" Text="From/To:" Width="40px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtFromto" runat="server" Width="220px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblForm" runat="server" Font-Bold="true" Text="From:" Width="20px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtFrom" runat="server" Width="80px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Font-Bold="true" Text="To:" Width="10px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtTo" runat="server" Width="50px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblParent" runat="server" Text="Parent Item"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadComboBox ID="ddlParentitem" runat="server" AutoPostBack="true" Filter="Contains" Width="300px" OnSelectedIndexChanged="ddlParentitem_SelectedIndexChanged" EmptyMessage="Type to select "></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvReport" runat="server" CellSpacing="0" GridLines="None" Width="100%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableViewState="false"
                GroupingEnabled="false" OnNeedDataSource="gvReport_NeedDataSource" OnItemDataBound="gvReport_ItemDataBound"
                OnItemCommand="gvReport_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDMAPPINGID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="2%">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="2%" />
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkUploadHeader" runat="server" AutoPostBack="true" OnPreRender="CheckAll"></telerik:RadCheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="ChkItem" runat="server" CommandName="CHECK" OnCheckedChanged="SaveCheckedValues" AutoPostBack="true"></telerik:RadCheckBox>
                                <telerik:RadLabel ID="lblReviewScheduleid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMappingId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Item Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="5%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCheckitemid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCHECKITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTREFNO") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Chapter" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChapterid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblChaptername" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Def Ref No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.REFNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Condition" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Details of Present Condition" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPresentCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action Required" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="13%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="9%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActionRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtActionRequired" runat="server" Width="99%" Resize="Both" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="NC/Obs" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNcobs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList runat="server" ID="ddlncn" Width="99%">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                        <telerik:DropDownListItem Text="Major NC" Value="1" />
                                        <telerik:DropDownListItem Text="NC" Value="2" />
                                        <telerik:DropDownListItem Text="Observations" Value="3" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Assigned To" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlAssignedTo" runat="server" Width="99%" DropDownHeight="50px" Filter="Contains"
                                    EnableDirectionDetection="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="ucDueDate" runat="server" Width="99%" />
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Verification Level" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbllevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONLEVEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="cmdVerificationlevel" runat="server" Width="99%" DropDownHeight="50px" Filter="Contains"
                                    EnableDirectionDetection="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
            <asp:Button ID="ucConfirm" runat="server" Text="confirmIssue" OnClick="ucConfirm_Click"/>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
