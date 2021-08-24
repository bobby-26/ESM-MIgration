<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselSurveyTemplate.aspx.cs" Inherits="Registers_RegistersVesselSurveyTemplate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Validity Cycle</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersValidityCycle" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Visible="false"></eluc:Status>
            <eluc:TabStrip ID="MenuRegistersSurveyTemplate" runat="server" OnTabStripCommand="MenuRegistersSurveyTemplate_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSurveyTemplate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSurveyTemplate_NeedDataSource" Height="93%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvSurveyTemplate_ItemDataBound"
                OnItemCommand="gvSurveyTemplate_ItemCommand"
                OnUpdateCommand="gvSurveyTemplate_UpdateCommand"
                OnSortCommand="gvSurveyTemplate_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTEMPLATEID" ShowFooter="true">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" Width="100%" MaxLength="10"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblTemplate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATENAME") %>' CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblTemplatesId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTemplateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATENAME") %>' CssClass="input_mandatory"
                                    Width="100%" MaxLength="150"></telerik:RadTextBox>
                                <telerik:RadLabel ID="lblTemplateId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>' Visible="false"></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtTemplateAdd" runat="server" CssClass="input_mandatory" Width="100%" MaxLength="150"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Validity(M)">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucDurationEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucDurationAdd" CssClass="input_mandatory" runat="server" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Audit Required Y/N">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuditRequiredYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITREQUIREDYN").ToString()=="1"? "Yes":"No" %>'></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkAuditRequiredYN" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDAUDITREQUIREDYN").ToString() == "1" ?true:false%>'
                                    Enabled="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITREQUIREDYN").ToString()=="1"? "Yes":"No" %>'
                                    Visible="false" AutoPostBack="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAuditRequiredYNEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDAUDITREQUIREDYN").ToString() == "1" ? true:false%>'
                                    Enabled="True" AutoPostBack="false"/>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAuditRequiredYNAdd" runat="server" Checked="false" AutoPostBack="false"/>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fas fa-plus-square"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
