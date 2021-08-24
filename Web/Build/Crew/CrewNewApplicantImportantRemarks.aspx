<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantImportantRemarks.aspx.cs" Inherits="CrewNewApplicantImportantRemarks" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Discussion forum</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= repDiscussion.ClientID %>"));
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
    <form id="frmCrewGeneralRemarks" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand" Title="Important Remarks"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                width="99%">
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                        <td>&nbsp; <span id="Span2" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center; align-items: baseline; color: red"></i></span>
                            <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="OnClick"
                                RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                                HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                                Text="1.Please add only entries relating finances and commitments here <br/>2.Please use this screen Judiciously<br/>3.All the remarks for the seafarer to be completed before he signs on the vessel. ">
                            </telerik:RadToolTip>
                        </td>
                    </td>
                    <td align="left" style="vertical-align: top;">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server"
                            CssClass="gridinput_mandatory" Height="49px" TextMode="MultiLine" Width="692px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="repDiscussion" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="repDiscussion_NeedDataSource" OnItemCommand="repDiscussion_ItemCommand"
                OnItemDataBound="repDiscussion_ItemDataBound" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText=" Posted By">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" ClientIDMode="AutoID"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.DESCRIPTION") %>' TargetControlId="lblRemarks" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRemarksEdit" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.DESCRIPTION").ToString() %>'>
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <ItemTemplate>
                                <%# Eval("POSTEDDATE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("POSTEDDATE")%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Y/N">
                            <ItemTemplate>
                                <%# Eval("FLDDONEYN")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkDoneYN" runat="server" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneRemarks" runat="server" ClientIDMode="AutoID"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDDONEREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDDONEREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDDONEREMARKS").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucDoneRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDONEREMARKS") %>' TargetControlId="lblDoneRemarks" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDoneRemarks" runat="server" Width="100%" Height="30px" CssClass="gridinput_mandatory"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblNotesId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTESID") %>'>
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action By">
                            <ItemTemplate>
                                <%# Eval("ACTIONBY")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action Date">
                            <ItemTemplate>
                                <%# Eval("ACTIONDATE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
