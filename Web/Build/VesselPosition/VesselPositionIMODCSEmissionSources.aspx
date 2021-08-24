<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSEmissionSources.aspx.cs" Inherits="VesselPositionIMODCSEmissionSources" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .textbox {
                margin: 0 !important;
                background: 0 !important;
                border: 0 !important;
                border-bottom: 1px solid black !important;
                outline: 0 !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVPRSLocation" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" Title="Ship engines and other fuel oil consumers and fuel oil types used"
                OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"></eluc:TabStrip>

            <table width="100%" runat="server" visible="false">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" Text="Vessel" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>

            </table>
            <table id="tblsearch" runat="server" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" Text="Ref No" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtParentComponentNumberfilter" runat="server" CssClass="input"
                            MaxLength="20" Width="80px">
                        </telerik:RadTextBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="Label1" Text="Emission Source" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtParentComponentNamefilter" runat="server" CssClass="input"
                            MaxLength="20" Width="120px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFuelType" Text="Fuel Type Used" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlECAOilType" runat="server" CssClass="input" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME">
                        </telerik:RadComboBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPerformence" Text="Power" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPerformence" runat="server" CssClass="input"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblYearOfInstallation" Text="Year Of Inst" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtYearOfInstallation" runat="server" CssClass="input"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblIdentificationNofilter" Text="SFOC" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIdentificationNofilter" runat="server" CssClass="input"></telerik:RadTextBox></td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="Location_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvEmissionSource" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="85%" CellPadding="3" OnItemCommand="gvEmissionSource_RowCommand" OnItemDataBound="gvEmissionSource_ItemDataBound"
                AllowSorting="false" OnNeedDataSource="gvEmissionSource_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnUpdateCommand="gvEmissionSource_RowUpdating" ShowFooter="false"
                ShowHeader="true" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEMISSIONANDFUELTYPEID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="No." HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblrownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmissionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONANDFUELTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpmscomponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Engines or other fuel oil consumers" HeaderStyle-Width="35%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: left; ">
                                            <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblComponentdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblemissionsourceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCEID") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblType" runat="server" Text="Type :"></telerik:RadLabel>
                                            <telerik:RadLabel ID="txtType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPE") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblModel" runat="server" Text="Model :"></telerik:RadLabel>
                                            <telerik:RadLabel ID="txtmodel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINEMODEL") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: left; ">
                                            <telerik:RadLabel ID="lblComponentedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblComponentditedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblemissionsourceidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCEID") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblTypeedit" runat="server" Text="Type :"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtTypeedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPE") %>' CssClass="textbox" Width="100px"></telerik:RadTextBox>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblModel" runat="server" Text="Model :"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtmodeledit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINEMODEL") %>' CssClass="textbox" Width="100px"></telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <HeaderStyle Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lnkSerNoHeader" runat="server"
                                    ForeColor="White">
                                    ID No 
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIDNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Power" HeaderStyle-Width="40%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: left; ">
                                            <telerik:RadLabel ID="lblSerialNo" runat="server" Text="Ser No :"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSerialNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblPowerHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERCAPTION") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPEFORMENCEPOWER") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPowerUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERUNITS") %>'></telerik:RadLabel>
                                            ,&nbsp            
                                                    <telerik:RadLabel ID="lblSFOCHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCLABEL") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSFOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDENTIFICATIONNO") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSFOCUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCUNITS") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblYrInstHeader" runat="server" Text="Year of Inst :"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblYrInst" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAROFINSTALLATION") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: left; ">
                                            <telerik:RadLabel ID="lblSerialNo" runat="server" Text="Ser No :"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>' CssClass="textbox" Width="60px"></telerik:RadTextBox>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblPower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERCAPTION") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtPower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPEFORMENCEPOWER") %>' CssClass="textbox" MaxLength="5" DecimalPlace="0"></eluc:Number>
                                            <telerik:RadLabel ID="lblPowerUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERUNITS") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblSFOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCLABEL") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtSFOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDENTIFICATIONNO") %>' CssClass="textbox" DecimalPlace="2" MaxLength="7"></eluc:Number>
                                            <telerik:RadLabel ID="lblSFOCUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCUNITS") %>'></telerik:RadLabel>
                                            ,&nbsp
                                                    <telerik:RadLabel ID="lblYrInst" runat="server" Text="Year of Inst :"></telerik:RadLabel>
                                            <eluc:Number ID="txtYrInst" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAROFINSTALLATION") %>' CssClass="textbox"></eluc:Number>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fuel Types Used" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFuelType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFuelTypeedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                               
                                    <telerik:RadCheckBoxList ID="OilTypeListEdit" RepeatDirection="Vertical" Height="120px" runat="server">
                                    </telerik:RadCheckBoxList>
                                
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applicable<br>for vessel" HeaderStyle-Width="7%" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="center" VerticalAlign="Middle"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApplicableyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLICABLEYN").ToString()=="1" ? "Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="height: 70px; overflow: auto; border: 1px; color: Black;">
                                    <telerik:RadCheckBox ID="chkappyn" runat="server"></telerik:RadCheckBox>
                                </div>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                             <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="80%" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
