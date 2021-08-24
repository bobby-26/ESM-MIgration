<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalOverhaulWorkOrder.aspx.cs" Inherits="DashboardTechnicalOverhaulWorkOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Overhaul Status</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function setSize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponentCategpry.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = setSize;
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .t-col, .t-container {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        p {
            padding-left: 10px;
        }

        .t-container {
            max-width: none !important;
        }
    </style>
</head>
<body onload="setSize();" onunload="setSize();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid ID="gvComponentCategpry" runat="server" AutoGenerateColumns="false" OnItemDataBound="gvComponentCategpry_ItemDataBound"
                OnNeedDataSource="gvComponentCategpry_NeedDataSource" ShowFooter="True" AllowFilteringByColumn="false" EnableLinqExpressions="false">
                <MasterTableView HeaderStyle-Font-Bold="true" FooterStyle-Font-Bold="true">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Major Overhauls" Name="mvh" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Overhauls" Name="ovh" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component No." HeaderStyle-Width="100px" DataField="FLDCOMPONENTNUMBER" FilterControlWidth="60px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="250px" DataField="FLDCOMPONENTNAME" FilterControlWidth="200px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overdue" ColumnGroupName="mvh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDMAJOROVERDUE" FooterText=" " UniqueName="FLDMAJOROVERDUE">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkMajorOverDue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJOROVERDUE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due in 30" UniqueName="FLDMAJORDUE30" ColumnGroupName="mvh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDMAJORDUE30" FooterText=" ">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkMajorDue30" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORDUE30") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due in 60" UniqueName="FLDMAJORDUE60" ColumnGroupName="mvh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDMAJORDUE60" FooterText=" ">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkMajorDue60" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORDUE60") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overdue" UniqueName="FLDOVERHAULOVERDUE" ColumnGroupName="ovh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDOVERHAULOVERDUE" FooterText=" ">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkOverHaulOverDue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERHAULOVERDUE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due in 30" UniqueName="FLDOVERHAULDUE30" ColumnGroupName="ovh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDOVERHAULDUE30" FooterText=" ">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkOverHaul30" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERHAULDUE30") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due in 60" UniqueName="FLDOVERHAULDUE60" ColumnGroupName="ovh" FilterControlWidth="50px"
                            Aggregate="Sum" DataField="FLDOVERHAULDUE60" FooterText=" ">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkOverHaul60" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERHAULDUE60") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponentCategpry.ClientID %>"));
                }, 200);
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
