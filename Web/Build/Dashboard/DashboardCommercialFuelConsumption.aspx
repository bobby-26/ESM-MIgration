<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommercialFuelConsumption.aspx.cs"
    Inherits="Dashboard_DashboardCommercialFuelConsumption" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var msg = <%=this.responseMsg%>;
        if(msg != 'success')
        {
            eval(msg);
        }
        ChartData = <%=this.seriesData%>;
    dateseries = <%=this.dateList%>;

    </script>
</telerik:RadCodeBlock>
