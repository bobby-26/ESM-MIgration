using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;

public partial class Inspection_InspectionJHARACommentsMoreInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["CommentsId"] = Request.QueryString["CommentsId"];
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.RACommentsMoreInfo(new Guid(ViewState["CommentsId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblArchivedBy.Text = dr["FLDARCHIVEDBY"].ToString();
            lblArchivedOn.Text = dr["FLDARCHIVEDDATE"].ToString();

        }
    }
}