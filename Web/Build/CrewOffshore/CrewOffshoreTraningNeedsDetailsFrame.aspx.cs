using SouthNests.Phoenix.CrewManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshore_CrewOffshoreTraningNeedsDetailsFrame : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void gvCrewTraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    public void BindData()
    {
       
        DataTable dt = PhoenixCrewOffshorePersonalMasterOverview.CrewTrainingNeedsCountList(
               General.GetNullableInteger(Filter.CurrentCrewSelection)
              );
        gvCrewTraining.DataSource = dt;

    }
}