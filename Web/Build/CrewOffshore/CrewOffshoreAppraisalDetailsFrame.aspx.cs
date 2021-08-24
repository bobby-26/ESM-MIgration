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
public partial class CrewOffshore_CrewOffshoreAppraisalDetailsFrame : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void gvCrewAppraisal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
               General.GetNullableInteger(Filter.CurrentCrewSelection)
               , General.GetNullableInteger(ViewState["VSLID"] != null ? ViewState["VSLID"].ToString() : string.Empty)
              , null
              , 1
              , 100
              , ref iRowCount
              , ref iTotalPageCount
              );
        gvCrewAppraisal.DataSource = ds.Tables[0];

    }

    protected void gvCrewAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlToolTip ucToolTipPlannedVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipPlannedVessel");
            if (ucToolTipPlannedVessel != null)
                ucToolTipPlannedVessel.Text = "HOD: " + drv["FLDHEADDEPTCOMMENT"].ToString() + "Master: " + drv["FLDMASTERCOMMENT"].ToString();
            // < eluc:tooltip id = "ucToolTipPlannedVessel" runat = "server" text = "HOD: " + DataBinder.Eval(Container, "DataItem.FLDSUPERINTENDENTCOMMENT") %> '+"Master:"" DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT") %>' targetcontrolid = "lblPlannedVessel" />
        }
    }
}