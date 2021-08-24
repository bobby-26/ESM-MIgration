using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class VesselPositionCargoExperienceList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                }
                else
                    ViewState["vesselid"] = "";

                if (Request.QueryString["fromdate"] != null)
                {
                    ViewState["fromdate"] = Request.QueryString["fromdate"].ToString();
                }
                else
                    ViewState["fromdate"] = "";

                if (Request.QueryString["todate"] != null)
                {
                    ViewState["todate"] = Request.QueryString["todate"].ToString();
                }
                else
                    ViewState["todate"] = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    
    public void BindData()
    {
        try
        {
            DataSet ds = PhoenixVesselPositionVoyageLoadDetails.ListCargoDetailsForCrewExperience(
            General.GetNullableInteger(ViewState["vesselid"].ToString()),
            General.GetNullableDateTime(ViewState["fromdate"].ToString()),
            General.GetNullableDateTime(ViewState["todate"].ToString()));
                gvCargoExp.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCargoExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {         
            BindData();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible      = true;
        }
    }
    protected void Rebind()
    {
        gvCargoExp.DataSource = null;
        gvCargoExp.Rebind();
    }
}
