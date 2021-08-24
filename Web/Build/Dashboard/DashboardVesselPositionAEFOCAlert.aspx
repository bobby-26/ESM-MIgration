<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselPositionAEFOCAlert.aspx.cs" 
    Inherits="DashboardVesselPositionAEFOCAlert" %>

<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">

    var msg = <%=this.responseMsg%>;
    if(msg != 'success')
    {
        eval(msg);
    }
    ChartData = <%=this.seriesdata%>;
    dateseries = <%=this.dateList%>;


</script>
</telerik:RadCodeBlock>