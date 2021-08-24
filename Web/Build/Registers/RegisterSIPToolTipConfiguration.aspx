<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterSIPToolTipConfiguration.aspx.cs" Inherits="RegisterSIPToolTipConfiguration" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx"%> 
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bunker Receipt</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
     </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager  ID="ToolkitScriptManager1" runat="server"/>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"/>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadLabel ID="lblCountry" style="color: Blue;" runat="server" >To break the sentance use the <b> &amp;lt;br&amp;gt; </b>  between the sentance</telerik:RadLabel>

        <telerik:RadFormDecorator ID="RadFormDecorator1" DecorationZoneID="gvSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvSIPTanksConfuguration" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false"
            Width="100%"  CellPadding="3" OnItemCommand="gvSIPTanksConfuguration_RowCommand" OnItemDataBound="gvSIPTanksConfuguration_ItemDataBound"
            ShowHeader="true" OnSortCommand="gvSIPTanksConfuguration_SortCommand" OnNeedDataSource="gvSIPTanksConfuguration_NeedDataSource"
            EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIPCONFIGURATIONID" >
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

            <Columns>
                <telerik:GridTemplateColumn HeaderText="Tank Name"  HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblHeaderTank" runat="server" Text="Mesure Name"></telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPCONFIGURATIONID") %>'></telerik:RadLabel>
                    </ItemTemplate>      
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="60%">
                    <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>
                    
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblCapaciltyHeader" runat="server">ToolTip Text&nbsp;
                        </telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>'></telerik:RadLabel>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPCONFIGURATIONID") %>'></telerik:RadLabel>    
                        <telerik:RadTextBox ID="txtCapaciltyEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>' TextMode="MultiLine" Rows="8" Width="98%"></telerik:RadTextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                            
                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblHeaderAction" runat="server" Text="Action"></telerik:RadLabel>
                    </HeaderTemplate>
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
           </MasterTableView>

        </telerik:RadGrid>               
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
