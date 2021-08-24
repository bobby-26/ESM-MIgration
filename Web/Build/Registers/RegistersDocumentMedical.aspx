<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentMedical.aspx.cs" Inherits="RegistersDocumentMedical" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medical Test</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentMedical" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuMedicalCost" runat="server" OnTabStripCommand="MedicalCost_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Medical Test"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
            <table id="tblConfigureDocumentMedical" >
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTestName" runat="server" Text="Test Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearchMedical" runat="server" MaxLength="100" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                    <td><telerik:RadCheckBox ID="chkincludeinactive" Text="Include Inactive" runat="server"></telerik:RadCheckBox></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersDocumentMedical" runat="server" OnTabStripCommand="RegistersDocumentMedical_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentMedical" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentMedical_ItemCommand" OnNeedDataSource="gvDocumentMedical_NeedDataSource" Height="80%"
                OnSortCommand="gvDocumentMedical_SortCommand" OnItemDataBound="gvDocumentMedical_ItemDataBound" EnableViewState="false" GroupingEnabled="false"
                EnableHeaderContextMenu="true" ShowFooter="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="None">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkincludeinactive" Text="Include Inactive" runat="server"></telerik:RadCheckBox>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Document Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Document Category"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:DocumentCategory ID="ucCategoryEdit" runat="server" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentCategory ID="ucCategoryAdd" runat="server" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name of Medical" HeaderStyle-Width="180px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <%-- <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDNAMEOFMEDICAL"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />--%>
                                <asp:LinkButton ID="lblNameOfMedicalHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAMEOFMEDICAL">
                                     Test Name&nbsp;</asp:LinkButton>
                                <%--  <img id="FLDNAMEOFMEDICAL" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentMedicalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTMEDICALID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNameOfMedical" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentMedicalIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTMEDICALID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNameOfMedicalEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'
                                    Width="95%" CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNameOfMedicalAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="95%" MaxLength="200" ToolTip="Enter Name of Medical">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblRankNameList" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="RANKLIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblRankName" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Vessel Type" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblVesselType" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Owner" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselTypeHeader" runat="server" Text="Owner"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblowner" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDOWNERLIST"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Company" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblCompany" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANIES"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Flag" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblFlag" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAG"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCodeEdit" Width="95%" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCodeAdd" Width="95%" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="P&I/UK P&I" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMEDICALTYPE").ToString().TrimEnd(',') %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBoxList ID="cblPIEdit" Width="100px" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE" Direction="Vertical"
                                    DataSource='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, 0, "P&I,UKP,PMU") %>'>
                                </telerik:RadCheckBoxList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBoxList ID="cblPIAdd" Width="100px" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE" Direction="Vertical"
                                    DataSource='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, 0, "P&I,UKP,PMU") %>'>
                                </telerik:RadCheckBoxList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="72px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Y/N" HeaderStyle-Width="72px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkExpiryYNEdit" runat="server"
                                    OnCheckedChanged="chkExpiryYNEdit_CheckedChanged" AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkExpiryYNAdd" runat="server" OnCheckedChanged="chkExpiryYNAdd_CheckedChanged" AutoPostBack="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Period" Visible="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYPERIOD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucExpiryPeriodEdit" runat="server" CssClass="input" MaxLength="2" Mask="99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYPERIOD") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucExpiryPeriodAdd" runat="server" CssClass="input" MaxLength="2" Mask="99" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="110px">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucFrequencyEdit" Width="90px" runat="server" HardTypeCode="250" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardList='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 250) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucFrequencyAdd" Width="90px" runat="server" HardTypeCode="250" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardList='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 250) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Stage">
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" AllowCustomText="true" EmptyMessage="Type to Select">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" AllowCustomText="true" EmptyMessage="Type to Select">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Mandatory Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                    Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Waiver Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaiverYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkWaiverYNEdit_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true"
                                    OnCheckedChanged="chkWaiverYNAdd_CheckedChanged">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="User Group to allow Waiver">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERGROUPNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="ImgUserGroup" runat="server"><i class="fas fa-glasses"></i> </asp:LinkButton>
                                <%-- <asp:ImageButton ID="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>--%>
                                <eluc:Tooltip ID="ucUserGroup" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERGROUPNAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                                <div style="height: 100px; width: 200px; overflow: auto;" class="input">
                                    <telerik:RadCheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input" AppendDataBoundItems="true" />--%>
                                <div style="height: 100px; width: 200px; overflow: auto;" class="input">
                                    <telerik:RadCheckBoxList ID="chkUserGroupAdd" RepeatDirection="Vertical" Enabled="false" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Addition Doc. Y/N" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionDocYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAdditionDocYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAdditionDocYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requires Authentication Y/N" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthReqYnYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAuthReqYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAuthReqYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show in Master's checklist onboard Y/N" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowInMasterChecklistYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkShowInMasterChecklistYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkShowInMasterChecklistYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PhotocopyAcceptableYn" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhotocopyAcceptableYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkPhotocopyAcceptableYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkPhotocopyAcceptableYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Age From" Visible="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgefrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGEFROM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAgefromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGEFROM") %>' CssClass="input_mandatory" Mask="99" MaxLength="2" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAgefromAdd" runat="server" CssClass="input_mandatory" MaxLength="2" Mask="99" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Age To" Visible="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgeTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGETO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAgeToEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGETO") %>' CssClass="input_mandatory" Mask="99" MaxLength="2" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAgeToAdd" runat="server" CssClass="input_mandatory" MaxLength="2" Mask="99" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
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
                                <%--  <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>--%>
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
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
