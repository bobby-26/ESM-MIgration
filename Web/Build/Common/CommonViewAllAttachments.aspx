<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonViewAllAttachments.aspx.cs"
    Inherits="CommonViewAllAttachments" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachments</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div3" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>        
    </div>

    <script type="text/javascript">
        function resizeFrame() {
            var obj = document.getElementById("gvAttachments");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 60 + "px";
            var cell = obj.rows[obj.rows.length - 1].cells[0];
            var rect = cell.getBoundingClientRect();
            var x = rect.left;
            var y = rect.top;
            var w = rect.right - rect.left;
            var h = rect.bottom - rect.top;
            cell.getElementsByTagName("iframe")[0].style.height = h - 30 + "px";            
        }
    </script>
  
</telerik:RadCodeBlock></head>
<body onload="resizeFrame();" onresize="resizeFrame();">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </asp:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader">
        <eluc:Title runat="server" ID="Attachment" Text="View Attachments" ShowMenu="false">
        </eluc:Title>
    </div>
    <br />
    <asp:GridView ID="gvAttachments" runat="server" AutoGenerateColumns="False" Font-Size="11px"
        AllowPaging="true" PageSize="1" PagerSettings-Mode="NextPrevious" PagerSettings-NextPageText="&nbsp;Next >> "
        PagerSettings-PreviousPageText="<< Prev&nbsp;" PagerSettings-Position="Top" Width="100%"
        CellPadding="5" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachments_RowDataBound"
        BorderWidth="0" OnPageIndexChanging="gvAttachments_PageIndexChanging">
        <Columns>
            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
            <asp:TemplateField>
                <HeaderStyle BorderWidth="0" HorizontalAlign="Center" />
                <ItemStyle BorderWidth="0" HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemTemplate>
                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                    <h3><asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label></h3>
                    <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></asp:Label>
                    <br />
                    <asp:Image ID="imgfiletype" runat="server" ImageAlign="AbsMiddle" Visible="false"/>
                    <iframe runat="server" id="ifMoreInfo" style="width: 100%; border:0">
                    </iframe>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </form>
</body>
</html>
