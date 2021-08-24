using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;

public partial class Options_OptionsVesselSynchronizationStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }
    public void BindData()
    {
        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ds = DataAccess.ExecSPReturnDataSet("PRALERTDATASYNCHRONIZERVESSELS", ParameterList);

        gvVesselSynchronizationStatus.DataSource = ds;
        gvVesselSynchronizationStatus.DataBind();
    }
}
