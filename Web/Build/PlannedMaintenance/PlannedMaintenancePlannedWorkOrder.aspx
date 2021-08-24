<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancePlannedWorkOrder.aspx.cs"
    Inherits="PlannedMaintenancePlannedWorkOrder" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manual" Src="~/UserControls/UserControlPMSManual.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Planned Jobs</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

            <table style="width: 100%; border: 1px; background-color: cornflowerblue; color: red; padding: 5px; font: bold">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblworkorderNo" Text="WORK ORDER NO:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" Text="CATEGORY:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanDate" Text="PLAN DATE:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDuration" Text="DURATION:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsible" Text="ASSIGNED TO:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="STATUS:" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand1" EnableViewState="false" Height="80%" OnSortCommand="gvWorkOrder_SortCommand"
                OnDeleteCommand="gvWorkOrder_DeleteCommand" OnItemDataBound="gvWorkOrder_ItemDataBound1">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Comp. No." AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="140px" HeaderText="Comp. Name" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="111px" HeaderText="Job Code." AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDJOBCODE" AllowFiltering="false" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Job Title" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" ShowFilterIcon="false" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select"
                                    Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                <telerik:RadToolTip ID="tipWorkName" runat="server" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'
                                    TargetControlID="lnkWorkorderName" RenderMode="Lightweight" ShowEvent="OnMouseOver">
                                </telerik:RadToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Class">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobclass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Details" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobDetail" runat="server" ToolTip="Click to see">
                                    <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-Width="80px" AllowSorting="true" ShowFilterIcon="false"
                            ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RAs" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel id="lblCompRaId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPJOBRAID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel id="lblRaId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRiskCreate" runat="server" Text="Create" CommandName="RACREATE" Visible="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkRiskView" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANUMBER") %>' CommandName="RAVIEW" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnRA">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRANumber" runat="server" CssClass="input_mandatory"
                                        MaxLength="50" Width="100px" Text="">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRA" runat="server"  Enabled="false" CssClass="hidden"
                                        MaxLength="50" Width="200px" Text="" Display="false">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowRA" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" 
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRAId" runat="server" MaxLength="20" Width="50px" CssClass="hidden"
                                        Text="">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRaType" runat="server" CssClass="hidden" MaxLength="2" Width="0px"
                                        Text=''>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PTW">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkPermit" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="JHA" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJHA" runat="server" Text="Click" CommandName="JHA"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Links" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <eluc:Manual ID="ucManual" runat="server" ComponentJobId='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Template" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTemplates" runat="server" Text="Download" CommandName="TEMPLATE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parts Required" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPartRequired" runat="server" Text="Show" CommandName="PARTSREQUIRED"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                   <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
