<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionInspectionList.aspx.cs" Inherits="InspectionInspectionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvInspection").height(browserHeight - 20);
            });
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvInspection.ClientID %>"));
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
    <form id="frmRegistersInspection" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false"></telerik:RadLabel>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" />
            <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="CloseWindow" OKText="Yes"
                CancelText="No" Visible="false" />--%>
            <table id="tblInspection" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 80%">
                        <eluc:Hard ID="ucInspectionCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            HardList="<%# PhoenixRegistersHard.ListHard(1,144) %>" HardTypeCode="144" OnTextChangedEvent="ucInspectionCategory_Changed" />
                        <eluc:Hard ID="ucInspectionType" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" HardList="<%# PhoenixRegistersHard.ListHard(1,148) %>" HardTypeCode="148" Visible="false" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInspection" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvInspection_ItemCommand" OnNeedDataSource="gvInspection_NeedDataSource"
                OnItemDataBound="gvInspection_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                OnSortCommand="gvInspection_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Company">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Company ID="ucCompanyEdit" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Enabled="false"
                                    CssClass="input_mandatory" AppendDataBoundItems="true" SelectedCompany='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Company ID="ucCompanyAdd" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" AppendDataBoundItems="true" Enabled="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInspectionTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInspectionType" runat="server" CssClass="input" Enabled="false" Width="80px"></telerik:RadTextBox>
                                <eluc:Hard ID="ucInspectionTypeAdd" runat="server" CssClass="input" Visible="false" AppendDataBoundItems="true"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,148) %>" HardTypeCode="148" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSPECTIONCATEGORYNAME" HeaderStyle-Width="140px" HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucInspectionCategoryEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,144) %>"
                                    HardTypeCode="144" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORYID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucInspectionCategoryAdd" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,144) %>"
                                    HardTypeCode="144" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSHORTCODE" HeaderStyle-Width="110px" HeaderText="Short Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtInspectionShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                    CssClass="gridinput_mandatory" Width="98%" MaxLength="300">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInspectionShortCodeAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                    MaxLength="300">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSPECTIONNAME" HeaderStyle-Width="330px" HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkInspectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtInspectionNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="300" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInspectionNameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                    MaxLength="300">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("YES"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Frequency (in months)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYINMONTHS") %>'
                                    Width="98%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtFrequencyEdit" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYINMONTHS") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtFrequencyAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="98%"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Active Inspection">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEINSPECTION") %>'
                                    Width="98%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlinspectionidEdit" runat="server" CssClass="input_mandatory" AllowCustomText="true" EmptyMessage="Type to Select" Width="98%">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="First Reminde">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFirstReminder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTREMINDER") %>'
                                    Width="100px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtFirstReminderEdit" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTREMINDER") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtFirstReminderAdd" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="100px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Second Reminder">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSecondReminder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDREMINDER") %>'
                                    Width="100px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSecondReminderEdit" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDREMINDER") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSecondReminderAdd" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" IsInteger="true" Width="100px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucEffectiveDateEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'
                                    DatePicker="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucEffectiveDateAdd" runat="server" CssClass="gridinput_mandatory" DatePicker="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtInspectionTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPE") %>'
                                    CssClass="input" MaxLength="20" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInspectionTypeAdd" runat="server" CssClass="input" Width="98%"
                                    MaxLength="20">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Question Type" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuestionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlQuestionTypeEdit" runat="server" Width="100%" DropDownHeight="50px" Filter="Contains"
                                    EnableDirectionDetection="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlQuestionTypeAdd" runat="server" Width="100%" DropDownHeight="50px" Filter="Contains"
                                    EnableDirectionDetection="true">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Copy" ToolTip="Copy" Width="20PX" Height="20PX"
                                    CommandName="COPY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCopy" Visible="false">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
