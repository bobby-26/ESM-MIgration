<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCertificates.aspx.cs"
    Inherits="RegistersCertificates" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlCertificateCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Certificates</title>
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
        <style type="text/css">
            .hidden {
                display: none;
            }

            .center {
                background: fixed !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCertificates" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureCertificates" width="100%">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td width="23.5">
                        <telerik:RadTextBox ID="txtCertificateCode" runat="server" MaxLength="6" Width="60%"></telerik:RadTextBox>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="23.5">
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" Width="60%"></telerik:RadTextBox>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblCertificateCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td width="23.5">
                        <eluc:Category ID="ddlCertificateCategory" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="BindData" Width="40%" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCertificates" runat="server" OnTabStripCommand="RegistersCertificates_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCertificates" RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" AllowPaging="true" EnableHeaderContextMenu="true"
                Width="100%" AllowCustomPaging="true" EnableViewState="false" AllowSorting="true" Height="85%" CellSpacing="0"
                OnItemCommand="gvCertificates_ItemCommand" ShowHeader="true" ShowFooter="true" OnItemDataBound="gvCertificates_ItemDataBound" OnUpdateCommand="gvCertificates_UpdateCommand"
                OnNeedDataSource="gvCertificates_NeedDataSource" OnSortCommand="gvCertificates_SortCommand" AllowMultiRowSelection="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDORDER") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOrderEdit" runat="server" MaxLength="4"
                                    Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDER") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOrderAdd" runat="server" Width="100%" MaxLength="4"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECODE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCertificatesCodeEdit" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCertificatesCodeAdd" Width="100%" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" FooterText="New Certificate">
                            <HeaderStyle Width="14%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificatesID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCertificatesName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'
                                    ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCertificatesIDEdit" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCertificatesNameEdit" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCertificatesNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" Width="100%" ToolTip="Enter Certificates Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Validity Cycle">
                            <HeaderStyle Width="14%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyCycle" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSURVEYCYCLETYPEDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSurveyCycleId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucValidityCycle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCYCLES") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlSurveyTemplateEdit" runat="server" 
                                    DataSource='<%#PhoenixRegistersCertificates.GetSurveyTemplateList() %>' DataValueField="FLDTEMPLATEID"
                                    DataTextField="FLDTEMPLATENAME">
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblSurveyCycleId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlSurveyTemplateAdd" runat="server" Width="100%" Filter="Contains"
                                    DataSource='<%#PhoenixRegistersCertificates.GetSurveyTemplateList() %>' DataValueField="FLDTEMPLATEID"
                                    DataTextField="FLDTEMPLATENAME">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Audit Required Y/N" Visible="false">
                            <HeaderStyle Width="0%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# (DataBinder.Eval(Container,"DataItem.FLDISAUDITINSPECTIONREQUIRED").ToString().Equals("1"))?"Yes":"No" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAuditInspectionEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISAUDITINSPECTIONREQUIRED").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAuditInspectionAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="No Expiry">
                            <HeaderStyle Width="0%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificateType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATETYPEDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificateTypeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATETYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCertificateTypeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATETYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkIsNoExpiryEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCERTIFICATETYPE").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkIsNoExpiryAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificateCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificateCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECATEGORY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificateCategoryHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCertificateCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECATEGORY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlCertificateCategoryEdit" runat="server"
                                    DataValueField="FLDCATEGORYID" DataTextField="FLDCATEGORYNAME" DataSource='<%#PhoenixRegistersVesselSurvey.CertificateCategoryList()%>'>
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblCertificateCategoryHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlCertificateCategoryAdd" runat="server" Width="100%" Filter="Contains"
                                    DataValueField="FLDCATEGORYID" DataTextField="FLDCATEGORYNAME" DataSource='<%#PhoenixRegistersVesselSurvey.CertificateCategoryList()%>'>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Use Ann.">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDUSEANNIVERSARYDATE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkUseAnniversaryDateYN" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDUSEANNIVERSARYDATE").ToString().Equals("Yes"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkUseAnniversaryDateYNAdd" runat="server" Checked="true"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department Responsible">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartmentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepartmentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Department ID="ucDepartmentEdit" runat="server" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Department ID="ucDepartmentAdd" runat="server" AppendDataBoundItems="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verification Required Y/N">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# (DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONREQUIRED").ToString().Equals("1"))?"Yes":"No" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkVerificationReq" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONREQUIRED").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkVerificationReqAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYN" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Checked="true"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="MAP" ID="cmdMapVesselType" ToolTip="Map Vessel Types">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="EXCLUDE" ID="cmdExcludeVessels" ToolTip="Exclude Vessels">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DISTRIBUTE" ID="cmdDistribute" ToolTip="Distribute">
                                    <span class="icon"><i class="fas fa-shipping-fast"></i></span>
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
