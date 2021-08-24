<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlQuickList.ascx.cs"
    Inherits="UserControlQuickList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">
    // Maintain scroll position on list box. 
    var xPosQuick, yPosQuick;
    var prmQuick = Sys.WebForms.PageRequestManager.getInstance();

    function BeginRequestHandler(sender, args) {
        var listBox = $get('<%= lstQuick.ClientID %>');

        if (listBox != null) {
            xPosQuick = listBox.scrollLeft;
            yPosQuick = listBox.scrollTop;
        }
    }

    function EndRequestHandler(sender, args) {
        var listBox = $get('<%= lstQuick.ClientID %>');

        if (listBox != null) {
            listBox.scrollLeft = xPosQuick;
            listBox.scrollTop = yPosQuick; 
        }
    }

    prmQuick.add_beginRequest(BeginRequestHandler);
    prmQuick.add_endRequest(EndRequestHandler); 
</script>

<div runat="server" id="DivRelationship" style="overflow-y: auto; overflow-x: hidden; height:80px;" >
    <telerik:RadListBox RenderMode="Lightweight" ID="lstQuick" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE"
        CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple" runat="server" Width="240px" OnDataBound="lstQuick_DataBound"
        OnTextChanged="lstQuick_TextChanged"></telerik:RadListBox>
</div>
