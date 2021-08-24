using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreExperienceDetailsFrame : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void gvCrewExp_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
       

       
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewCompanyExperience.CrewCompanyExperienceSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , 1
            , 1000
            , ref iRowCount
            , ref iTotalPageCount);
    

        gvCrewExp.DataSource = ds;
       

      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
}