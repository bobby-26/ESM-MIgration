<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselTypeList.ascx.cs"
    Inherits="UserControlVesselTypeList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
    // Maintain scroll position on list box. 
    var xPosVesselType, yPosVesselType;
    var prmVesselType = Sys.WebForms.PageRequestManager.getInstance();

    function BeginRequestHandler(sender, args) {
        var listBox = $get('<%= lstVesselType.ClientID %>');

        if (listBox != null) {
            xPosVesselType = listBox.scrollLeft;
            yPosVesselType = listBox.scrollTop;
        }
    }

    function EndRequestHandler(sender, args) {
        var listBox = $get('<%= lstVesselType.ClientID %>');

        if (listBox != null) {
            listBox.scrollLeft = xPosVesselType;
            listBox.scrollTop = yPosVesselType;
        }
    }

    prmVesselType.add_beginRequest(BeginRequestHandler);
    prmVesselType.add_endRequest(EndRequestHandler); 
</script>

<div runat="server" id="divVesselTypeList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstVesselType" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnTextChanged="lstVesselType_TextChanged"
        OnDataBound="lstVesselType_DataBound"></telerik:RadListBox>
</div>
