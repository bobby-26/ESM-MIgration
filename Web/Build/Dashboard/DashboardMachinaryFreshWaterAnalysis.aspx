<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMachinaryFreshWaterAnalysis.aspx.cs" 
    Inherits="DashboardMachinaryFreshWaterAnalysis" %>
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