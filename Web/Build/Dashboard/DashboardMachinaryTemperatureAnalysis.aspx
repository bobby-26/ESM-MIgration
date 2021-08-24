<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMachinaryTemperatureAnalysis.aspx.cs" 
    Inherits="DashboardMachinaryTemperatureAnalysis" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">

    var msg = <%=this.responseMsg%>;
    if(msg != 'success')
    {
        eval(msg);
    }
    ChartData = <%=this.seriesdata%>;
    dateseries = <%=this.dateList%>;
    ChartData1 = <%=this.seriesdata1%>;


</script>
    </telerik:RadCodeBlock>